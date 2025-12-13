using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public record GetCampaignQuery(string CampaignId) : IQuery<Result<Campaign>>;
}
