using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;
namespace VietDonate.Application.UseCases.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository userRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<UpdateUserCommand, Result<UpdateUserResult>>
    {
        public async Task<Result<UpdateUserResult>> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<UpdateUserResult>.ValidationFailure(UpdateUserErrors.Unauthorized);
            }

            var user = await userRepository.GetByIdAsync(userId.Value, cancellationToken);
            if (user == null || user.UserInformation == null)
            {
                return Result<UpdateUserResult>.ValidationFailure(UpdateUserErrors.UserNotFound);
            }

            var validationResult = await ValidateUpdateDataAsync(command, user, cancellationToken);
            if (validationResult.IsFailure)
            {
                return Result<UpdateUserResult>.ValidationFailure(validationResult.Error);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                UpdateUserInformation(user.UserInformation, command);
                
                var phoneToUpdate = string.IsNullOrWhiteSpace(command.Phone) 
                    ? null 
                    : command.Phone;
                var emailToUpdate = string.IsNullOrWhiteSpace(command.Email) 
                    ? null 
                    : command.Email;
                
                await userRepository.UpdateUserInformationPropertiesAsync(
                    user.UserInformation,
                    command.FullName,
                    phoneToUpdate,
                    emailToUpdate,
                    command.Address,
                    command.AvtUrl,
                    cancellationToken);
                
                await userRepository.UpdateAsync(user, cancellationToken);

                return Result.Success(new UpdateUserResult(
                    UserId: user.Id,
                    Message: SuccessMessages.User.UpdateSuccessful
                ));
            });
        }

        private async Task<Result> ValidateUpdateDataAsync(
            UpdateUserCommand command,
            Domain.Model.User.UserIdentity user,
            CancellationToken cancellationToken)
        {
            // Determine the final Phone and Email values after update
            // If command provides null/empty, keep the existing value
            var finalPhone = string.IsNullOrWhiteSpace(command.Phone) 
                ? user.UserInformation?.Phone 
                : command.Phone;
            var finalEmail = string.IsNullOrWhiteSpace(command.Email) 
                ? user.UserInformation?.Email 
                : command.Email;

            // After update, user must have at least one contact method
            if (string.IsNullOrWhiteSpace(finalPhone) && string.IsNullOrWhiteSpace(finalEmail))
            {
                return Result.Failure(UpdateUserErrors.ContactMethodRequired);
            }

            // Check email uniqueness only if email is being changed
            if (!string.IsNullOrWhiteSpace(command.Email) && 
                command.Email != user.UserInformation?.Email &&
                await userRepository.EmailExistsAsync(command.Email, cancellationToken))
            {
                return Result.Failure(UpdateUserErrors.EmailExists);
            }

            // Check phone uniqueness only if phone is being changed
            if (!string.IsNullOrWhiteSpace(command.Phone) && 
                command.Phone != user.UserInformation?.Phone &&
                await userRepository.PhoneExistsAsync(command.Phone, cancellationToken))
            {
                return Result.Failure(UpdateUserErrors.PhoneExists);
            }

            return Result.Success();
        }

        private void UpdateUserInformation(
            Domain.Model.User.UserInformation userInfo,
            UpdateUserCommand command)
        {
            userInfo.DateOfBirth = ConvertToUtc(command.DateOfBirth);
            userInfo.Status = command.Status;
            userInfo.VerificationStatus = command.VerificationStatus;
            userInfo.IdentityNumber = command.IdentityNumber;
            userInfo.OrganizationName = command.OrganizationName;
            userInfo.OrganizationTaxCode = command.OrganizationTaxCode;
            userInfo.OrganizationRegisterNumber = command.OrganizationRegisterNumber;
            userInfo.OrganizationLegalRepresentative = command.OrganizationLegalRepresentative;
            userInfo.BankAccountNumber = command.BankAccountNumber;
            userInfo.BankName = command.BankName;
            userInfo.BankBranch = command.BankBranch;
            userInfo.StaffNumber = command.StaffNumber;
            userInfo.UpdateTime = DateTime.UtcNow;
        }

        private static DateTime? ConvertToUtc(DateTime? dateTime)
        {
            if (dateTime == null) return null;
            
            return dateTime.Value.Kind switch
            {
                DateTimeKind.Utc => dateTime.Value,
                DateTimeKind.Local => dateTime.Value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) // Unspecified -> treat as UTC
            };
        }
    }
}

