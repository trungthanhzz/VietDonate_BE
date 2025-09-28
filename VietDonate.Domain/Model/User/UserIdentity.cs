#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Domain.Common;

namespace VietDonate.Domain.Model.User
{
    public class UserIdentity(
        Guid id,
        string userName,
        string normalizedUserName,
        string passwordHash,
        bool isActive,
        string concurrenceStamp,
        string securityStamp,
        Guid roleId)
        : Entity(id)
    {
        public string UserName { get; } = userName;
        public string NormalizedUserName { get; } = normalizedUserName;
        public string PasswordHash { get; } = passwordHash;
        public bool IsActive { get; } = isActive;
        public string ConcurrenceStamp { get; } = concurrenceStamp;
        public string SecurityStamp { get; } = securityStamp;
        public DateTime CreatedDate { get; } = DateTime.UtcNow;

        public UserInformation? UserInformation { get; set; }
        public Guid RoleId { get; set; } = roleId;
        public Role Role { get; set; } = null!;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
