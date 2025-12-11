using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.User;
using VietDonate.Application.Common.Handlers;

namespace VietDonate.Application.UseCases.Users.Commands.Register
{
    public class RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<RegisterUserCommand,
                Result<RegisterUserResult>>
    {
        public async Task<Result<RegisterUserResult>> Handle(
            RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRegistrationDataAsync(command, cancellationToken);
            if (validationResult.IsFailure)
                return Result<RegisterUserResult>.ValidationFailure(validationResult.Error);

            var userEntities = CreateUserEntities(command);

            return await ExecuteInTransactionAsync(async () =>
            {
                await userRepository.AddAsync(userEntities.UserIdentity, cancellationToken);

                return Result.Success(new RegisterUserResult(
                    UserId: userEntities.UserIdentity.Id,
                    UserName: userEntities.UserIdentity.UserName,
                    FullName: userEntities.UserInformation.FullName,
                    Message: SuccessMessages.User.RegistrationSuccessful
                ));
            });
        }

        private async Task<Result> ValidateRegistrationDataAsync(RegisterUserCommand command, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Phone) && 
                string.IsNullOrWhiteSpace(command.Email))
                return Result.Failure(RegisterUserErrors.ContactMethodRequired);

            if (await userRepository.UserNameExistsAsync(command.UserName, cancellationToken))
                return Result.Failure(RegisterUserErrors.UsernameExists);

            if (!string.IsNullOrWhiteSpace(command.Email) &&
                await userRepository.EmailExistsAsync(command.Email, cancellationToken))
                return Result.Failure(RegisterUserErrors.EmailExists);

            if (!string.IsNullOrWhiteSpace(command.Phone) &&
                await userRepository.PhoneExistsAsync(command.Phone, cancellationToken))
                return Result.Failure(RegisterUserErrors.PhoneExists);

            return Result.Success();
        }

        private (UserIdentity UserIdentity, UserInformation UserInformation) CreateUserEntities(RegisterUserCommand command)
        {
            var passwordHash = passwordHasher.HashPassword(command.Password);
            var securityStamp = Guid.NewGuid().ToString();
            var concurrenceStamp = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid();

            var userIdentity = new UserIdentity(
                id: userId,
                userName: command.UserName,
                normalizedUserName: command.UserName.ToUpperInvariant(),
                passwordHash: passwordHash,
                isActive: true,
                concurrenceStamp: concurrenceStamp,
                securityStamp: securityStamp
            );

            var userInformation = new UserInformation(
                id: userId,
                fullName: command.FullName,
                phone: command.Phone ?? string.Empty,
                email: command.Email ?? string.Empty,
                address: command.Address,
                avtUrl: string.Empty
            );

            userIdentity.UserInformation = userInformation;
            userInformation.UserIdentity = userIdentity;

            return (userIdentity, userInformation);
        }
    }
}