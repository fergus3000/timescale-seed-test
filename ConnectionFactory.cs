using Npgsql;

namespace TimescaleSeedTest
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IPostgresStorageConfig _postgresStorageConfig;

        public ConnectionFactory(IPostgresStorageConfig postgresStorageConfig)
        {
            _postgresStorageConfig = postgresStorageConfig;
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_postgresStorageConfig.PgConnectionString);
        }

        public NpgsqlConnection GetConnectionForDatabase(string dbName)
        {
            var sb = new NpgsqlConnectionStringBuilder(_postgresStorageConfig.PgConnectionString) {Database = dbName};
            return new NpgsqlConnection(sb.ToString());
        }
    }
}
