using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetTopEndingCampaigns
{
    public record GetTopEndingCampaignsQuery(
        int Count = 10) : IQuery<Result<GetTopEndingCampaignsResult>>;
}

