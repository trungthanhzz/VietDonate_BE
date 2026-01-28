using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Comments;

namespace VietDonate.Application.UseCases.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        IPostRepository postRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<CreateCommentCommand, Result<CreateCommentResult>>
    {
        private const string PostCommentType = "post";

        public async Task<Result<CreateCommentResult>> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<CreateCommentResult>.ValidationFailure(CreateCommentErrors.Unauthorized);
            }

            if (string.IsNullOrWhiteSpace(command.Content))
            {
                return Result.Failure<CreateCommentResult>(CreateCommentErrors.ContentRequired);
            }

            var post = await postRepository.GetByIdAsync(command.PostId, cancellationToken);
            if (post == null)
            {
                return Result.Failure<CreateCommentResult>(CreateCommentErrors.PostNotFound);
            }

            if (command.ParentId != null)
            {
                var parentComment = await commentRepository.GetByIdAsync(command.ParentId.Value, cancellationToken);
                if (parentComment == null || parentComment.PostId != command.PostId)
                {
                    return Result.Failure<CreateCommentResult>(CreateCommentErrors.ParentCommentNotFound);
                }
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                var commentId = Guid.NewGuid();
                var comment = new Comment(commentId, command.Content.Trim(), PostCommentType, userId.Value)
                {
                    PostId = command.PostId,
                    ParentId = command.ParentId,
                    CreateTime = DateTime.UtcNow
                };

                await commentRepository.AddAsync(comment, cancellationToken);

                post.CommentCount += 1;
                await postRepository.UpdateAsync(post, cancellationToken);

                return Result.Success(new CreateCommentResult(
                    CommentId: commentId,
                    Message: SuccessMessages.Comment.CreatedSuccessfully));
            });
        }
    }
}

