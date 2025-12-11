using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Auths.Commands.Logout
{
    public class LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IRedisService redisService,
        IRequestContextService requestContextService)
        : ICommandHandler<LogoutCommand, Result<LogoutResult>>
    {
        public async Task<Result<LogoutResult>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userValidationResult = ValidateUserContext();
            if (userValidationResult.IsFailure)
                return userValidationResult;

            var tokenValidationResult = await ValidateRefreshTokenAsync(request.RefreshToken);
            if (tokenValidationResult.IsFailure)
                return tokenValidationResult;

            var blacklistResult = await BlacklistJwtTokenAsync();
            if (blacklistResult.IsFailure)
                return blacklistResult;

            var revokeResult = await RevokeRefreshTokenAsync(request.RefreshToken);
            if (revokeResult.IsFailure)
                return revokeResult;

            return Result.Success(new LogoutResult(SuccessMessages.Auth.LogoutSuccessful));
        }

        private Result<LogoutResult> ValidateUserContext()
        {
            if (string.IsNullOrEmpty(requestContextService.UserId))
                return Result.Failure<LogoutResult>(LogoutErrors.UserIdRequired);

            return Result.Success(new LogoutResult(SuccessMessages.Auth.UserContextValid));
        }

        private async Task<Result<LogoutResult>> ValidateRefreshTokenAsync(string refreshToken)
        {
            var token = await refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (token == null)
                return Result.Failure<LogoutResult>(LogoutErrors.RefreshTokenNotFound);

            if (token.IsExpired || token.IsRevoked)
                return Result.Failure<LogoutResult>(LogoutErrors.RefreshTokenNotValid);

            if (token.UserId.ToString() != requestContextService.UserId)
                return Result.Failure<LogoutResult>(LogoutErrors.TokenMismatch);

            return Result.Success(new LogoutResult(SuccessMessages.Auth.RefreshTokenValid));
        }

        private async Task<Result<LogoutResult>> BlacklistJwtTokenAsync()
        {
            var jti = requestContextService.Jti;
            if (string.IsNullOrEmpty(jti))
                return Result.Success(new LogoutResult(SuccessMessages.Auth.NoJwtToBlacklist));

            var blacklistKey = ObjectExtentions.GetKeyBlackListRedis(jti);
            var blacklistExpiry = requestContextService.GetJwtTtl();

            var expiry = CalculateBlacklistExpiry(blacklistExpiry);

            var success = await redisService.SetAsync(blacklistKey, "revoked", expiry);
            if (!success)
            {
                await CleanupBlacklistKeyAsync(blacklistKey);
                return Result.Failure<LogoutResult>(LogoutErrors.BlacklistFailed);
            }

            return Result.Success(new LogoutResult(SuccessMessages.Auth.JwtTokenBlacklisted));
        }

        private async Task<Result<LogoutResult>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var revokeSuccess = await refreshTokenRepository.RevokeTokenAsync(refreshToken);
            if (!revokeSuccess)
                return Result.Failure<LogoutResult>(LogoutErrors.RevokeFailed);

            return Result.Success(new LogoutResult(SuccessMessages.Auth.RefreshTokenRevoked));
        }

        private static TimeSpan CalculateBlacklistExpiry(TimeSpan? blacklistExpiry)
        {
            return blacklistExpiry.HasValue && blacklistExpiry.Value > TimeSpan.Zero 
                ? blacklistExpiry.Value 
                : TimeSpan.FromMinutes(5);
        }

        private async Task CleanupBlacklistKeyAsync(string blacklistKey)
        {
            _ = await redisService.RemoveAsync(blacklistKey);
        }
    }
}
