using System.ComponentModel.DataAnnotations;

namespace VietDonate.Infrastructure.ModelInfrastructure.Users.Contracts
{
    public record UpdateUserRoleRequest(
        [Required]
        string NewRole
    );
}
