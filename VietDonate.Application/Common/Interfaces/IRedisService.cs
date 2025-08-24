namespace VietDonate.Application.Common.Interfaces
{
    public interface IRedisService
    {
        Task<T?> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<bool> RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<bool> SetHashAsync(string key, string field, string value);
        Task<string?> GetHashAsync(string key, string field);
        Task<Dictionary<string, string>> GetAllHashAsync(string key);
        Task<bool> RemoveHashAsync(string key, string field);
        Task<bool> HashExistsAsync(string key, string field);
        Task<bool> SetExpiryAsync(string key, TimeSpan expiry);
        Task<TimeSpan?> GetExpiryAsync(string key);
    }
}