using Microsoft.IdentityModel.Tokens;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Exceptions;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using MindKeeper.Shared.Core;
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

        public async Task<User> CreateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new AppValidationException("Invalid username.");

            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Invalid password.");

            var userByName = await _userRepository.Get(username, isNormalizedSearch: true);
            if (userByName != null)
                throw new ApiException("Username is used already.");

            var passwordHasher = new PasswordHasher();
            var passwordHash = passwordHasher.CreateHash(password);
            var createdUser = await _userRepository.Create(username, passwordHash);

            createdUser.PasswordHash = string.Empty;

            return createdUser;
        }

        public async Task<string> CreateAccessToken(string username, string password)
        {
            const string credentialsError = "Invalid username or password.";
            
            var userByName = await _userRepository.Get(username, isNormalizedSearch: false);
            if (userByName == null)
                throw new ApiException(credentialsError);

            var passwordHasher = new PasswordHasher();
            if (!passwordHasher.ArePasswordsEqual(password, userByName.PasswordHash))
                throw new ApiException(credentialsError);

            var userIdentity = GetUserClaims(userByName);
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
