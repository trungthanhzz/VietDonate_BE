using ErrorOr;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Auths.Commands.Logout
{
    public class LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IRedisService redisService,
        IRequestContextService requestContextService)
        : ICommandHandler<LogoutCommand, ErrorOr<LogoutResult>>
    {
        public async Task<ErrorOr<LogoutResult>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (!requestContextService.IsAuthenticated || string.IsNullOrEmpty(requestContextService.UserId))
                return Error.Unauthorized("Logout.Unauthorized", "User not authenticated or user ID not found");

            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            if (refreshToken == null)
                return Error.NotFound("Logout.InvalidToken", "Invalid refresh token");

            if (refreshToken.IsRevoked)
                return Error.Validation("Logout.TokenRevoked", "Refresh token has already been revoked");

            if (refreshToken.IsExpired)
                return Error.Validation("Logout.TokenExpired", "Refresh token has expired");

            if (refreshToken.UserId.ToString() != requestContextService.UserId)
                return Error.Forbidden("Logout.TokenMismatch", "Refresh token does not belong to the authenticated user");

            var jti = requestContextService.Jti;
            if (!string.IsNullOrEmpty(jti))
            {
                var blacklistKey = ObjectExtentions.GetKeyBlackListRedis(jti);
                var blacklistExpiry = requestContextService.GetJwtTtl();

                if (blacklistExpiry.HasValue && blacklistExpiry.Value > TimeSpan.Zero)
                {
                    var success = await redisService.SetAsync(blacklistKey, "revoked", blacklistExpiry.Value);
                    if (!success)
                        return Error.Failure("Logout.Failed", "Failed to add JWT token to blacklist");
                }
                else
                {
                    var success = await redisService.SetAsync(blacklistKey, "revoked", TimeSpan.FromMinutes(5));
                    if (!success)
                    {
                        _ = await redisService.RemoveAsync(blacklistKey);
                        return Error.Failure("Logout.Failed", "Failed to add JWT token to blacklist with fallback TTL");
                    }
                }
            }

            var revokeSuccess = await refreshTokenRepository.RevokeTokenAsync(request.RefreshToken);
            if (!revokeSuccess)
                return Error.Failure("Logout.Failed", "Failed to revoke refresh token");

            return new LogoutResult("Logout successful");
        }
    }
}
