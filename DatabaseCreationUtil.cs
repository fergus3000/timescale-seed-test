using System.IO;

namespace TimescaleSeedTest
{
    public class DatabaseCreationUtil
    {
        const string DbName = "timescale-test";
        private readonly IConnectionFactory _connectionFactory;

        public DatabaseCreationUtil(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void CreateDatabase(bool dropIfExists)
        {
            if (dropIfExists)
                DropDatabase(DbName);
            CreateDatabase();
            CreateDbSchema();
        }

        private void DropDatabase(string dbName)
        {
            using (var dbConn = _connectionFactory.GetConnectionForDatabase("postgres"))
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand($"DROP DATABASE IF EXISTS \"{dbName}\""))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateDatabase()
        {
            var script = File.ReadAllText(@"DbScripts/CreateDatabase.sql");
            using (var dbConn = _connectionFactory.GetConnectionForDatabase("postgres"))
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand(script))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateDbSchema()
        {
            var script = File.ReadAllText(@"DbScripts/CreateTestEventTable.sql");
            using (var dbConn = _connectionFactory.GetConnection())
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand(script))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
