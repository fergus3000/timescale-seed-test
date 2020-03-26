using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            Console.WriteLine("Batch generating");
            var eventBatches = EventDataGenerator.BatchGenerateEventsChronological(seedSettings);
            var batchCount = eventBatches.Count();
            Console.WriteLine("Seeding events");
            var mainSeedTimer = Stopwatch.StartNew();
            var opt = new ParallelOptions()
            {
                MaxDegreeOfParallelism = seedSettings.MaxParallelThreads
            };
            Parallel.ForEach(eventBatches, opt, (evts, loopState, batchNo) =>
            {
                var batchSw = Stopwatch.StartNew();
                var repo = new EventRepo(connFactory);
                repo.Insert(evts, CancellationToken.None).GetAwaiter().GetResult();
                batchSw.Stop();
                Console.WriteLine($"Wrote batch {batchNo}/{batchCount}. Batch took {batchSw.Elapsed:ss\\.ffffff} sec");
            });
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
