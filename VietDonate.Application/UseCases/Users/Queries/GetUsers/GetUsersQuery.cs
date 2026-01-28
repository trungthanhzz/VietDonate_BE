using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Users.Queries.GetUsers
{
    public record GetUsersQuery(
        int Page,
        int PageSize,
        RoleType? Role = null,
        string? Email = null,
        string? Name = null) : IQuery<Result<GetUsersResult>>;
}
