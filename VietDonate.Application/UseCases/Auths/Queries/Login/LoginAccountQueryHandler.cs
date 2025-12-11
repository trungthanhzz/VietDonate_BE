using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.User;


namespace VietDonate.Application.UseCases.Auths.Queries.Login
{
    public class LoginAccountQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork) : IQueryHandler<LoginAccountQuery, Result<LoginResult>>
    {
        public async Task<Result<LoginResult>> Handle(LoginAccountQuery query, CancellationToken cancellationToken)
        {
            var user = await GetUserByUsernameAsync(query.UserName, cancellationToken);
            if (user.IsFailure)
                return Result.Failure<LoginResult>(user.Error);

            var validationResult = ValidateUserCredentials(user.Value, query.Password);
            if (validationResult.IsFailure)
                return Result.Failure<LoginResult>(validationResult.Error);

            var accessToken = jwtTokenGenerator.GenerateToken(
                id: user.Value.Id,
                jti: Guid.NewGuid(),
                permissions: new List<string>(),
                roles: new List<string>()
            );

            return await CreateRefreshTokenInTransactionAsync(user.Value, query.IsRemember, accessToken);
        }

        private async Task<Result<UserIdentity>> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUserNameAsync(username, cancellationToken);
            if (user == null)
                return Result.Failure<UserIdentity>(LoginErrors.UserNotFound);

            return Result.Success(user);
        }

        private Result ValidateUserCredentials(UserIdentity user, string password)
        {
            if (!user.IsActive)
                return Result.Failure(LoginErrors.UserInactive);

            if (!passwordHasher.VerifyPassword(password, user.PasswordHash))
                return Result.Failure(LoginErrors.InvalidPassword);

            return Result.Success();
        }

        private async Task<Result<LoginResult>> CreateRefreshTokenInTransactionAsync(
            UserIdentity user,
            bool isRemember,
            string accessToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var refreshTokenValue = Guid.NewGuid().ToString();
                var refreshTokenExpiresAt = isRemember
                    ? DateTime.UtcNow.AddDays(30)
                    : DateTime.UtcNow.AddDays(7);

                var refreshToken = new RefreshToken(
                    id: Guid.NewGuid(),
                    userId: user.Id,
                    token: refreshTokenValue,
                    expiresAt: refreshTokenExpiresAt,
                    isRemember: isRemember);

                var (createdToken, success) = await refreshTokenRepository.AddAsync(refreshToken);

                if (!success)
                {
                    await unitOfWork.RollbackAsync();
                    return Result.Failure<LoginResult>(LoginErrors.CreateRefreshTokenFailed);
                }

                await unitOfWork.CommitAsync();

                return Result.Success(new LoginResult(
                    AccessToken: accessToken,
                    RefreshToken: createdToken.Token,
                    ExpireDate: 3600));
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return Result.Failure<LoginResult>(LoginErrors.CreateRefreshTokenFailed);
            }
            finally
            {
                await unitOfWork.DisposeAsync();
            }
        }
    }
}
