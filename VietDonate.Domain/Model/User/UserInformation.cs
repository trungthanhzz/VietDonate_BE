using VietDonate.Domain.Common;

namespace VietDonate.Domain.Model.User
{
    public class UserInformation(
        Guid id,
        string fullName,
        string phone,
        string email,
        string address,
        string avtUrl)
        : Entity(id)
    {
        public string FullName { get; } = fullName;
        public string Phone { get; } = phone;
        public string Email { get; } = email;
        public string Address { get; } = address;
        public string AvtUrl { get; } = avtUrl;
        public DateTime CreateTime { get; } = DateTime.UtcNow;
        public DateTime? DateOfBirth { get; set; }
        public string? Status { get; set; }
        public string? VerificationStatus { get; set; }
        public string? IdentityNumber { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationTaxCode { get; set; }
        public string? OrganizationRegisterNumber { get; set; }
        public string? OrganizationLegalRepresentative { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankBranch { get; set; }
        public decimal TotalDonated { get; set; } = 0;
        public decimal TotalRecieved { get; set; } = 0;
        public int CampaignCount { get; set; } = 0;
        public string? StaffNumber { get; set; }
        public DateTime? UpdateTime { get; set; }

        public UserIdentity UserIdentity { get; set; }
    }
}
