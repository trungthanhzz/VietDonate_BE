using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetAllCampaigns
{
    public record GetAllCampaignsQuery() : IQuery<Result<GetAllCampaignsResult>>;
}
