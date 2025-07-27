using System.Data.Common;
using Npgsql;

namespace VietDonate.Infrastructure.Configurations
{
    public class DbConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int TimeoutSeconds { get; set; } = 30;

        public string BuildConnectionString()
        {
            return $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password};TrustServerCertificate=True;";
        }

        public DbConnection CreateConnection()
        {
            return new NpgsqlConnection(BuildConnectionString());
        }
    }
} 