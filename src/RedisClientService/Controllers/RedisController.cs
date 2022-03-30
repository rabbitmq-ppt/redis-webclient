using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisClientService.Infrastructure;
using RedisClientService.Models;
using System.Threading.Tasks;

namespace RedisClientService.Controllers
{
    [Route("redis")]
    public class RedisController : ControllerBase
    {
        private readonly ILogger<RedisController> _logger;
        private readonly IRedisCache _redisCache;
        public RedisController(IRedisCache redisCache, ILogger<RedisController> logger)
        {
            _logger = logger;
            _redisCache = redisCache;

        }

        [Route("index")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RedisInfo))]
        [HttpGet()]
        public IActionResult Index()
        {
            return Ok(_redisCache.GetRedisInfo());
        }

        [HttpPost("value/string")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StringValue))]
        public async Task<IActionResult> SetValue(StringValue stringValue)
        {

            await _redisCache.AddOrUpdateString(stringValue.Key, stringValue.Value);
            _logger.LogInformation("[RedisClientService] String value cached in redis");
            return Ok(stringValue);
        }

        [HttpGet("value/string")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StringValue))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetValue(string key)
        {
            var result = await _redisCache.Get(key);
            if (!string.IsNullOrEmpty(result))
            {
                _logger.LogInformation($"[RedisClientService] String value from redis: {result}");
                return Ok(new StringValue { Key = key, Value = result });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
