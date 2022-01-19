using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MindKeeper.Api.Core.Auth
{
    public static class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient";
        
        private const string KEY = "todo_some_secret_key_here!123";

        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
