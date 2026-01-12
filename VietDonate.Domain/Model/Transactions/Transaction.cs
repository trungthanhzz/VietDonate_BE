using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Donations;

namespace VietDonate.Domain.Model.Transactions
{
    public class Transaction : Entity
    {
        public Guid? DonationId { get; set; }
        public string Gateway { get; set; } = null!; // 'momo', 'vnpay', 'bank_transfer'
        public string? GatewayTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public string? FailureReason { get; set; }
        public string? GatewayResponse { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedTime { get; set; }

        public Donation? Donation { get; set; }

        public Transaction(Guid id, string gateway, decimal amount) : base(id)
        {
            Gateway = gateway;
            Amount = amount;
            CreatedTime = DateTime.UtcNow;
        }

        private Transaction()
        {
        }
    }
}

