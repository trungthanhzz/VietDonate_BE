using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Queries.GetCommentsByPost
{
    public record GetCommentsByPostQuery(Guid PostId) : IQuery<Result<GetCommentsByPostResult>>;
}

