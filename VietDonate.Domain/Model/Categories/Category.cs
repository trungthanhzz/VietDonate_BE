using VietDonate.Domain.Common;
using VietDonate.Domain.Model.User;

namespace VietDonate.Domain.Model.Categories
{
    public class Category : Entity
    {
        public string Name { get; set; } = null!;
        public string? Status { get; set; }
        public string Type { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? OtherInfo { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTime { get; set; }

        public UserIdentity? CreatedByUser { get; set; }

        public Category(Guid id, string name, string type) : base(id)
        {
            Name = name;
            Type = type;
            CreatedTime = DateTime.UtcNow;
        }

        private Category()
        {
        }
    }
}

