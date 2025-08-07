using ErrorOr;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.Application.UseCases.Users.Commands.Register
{
    public record RegisterUserCommand(
        string UserName,
        string Password,
        string FullName,
        string Phone,
        string Email,
        string Address
    ) : ICommand<ErrorOr<RegisterUserResult>>;
} 