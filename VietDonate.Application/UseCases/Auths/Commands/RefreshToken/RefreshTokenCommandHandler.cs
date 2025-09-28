using ErrorOr;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Handlers;

namespace VietDonate.Application.UseCases.Auths.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : BaseCommandHandler, ICommandHandler<RefreshTokenCommand, ErrorOr<RefreshTokenResult>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Validate refresh token
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            
            if (refreshToken == null)
            {
                return Error.NotFound("RefreshToken.NotFound", "Invalid refresh token");
            }

            if (refreshToken.IsRevoked)
            {
                return Error.Validation("RefreshToken.Revoked", "Refresh token has been revoked");
            }

            if (refreshToken.IsExpired)
            {
                return Error.Validation("RefreshToken.Expired", "Refresh token has expired");
            }

            // Get user
            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
            if (user == null)
            {
                return Error.NotFound("User.NotFound", "User not found");
            }

            if (!user.IsActive)
            {
                return Error.Validation("User.Inactive", "User account is deactivated");
            }

            // Execute in transaction
            return await ExecuteInTransactionAsync(async () =>
            {
                // Revoke the old refresh token
                var revokeSuccess = await _refreshTokenRepository.RevokeTokenAsync(request.RefreshToken);
                if (!revokeSuccess)
                {
                    throw new InvalidOperationException("Failed to revoke old refresh token");
                }

                // Generate new access token
                var accessToken = _jwtTokenGenerator.GenerateToken(
                    id: user.Id,
                    firstName: user.UserInformation?.FullName ?? string.Empty,
                    lastName: string.Empty,
                    email: user.UserInformation?.Email ?? string.Empty,
                    permissions: new List<string>(),
                    roles: user.Role.Name
                );

                // Generate new refresh token
                var newRefreshToken = Guid.NewGuid().ToString();
                var newRefreshTokenExpiresAt = refreshToken.IsRemember 
                    ? DateTime.UtcNow.AddDays(30)
                    : DateTime.UtcNow.AddDays(0);

                // Create new refresh token record
                var newRefreshTokenEntity = new Domain.Model.User.RefreshToken(
                    id: Guid.NewGuid(),
                    userId: user.Id,
                    token: newRefreshToken,
                    expiresAt: newRefreshTokenExpiresAt,
                    isRemember: refreshToken.IsRemember);

                var (createdToken, createSuccess) = await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
                if (!createSuccess)
                {
                    throw new InvalidOperationException("Failed to create new refresh token");
                }

                return new RefreshTokenResult(
                    AccessToken: accessToken,
                    RefreshToken: createdToken.Token,
                    ExpireDate: 3600);
            });
        }
    }
} 
