using System;
using Npgsql;
using NpgsqlTypes;

namespace TimescaleSeedTest
{
    public static class NpgsqlExtensions
    {
        public static NpgsqlCommand CreateCommand(this NpgsqlConnection connection, string commandText)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            return cmd;
        }

        public static void AddParameter(this NpgsqlCommand command, string parameterName, DateTime time)
        {
            command.Parameters.AddWithValue(parameterName, NpgsqlDbType.Timestamp, time);
        }

        public static void AddParameter(this NpgsqlCommand command, string parameterName, object value)
        {
            command.Parameters.AddWithValue(parameterName, value ?? DBNull.Value);
        }

        public static void AddParameter(this NpgsqlCommand command, string parameterName, NpgsqlDbType dbType, object value)
        {
            command.Parameters.AddWithValue(parameterName, dbType, value ?? DBNull.Value);
        }

        public static void AddJsonParameter(this NpgsqlCommand command, string parameterName, string jsonString)
        {
            command.Parameters.AddWithValue(parameterName, NpgsqlDbType.Jsonb, jsonString);
        }

        public static DateTime GetDateTimeAsUtc(this NpgsqlDataReader reader, int ordinal)
        {
            var dt = reader.GetDateTime(ordinal);
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }
    }
}
