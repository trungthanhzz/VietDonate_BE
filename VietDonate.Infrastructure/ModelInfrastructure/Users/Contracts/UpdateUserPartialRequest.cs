using System.ComponentModel.DataAnnotations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Users.Contracts
{
    public record UpdateUserPartialRequest(
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        string? FullName,
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        string? Phone,
        
        [EmailAddress(ErrorMessage = "Invalid email format")]
        string? Email,
        
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters")]
        string? Address,
        
        [StringLength(1000, ErrorMessage = "Avatar URL must not exceed 1000 characters")]
        string? AvtUrl,
        
        DateTime? DateOfBirth,
        
        [StringLength(50, ErrorMessage = "Status must not exceed 50 characters")]
        string? Status,
        
        [StringLength(50, ErrorMessage = "Verification status must not exceed 50 characters")]
        string? VerificationStatus,
        
        [StringLength(50, ErrorMessage = "Identity number must not exceed 50 characters")]
        string? IdentityNumber,
        
        [StringLength(255, ErrorMessage = "Organization name must not exceed 255 characters")]
        string? OrganizationName,
        
        [StringLength(50, ErrorMessage = "Organization tax code must not exceed 50 characters")]
        string? OrganizationTaxCode,
        
        [StringLength(50, ErrorMessage = "Organization register number must not exceed 50 characters")]
        string? OrganizationRegisterNumber,
        
        [StringLength(255, ErrorMessage = "Organization legal representative must not exceed 255 characters")]
        string? OrganizationLegalRepresentative,
        
        [StringLength(50, ErrorMessage = "Bank account number must not exceed 50 characters")]
        string? BankAccountNumber,
        
        [StringLength(255, ErrorMessage = "Bank name must not exceed 255 characters")]
        string? BankName,
        
        [StringLength(255, ErrorMessage = "Bank branch must not exceed 255 characters")]
        string? BankBranch,
        
        [StringLength(50, ErrorMessage = "Staff number must not exceed 50 characters")]
        string? StaffNumber
    );
}

