using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Campaigns.Commands.CreateCampaign;
using VietDonate.Application.UseCases.Campaigns.Commands.UpdateCampaign;
using VietDonate.Application.UseCases.Campaigns.Commands.DeleteCampaign;
using VietDonate.Application.UseCases.Campaigns.Commands.ApproveCampaign;
using VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign;
using VietDonate.Application.UseCases.Campaigns.Queries.GetCampaigns;
using VietDonate.Infrastructure.ModelInfrastructure.Campaigns.Contracts;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController(ISender mediator) : ApiController
    {
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("")]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest request)
        {
            var command = new CreateCampaignCommand(
                request.Name,
                request.ShortDescription,
                request.FullStory,
                request.TargetAmount,
                request.Type,
                request.UrgencyLevel,
                request.AllowComment,
                request.AllowDonate,
                request.TargetItems,
                request.StartTime,
                request.EndTime
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: createResult => Ok(createResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public async Task<IActionResult> GetCampaign(Guid id)
        {
            var query = new GetCampaignQuery(id);
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: campaignResult => Ok(campaignResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> GetCampaigns(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? name = null,
            [FromQuery] string? status = null,
            [FromQuery] string? type = null,
            [FromQuery] string? urgencyLevel = null,
            [FromQuery] Guid? ownerId = null,
            [FromQuery] string? description = null)
        {
            var query = new GetCampaignsQuery(
                Page: page,
                PageSize: pageSize,
                Name: name,
                Status: status,
                Type: type,
                UrgencyLevel: urgencyLevel,
                OwnerId: ownerId,
                Description: description);

            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: campaignsResult => Ok(campaignsResult),
                onFailure: Problem
            );
        }

        [HttpPut]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCampaign(Guid id, [FromBody] UpdateCampaignRequest request)
        {
            var command = new UpdateCampaignCommand(
                id,
                request.Name,
                request.ShortDescription,
                request.FullStory,
                request.TargetAmount,
                request.Type,
                request.UrgencyLevel,
                request.Status,
                request.AllowComment,
                request.AllowDonate,
                request.TargetItems,
                request.StartTime,
                request.EndTime
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: updateResult => Ok(updateResult),
                onFailure: Problem
            );
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireStaffOnly)]
        [Route("{id}/approve")]
        public async Task<IActionResult> ApproveCampaign(Guid id, [FromBody] ApproveCampaignRequest request)
        {
            var command = new ApproveCampaignCommand(
                CampaignId: id,
                Status: request.Status,
                RejectionReason: request.RejectionReason
            );
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: approveResult => Ok(approveResult),
                onFailure: Problem
            );
        }

        [HttpDelete]
        [Authorize(Policy = AuthorizationPolicies.RequireStaff)]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCampaign(Guid id)
        {
            var command = new DeleteCampaignCommand(id);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: deleteResult => Ok(deleteResult),
                onFailure: Problem
            );
        }
    }
}
