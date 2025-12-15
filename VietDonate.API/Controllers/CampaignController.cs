using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController(IMediator _mediator) : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignAsync(Guid id)
        {
            var query = new GetCampaignQuery(id);
            var result = await _mediator.Send(query);
            return result.Match(
                campaign => Ok(campaign),
                Problem);
        }


    }
}
