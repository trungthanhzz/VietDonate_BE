using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;
using VietDonate.Domain.Model.User;

namespace VietDonate.Application.UseCases.Auths.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<ChangePasswordCommand, Result<ChangePasswordResult>>
    {
        public async Task<Result<ChangePasswordResult>> Handle(
            ChangePasswordCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = ValidateRequest(command);
            if (validationResult.IsFailure)
                return Result.Failure<ChangePasswordResult>(validationResult.Error);

            var userResult = await GetUserAsync(cancellationToken);
            if (userResult.IsFailure)
                return Result.Failure<ChangePasswordResult>(userResult.Error);

            var passwordValidationResult = ValidateOldPassword(userResult.Value, command.OldPassword);
            if (passwordValidationResult.IsFailure)
                return Result.Failure<ChangePasswordResult>(passwordValidationResult.Error);

            if (passwordHasher.VerifyPassword(command.NewPassword, userResult.Value.PasswordHash))
                return Result.Failure<ChangePasswordResult>(ChangePasswordErrors.SamePassword);

            return await UpdatePasswordAsync(userResult.Value, command.NewPassword, cancellationToken);
        }

        private Result ValidateRequest(ChangePasswordCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.OldPassword))
                return Result.Failure(ChangePasswordErrors.OldPasswordRequired);

            if (string.IsNullOrWhiteSpace(command.NewPassword))
                return Result.Failure(ChangePasswordErrors.NewPasswordRequired);

            return Result.Success();
        }

        private async Task<Result<UserIdentity>> GetUserAsync(CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
                return Result.Failure<UserIdentity>(ChangePasswordErrors.UserIdRequired);

            var user = await userRepository.GetByIdAsync(userId.Value, cancellationToken);
            if (user == null)
                return Result.Failure<UserIdentity>(ChangePasswordErrors.UserNotFound);

            return Result.Success(user);
        }

        private Result ValidateOldPassword(UserIdentity user, string oldPassword)
        {
            if (!passwordHasher.VerifyPassword(oldPassword, user.PasswordHash))
                return Result.Failure(ChangePasswordErrors.InvalidOldPassword);

            return Result.Success();
        }

        private async Task<Result<ChangePasswordResult>> UpdatePasswordAsync(
            UserIdentity user,
            string newPassword,
            CancellationToken cancellationToken)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var newPasswordHash = passwordHasher.HashPassword(newPassword);
                await userRepository.UpdatePasswordAsync(user, newPasswordHash, cancellationToken);

                return Result.Success(new ChangePasswordResult(SuccessMessages.Auth.PasswordChangeSuccessful));
            });
        }
    }
}
