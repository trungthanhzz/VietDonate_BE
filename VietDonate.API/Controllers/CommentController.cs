using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Comments.Commands.CreateComment;
using VietDonate.Application.UseCases.Comments.Commands.DeleteComment;
using VietDonate.Application.UseCases.Comments.Queries.GetCommentsByPost;
using VietDonate.Infrastructure.ModelInfrastructure.Comments.Contracts;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(ISender mediator) : ApiController
    {
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var command = new CreateCommentCommand(
                PostId: request.PostId,
                Content: request.Content,
                ParentId: request.ParentId);

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: createResult => Ok(createResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(Guid postId)
        {
            var query = new GetCommentsByPostQuery(postId);
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: commentsResult => Ok(commentsResult),
                onFailure: Problem
            );
        }

        [HttpDelete]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var command = new DeleteCommentCommand(id);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: deleteResult => Ok(deleteResult),
                onFailure: Problem
            );
        }
    }
}

