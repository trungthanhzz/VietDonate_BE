using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler(
        ICommentRepository commentRepository,
        IPostRepository postRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<DeleteCommentCommand, Result<DeleteCommentResult>>
    {
        public async Task<Result<DeleteCommentResult>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<DeleteCommentResult>.ValidationFailure(DeleteCommentErrors.Unauthorized);
            }

            var comment = await commentRepository.GetByIdAsync(command.CommentId, cancellationToken);
            if (comment == null)
            {
                return Result.Failure<DeleteCommentResult>(DeleteCommentErrors.CommentNotFound);
            }

            if (comment.UserId != userId.Value)
            {
                return Result.Failure<DeleteCommentResult>(DeleteCommentErrors.Forbidden);
            }

            if (comment.PostId == null)
            {
                return Result.Failure<DeleteCommentResult>(DeleteCommentErrors.PostNotFound);
            }

            var post = await postRepository.GetByIdAsync(comment.PostId.Value, cancellationToken);
            if (post == null)
            {
                return Result.Failure<DeleteCommentResult>(DeleteCommentErrors.PostNotFound);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                await commentRepository.RemoveAsync(comment, cancellationToken);

                post.CommentCount = Math.Max(0, post.CommentCount - 1);
                await postRepository.UpdateAsync(post, cancellationToken);

                return Result.Success(new DeleteCommentResult(
                    CommentId: command.CommentId,
                    Message: SuccessMessages.Comment.DeletedSuccessfully));
            });
        }
    }
}

