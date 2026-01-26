using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.UpdateMedia
{
    public class UpdateMediaCommandHandler(
        IMediaRepository mediaRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<UpdateMediaCommand, Result<UpdateMediaResult>>
    {
        public async Task<Result<UpdateMediaResult>> Handle(
            UpdateMediaCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<UpdateMediaResult>.ValidationFailure(UpdateMediaErrors.Unauthorized);
            }

            var media = await mediaRepository.GetByIdAsync(command.MediaId, cancellationToken);
            if (media == null)
            {
                return Result<UpdateMediaResult>.ValidationFailure(UpdateMediaErrors.MediaNotFound);
            }

            // Check if user owns the media or is admin/staff
            if (media.UserId != userId.Value)
            {
                if (!requestContextService.HasAnyRole("Admin", "Staff"))
                {
                    return Result<UpdateMediaResult>.ValidationFailure(UpdateMediaErrors.Unauthorized);
                }
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                if (command.DisplayOrder.HasValue)
                {
                    media.DisplayOrder = command.DisplayOrder.Value;
                }

                if (command.Status != null)
                {
                    media.Status = command.Status;
                }

                media.UpdateTime = DateTime.UtcNow;

                await mediaRepository.UpdateAsync(media, cancellationToken);

                return Result.Success(new UpdateMediaResult(
                    MediaId: media.Id,
                    DisplayOrder: media.DisplayOrder,
                    Status: media.Status,
                    Message: SuccessMessages.Media.UpdatedSuccessfully
                ));
            });
        }
    }
}
