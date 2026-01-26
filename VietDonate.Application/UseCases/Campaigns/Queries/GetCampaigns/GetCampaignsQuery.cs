using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaigns
{
    public record GetCampaignsQuery(
        int Page,
        int PageSize,
        string? Name = null,
        string? Status = null,
        string? Type = null,
        string? UrgencyLevel = null,
        Guid? OwnerId = null,
        string? Description = null) : IQuery<Result<GetCampaignsResult>>;
}
