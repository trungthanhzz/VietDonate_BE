using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Posts.Commands.CreatePost;
using VietDonate.Application.UseCases.Posts.Commands.DeletePost;
using VietDonate.Application.UseCases.Posts.Commands.UpdatePost;
using VietDonate.Application.UseCases.Posts.Queries.GetPostsByCampaign;
using VietDonate.Infrastructure.ModelInfrastructure.Posts.Contracts;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(ISender mediator) : ApiController
    {
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var command = new CreatePostCommand(
                Title: request.Title,
                Content: request.Content,
                PostType: request.PostType,
                CampaignId: request.CampaignId,
                Status: request.Status,
                ProofType: request.ProofType,
                ProofDate: request.ProofDate
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: createResult => Ok(createResult),
                onFailure: Problem
            );
        }

        [HttpPut]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostRequest request)
        {
            var command = new UpdatePostCommand(
                PostId: id,
                Title: request.Title,
                Content: request.Content,
                PostType: request.PostType,
                CampaignId: request.CampaignId,
                Status: request.Status,
                ProofType: request.ProofType,
                ProofDate: request.ProofDate
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: updateResult => Ok(updateResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("campaign/{campaignId}")]
        public async Task<IActionResult> GetPostsByCampaign(Guid campaignId)
        {
            var query = new GetPostsByCampaignQuery(campaignId);
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: postsResult => Ok(postsResult),
                onFailure: Problem
            );
        }

        [HttpDelete]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var command = new DeletePostCommand(id);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: deleteResult => Ok(deleteResult),
                onFailure: Problem
            );
        }
    }
}

