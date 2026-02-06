using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Posts;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Likes
{
    public class Like : Entity
    {
        public string Type { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid? PostId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public UserIdentity User { get; set; } = null!;
        public Post? Post { get; set; }

        public Like(Guid id, string type, Guid userId) : base(id)
        {
            Type = type;
            UserId = userId;
            CreatedTime = DateTime.UtcNow;
        }

        private Like()
        {
        }
    }
}

