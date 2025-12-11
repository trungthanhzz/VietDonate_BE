using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Commands.Logout
{
    public record LogoutCommand(string RefreshToken) : ICommand<Result<LogoutResult>>;
}
