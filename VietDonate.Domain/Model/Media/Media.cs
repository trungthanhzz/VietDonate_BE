using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Domain.Model.Posts;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Media
{
    public class Media : Entity
    {
        public string Type { get; set; } = null!;
        public string? Status { get; set; }
        public Guid? PostId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CampaignId { get; set; }
        public string Path { get; set; } = null!;
        public int DisplayOrder { get; set; } = 0;
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTime { get; set; }

        public Post? Post { get; set; }
        public UserIdentity? User { get; set; }
        public Campaign? Campaign { get; set; }

        public Media(Guid id, string type, string path) : base(id)
        {
            Type = type;
            Path = path;
            CreateTime = DateTime.UtcNow;
        }

        private Media()
        {
        }
    }
}

