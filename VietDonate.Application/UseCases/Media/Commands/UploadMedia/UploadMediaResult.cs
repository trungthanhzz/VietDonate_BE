namespace VietDonate.Application.UseCases.Media.Commands.UploadMedia
{
    public record UploadMediaResult(
        Guid MediaId,
        string FileName,
        string ContentType,
        string Url,
        long FileSize,
        bool IsTemp
    );
}
