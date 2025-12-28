using Amazon.S3;
using Amazon.S3.Model;

using Microsoft.Extensions.Options;

using VietDonate.Application.Common.Interfaces.IService;
using VietDonate.Infrastructure.Configurations;

namespace VietDonate.Infrastructure.Common.S3;

public class S3FileStorageService(
  IAmazonS3 s3,
  IOptions<S3Config> options
) : IFileStorageService
{
  private readonly S3Config _config = options.Value;

  public async Task<string> UploadAsync(
    Stream stream,
    string fileName,
    string contentType,
    CancellationToken cancellationToken = default)
  {
    var key = GenerateObjectKey(fileName);

    var request = new PutObjectRequest
    {
      BucketName = _config.BucketName,
      Key = key,
      InputStream = stream,
      ContentType = contentType,
      CannedACL = S3CannedACL.PublicRead
    };

    await s3.PutObjectAsync(request, cancellationToken);

    return $"{_config.PublicBaseUrl}/{key}";
  }

  public async Task DeleteAsync(string fileUrl)
  {
    var key = ExtractKeyFromUrl(fileUrl);

    await s3.DeleteObjectAsync(
      _config.BucketName,
      key);
  }

  private static string GenerateObjectKey(string fileName)
  {
    var ext = Path.GetExtension(fileName);
    return $"campaign/{Guid.NewGuid()}{ext}";
  }

  private static string ExtractKeyFromUrl(string url)
  {
    return new Uri(url).AbsolutePath.TrimStart('/');
  }
}
