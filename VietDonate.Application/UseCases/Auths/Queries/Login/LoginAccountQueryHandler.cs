using ErrorOr;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Domain.Model.User;

namespace VietDonate.Application.UseCases.Auths.Queries.Login
{
    public class LoginAccountQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork) : IQueryHandler<LoginAccountQuery, ErrorOr<LoginResult>>
    {
        public async Task<ErrorOr<LoginResult>> Handle(LoginAccountQuery query, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUserNameAsync(query.UserName, cancellationToken);
            
            if (user == null)
            {
                return Error.NotFound("User.Login", "User not found.");
            }
            
            if (!user.IsActive)
            {
                return Error.Validation("User.Login", "User account is deactivated.");
            }
            
            if (!passwordHasher.VerifyPassword(query.Password, user.PasswordHash))
            {
                return Error.Validation("User.Login", "Invalid password.");
            }
            
            var accessToken = jwtTokenGenerator.GenerateToken(
                id: user.Id,
                jti: Guid.NewGuid(),        
                permissions: new List<string>(),
                roles: new List<string>()
            );
            
            await unitOfWork.BeginTransactionAsync();
            
            try
            {
                var refreshTokenValue = Guid.NewGuid().ToString();
                var refreshTokenExpiresAt = query.IsRemember 
                    ? DateTime.UtcNow.AddDays(30)
                    : DateTime.UtcNow.AddDays(7);
                
                var refreshToken = new RefreshToken(
                    id: Guid.NewGuid(),
                    userId: user.Id,
                    token: refreshTokenValue,
                    expiresAt: refreshTokenExpiresAt,
                    isRemember: query.IsRemember);
                
                var (createdToken, success) = await refreshTokenRepository.AddAsync(refreshToken);
                
                if (!success)
                {
                    await unitOfWork.RollbackAsync();
                    return Error.Failure("User.Login", "Failed to create refresh token");
                }
                
                await unitOfWork.CommitAsync();
                
                return new LoginResult(
                    AccessToken: accessToken,
                    RefreshToken: createdToken.Token,
                    ExpireDate: 3600);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return Error.Failure("User.Login", ex.Message);
            }
            finally
            {
                await unitOfWork.DisposeAsync();
            }
        }
    }
}
