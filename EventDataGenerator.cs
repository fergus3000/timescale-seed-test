using System.Collections.Generic;

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
    }
}
