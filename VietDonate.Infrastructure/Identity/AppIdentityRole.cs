using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace VietDonate.Infrastructure.Identity
{
    public class AppIdentityRole : IdentityRole<Guid>
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
