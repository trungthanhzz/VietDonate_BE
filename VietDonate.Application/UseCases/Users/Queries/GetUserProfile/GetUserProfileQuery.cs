using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Queries.GetUserProfile
{
    public record GetUserProfileQuery() : IQuery<Result<GetUserProfileResult>>;
}

