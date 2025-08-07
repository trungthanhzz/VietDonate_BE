using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietDonate.Infrastructure.ModelInfrastructure.Auths.Contracts
{
    public record LoginRequest(
        [Required]
        string UserName,
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        string Password,
        bool IsRemember = false);
}
