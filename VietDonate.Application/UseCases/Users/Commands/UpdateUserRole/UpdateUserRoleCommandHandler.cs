using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;
using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommandHandler(
        IUserRepository userRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<UpdateUserRoleCommand, Result<UpdateUserRoleResult>>
    {
        public async Task<Result<UpdateUserRoleResult>> Handle(
            UpdateUserRoleCommand command,
            CancellationToken cancellationToken)
        {
            if (!requestContextService.HasRole(nameof(RoleType.Admin)))
            {
                return Result<UpdateUserRoleResult>.ValidationFailure(UpdateUserRoleErrors.Unauthorized);
            }

            var currentUserId = requestContextService.GetUserIdAsGuid();
            if (currentUserId == null)
            {
                return Result<UpdateUserRoleResult>.ValidationFailure(UpdateUserRoleErrors.Unauthorized);
            }

            // Cannot change own role
            if (currentUserId.Value == command.UserId)
            {
                return Result<UpdateUserRoleResult>.ValidationFailure(UpdateUserRoleErrors.CannotChangeOwnRole);
            }

            // Validate role: Cannot change to Guest or Admin
            if (command.NewRole == RoleType.Guest || command.NewRole == RoleType.Admin)
            {
                return Result<UpdateUserRoleResult>.ValidationFailure(UpdateUserRoleErrors.InvalidRole);
            }

            // Get the user to update
            var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user == null)
            {
                return Result<UpdateUserRoleResult>.ValidationFailure(UpdateUserRoleErrors.UserNotFound);
            }

            // Check if role is already the same
            if (user.RoleType == command.NewRole)
            {
                return Result<UpdateUserRoleResult>.ValidationFailure(UpdateUserRoleErrors.SameRole);
            }

            var oldRole = user.RoleType;

            return await ExecuteInTransactionAsync(async () =>
            {
                // Update the role
                await userRepository.UpdateRoleAsync(user, command.NewRole, cancellationToken);

                return Result.Success(new UpdateUserRoleResult(
                    UserId: user.Id,
                    OldRole: oldRole,
                    NewRole: command.NewRole,
                    Message: SuccessMessages.User.RoleUpdateSuccessful
                ));
            });
        }
    }
}
