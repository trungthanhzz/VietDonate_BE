using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetCampaignMedia
{
    public record GetCampaignMediaQuery(Guid CampaignId) : IQuery<Result<GetCampaignMediaResult>>;
}
