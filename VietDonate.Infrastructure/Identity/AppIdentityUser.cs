using Microsoft.AspNetCore.Identity;

namespace VietDonate.Infrastructure.Identity
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
} 