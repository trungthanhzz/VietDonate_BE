using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserRole
{
    public record UpdateUserRoleCommand(
        Guid UserId,
        RoleType NewRole
    ) : ICommand<Result<UpdateUserRoleResult>>;
}
