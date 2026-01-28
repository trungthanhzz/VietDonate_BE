using System.ComponentModel.DataAnnotations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Campaigns.Contracts
{
    public record ApproveCampaignRequest(
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Approved|Rejected)$", ErrorMessage = "Status must be either 'Approved' or 'Rejected'")]
        string Status,
        
        [StringLength(1000, ErrorMessage = "Rejection reason cannot exceed 1000 characters")]
        string? RejectionReason = null
    );
}
