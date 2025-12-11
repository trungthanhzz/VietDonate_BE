namespace VietDonate.Infrastructure.Common.Middleware
{
    public class JwtBlacklistOptions
    {
        public bool EnableBlacklistCheck { get; set; } = true;
        public string[] ExcludedPaths { get; set; } = Array.Empty<string>();
        public string[] ExcludedMethods { get; set; } = Array.Empty<string>();
        public bool LogBlockedRequests { get; set; } = true;
        public string BlacklistKeyPrefix { get; set; } = "bl:acc:";
    }
}
