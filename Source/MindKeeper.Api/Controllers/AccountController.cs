using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Shared.Wrappers;
using MindKeeper.Api.Services.Users;
using MindKeeper.Shared.Models.ApiModels.Accounts;
using System.Threading.Tasks;

namespace MindKeeper.Api.Controllers
{
    [ApiController]
    [ControllerRoute]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(
            ILogger<AccountController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<AppResponse> Registration([FromBody] RegistrationRequest request)
        {
            var result = await _userService.CreateUser(request.Name, request.Password);

            return AppResponse.Ok();
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<AppResponse<TokenResult>> Token([FromBody] TokenRequest request)
        {
            var result = await _userService.CreateAccessToken(request.Name, request.Password);

            var response = new TokenResult
            {
                AccessToken = result
            };

            return AppResponse<TokenResult>.Ok(response);
        }
    }
}
