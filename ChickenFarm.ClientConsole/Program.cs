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
                    await SpawnFarms(client, 1);
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

        private static async Task SpawnFarms(IClusterClient client, int farmCount)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < farmCount; i++)
            {
                tasks.Add(SpawnFarm(client, Guid.NewGuid()));
            }

            Task.WaitAll(tasks.ToArray());

            await Task.CompletedTask;
        }

        private static Task SpawnFarm(IClusterClient client, Guid farmId)
        {
            var farm = client.GetGrain<IFarm>(farmId);
            return farm.Initialise();
        }
    }
}
