namespace VietDonate.Application.Common.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder = "", CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string fileKey, CancellationToken cancellationToken = default);
        Task<string> GetUrlAsync(string fileKey);
        Task<string> GeneratePresignedUrlAsync(string fileKey, int expirationMinutes = 60);
        Task<bool> FileExistsAsync(string fileKey, CancellationToken cancellationToken = default);
    }
}
