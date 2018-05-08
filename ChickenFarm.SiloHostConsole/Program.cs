using ChickenFarm.Grains;
using ChickenFarm.SiloHostConsole.Seed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
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
                var host = await StartSilo();
                Console.WriteLine($"=============================================================={Environment.NewLine}Chicken Farm Silo Hosted!{Environment.NewLine}==============================================================");

                
                Console.WriteLine($"=============================================================={Environment.NewLine}Chicken Farm Seed Completed!{Environment.NewLine}==============================================================");

                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .AddMemoryGrainStorage("DevStore")
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "ChickenFarm";
                })
                .AddStartupTask(async (IServiceProvider services, CancellationToken cancellationToken) =>
                {
                    var grainFactory = services.GetRequiredService<IGrainFactory>();
                    var seedData = new SeedData(grainFactory);
                    await seedData.Initialise();
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Farm).Assembly).WithReferences())
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ChickenHouse).Assembly).WithReferences())
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Chicken).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
