using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Media.Commands.AssignMediaToCampaign;
using VietDonate.Application.UseCases.Media.Commands.DeleteMedia;
using VietDonate.Application.UseCases.Media.Commands.UpdateMedia;
using VietDonate.Application.UseCases.Media.Commands.UploadMedia;
using VietDonate.Application.UseCases.Media.Queries.GetCampaignMedia;
using VietDonate.Application.UseCases.Media.Queries.GetMedia;
using VietDonate.Application.UseCases.Media.Queries.GetTempMedia;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController(ISender mediator) : ApiController
    {
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("upload")]
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Message = "File is required" });
            }

            using var stream = file.OpenReadStream();
            var command = new UploadMediaCommand(
                FileStream: stream,
                FileName: file.FileName,
                ContentType: file.ContentType,
                FileSize: file.Length
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: uploadResult => Ok(uploadResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public async Task<IActionResult> GetMedia(Guid id)
        {
            var query = new GetMediaQuery(id);
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: mediaResult => Ok(mediaResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("campaign/{campaignId}")]
        public async Task<IActionResult> GetCampaignMedia(Guid campaignId)
        {
            var query = new GetCampaignMediaQuery(campaignId);
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: mediaResult => Ok(mediaResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("temp")]
        public async Task<IActionResult> GetTempMedia()
        {
            var query = new GetTempMediaQuery();
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: tempMediaResult => Ok(tempMediaResult),
                onFailure: Problem
            );
        }

        [HttpPut]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMedia(Guid id, [FromBody] UpdateMediaRequest request)
        {
            var command = new UpdateMediaCommand(
                MediaId: id,
                DisplayOrder: request.DisplayOrder,
                Status: request.Status
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: updateResult => Ok(updateResult),
                onFailure: Problem
            );
        }

        [HttpDelete]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMedia(Guid id)
        {
            var command = new DeleteMediaCommand(id);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: deleteResult => Ok(deleteResult),
                onFailure: Problem
            );
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("assign")]
        public async Task<IActionResult> AssignMediaToCampaign([FromBody] AssignMediaToCampaignRequest request)
        {
            var command = new AssignMediaToCampaignCommand(
                CampaignId: request.CampaignId,
                TempMediaIds: request.TempMediaIds ?? new List<Guid>(),
                ExistingMediaIds: request.ExistingMediaIds
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: assignResult => Ok(assignResult),
                onFailure: Problem
            );
        }
    }

    public record UpdateMediaRequest(int? DisplayOrder, string? Status);
    public record AssignMediaToCampaignRequest(
        Guid CampaignId,
        List<Guid>? TempMediaIds,
        List<Guid>? ExistingMediaIds
    );
}
