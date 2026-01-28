using System.ComponentModel.DataAnnotations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Posts.Contracts
{
    public record CreatePostRequest(
        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Post title must be between 3 and 255 characters")]
        string Title,

        [Required]
        [StringLength(20000, MinimumLength = 1, ErrorMessage = "Post content must be between 1 and 20000 characters")]
        string Content,

        [Required]
        [StringLength(50, ErrorMessage = "Post type cannot exceed 50 characters")]
        string PostType,

        Guid? CampaignId,

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        string? Status,

        [StringLength(50, ErrorMessage = "Proof type cannot exceed 50 characters")]
        string? ProofType,

        DateTime? ProofDate
    );
}
