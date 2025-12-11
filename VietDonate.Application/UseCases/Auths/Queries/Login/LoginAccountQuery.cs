using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Queries.Login
{
    public record LoginAccountQuery(
        string UserName,
        string Password,
        bool IsRemember = false)
        : IQuery<Result<LoginResult>>;
}
