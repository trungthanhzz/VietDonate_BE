using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetMedia
{
    public record GetMediaQuery(Guid MediaId) : IQuery<Result<GetMediaResult>>;
}
