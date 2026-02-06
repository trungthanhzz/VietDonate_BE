using Microsoft.Extensions.Logging;
using VietDonate.Application.Common.Handlers;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.UploadMedia
{
    public class UploadMediaCommandHandler(
        IStorageService storageService,
        ITempMediaService tempMediaService,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork,
        IFileValidationSettings validationSettings,
        ILogger<UploadMediaCommandHandler> logger)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<UploadMediaCommand, Result<UploadMediaResult>>
    {
        public async Task<Result<UploadMediaResult>> Handle(
            UploadMediaCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<UploadMediaResult>.ValidationFailure(UploadMediaErrors.Unauthorized);
            }

            var validationResult = ValidateUploadData(command);
            if (validationResult.IsFailure)
            {
                return Result<UploadMediaResult>.ValidationFailure(validationResult.Error);
            }

            try
            {
                var mediaId = Guid.NewGuid();
                var s3Key = await storageService.UploadAsync(
                    command.FileStream,
                    command.FileName,
                    command.ContentType,
                    folder: "temp",
                    cancellationToken);

                var tempKey = await tempMediaService.SaveTempMediaAsync(
                    userId.Value,
                    mediaId,
                    command.FileName,
                    command.ContentType,
                    s3Key,
                    command.FileSize,
                    cancellationToken);

                if (string.IsNullOrEmpty(tempKey))
                {
                    // Cleanup: delete from S3 if temp save failed
                    await storageService.DeleteAsync(s3Key, cancellationToken);
                    return Result<UploadMediaResult>.ValidationFailure(UploadMediaErrors.UploadFailed);
                }

                var url = await storageService.GetUrlAsync(s3Key);

                return Result.Success(new UploadMediaResult(
                    MediaId: mediaId,
                    FileName: command.FileName,
                    ContentType: command.ContentType,
                    Url: url,
                    FileSize: command.FileSize,
                    IsTemp: true
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading media. FileName: {FileName}", command.FileName);
                return Result<UploadMediaResult>.ValidationFailure(UploadMediaErrors.UploadFailed);
            }
        }

        private Result ValidateUploadData(UploadMediaCommand command)
        {
            if (command.FileStream == null || command.FileStream.Length == 0)
            {
                return Result.Failure(UploadMediaErrors.FileRequired);
            }

            if (string.IsNullOrWhiteSpace(command.FileName))
            {
                return Result.Failure(UploadMediaErrors.FileNameRequired);
            }

            if (string.IsNullOrWhiteSpace(command.ContentType))
            {
                return Result.Failure(UploadMediaErrors.ContentTypeRequired);
            }

            if (command.FileSize > validationSettings.MaxFileSizeBytes)
            {
                return Result.Failure(UploadMediaErrors.FileSizeExceeded);
            }

            if (!validationSettings.AllowedFileTypes.Contains(command.ContentType, StringComparer.OrdinalIgnoreCase))
            {
                return Result.Failure(UploadMediaErrors.InvalidFileType);
            }

            return Result.Success();
        }
    }
}
