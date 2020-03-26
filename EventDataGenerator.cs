using System.Collections.Generic;
using MoreLinq;

namespace TimescaleSeedTest
{
    public static class EventDataGenerator
    {
        public static IEnumerable<SomeEvent> GenerateEvents(SeederConfig config, int seriesId)
        {
            var eventIntervalSec = 1.0d / config.EventFrequencyHz;
            var eventTime = config.SeriesStartDate;
            var endTime = config.SeriesStartDate.AddMonths(config.SeriesLengthMonths);
            do
            {
                var ev = new SomeEvent()
                {
                    SeriesId = seriesId,
                    Time = eventTime,
                    Value = 1.0f,
                };
                eventTime = eventTime.AddSeconds(eventIntervalSec);
                yield return ev;
            } while (eventTime < endTime);
        }

        public static IEnumerable<IEnumerable<SomeEvent>> BatchGenerateEventsChronological(SeederConfig config)
        {
            var eventsSource = GenerateEventsChronological(config);
            return eventsSource.Batch(config.WriteBatchSize);
        }

        public static IEnumerable<SomeEvent> GenerateEventsChronological(SeederConfig config)
        {
            var eventIntervalSec = 1.0d / config.EventFrequencyHz;
            var eventTime = config.SeriesStartDate;
            var endTime = config.SeriesStartDate.AddMonths(config.SeriesLengthMonths);
            var startId = config.InitialSeriesId;
            var nSeries = config.NbrSeriesToSeed;
            do
            {
                for (var i = startId; i < startId + nSeries; i++)
                {
                    var ev = new SomeEvent()
                    {
                        SeriesId = i,
                        Time = eventTime,
                        Value = 1.0f,
                    };
                    yield return ev;
                }
                eventTime = eventTime.AddSeconds(eventIntervalSec);
            } while (eventTime < endTime);
        }
    }
}
