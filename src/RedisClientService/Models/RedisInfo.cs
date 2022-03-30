namespace RedisClientService.Models
{
    public class RedisInfo
    {
        public string Status { get; set; }
        public long OperationCount { get; set; }
        public bool IsConnected { get; set; }
        public bool IsConnnecting { get; set; }
    }
}
