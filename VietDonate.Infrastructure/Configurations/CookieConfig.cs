namespace VietDonate.Infrastructure.Configurations;

public class CookieConfig
{
    public const string Section = "CookieSettings";
    
    public string AccessTokenCookieName { get; set; } = "access_token";
    public string RefreshTokenCookieName { get; set; } = "refresh_token";
    public bool HttpOnly { get; set; } = true;
    public bool Secure { get; set; } = true;
    public string SameSite { get; set; } = "None";
    public string? Domain { get; set; }
    public string Path { get; set; } = "/";
}

