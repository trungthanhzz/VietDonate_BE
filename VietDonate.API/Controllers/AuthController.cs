using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using VietDonate.API.Common;
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
                Ok,
                Problem);
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var result = await mediator.Send(command);
            return result.Match(
                Ok,
                Problem);
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var command = new LogoutCommand(request.RefreshToken);
            var result = await mediator.Send(command);
            return result.Match(
                Ok,
                Problem);
        }
    }

    public record RefreshTokenRequest(string RefreshToken);
    public record LogoutRequest(string RefreshToken);
}
