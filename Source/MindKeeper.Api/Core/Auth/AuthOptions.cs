using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace MindKeeper.Api.Core.Auth
{
    public static class AuthOptions
    {
        static AuthOptions()
        {
            _key = Environment.GetEnvironmentVariable("TOKEN_SECURITY_KEY")
                ?? throw new NotImplementedException("Token key not implemented in variables.");
        }

        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient";
        
        private static readonly string _key;

        public const int LIFETIME = 365 * 24 * 60; // TODO: RefreshToken
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
