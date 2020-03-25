using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TimescaleSeedTest
{
    public class SomeEvent
    {
        public int SeriesId { get; set; }
        public DateTime Time { get; set; }
        public float Value { get; set; }
    }

    public class EventRepo
    {
        private readonly IConnectionFactory _connectionFactory;

        public EventRepo(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IAsyncEnumerable<SomeEvent> Read(int seriesId, DateTime start, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(IEnumerable<SomeEvent> events, CancellationToken ct)
        {
            await using var dbConn = _connectionFactory.GetConnection();
            await dbConn.OpenAsync(ct);
            await using var writer = dbConn.BeginBinaryImport("COPY event_table (series_id, event_time, event_data) FROM STDIN (FORMAT BINARY)");
            foreach(var ev in events)
            {
                await writer.WriteRowAsync(ct, ev.SeriesId, ev.Time, ev.Value);
            }
            await writer.CompleteAsync(ct);
        }
    }
}
