using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace TimescaleSeedTest
{
    public class SeederConfig
    {
        private readonly IConfiguration _config;
        public SeederConfig(IConfiguration config)
        {
            _config = config;
            var seedingSection = _config.GetSection("DbSeeding");
            InitialSeriesId = Convert.ToInt32(seedingSection["InitialSeriesId"]);
            NbrSeriesToSeed = Convert.ToInt32(seedingSection["NbrSeriesToSeed"]);
            SeriesLengthMonths = Convert.ToInt32(seedingSection["SeriesLengthMonths"]);
            EventFrequencyHz = Convert.ToInt32(seedingSection["EventFrequencyHz"]);
            SeriesStartDate = DateTime.Parse(seedingSection["SeriesStartDate"], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            WriteBatchSize = Convert.ToInt32(seedingSection["WriteBatchSize"]);
            MaxParallelThreads = Convert.ToInt32(seedingSection["MaxParallelThreads"]);
        }

        public int InitialSeriesId { get; }
        public int NbrSeriesToSeed { get; }
        public int SeriesLengthMonths { get; }
        public DateTime SeriesStartDate { get; }
        public int EventFrequencyHz { get; }
        public int WriteBatchSize { get; }
        public int MaxParallelThreads { get; }

        public override string ToString()
        {
            return
                $"Seed {NbrSeriesToSeed} series for {SeriesLengthMonths} months. Start date {SeriesStartDate}, event frequency {EventFrequencyHz}Hz. {WriteBatchSize} batches, {MaxParallelThreads} parallel threads";
        }
    }
}
