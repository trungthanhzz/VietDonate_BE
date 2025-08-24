using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VietDonate.Application.Common.Interfaces;

namespace VietDonate.Infrastructure.Common.Redis
{
    public class RedisService(IDistributedCache cache, ILogger<RedisService> logger) : IRedisService
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await cache.GetStringAsync(key);
                if (string.IsNullOrEmpty(value))
                    return default;

                return JsonSerializer.Deserialize<T>(value, _jsonOptions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting value from Redis for key: {Key}", key);
                return default;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                var jsonValue = JsonSerializer.Serialize(value, _jsonOptions);
                var options = new DistributedCacheEntryOptions();

                if (expiry.HasValue)
                {
                    options.SetAbsoluteExpiration(expiry.Value);
                }
                await cache.SetStringAsync(key, jsonValue, options);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting value to Redis for key: {Key}", key);
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                await cache.RemoveAsync(key);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing key from Redis: {Key}", key);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var value = await cache.GetStringAsync(key);
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking existence in Redis for key: {Key}", key);
                return false;
            }
        }

        public async Task<bool> SetHashAsync(string key, string field, string value)
        {
            try
            {
                var hashKey = $"{key}:hash";
                var fieldValue = JsonSerializer.Serialize(value, _jsonOptions);
                await cache.SetStringAsync($"{hashKey}:{field}", fieldValue);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting hash field in Redis for key: {Key}, field: {Field}", key, field);
                return false;
            }
        }

        public async Task<string?> GetHashAsync(string key, string field)
        {
            try
            {
                var hashKey = $"{key}:hash";
                var value = await cache.GetStringAsync($"{hashKey}:{field}");
                return value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting hash field from Redis for key: {Key}, field: {Field}", key, field);
                return null;
            }
        }

        public async Task<Dictionary<string, string>> GetAllHashAsync(string key)
        {
            try
            {
                // Note: This is a simplified implementation
                // In a real scenario, you might want to use Redis Hash operations
                var hashKey = $"{key}:hash";
                var result = new Dictionary<string, string>();
                
                // This is a placeholder - in real implementation you'd use Redis Hash commands
                logger.LogWarning("GetAllHashAsync is not fully implemented - consider using Redis Hash operations");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting all hash fields from Redis for key: {Key}", key);
                return new Dictionary<string, string>();
            }
        }

        public async Task<bool> RemoveHashAsync(string key, string field)
        {
            try
            {
                var hashKey = $"{key}:hash";
                await cache.RemoveAsync($"{hashKey}:{field}");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing hash field from Redis for key: {Key}, field: {Field}", key, field);
                return false;
            }
        }

        public async Task<bool> HashExistsAsync(string key, string field)
        {
            try
            {
                var hashKey = $"{key}:hash";
                var value = await cache.GetStringAsync($"{hashKey}:{field}");
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking hash field existence in Redis for key: {Key}, field: {Field}", key, field);
                return false;
            }
        }

        public async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
        {
            try
            {
                var value = await cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(value))
                {
                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expiry
                    };
                    await cache.SetStringAsync(key, value, options);
                    return true;
                }
                return false; // Key doesn't exist
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting expiry for key: {Key}", key);
                return false;
            }
        }

        public async Task<TimeSpan?> GetExpiryAsync(string key)
        {
            try
            {
                // Note: IDistributedCache doesn't provide direct access to TTL
                // This is a placeholder - in real implementation you'd use Redis TTL command
                logger.LogWarning("GetExpiryAsync is not fully implemented - consider using Redis TTL command");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting expiry for key: {Key}", key);
                return null;
            }
        }
    }
}

