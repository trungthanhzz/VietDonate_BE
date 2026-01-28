using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Likes;

namespace VietDonate.Application.UseCases.Likes.Commands.ToggleLikePost
{
    public class ToggleLikePostCommandHandler(
        ILikeRepository likeRepository,
        IPostRepository postRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<ToggleLikePostCommand, Result<ToggleLikePostResult>>
    {
        private const string PostLikeType = "post";

        public async Task<Result<ToggleLikePostResult>> Handle(ToggleLikePostCommand command, CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<ToggleLikePostResult>.ValidationFailure(ToggleLikePostErrors.Unauthorized);
            }

            var post = await postRepository.GetByIdAsync(command.PostId, cancellationToken);
            if (post == null)
            {
                return Result.Failure<ToggleLikePostResult>(ToggleLikePostErrors.PostNotFound);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                var existingLike = await likeRepository.GetByUserAndPostAsync(userId.Value, command.PostId, cancellationToken);
                if (existingLike == null)
                {
                    var like = new Like(Guid.NewGuid(), PostLikeType, userId.Value)
                    {
                        PostId = command.PostId
                    };

                    await likeRepository.AddAsync(like, cancellationToken);

                    post.LikeCount += 1;
                    await postRepository.UpdateAsync(post, cancellationToken);

                    return Result.Success(new ToggleLikePostResult(
                        PostId: command.PostId,
                        IsLiked: true,
                        LikeCount: post.LikeCount,
                        Message: SuccessMessages.Like.LikedSuccessfully));
                }

                await likeRepository.RemoveAsync(existingLike, cancellationToken);

                post.LikeCount = Math.Max(0, post.LikeCount - 1);
                await postRepository.UpdateAsync(post, cancellationToken);

                return Result.Success(new ToggleLikePostResult(
                    PostId: command.PostId,
                    IsLiked: false,
                    LikeCount: post.LikeCount,
                    Message: SuccessMessages.Like.UnlikedSuccessfully));
            });
        }
    }
}

