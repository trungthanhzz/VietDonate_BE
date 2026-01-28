namespace VietDonate.Application.Common.Interfaces
{
    public interface ITempMediaService
    {
        Task<string> SaveTempMediaAsync(Guid userId, Guid mediaId, string fileName, string contentType, string s3Key, long fileSize, CancellationToken cancellationToken = default);
        Task<TempMediaInfo?> GetTempMediaAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken = default);
        Task<List<TempMediaInfo>> GetUserTempMediaAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteTempMediaAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken = default);
        Task<bool> AssignToCampaignAsync(Guid userId, List<Guid> tempMediaIds, Guid campaignId, CancellationToken cancellationToken = default);
    }

    public record TempMediaInfo(
        Guid MediaId,
        string FileName,
        string ContentType,
        string S3Key,
        long FileSize,
        DateTime UploadTime,
        string Url
    );
}
