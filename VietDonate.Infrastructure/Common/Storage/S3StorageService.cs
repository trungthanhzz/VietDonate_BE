using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Infrastructure.Configurations;

namespace VietDonate.Infrastructure.Common.Storage
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3Config _config;
        private readonly ILogger<S3StorageService> _logger;

        public S3StorageService(IOptions<S3Config> config, ILogger<S3StorageService> logger)
        {
            _config = config.Value;
            _logger = logger;

            var s3Config = new AmazonS3Config
            {
                ServiceURL = _config.Endpoint,
                ForcePathStyle = true
            };


            _s3Client = new AmazonS3Client(_config.AccessKey, _config.SecretKey, s3Config);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder = "", CancellationToken cancellationToken = default)
        {
            try
            {
                var key = string.IsNullOrEmpty(folder) 
                    ? $"media/{Guid.NewGuid()}/{fileName}"
                    : $"{folder.TrimEnd('/')}/{Guid.NewGuid()}/{fileName}";

                var request = new PutObjectRequest
                {
                    BucketName = _config.BucketName,
                    Key = key,
                    InputStream = fileStream,
                    AutoCloseStream = true,
                    UseChunkEncoding = false
                };

                await _s3Client.PutObjectAsync(request, cancellationToken);

                _logger.LogInformation("File uploaded successfully to S3. Key: {Key}", key);
                return key;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to S3. FileName: {FileName}", fileName);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string fileKey, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _config.BucketName,
                    Key = fileKey
                };

                await _s3Client.DeleteObjectAsync(request, cancellationToken);
                _logger.LogInformation("File deleted successfully from S3. Key: {Key}", fileKey);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file from S3. Key: {Key}", fileKey);
                return false;
            }
        }

        public async Task<string> GetUrlAsync(string fileKey)
        {
            if (!string.IsNullOrEmpty(_config.BaseUrl))
            {
                return $"{_config.BaseUrl.TrimEnd('/')}/{fileKey}";
            }

            // Generate presigned URL if BaseUrl is not configured
            return await GeneratePresignedUrlAsync(fileKey, _config.PresignedUrlExpirationMinutes);
        }

        public async Task<string> GeneratePresignedUrlAsync(string fileKey, int expirationMinutes = 60)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _config.BucketName,
                    Key = fileKey,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
                };

                var url = await _s3Client.GetPreSignedURLAsync(request);
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating presigned URL for key: {Key}", fileKey);
                throw;
            }
        }

        public async Task<bool> FileExistsAsync(string fileKey, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _config.BucketName,
                    Key = fileKey
                };

                await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking file existence in S3. Key: {Key}", fileKey);
                return false;
            }
        }
    }
}
