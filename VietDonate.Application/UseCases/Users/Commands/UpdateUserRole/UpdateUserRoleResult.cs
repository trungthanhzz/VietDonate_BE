using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserRole
{
    public record UpdateUserRoleResult(
        Guid UserId,
        RoleType OldRole,
        RoleType NewRole,
        string Message
    );
}
