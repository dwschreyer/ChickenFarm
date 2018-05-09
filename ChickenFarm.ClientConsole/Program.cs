using ChickenFarm.GrainContracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ChickenFarm.ClientConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }
        
        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await StartClientWithRetries())
                {
                    var farmId = new Guid("3A55870F-3DBC-4D2F-B1DC-160E2D964DFB");
                    var farm = client.GetGrain<IFarm>(farmId);

                    var chickenHouseId = new Guid("DD673B1B-A189-4EA6-B03D-FCC26B9C724D");
                    var chickenHouse = client.GetGrain<IChickenHouse>(chickenHouseId);

                    var name = await farm.GetName();
                    Console.WriteLine($"{Environment.NewLine}{new string('=', 80)}{Environment.NewLine}Found Farm: {name}");

                    await LoopThroughGrains(client);

                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task LoopThroughGrains(IClusterClient client)
        {
            var rnd = new Random();
            var sw = new Stopwatch();
            for (int i = 0; i < 100; i++)
            {
                sw.Restart();
                var farmId = new Guid("3A55870F-3DBC-4D2F-B1DC-160E2D964DFB");
                var farm = client.GetGrain<IFarm>(farmId);
                var name = await farm.GetName();
                sw.Stop();
                Console.WriteLine($"{name} in {sw.ElapsedMilliseconds}ms!");
                await Task.Delay(rnd.Next(5, 50));
            }
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    client = new ClientBuilder()
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "HelloWorldApp";
                        })
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IFarm).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IChickenHouse).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IChicken).Assembly).WithReferences())
                        .ConfigureLogging(logging => logging.AddConsole())
                        .Build();

                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }
    }
}
