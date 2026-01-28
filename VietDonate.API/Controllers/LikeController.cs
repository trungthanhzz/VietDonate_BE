using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Likes.Commands.ToggleLikePost;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController(ISender mediator) : ApiController
    {
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("post/{postId}")]
        public async Task<IActionResult> ToggleLikePost(Guid postId)
        {
            var command = new ToggleLikePostCommand(postId);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: toggleResult => Ok(toggleResult),
                onFailure: Problem
            );
        }
    }
}

