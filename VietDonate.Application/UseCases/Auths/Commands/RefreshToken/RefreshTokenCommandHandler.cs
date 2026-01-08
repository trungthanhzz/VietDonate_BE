using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork), ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResult>>
    {
        private const int RememberMeRefreshTokenDays = 30;
        private const int DefaultRefreshTokenDays = 0;

        public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenResult = await GetAndValidateRefreshTokenAsync(request.RefreshToken);
            if (refreshTokenResult.IsFailure)
                return Result<RefreshTokenResult>.ValidationFailure(refreshTokenResult.Error);

            var refreshToken = refreshTokenResult.Value;

            var userValidationResult = await ValidateUserAsync(refreshToken.UserId, cancellationToken);
            if (userValidationResult.IsFailure)
                return Result<RefreshTokenResult>.ValidationFailure(userValidationResult.Error);

            var user = userValidationResult.Value;

            return await ExecuteTokenRefreshTransactionAsync(request.RefreshToken, user, refreshToken.IsRemember);
        }

        private async Task<Result<Domain.Model.User.RefreshToken>> GetAndValidateRefreshTokenAsync(string token)
        {
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(token);
            var validationResult = await ValidateRefreshTokenAsync(refreshToken);
            
            if (validationResult.IsFailure)
                return Result.Failure<Domain.Model.User.RefreshToken>(validationResult.Error);

            return Result.Success(refreshToken);
        }

        private async Task<Result<RefreshTokenResult>> ExecuteTokenRefreshTransactionAsync(
            string oldRefreshToken, 
            Domain.Model.User.UserIdentity user, 
            bool isRemember)
        {
            return (Result<RefreshTokenResult>)await ExecuteInTransactionAsync(async () =>
            {
                var revokeResult = await RevokeOldRefreshTokenAsync(oldRefreshToken);
                if (revokeResult.IsFailure)
                    return revokeResult;

                var newTokensResult = await GenerateNewTokensAsync(user, isRemember);
                if (newTokensResult.IsFailure)
                    return newTokensResult;

                return CreateRefreshTokenResult(newTokensResult.Value);
            });
        }

        private async Task<Result> RevokeOldRefreshTokenAsync(string refreshToken)
        {
            var revokeSuccess = await refreshTokenRepository.RevokeTokenAsync(refreshToken);
            if (!revokeSuccess)
                return Result.Failure(RefreshTokenErrors.RevokeFailed);

            return Result.Success();
        }

        private Result<RefreshTokenResult> CreateRefreshTokenResult((string AccessToken, string RefreshToken) tokens)
        {
            var accessTokenExpirationSeconds = jwtTokenGenerator.GetAccessTokenExpirationInMinutes() * 60;
            
            return Result.Success(new RefreshTokenResult(
                AccessToken: tokens.AccessToken,
                RefreshToken: tokens.RefreshToken,
                ExpireDate: accessTokenExpirationSeconds));
        }

        private async Task<Result> ValidateRefreshTokenAsync(Domain.Model.User.RefreshToken token)
        {
            if (token == null)
                return Result.Failure(RefreshTokenErrors.RefreshTokenNotFound);

            if (token.IsRevoked)
                return Result.Failure(RefreshTokenErrors.RefreshTokenRevoked);

            if (token.IsExpired)
                return Result.Failure(RefreshTokenErrors.RefreshTokenExpired);

            return Result.Success();
        }

        private async Task<Result<Domain.Model.User.UserIdentity>> ValidateUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                return Result.Failure<Domain.Model.User.UserIdentity>(RefreshTokenErrors.UserNotFound);

            if (!user.IsActive)
                return Result.Failure<Domain.Model.User.UserIdentity>(RefreshTokenErrors.UserInactive);

            return Result.Success(user);
        }

        private async Task<Result<(string AccessToken, string RefreshToken)>> GenerateNewTokensAsync(
            Domain.Model.User.UserIdentity user, 
            bool isRemember)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshTokenResult = await CreateRefreshTokenAsync(user, isRemember);
            
            if (refreshTokenResult.IsFailure)
                return Result.Failure<(string, string)>(refreshTokenResult.Error);

            return Result.Success((accessToken, refreshTokenResult.Value));
        }

        private string GenerateAccessToken(Domain.Model.User.UserIdentity user)
        {
            return jwtTokenGenerator.GenerateToken(
                id: user.Id,
                jti: Guid.NewGuid(), 
                permissions: new List<string>(),
                roles: new List<string> { user.RoleType.ToString() }
            );
        }

        private async Task<Result<string>> CreateRefreshTokenAsync(Domain.Model.User.UserIdentity user, bool isRemember)
        {
            var newRefreshToken = Guid.NewGuid().ToString();
            var expirationDays = isRemember ? RememberMeRefreshTokenDays : DefaultRefreshTokenDays;
            var newRefreshTokenExpiresAt = DateTime.UtcNow.AddDays(expirationDays);

            var newRefreshTokenEntity = new Domain.Model.User.RefreshToken(
                id: Guid.NewGuid(),
                userId: user.Id,
                token: newRefreshToken,
                expiresAt: newRefreshTokenExpiresAt,
                isRemember: isRemember);

            var (createdToken, createSuccess) = await refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            if (!createSuccess)
                return Result.Failure<string>(RefreshTokenErrors.CreateTokenFailed);

            return Result.Success(createdToken.Token);
        }
    }
} 
