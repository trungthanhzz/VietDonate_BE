using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.DeleteMedia
{
    public class DeleteMediaCommandHandler(
        IMediaRepository mediaRepository,
        IStorageService storageService,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<DeleteMediaCommand, Result<DeleteMediaResult>>
    {
        public async Task<Result<DeleteMediaResult>> Handle(
            DeleteMediaCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<DeleteMediaResult>.ValidationFailure(DeleteMediaErrors.Unauthorized);
            }

            var media = await mediaRepository.GetByIdAsync(command.MediaId, cancellationToken);
            if (media == null)
            {
                return Result<DeleteMediaResult>.ValidationFailure(DeleteMediaErrors.MediaNotFound);
            }

            // Check if user owns the media or is admin/staff
            if (media.UserId != userId.Value)
            {
                if (!requestContextService.HasAnyRole("Admin", "Staff"))
                {
                    return Result<DeleteMediaResult>.ValidationFailure(DeleteMediaErrors.Unauthorized);
                }
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                // Delete from S3
                var deleted = await storageService.DeleteAsync(media.Path, cancellationToken);
                if (!deleted)
                {
                    // Log warning but continue with DB deletion
                }

                // Delete from database
                await mediaRepository.DeleteAsync(media, cancellationToken);

                return Result.Success(new DeleteMediaResult(
                    MediaId: command.MediaId,
                    Message: SuccessMessages.Media.DeletedSuccessfully
                ));
            });
        }
    }
}
