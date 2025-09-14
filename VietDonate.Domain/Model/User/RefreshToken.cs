using System;
using VietDonate.Domain.Common;

namespace VietDonate.Domain.Model.User
{
    public class RefreshToken(
        Guid id,
        Guid userId,
        string token,
        DateTime expiresAt,
        bool isRemember,
        string? deviceInfo = null)
        : Entity(id)
    {
        public Guid UserId { get; } = userId;
        public string Token { get; } = token;
        public DateTime ExpiresAt { get; } = expiresAt;
        public bool IsRemember { get; } = isRemember;
        public string? DeviceInfo { get; } = deviceInfo;
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; private set; }
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public UserIdentity UserIdentity { get; set; } = null!;

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}

