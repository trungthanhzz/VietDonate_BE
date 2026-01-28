using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Queries.GetPostsByCampaign
{
    public record GetPostsByCampaignQuery(Guid CampaignId) : IQuery<Result<GetPostsByCampaignResult>>;
}
