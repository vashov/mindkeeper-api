using Microsoft.IdentityModel.Tokens;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Services.Users.Models;
using MindKeeper.Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Users
{
    public class UserService : IUserService
    {
        public async Task<OperationResult<string>> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<long>> CreateUser(string mail, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<string>> CreateAccessToken(string mail, string password)
        {
            var findUserResult = await FindUser(mail, password);
            if (!findUserResult.IsOk)
                return OperationResult<string>.Error(findUserResult.ErrorMessage);

            var userIdentity = await GetUserClaims(findUserResult.Data.Id);
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

        private async Task<OperationResult<User>> FindUser(string mail, string password)
        {
            return OperationResult<User>.Error("Invalid mail or password.");
        }

        private async Task<ClaimsIdentity> GetUserClaims(int userId)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "someLogin"),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "someRole")
                };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
