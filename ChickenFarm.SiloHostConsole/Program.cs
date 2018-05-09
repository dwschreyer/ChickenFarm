using ChickenFarm.GrainContracts;
using ChickenFarm.Grains;
using ChickenFarm.SiloHostConsole.Seed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ChickenFarm.SiloHostConsole
{
    class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var silo = CreateSilo();
                var client = CreateClient();
                var dataFilePath = GetDataFilePath();

                await RunAsync(silo, client, dataFilePath);

                Console.WriteLine("Ready to roll!");

                Console.ReadLine();
                
                await client.Close();
                await silo.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task RunAsync(ISiloHost silo, IClusterClient client, string dataFilePath)
        {
            await silo.StartAsync();
            await client.Connect();

            var seedData = new SeedData(client, dataFilePath);
            await seedData.Initialise();
        }

        private static string GetDataFilePath()
        {
            var locationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dataFilePath = Path.Combine(locationPath, "Data", "FarmData.json");
            return dataFilePath;
        }

        private static ISiloHost CreateSilo()
        {
            var silo = new SiloHostBuilder()
                .UseLocalhostClustering()
                .AddMemoryGrainStorage("ChickenFarmStorage")
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "ChickenFarmApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Farm).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            return silo;
        }

        private static IClusterClient CreateClient()
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "ChickenFarmApp";
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IFarm).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            return client;
        }
        
    }
}
