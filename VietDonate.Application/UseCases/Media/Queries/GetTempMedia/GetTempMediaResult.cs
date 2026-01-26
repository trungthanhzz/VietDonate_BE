namespace VietDonate.Application.UseCases.Media.Queries.GetTempMedia
{
    public record GetTempMediaResult(
        List<TempMediaItem> TempMediaItems
    );

    public record TempMediaItem(
        Guid MediaId,
        string FileName,
        string ContentType,
        string Url,
        long FileSize,
        DateTime UploadTime
    );
}
