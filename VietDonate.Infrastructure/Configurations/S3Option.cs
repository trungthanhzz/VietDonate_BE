namespace VietDonate.Infrastructure.Configurations;

public class S3Config
{
  public string BucketName { get; set; } = null!;
  public string Region { get; set; } = null!;
  public string PublicBaseUrl { get; set; } = null!;
}
