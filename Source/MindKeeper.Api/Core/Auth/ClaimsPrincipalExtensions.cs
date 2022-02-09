using System;
using System.Security.Claims;

namespace MindKeeper.Api.Core.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal identity)
        {
            var claim = identity.FindFirst(AppClaimTypes.UserId);
            return Guid.Parse(claim.Value);
        }
    }
}
