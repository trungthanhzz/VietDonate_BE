using VietDonate.Domain.Common;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Notifications
{
    public class Notification : Entity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedTime { get; set; }

        public UserIdentity User { get; set; } = null!;

        public Notification(Guid id, Guid userId, string title, string content) : base(id)
        {
            UserId = userId;
            Title = title;
            Content = content;
            CreatedTime = DateTime.UtcNow;
        }

        private Notification()
        {
        }
    }
}

