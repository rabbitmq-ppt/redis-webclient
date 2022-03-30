namespace RedisClientService.Configuration
{
    public interface IRedisCsConfig
    {
        string ConnectionString { get; set; }
    }
}