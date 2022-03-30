using RedisClientService.Models;
using System.Threading.Tasks;

namespace RedisClientService.Infrastructure
{
    public interface IRedisCache
    {
        Task AddOrUpdateString(string key, string value);
        Task<string> Get(string key);
        RedisInfo GetRedisInfo();
    }
}
