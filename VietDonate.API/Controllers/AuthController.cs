using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Mediator;
using VietDonate.Infrastructure.ModelInfrastructure.Auths.Contracts;
using VietDonate.Application.UseCases.Auths.Commands.RefreshToken;
using VietDonate.Application.UseCases.Auths.Commands.Logout;
using VietDonate.Application.UseCases.Auths.Queries.Login;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ApiController
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var query = new LoginAccountQuery(
                UserName: request.UserName,
                Password: request.Password,
                IsRemember: request.IsRemember);

            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: loginResult => Ok(loginResult),
                onFailure: Problem
            );
        }

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: refreshResult => Ok(refreshResult),
                onFailure: Problem
            );
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var command = new LogoutCommand(request.RefreshToken);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: logoutResult => Ok(logoutResult),
                onFailure: Problem
            );
        }
    }
}
