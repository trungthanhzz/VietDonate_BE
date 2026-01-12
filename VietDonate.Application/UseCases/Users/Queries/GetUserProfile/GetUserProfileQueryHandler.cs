using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler(
        IRequestContextService requestContextService,
        IUserRepository userRepository) : IQueryHandler<GetUserProfileQuery, Result<GetUserProfileResult>>
    {
        public async Task<Result<GetUserProfileResult>> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result.Failure<GetUserProfileResult>(GetUserProfileErrors.Unauthorized);
            }

            var user = await userRepository.GetByIdAsync(userId.Value, cancellationToken);
            if (user == null)
            {
                return Result.Failure<GetUserProfileResult>(GetUserProfileErrors.UserNotFound);
            }

            var userInfo = user.UserInformation;
            if (userInfo == null)
            {
                return Result.Failure<GetUserProfileResult>(GetUserProfileErrors.UserNotFound);
            }

            var result = new GetUserProfileResult(
                Id: user.Id,
                UserName: user.UserName,
                FullName: userInfo.FullName,
                Email: userInfo.Email,
                Phone: userInfo.Phone,
                Address: userInfo.Address,
                AvtUrl: userInfo.AvtUrl,
                CreatedDate: user.CreatedDate,
                DateOfBirth: userInfo.DateOfBirth,
                Status: userInfo.Status,
                VerificationStatus: userInfo.VerificationStatus,
                TotalDonated: userInfo.TotalDonated,
                TotalRecieved: userInfo.TotalRecieved,
                CampaignCount: userInfo.CampaignCount,
                Role: requestContextService.Roles.FirstOrDefault() ?? string.Empty);

            return Result.Success(result);
        }
    }
}

