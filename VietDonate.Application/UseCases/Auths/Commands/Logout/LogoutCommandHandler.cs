using ErrorOr;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.Application.UseCases.Auths.Commands.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, ErrorOr<LogoutResult>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<ErrorOr<LogoutResult>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var success = await _refreshTokenRepository.RevokeTokenAsync(request.RefreshToken);
            
            if (success)
            {
                return new LogoutResult("Logout successful");
            }
            else
            {
                return Error.Failure("Logout.Failed", "Failed to revoke refresh token");
            }
        }
    }
}
