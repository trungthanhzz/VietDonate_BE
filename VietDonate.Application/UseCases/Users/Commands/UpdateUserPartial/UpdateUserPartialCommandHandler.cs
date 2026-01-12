using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;

namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserPartial
{
    public class UpdateUserPartialCommandHandler(
        IUserRepository userRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<UpdateUserPartialCommand, Result<UpdateUserPartialResult>>
    {
        public async Task<Result<UpdateUserPartialResult>> Handle(
            UpdateUserPartialCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<UpdateUserPartialResult>.ValidationFailure(UpdateUserPartialErrors.Unauthorized);
            }

            var user = await userRepository.GetByIdAsync(userId.Value, cancellationToken);
            if (user == null || user.UserInformation == null)
            {
                return Result<UpdateUserPartialResult>.ValidationFailure(UpdateUserPartialErrors.UserNotFound);
            }

            // Check if there are any fields to update
            if (!HasFieldsToUpdate(command))
            {
                return Result<UpdateUserPartialResult>.ValidationFailure(UpdateUserPartialErrors.NoFieldsToUpdate);
            }

            var validationResult = await ValidatePartialUpdateDataAsync(command, user, cancellationToken);
            if (validationResult.IsFailure)
            {
                return Result<UpdateUserPartialResult>.ValidationFailure(validationResult.Error);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                UpdateUserInformationPartial(user.UserInformation, command);
                
                // Update get-only properties from primary constructor (only if provided)
                await userRepository.UpdateUserInformationPropertiesAsync(
                    user.UserInformation,
                    command.FullName,
                    command.Phone,
                    command.Email,
                    command.Address,
                    command.AvtUrl,
                    cancellationToken);
                
                await userRepository.UpdateAsync(user, cancellationToken);

                return Result.Success(new UpdateUserPartialResult(
                    UserId: user.Id,
                    Message: SuccessMessages.User.UpdateSuccessful
                ));
            });
        }

        private bool HasFieldsToUpdate(UpdateUserPartialCommand command)
        {
            return command.FullName != null ||
                   command.Phone != null ||
                   command.Email != null ||
                   command.Address != null ||
                   command.AvtUrl != null ||
                   command.DateOfBirth.HasValue ||
                   command.Status != null ||
                   command.VerificationStatus != null ||
                   command.IdentityNumber != null ||
                   command.OrganizationName != null ||
                   command.OrganizationTaxCode != null ||
                   command.OrganizationRegisterNumber != null ||
                   command.OrganizationLegalRepresentative != null ||
                   command.BankAccountNumber != null ||
                   command.BankName != null ||
                   command.BankBranch != null ||
                   command.StaffNumber != null;
        }

        private async Task<Result> ValidatePartialUpdateDataAsync(
            UpdateUserPartialCommand command,
            Domain.Model.User.UserIdentity user,
            CancellationToken cancellationToken)
        {
            // Check if after update, at least one contact method exists
            var newPhone = command.Phone ?? user.UserInformation?.Phone;
            var newEmail = command.Email ?? user.UserInformation?.Email;

            if (string.IsNullOrWhiteSpace(newPhone) && string.IsNullOrWhiteSpace(newEmail))
            {
                return Result.Failure(UpdateUserPartialErrors.ContactMethodRequired);
            }

            // Check email uniqueness if email is being changed
            if (command.Email != null && 
                command.Email != user.UserInformation?.Email &&
                await userRepository.EmailExistsAsync(command.Email, cancellationToken))
            {
                return Result.Failure(UpdateUserPartialErrors.EmailExists);
            }

            // Check phone uniqueness if phone is being changed
            if (command.Phone != null && 
                command.Phone != user.UserInformation?.Phone &&
                await userRepository.PhoneExistsAsync(command.Phone, cancellationToken))
            {
                return Result.Failure(UpdateUserPartialErrors.PhoneExists);
            }

            return Result.Success();
        }

        private void UpdateUserInformationPartial(
            Domain.Model.User.UserInformation userInfo,
            UpdateUserPartialCommand command)
        {
            // Update properties that have setters (only if provided)
            if (command.DateOfBirth.HasValue)
                userInfo.DateOfBirth = command.DateOfBirth;

            if (command.Status != null)
                userInfo.Status = command.Status;

            if (command.VerificationStatus != null)
                userInfo.VerificationStatus = command.VerificationStatus;

            if (command.IdentityNumber != null)
                userInfo.IdentityNumber = command.IdentityNumber;

            if (command.OrganizationName != null)
                userInfo.OrganizationName = command.OrganizationName;

            if (command.OrganizationTaxCode != null)
                userInfo.OrganizationTaxCode = command.OrganizationTaxCode;

            if (command.OrganizationRegisterNumber != null)
                userInfo.OrganizationRegisterNumber = command.OrganizationRegisterNumber;

            if (command.OrganizationLegalRepresentative != null)
                userInfo.OrganizationLegalRepresentative = command.OrganizationLegalRepresentative;

            if (command.BankAccountNumber != null)
                userInfo.BankAccountNumber = command.BankAccountNumber;

            if (command.BankName != null)
                userInfo.BankName = command.BankName;

            if (command.BankBranch != null)
                userInfo.BankBranch = command.BankBranch;

            if (command.StaffNumber != null)
                userInfo.StaffNumber = command.StaffNumber;

            userInfo.UpdateTime = DateTime.UtcNow;

            // For get-only properties from primary constructor (FullName, Phone, Email, Address, AvtUrl)
            // These will be updated through EF Core Entry API in the repository
        }
    }
}

