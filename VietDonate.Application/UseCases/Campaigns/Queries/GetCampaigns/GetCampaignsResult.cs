namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaigns
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
        Guid OwnerId,
        string OwnerName
    );

    public record PaginationMetadata(
        int Page,
        int PageSize,
        int TotalCount,
        int TotalPages,
        bool HasPreviousPage,
        bool HasNextPage
    );

    public record GetCampaignsResult(
        List<CampaignItem> Campaigns,
        PaginationMetadata Pagination
    );
}
