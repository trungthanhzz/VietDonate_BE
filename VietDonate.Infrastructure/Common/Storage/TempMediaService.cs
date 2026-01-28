using Microsoft.Extensions.Logging;
using VietDonate.Application.Common.Interfaces;

namespace VietDonate.Infrastructure.Common.Storage
{
    public class TempMediaService : ITempMediaService
    {
        private readonly IRedisService _redisService;
        private readonly IStorageService _storageService;
        private readonly ILogger<TempMediaService> _logger;
        private const int TempMediaTtlHours = 24;

        public TempMediaService(
            IRedisService redisService,
            IStorageService storageService,
            ILogger<TempMediaService> logger)
        {
            _redisService = redisService;
            _storageService = storageService;
            _logger = logger;
        }

        private string GetTempMediaKey(Guid userId, Guid mediaId)
        {
            return $"temp:media:{userId}:{mediaId}";
        }

        private string GetUserTempMediaListKey(Guid userId)
        {
            return $"temp:media:list:{userId}";
        }

        public async Task<string> SaveTempMediaAsync(Guid userId, Guid mediaId, string fileName, string contentType, string s3Key, long fileSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var url = await _storageService.GetUrlAsync(s3Key);
                var tempMedia = new TempMediaInfo(
                    MediaId: mediaId,
                    FileName: fileName,
                    ContentType: contentType,
                    S3Key: s3Key,
                    FileSize: fileSize,
                    UploadTime: DateTime.UtcNow,
                    Url: url
                );

                var key = GetTempMediaKey(userId, mediaId);
                var expiry = TimeSpan.FromHours(TempMediaTtlHours);

                var saved = await _redisService.SetAsync(key, tempMedia, expiry);
                if (!saved)
                {
                    _logger.LogWarning("Failed to save temp media to Redis. Key: {Key}", key);
                    return string.Empty;
                }

                // Also add to user's temp media list
                var listKey = GetUserTempMediaListKey(userId);
                var existingList = await _redisService.GetAsync<List<Guid>>(listKey) ?? new List<Guid>();
                if (!existingList.Contains(mediaId))
                {
                    existingList.Add(mediaId);
                    await _redisService.SetAsync(listKey, existingList, expiry);
                }

                _logger.LogInformation("Temp media saved successfully. UserId: {UserId}, MediaId: {MediaId}", userId, mediaId);
                return key;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving temp media. UserId: {UserId}, MediaId: {MediaId}", userId, mediaId);
                throw;
            }
        }

        public async Task<TempMediaInfo?> GetTempMediaAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken = default)
        {
            try
            {
                var key = GetTempMediaKey(userId, mediaId);
                var tempMedia = await _redisService.GetAsync<TempMediaInfo>(key);
                
                if (tempMedia != null)
                {
                    // Refresh URL in case it expired
                    tempMedia = tempMedia with { Url = await _storageService.GetUrlAsync(tempMedia.S3Key) };
                }

                return tempMedia;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting temp media. UserId: {UserId}, MediaId: {MediaId}", userId, mediaId);
                return null;
            }
        }

        public async Task<List<TempMediaInfo>> GetUserTempMediaAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var listKey = GetUserTempMediaListKey(userId);
                var mediaIds = await _redisService.GetAsync<List<Guid>>(listKey) ?? new List<Guid>();

                var tempMediaList = new List<TempMediaInfo>();
                foreach (var mediaId in mediaIds)
                {
                    var tempMedia = await GetTempMediaAsync(userId, mediaId, cancellationToken);
                    if (tempMedia != null)
                    {
                        tempMediaList.Add(tempMedia);
                    }
                }

                return tempMediaList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user temp media list. UserId: {UserId}", userId);
                return new List<TempMediaInfo>();
            }
        }

        public async Task<bool> DeleteTempMediaAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken = default)
        {
            try
            {
                var key = GetTempMediaKey(userId, mediaId);
                var deleted = await _redisService.RemoveAsync(key);

                // Remove from user's temp media list
                var listKey = GetUserTempMediaListKey(userId);
                var existingList = await _redisService.GetAsync<List<Guid>>(listKey) ?? new List<Guid>();
                existingList.Remove(mediaId);
                if (existingList.Count > 0)
                {
                    var expiry = TimeSpan.FromHours(TempMediaTtlHours);
                    await _redisService.SetAsync(listKey, existingList, expiry);
                }
                else
                {
                    await _redisService.RemoveAsync(listKey);
                }

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting temp media. UserId: {UserId}, MediaId: {MediaId}", userId, mediaId);
                return false;
            }
        }

        public async Task<bool> AssignToCampaignAsync(Guid userId, List<Guid> tempMediaIds, Guid campaignId, CancellationToken cancellationToken = default)
        {
            try
            {
                // This method is called by the AssignMediaToCampaignCommandHandler
                // It just removes temp media from Redis - the actual DB creation is done in the handler
                foreach (var mediaId in tempMediaIds)
                {
                    await DeleteTempMediaAsync(userId, mediaId, cancellationToken);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning temp media to campaign. UserId: {UserId}, CampaignId: {CampaignId}", userId, campaignId);
                return false;
            }
        }
    }
}
