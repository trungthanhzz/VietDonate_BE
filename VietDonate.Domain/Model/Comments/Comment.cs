using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Posts;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Comments
{
    public class Comment : Entity
    {
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsHidden { get; set; } = false;
        public string? ReasonHiden { get; set; }
        public Guid? HidentBy { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTime { get; set; }

        public UserIdentity User { get; set; } = null!;
        public Post? Post { get; set; }
        public Comment? Parent { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        public UserIdentity? HiddenByUser { get; set; }

        public Comment(Guid id, string content, string type, Guid userId) : base(id)
        {
            Content = content;
            Type = type;
            UserId = userId;
            CreateTime = DateTime.UtcNow;
        }

        private Comment()
        {
        }
    }
}

