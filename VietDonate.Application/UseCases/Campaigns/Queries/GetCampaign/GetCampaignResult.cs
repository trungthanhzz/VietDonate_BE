namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public record GetCampaignResult(
        Guid Id,
        string Code,
        string Name,
        DateTime CreatedDate,
        string ShortDescription,
        string? FullStory,
        decimal? TargetAmount,
        decimal? CurrentAmount,
        string Type,
        string UrgencyLevel,
        string Status,
        bool AllowComment,
        bool AllowDonate,
        string? TargetItems,
        string? CurrentItems,
        DateTime? StartTime,
        DateTime? EndTime,
        DateTime? ApprovedTime,
        DateTime? CompletedTime,
        string? VerificationNote,
        string? RejectionReason,
        string? FactCheckNote,
        int ViewCount,
        int DonorCount,
        DateTime? CreateTime,
        DateTime? UpdateTime,
        Guid OwnerId
    );
}
