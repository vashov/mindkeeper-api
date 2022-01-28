using Microsoft.AspNetCore.Mvc;

namespace MindKeeper.Api.Core.Routing
{
    public class ControllerRouteAttribute : RouteAttribute
    {
        public ControllerRouteAttribute() : base("api/[controller]")
        {
        }
    }
}
