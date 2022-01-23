using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Api.Services.Users;
using MindKeeper.Shared.Models;
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
        public async Task<OperationResult> Registration([FromBody] RegistrationRequest request)
        {
            var result = await _userService.CreateUser(request.Mail, request.Password);

            return (OperationResult)result;
        }

        [HttpPost("[action]")]
        public async Task<OperationResult<TokenResponse>> Token([FromBody] TokenRequest request)
        {
            var result = await _userService.CreateAccessToken(request.Username, request.Password);
            if (!result.IsOk)
                return OperationResult<TokenResponse>.Error(result.ErrorMessage);

            var response = new TokenResponse
            {
                AccessToken = result.Data
            };

            return OperationResult<TokenResponse>.Ok(response);
        }
    }
}
