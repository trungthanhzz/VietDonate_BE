using ErrorOr;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.Application.UseCases.Auths.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : ICommand<ErrorOr<RefreshTokenResult>>;
} 
