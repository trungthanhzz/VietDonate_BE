using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Posts
{
    public class Post : Entity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string PostType { get; set; } = null!; // 'update', 'proof', 'fact', 'news'
        public string? Status { get; set; }
        public Guid UserId { get; set; }
        public Guid? CampaignId { get; set; }
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public string? ProofType { get; set; }
        public DateTime? ProofDate { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTime { get; set; }

        public UserIdentity User { get; set; } = null!;
        public Campaign? Campaign { get; set; }

        public Post(Guid id, string title, string content, string postType, Guid userId) : base(id)
        {
            Title = title;
            Content = content;
            PostType = postType;
            UserId = userId;
            CreateTime = DateTime.UtcNow;
        }

        private Post()
        {
        }
    }
}

