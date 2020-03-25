using Npgsql;

namespace TimescaleSeedTest
{
    public interface IConnectionFactory
    {
        NpgsqlConnection GetConnection();
        NpgsqlConnection GetConnectionForDatabase(string dbName);
    }
}