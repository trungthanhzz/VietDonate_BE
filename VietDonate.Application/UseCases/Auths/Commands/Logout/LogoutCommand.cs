using ErrorOr;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.Application.UseCases.Auths.Commands.Logout
{
    public record LogoutCommand(string RefreshToken) : ICommand<ErrorOr<LogoutResult>>;
}
