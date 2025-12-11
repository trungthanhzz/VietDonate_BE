namespace VietDonate.Infrastructure.Configurations
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string InstanceName { get; set; } = "VietDonate";
        public int DefaultDatabase { get; set; } = 0;
        public int ConnectTimeout { get; set; } = 5000;
        public int SyncTimeout { get; set; } = 5000;
        public bool AbortConnect { get; set; } = false;
    }
}

