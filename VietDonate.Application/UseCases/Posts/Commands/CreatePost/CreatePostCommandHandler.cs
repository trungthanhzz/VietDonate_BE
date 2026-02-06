using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Posts;

namespace VietDonate.Application.UseCases.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler(
        IPostRepository postRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<CreatePostCommand, Result<CreatePostResult>>
    {
        private static readonly HashSet<string> AllowedPostTypes =
        [
            "update",
            "proof",
            "fact",
            "news"
        ];

        public async Task<Result<CreatePostResult>> Handle(
            CreatePostCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<CreatePostResult>.ValidationFailure(CreatePostErrors.Unauthorized);
            }

            var validationResult = Validate(command);
            if (validationResult.IsFailure)
            {
                return Result<CreatePostResult>.ValidationFailure(validationResult.Error!);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                var postId = Guid.NewGuid();
                var post = new Post(
                    id: postId,
                    title: command.Title.Trim(),
                    content: command.Content,
                    postType: command.PostType.Trim(),
                    userId: userId.Value);

                post.CampaignId = command.CampaignId;
                post.Status = command.Status;
                post.ProofType = command.ProofType;
                post.ProofDate = command.ProofDate;
                post.CreateTime = DateTime.UtcNow;

                await postRepository.AddAsync(post, cancellationToken);

                return Result.Success(new CreatePostResult(
                    PostId: postId,
                    Message: SuccessMessages.Post.CreatedSuccessfully));
            });
        }

        private static Result Validate(CreatePostCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Title))
            {
                return Result.Failure(CreatePostErrors.TitleRequired);
            }

            if (string.IsNullOrWhiteSpace(command.Content))
            {
                return Result.Failure(CreatePostErrors.ContentRequired);
            }

            if (string.IsNullOrWhiteSpace(command.PostType))
            {
                return Result.Failure(CreatePostErrors.PostTypeRequired);
            }

            if (!AllowedPostTypes.Contains(command.PostType.Trim()))
            {
                return Result.Failure(CreatePostErrors.InvalidPostType);
            }

            return Result.Success();
        }
    }
}
