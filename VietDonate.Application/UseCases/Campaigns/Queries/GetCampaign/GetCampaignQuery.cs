using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public record GetCampaignQuery(Guid CampaignId) : IQuery<Result<Campaign>>;
}
