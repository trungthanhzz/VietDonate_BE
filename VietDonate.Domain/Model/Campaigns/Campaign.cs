using VietDonate.Domain.Common;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Campaigns
{
    public class Campaign : Entity
    {
        public string Code { get; } = null!;
        public string Name { get; } = null!;
        public DateTime CreatedDate { get; }

        public string ShortDescription { get; set; } = null!;
        public string? FullStory { get; set; }
        public decimal? TargetAmount { get; set; }
        public decimal? CurrentAmount { get; set; }
        public string Type { get; set; } = null!;
        public string UrgencyLevel { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool AllowComment { get; set; }
        public bool AllowDonate { get; set; }
        public string? TargetItems { get; set; }
        public string? CurrentItems { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public string? VerificationNote { get; set; }
        public string? RejectionReason { get; set; }
        public string? FactCheckNote { get; set; }
        public int ViewCount { get; set; } = 0;
        public int DonorCount { get; set; } = 0;
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public Guid? ApprovedId { get; set; }
        public Guid OwnerId { get; set; }

        public UserIdentity? ApprovedByUser { get; set; }
        public UserIdentity OwnerUser { get; set; } = null!;

        public Campaign(Guid id,
                        string name,
                        string code,
                        DateTime createdDate) : base(id)
        {
            Code = code;
            Name = name;
            CreatedDate = DateTime.UtcNow;
        }

        private Campaign()
        {
            
        }
    }
}
