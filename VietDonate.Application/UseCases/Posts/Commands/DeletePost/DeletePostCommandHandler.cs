using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Common;

namespace VietDonate.Application.UseCases.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler(
        IPostRepository postRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<DeletePostCommand, Result<DeletePostResult>>
    {
        public async Task<Result<DeletePostResult>> Handle(
            DeletePostCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<DeletePostResult>.ValidationFailure(DeletePostErrors.Unauthorized);
            }

            var post = await postRepository.GetByIdAsync(command.PostId, cancellationToken);
            if (post == null)
            {
                return Result<DeletePostResult>.ValidationFailure(DeletePostErrors.PostNotFound);
            }

            if (!CanDelete(requestContextService, post.UserId, userId.Value))
            {
                return Result<DeletePostResult>.ValidationFailure(DeletePostErrors.Forbidden);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                await postRepository.RemoveAsync(post, cancellationToken);

                return Result.Success(new DeletePostResult(
                    PostId: command.PostId,
                    Message: SuccessMessages.Post.DeletedSuccessfully));
            });
        }

        private static bool CanDelete(
            IRequestContextService requestContextService,
            Guid postOwnerId,
            Guid requesterId)
        {
            if (postOwnerId == requesterId)
            {
                return true;
            }

            return requestContextService.HasAnyRole(nameof(RoleType.Admin), nameof(RoleType.Staff));
        }
    }
}

