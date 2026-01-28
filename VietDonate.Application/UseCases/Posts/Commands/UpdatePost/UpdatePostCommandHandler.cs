using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandHandler(
        IPostRepository postRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<UpdatePostCommand, Result<UpdatePostResult>>
    {
        private static readonly HashSet<string> AllowedPostTypes =
        [
            "update",
            "proof",
            "fact",
            "news"
        ];

        public async Task<Result<UpdatePostResult>> Handle(
            UpdatePostCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<UpdatePostResult>.ValidationFailure(UpdatePostErrors.Unauthorized);
            }

            var post = await postRepository.GetByIdAsync(command.PostId, cancellationToken);
            if (post == null)
            {
                return Result<UpdatePostResult>.ValidationFailure(UpdatePostErrors.PostNotFound);
            }

            if (post.UserId != userId.Value)
            {
                return Result<UpdatePostResult>.ValidationFailure(UpdatePostErrors.Forbidden);
            }

            var validationResult = Validate(command);
            if (validationResult.IsFailure)
            {
                return Result<UpdatePostResult>.ValidationFailure(validationResult.Error!);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                ApplyUpdates(post, command);
                post.UpdateTime = DateTime.UtcNow;

                await postRepository.UpdateAsync(post, cancellationToken);

                return Result.Success(new UpdatePostResult(
                    PostId: post.Id,
                    Message: SuccessMessages.Post.UpdatedSuccessfully));
            });
        }

        private static Result Validate(UpdatePostCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.PostType))
            {
                return Result.Success();
            }

            if (!AllowedPostTypes.Contains(command.PostType.Trim()))
            {
                return Result.Failure(UpdatePostErrors.InvalidPostType);
            }

            return Result.Success();
        }

        private static void ApplyUpdates(
            Domain.Model.Posts.Post post,
            UpdatePostCommand command)
        {
            if (!string.IsNullOrWhiteSpace(command.Title))
            {
                post.Title = command.Title.Trim();
            }

            if (!string.IsNullOrWhiteSpace(command.Content))
            {
                post.Content = command.Content;
            }

            if (!string.IsNullOrWhiteSpace(command.PostType))
            {
                post.PostType = command.PostType.Trim();
            }

            if (command.CampaignId.HasValue)
            {
                post.CampaignId = command.CampaignId.Value;
            }

            if (command.Status != null)
            {
                post.Status = command.Status;
            }

            if (command.ProofType != null)
            {
                post.ProofType = command.ProofType;
            }

            if (command.ProofDate.HasValue)
            {
                post.ProofDate = command.ProofDate;
            }
        }
    }
}
