namespace VietDonate.Infrastructure.Configurations
{
    public class S3Config
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = "us-east-1";
        public bool UseSSL { get; set; } = true;
        public bool ForcePathStyle { get; set; } = false;
        public string? BaseUrl { get; set; }
        public int PresignedUrlExpirationMinutes { get; set; } = 60;
        public long MaxFileSizeBytes { get; set; } = 100 * 1024 * 1024; // 100MB default
        public string[] AllowedFileTypes { get; set; } = { "image/jpeg", "image/png", "image/gif", "image/webp", "video/mp4", "video/quicktime" };
    }
}
