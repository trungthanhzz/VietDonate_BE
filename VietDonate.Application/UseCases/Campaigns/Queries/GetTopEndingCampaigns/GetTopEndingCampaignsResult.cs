using VietDonate.Application.UseCases.Campaigns.Queries.GetCampaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetTopEndingCampaigns
{
    public record DashboardCampaignCreator(
        Guid Id,
        string Name,
        string AvatarUrl);

    public record DashboardCampaignItem(
        Guid Id,
        string Code,
        string Name,
        string ImageUrl,
        DateTime? StartTime,
        DateTime? EndTime,
        decimal? TargetAmount,
        decimal? CurrentAmount,
        decimal PercentAchieved,
        int DonorCount,
        DashboardCampaignCreator Creator);

    public record GetTopEndingCampaignsResult(
        List<DashboardCampaignItem> Campaigns);
}

