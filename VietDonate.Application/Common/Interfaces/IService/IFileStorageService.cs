namespace VietDonate.Application.Common.Interfaces.IService;

public interface IFileStorageService
{
  Task<string> UploadAsync(
    Stream stream,
    string fileName,
    string contentType,
    CancellationToken cancellationToken = default
  );

  Task DeleteAsync(
    string fileUrl
  );
}