using System.Security.Claims;

namespace MindKeeper.Api.Core.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal identity)
        {
            var claim = identity.FindFirst(AppClaimTypes.UserId);
            return int.Parse(claim.Value);
        }
    }
}
