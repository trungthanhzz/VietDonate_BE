using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Donations
{
    public class Donation : Entity
    {
        public Guid CampaignId { get; set; }
        public Guid? DonorId { get; set; }
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? GoodsDetails { get; set; }
        public string? Message { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTime { get; set; }

        public Campaign Campaign { get; set; } = null!;
        public UserIdentity? Donor { get; set; }

        public Donation(Guid id, Guid campaignId, string type, decimal amount) : base(id)
        {
            CampaignId = campaignId;
            Type = type;
            Amount = amount;
            CreateTime = DateTime.UtcNow;
        }

        private Donation()
        {
        }
    }
}

