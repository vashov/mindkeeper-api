using Microsoft.IdentityModel.Tokens;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Exceptions;
using MindKeeper.Api.Data.Repositories.Users;
using MindKeeper.Api.Data.Repositories.Users.Models;
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

        public async Task<string> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CreateUser(string mail, string password)
        {
            if (string.IsNullOrWhiteSpace(mail))
                throw new AppValidationException("Invalid username.");

            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Invalid password.");

            var userByMail = await _userRepository.Get(mail, isNormalizedSearch: true);
            if (userByMail != null)
                throw new ApiException("Username is used already.");

            var passwordHasher = new PasswordHasher();
            var passwordHash = passwordHasher.CreateHash(password);
            var createdUser = await _userRepository.Create(mail, passwordHash);

            createdUser.PasswordHash = string.Empty;

            return createdUser;
        }

        public async Task<string> CreateAccessToken(string mail, string password)
        {
            const string credentialsError = "Invalid mail or password.";
            
            var userByMail = await _userRepository.Get(mail, isNormalizedSearch: false);
            if (userByMail == null)
                throw new ApiException(credentialsError);

            var passwordHasher = new PasswordHasher();
            if (!passwordHasher.ArePasswordsEqual(password, userByMail.PasswordHash))
                throw new ApiException(credentialsError);

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

            return encodedJwt;
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
