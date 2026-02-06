using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Queries.GetCommentsByPost
{
    public class GetCommentsByPostQueryHandler(
        ICommentRepository commentRepository,
        IPostRepository postRepository)
        : IQueryHandler<GetCommentsByPostQuery, Result<GetCommentsByPostResult>>
    {
        public async Task<Result<GetCommentsByPostResult>> Handle(GetCommentsByPostQuery query, CancellationToken cancellationToken)
        {
            var post = await postRepository.GetByIdAsync(query.PostId, cancellationToken);
            if (post == null)
            {
                return Result.Failure<GetCommentsByPostResult>(GetCommentsByPostErrors.PostNotFound);
            }

            var comments = await commentRepository.GetByPostIdAsync(query.PostId, cancellationToken);
            var items = comments.Select(c => new CommentItem(
                Id: c.Id,
                Content: c.Content,
                UserId: c.UserId,
                UserName: c.User?.UserName,
                UserFullName: c.User?.UserInformation?.FullName,
                CreateTime: c.CreateTime
            )).ToList();

            return Result.Success(new GetCommentsByPostResult(
                PostId: query.PostId,
                Comments: items));
        }
    }
}

