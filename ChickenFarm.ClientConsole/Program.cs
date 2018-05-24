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
                    Console.WriteLine("Press any key to start the app.");
                    Console.ReadKey();
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

            var farmList = client.GetGrain<IPropertyList>(Guid.Empty);
            var farmIds = await farmList.GetList();

            foreach (var farmId in farmIds)
            {
                sw.Restart();
                var farm = client.GetGrain<IProperty>(farmId);
                var name = await farm.GetName();
                sw.Stop();
                Console.WriteLine($"{name} in {sw.ElapsedMilliseconds}ms!");
                //await Task.Delay(rnd.Next(5, 50));
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
                            options.ClusterId = "orleans-docker";
                            options.ServiceId = "HelloWorldApp";
                        })
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IPropertyList).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IProperty).Assembly).WithReferences())
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
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    Console.ResetColor();
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
