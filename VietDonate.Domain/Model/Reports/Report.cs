using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Reports
{
    public class Report : Entity
    {
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Status { get; set; }
        public Guid ReposterId { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? ResolvedBy { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedTime { get; set; }

        public UserIdentity Reposter { get; set; } = null!;
        public Campaign? Campaign { get; set; }
        public UserIdentity? ResolvedByUser { get; set; }

        public Report(Guid id, string type, string description, Guid reposterId) : base(id)
        {
            Type = type;
            Description = description;
            ReposterId = reposterId;
            CreatedTime = DateTime.UtcNow;
        }

        private Report()
        {
        }
    }
}

