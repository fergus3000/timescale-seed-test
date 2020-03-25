using Microsoft.Extensions.Configuration;

namespace TimescaleSeedTest
{
    public interface IPostgresStorageConfig
    {
        string PgConnectionString { get; }
    }

    public class PostgresStorageConfig : IPostgresStorageConfig
    {
        readonly IConfiguration _config;

        public PostgresStorageConfig(IConfiguration config)
        {
            _config = config;
        }

        public string PgConnectionString => _config.GetConnectionString("postgres");
    }
}
