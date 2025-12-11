using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Commands.Register
{
    public record RegisterUserCommand(
        string UserName,
        string Password,
        string FullName,
        string Phone,
        string Email,
        string Address
    ) : ICommand<Result<RegisterUserResult>>;
} 