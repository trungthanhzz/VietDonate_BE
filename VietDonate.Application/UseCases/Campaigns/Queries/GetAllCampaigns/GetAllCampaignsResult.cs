namespace VietDonate.Application.UseCases.Campaigns.Queries.GetAllCampaigns
{
    public record CampaignItem(
        Guid Id,
        string Code,
        string Name,
        DateTime CreatedDate,
        string ShortDescription,
        decimal? TargetAmount,
        decimal? CurrentAmount,
        string Type,
        string UrgencyLevel,
        string Status,
        int ViewCount,
        int DonorCount,
        Guid OwnerId
    );

    public record GetAllCampaignsResult(List<CampaignItem> Campaigns);
}
