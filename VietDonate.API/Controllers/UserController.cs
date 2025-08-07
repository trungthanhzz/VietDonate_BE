using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Users.Commands.Register;
using VietDonate.Infrastructure.ModelInfrastructure.Users.Contracts;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ISender mediator) : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            // Validate the request
            if (!request.IsValid)
            {
                return BadRequest(new { Message = "At least one contact method (phone or email) is required." });
            }

            var command = new RegisterUserCommand(
                request.UserName,
                request.Password,
                request.FullName,
                request.Phone,
                request.Email,
                request.Address
            );

            var result = await mediator.Send(command);

            return result.Match(
                success => Ok(new 
                {
                    success.Message,
                    success.UserId,
                    success.UserName,
                    success.FullName
                }),
                Problem
            );
        }
    }
}
