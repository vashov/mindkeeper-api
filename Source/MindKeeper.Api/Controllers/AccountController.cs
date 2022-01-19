using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Services.Users;
using MindKeeper.Shared;
using MindKeeper.Shared.ApiModels.Accounts;
using System.Threading.Tasks;

namespace MindKeeper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(
            ILogger<AccountController> logger,
            IUserService accountService)
        {
            _logger = logger;
            _userService = accountService;
        }

        [HttpPost("[action]")]
        public async Task<OperationResult> Registration([FromBody] RegistrationRequest request)
        {
            var result = await _userService.CreateUser(request.Mail, request.Password);

            return (OperationResult)result;
        }
    }
}
