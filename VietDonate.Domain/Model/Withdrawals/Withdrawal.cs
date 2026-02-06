using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Withdrawals
{
    public class Withdrawal : Entity
    {
        public Guid CampaignId { get; set; }
        public decimal Amount { get; set; }
        public DateTime WithdrawalDate { get; set; } = DateTime.UtcNow.Date;
        public string Purpose { get; set; } = null!;
        public string? Status { get; set; }
        public Guid RequestedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public Guid? RejectedBy { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedTime { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public DateTime? CompletedTime { get; set; }

        public Campaign Campaign { get; set; } = null!;
        public UserIdentity RequestedByUser { get; set; } = null!;
        public UserIdentity? ApprovedByUser { get; set; }
        public UserIdentity? RejectedByUser { get; set; }

        public Withdrawal(Guid id, Guid campaignId, decimal amount, DateTime withdrawalDate, string purpose, Guid requestedBy) : base(id)
        {
            CampaignId = campaignId;
            Amount = amount;
            WithdrawalDate = withdrawalDate;
            Purpose = purpose;
            RequestedBy = requestedBy;
            CreatedTime = DateTime.UtcNow;
        }

        private Withdrawal()
        {
        }
    }
}

