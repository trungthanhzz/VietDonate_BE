using System.ComponentModel.DataAnnotations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Users.Contracts
{
    public record UserRegistrationRequest(
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        string UserName,
        
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        string Password,
        
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        string FullName,
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        string? Phone,
        
        [EmailAddress(ErrorMessage = "Invalid email format")]
        string? Email,
        
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters")]
        string Address
    )
    {
        public bool IsValid => !string.IsNullOrWhiteSpace(Phone) || !string.IsNullOrWhiteSpace(Email);
    }
}
