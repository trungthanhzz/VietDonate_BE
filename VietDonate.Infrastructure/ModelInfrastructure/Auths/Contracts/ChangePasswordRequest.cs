using System.ComponentModel.DataAnnotations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Auths.Contracts
{
    public record ChangePasswordRequest(
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Old password must be at least 6 characters")]
        string OldPassword,
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters")]
        string NewPassword);
}
