using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetTempMedia
{
    public record GetTempMediaQuery() : IQuery<Result<GetTempMediaResult>>;
}
