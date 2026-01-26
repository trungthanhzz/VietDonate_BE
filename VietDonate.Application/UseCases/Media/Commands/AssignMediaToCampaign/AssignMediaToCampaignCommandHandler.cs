using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Media;

namespace VietDonate.Application.UseCases.Media.Commands.AssignMediaToCampaign
{
    public class AssignMediaToCampaignCommandHandler(
        ICampaignRepository campaignRepository,
        IMediaRepository mediaRepository,
        ITempMediaService tempMediaService,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<AssignMediaToCampaignCommand, Result<AssignMediaToCampaignResult>>
    {
        public async Task<Result<AssignMediaToCampaignResult>> Handle(
            AssignMediaToCampaignCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<AssignMediaToCampaignResult>.ValidationFailure(AssignMediaToCampaignErrors.Unauthorized);
            }

            // Validate campaign exists and user has permission
            var campaign = await campaignRepository.GetByIdAsync(command.CampaignId, cancellationToken);
            if (campaign == null)
            {
                return Result<AssignMediaToCampaignResult>.ValidationFailure(AssignMediaToCampaignErrors.CampaignNotFound);
            }

            // Check if user owns the campaign or is admin/staff
            if (campaign.OwnerId != userId.Value)
            {
                if (!requestContextService.HasAnyRole("Admin", "Staff"))
                {
                    return Result<AssignMediaToCampaignResult>.ValidationFailure(AssignMediaToCampaignErrors.Unauthorized);
                }
            }

            // Validate that we have media to assign
            var hasTempMedia = command.TempMediaIds != null && command.TempMediaIds.Count > 0;
            var hasExistingMedia = command.ExistingMediaIds != null && command.ExistingMediaIds.Count > 0;

            if (!hasTempMedia && !hasExistingMedia)
            {
                return Result<AssignMediaToCampaignResult>.ValidationFailure(AssignMediaToCampaignErrors.NoMediaToAssign);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                var assignedMediaIds = new List<Guid>();
                var displayOrder = 0;

                // Process temp media: Move from Redis to DB
                if (hasTempMedia)
                {
                    foreach (var tempMediaId in command.TempMediaIds!)
                    {
                        var tempMedia = await tempMediaService.GetTempMediaAsync(userId.Value, tempMediaId, cancellationToken);
                        if (tempMedia == null)
                        {
                            continue; // Skip if not found
                        }

                        // Create Media entity
                        var media = new Domain.Model.Media.Media(
                            id: tempMedia.MediaId,
                            type: GetMediaTypeFromContentType(tempMedia.ContentType),
                            path: tempMedia.S3Key
                        );

                        media.CampaignId = command.CampaignId;
                        media.UserId = userId.Value;
                        media.DisplayOrder = displayOrder++;
                        media.Status = "Active";
                        media.CreateTime = DateTime.UtcNow;

                        await mediaRepository.AddAsync(media, cancellationToken);
                        assignedMediaIds.Add(media.Id);

                        // Remove from temp storage
                        await tempMediaService.DeleteTempMediaAsync(userId.Value, tempMediaId, cancellationToken);
                    }
                }

                // Process existing media: Update CampaignId
                if (hasExistingMedia)
                {
                    foreach (var mediaId in command.ExistingMediaIds!)
                    {
                        var media = await mediaRepository.GetByIdAsync(mediaId, cancellationToken);
                        if (media == null)
                        {
                            continue; // Skip if not found
                        }

                        // Check if user owns the media
                        if (media.UserId != userId.Value)
                        {
                            if (!requestContextService.HasAnyRole("Admin", "Staff"))
                            {
                                continue; // Skip unauthorized media
                            }
                        }

                        media.CampaignId = command.CampaignId;
                        media.DisplayOrder = displayOrder++;
                        media.UpdateTime = DateTime.UtcNow;

                        await mediaRepository.UpdateAsync(media, cancellationToken);
                        assignedMediaIds.Add(media.Id);
                    }
                }

                if (assignedMediaIds.Count == 0)
                {
                    return Result<AssignMediaToCampaignResult>.ValidationFailure(AssignMediaToCampaignErrors.NoMediaToAssign);
                }

                return Result.Success(new AssignMediaToCampaignResult(
                    CampaignId: command.CampaignId,
                    AssignedCount: assignedMediaIds.Count,
                    AssignedMediaIds: assignedMediaIds,
                    Message: SuccessMessages.Media.AssignedSuccessfully
                ));
            });
        }

        private string GetMediaTypeFromContentType(string contentType)
        {
            if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return "Image";
            }
            if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            {
                return "Video";
            }
            return "File";
        }
    }
}
