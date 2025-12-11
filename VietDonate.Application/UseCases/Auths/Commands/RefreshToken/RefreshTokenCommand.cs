using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : ICommand<Result<RefreshTokenResult>>;
} 
