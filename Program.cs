using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace TimescaleSeedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Database seed test");
            var conf = ReadConfigfuration();
            var pgConf = new PostgresStorageConfig(conf);
            var seedSettings = new SeederConfig(conf);
            Console.WriteLine("Seed settings:" + seedSettings);
            Console.WriteLine("Y to create schema");
            var createSchema = Console.ReadLine().ToUpper() == "Y";
            var connFactory = new ConnectionFactory(pgConf);
            if (createSchema)
            {
                Console.WriteLine("Creating db");
                var createUtil = new DatabaseCreationUtil(connFactory);
                createUtil.CreateDatabase(true);
            }

            Console.WriteLine("Seeding events");
            var mainSeedTimer = Stopwatch.StartNew();
            var eventRepo = new EventRepo(connFactory);
            for (var i = 0; i < seedSettings.NbrSeriesToSeed; i++)
            {
                var cts = new CancellationTokenSource(TimeSpan.FromMinutes(60));
                var seriesId = seedSettings.InitialSeriesId + i;
                Console.WriteLine($"About to seed series dbId {seriesId}");
                var seriesTimer = Stopwatch.StartNew();
                var events = EventDataGenerator.GenerateEvents(seedSettings, seriesId);
                eventRepo.Insert(events, cts.Token).GetAwaiter().GetResult();
                seriesTimer.Stop();
                Console.WriteLine($"Series dbId {seriesId} finished in {seriesTimer.Elapsed}.");
            }
            mainSeedTimer.Stop();
            Console.WriteLine($"Finished in {mainSeedTimer.Elapsed}. Press enter");
            Console.ReadLine();
        }

        static IConfiguration ReadConfigfuration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }
    }
}
