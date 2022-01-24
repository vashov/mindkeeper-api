using Microsoft.IdentityModel.Tokens;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Data.Repositories.Users;
using MindKeeper.Api.Data.Repositories.Users.Models;
using MindKeeper.Shared.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<string>> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<User>> CreateUser(string mail, string password)
        {
            if (string.IsNullOrWhiteSpace(mail))
                return OperationResult<User>.Error("Invalid username.");

            if (string.IsNullOrWhiteSpace(password))
                return OperationResult<User>.Error("Invalid password.");

            var userByMail = await _userRepository.Get(mail, isNormalizedSearch: true);
            if (userByMail != null)
                return OperationResult<User>.Error("Username is used already.");

            var passwordHasher = new PasswordHasher();
            var passwordHash = passwordHasher.CreateHash(password);
            var createdUser = await _userRepository.Create(mail, passwordHash);

            createdUser.PasswordHash = string.Empty;

            return OperationResult<User>.Ok(createdUser);
        }

        public async Task<OperationResult<string>> CreateAccessToken(string mail, string password)
        {
            const string credentialsError = "Invalid mail or password.";
            
            var userByMail = await _userRepository.Get(mail, isNormalizedSearch: false);
            if (userByMail == null)
                return OperationResult<string>.Error(credentialsError);

            var passwordHasher = new PasswordHasher();
            if (!passwordHasher.ArePasswordsEqual(password, userByMail.PasswordHash))
                return OperationResult<string>.Error(credentialsError);

            var userIdentity = GetUserClaims(userByMail);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: userIdentity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return OperationResult<string>.Ok(encodedJwt);
        }

        private ClaimsIdentity GetUserClaims(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(AppClaimTypes.UserId, user.Id.ToString())
                    //new Claim(ClaimsIdentity.DefaultNameClaimType, "someLogin"),
                    //new Claim(ClaimsIdentity.DefaultRoleClaimType, "someRole")
                };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
