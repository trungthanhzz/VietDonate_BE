using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.Application.Common.Interfaces;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRedisController : ApiController
    {
        private readonly IRedisService _redisService;

        public TestRedisController(IRedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetValue([FromBody] SetValueRequest request)
        {
            await _redisService.SetAsync(request.Key, request.Value, request.Expiry);
            return Ok(new { Message = "Value set successfully" });
        }

        [HttpGet("get/{key}")]
        public async Task<IActionResult> GetValue(string key)
        {
            var value = await _redisService.GetAsync<string>(key);
            return Ok(new { Key = key, Value = value });
        }

        [HttpDelete("remove/{key}")]
        public async Task<IActionResult> RemoveValue(string key)
        {
            await _redisService.RemoveAsync(key);
            return Ok(new { Message = "Value removed successfully" });
        }

        [HttpGet("exists/{key}")]
        public async Task<IActionResult> Exists(string key)
        {
            var exists = await _redisService.ExistsAsync(key);
            return Ok(new { Key = key, Exists = exists });
        }
    }

    public record SetValueRequest(string Key, string Value, TimeSpan? Expiry = null);
}

