using Microsoft.Extensions.Logging;
using RedisClientService.Models;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace RedisClientService.Infrastructure
{
    public class RedisCache : IRedisCache
    {
        private readonly IDatabaseAsync _databaseAsync;
        private readonly ILogger<RedisCache> _logger;

        public RedisInfo GetRedisInfo()
        {
            return new RedisInfo
            {
                IsConnected = _databaseAsync.Multiplexer.IsConnected,
                Status = _databaseAsync.Multiplexer.GetStatus(),
                IsConnnecting = _databaseAsync.Multiplexer.IsConnecting,
                OperationCount = _databaseAsync.Multiplexer.OperationCount
            };
        }

        public RedisCache(ConnectionMultiplexer connectionMultiplexer, ILogger<RedisCache> logger)
        {
            _logger = logger;
            try
            {
                _logger.LogInformation("[RedisClientService] connectionMultiplexer GetDataBase");
                _databaseAsync = connectionMultiplexer.GetDatabase();
                _logger.LogInformation("[RedisClientService] connectionMultiplexer GetDataBase succeed.");
            }
            catch (Exception e)
            {
                _logger.LogError($"[RedisClientService] {e}");
                throw;
            }
        }

        public async Task AddOrUpdateString(string key, string value)
        {
            try
            {
                await _databaseAsync.StringSetAsync(key, value);
            }
            catch (Exception e)
            {
                _logger.LogError($"[RedisClientService] {e}");
                throw;
            }
        }

        public async Task<string> Get(string key)
        {
            try
            {
                var result = await _databaseAsync.StringGetAsync(key);
                if (!result.HasValue)
                {
                    return null;
                }
                return result.ToString();
            }
            catch (Exception e)
            {
                _logger.LogError($"[RedisClientService] {e}");
                throw;
            }
        }
    }
}
