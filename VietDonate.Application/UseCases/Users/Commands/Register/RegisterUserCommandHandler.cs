using ErrorOr;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Domain.Model.User;
using VietDonate.Application.Common.Handlers;
using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Users.Commands.Register
{
    public class RegisterUserCommandHandler : BaseCommandHandler, ICommandHandler<RegisterUserCommand, ErrorOr<RegisterUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
        }

        public async Task<ErrorOr<RegisterUserResult>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            // Validate that at least one contact method is provided
            if (string.IsNullOrWhiteSpace(command.Phone) && string.IsNullOrWhiteSpace(command.Email))
            {
                return Error.Validation("User.Registration", "At least one contact method (phone or email) is required.");
            }

            // Check if username already exists
            if (await _userRepository.UserNameExistsAsync(command.UserName, cancellationToken))
            {
                return Error.Conflict("User.Registration", "Username already exists.");
            }

            // Check if email already exists (if provided)
            if (!string.IsNullOrWhiteSpace(command.Email) && 
                await _userRepository.EmailExistsAsync(command.Email, cancellationToken))
            {
                return Error.Conflict("User.Registration", "Email already exists.");
            }

            // Check if phone already exists (if provided)
            if (!string.IsNullOrWhiteSpace(command.Phone) && 
                await _userRepository.PhoneExistsAsync(command.Phone, cancellationToken))
            {
                return Error.Conflict("User.Registration", "Phone number already exists.");
            }

            // Hash the password
            var passwordHash = _passwordHasher.HashPassword(command.Password);

            // Generate security stamps
            var securityStamp = Guid.NewGuid().ToString();
            var concurrenceStamp = Guid.NewGuid().ToString();

            // Generate a single ID for both entities
            var userId = Guid.NewGuid();
            var userRoleId = await _roleRepository.GetRoleIdByNameAsync(Roles.User, cancellationToken);
            // Create UserIdentity
            var userIdentity = new UserIdentity(
                id: userId,
                userName: command.UserName,
                normalizedUserName: command.UserName.ToUpperInvariant(),
                passwordHash: passwordHash,
                isActive: true,
                roleId: userRoleId,
                concurrenceStamp: concurrenceStamp,
                securityStamp: securityStamp
            );

            // Create UserInformation with the same ID
            var userInformation = new UserInformation(
                id: userId,
                fullName: command.FullName,
                phone: command.Phone ?? string.Empty,
                email: command.Email ?? string.Empty,
                address: command.Address,
                avtUrl: string.Empty
            );

            // Set up the relationship
            userIdentity.UserInformation = userInformation;
            userInformation.UserIdentity = userIdentity;

            return await ExecuteInTransactionAsync(async () =>
            {
                await _userRepository.AddAsync(userIdentity, cancellationToken);
                
                return new RegisterUserResult(
                    UserId: userIdentity.Id,
                    UserName: userIdentity.UserName,
                    FullName: userInformation.FullName,
                    Message: "User registered successfully"
                );
            });
        }
    }
} 