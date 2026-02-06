using System.ComponentModel.DataAnnotations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Campaigns.Contracts
{
    public record UpdateCampaignRequest(
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Campaign name must be between 3 and 200 characters")]
        string? Name,
        
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Short description must be between 10 and 500 characters")]
        string? ShortDescription,
        
        [StringLength(5000, ErrorMessage = "Full story cannot exceed 5000 characters")]
        string? FullStory,
        
        [Range(0, double.MaxValue, ErrorMessage = "Target amount must be greater than or equal to 0")]
        decimal? TargetAmount,
        
        [StringLength(50, ErrorMessage = "Campaign type cannot exceed 50 characters")]
        string? Type,
        
        [StringLength(50, ErrorMessage = "Urgency level cannot exceed 50 characters")]
        string? UrgencyLevel,
        
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        string? Status,
        
        bool? AllowComment,
        bool? AllowDonate,
        
        string? TargetItems,
        DateTime? StartTime,
        DateTime? EndTime
    );
}
