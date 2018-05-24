using ChickenFarm.GrainContracts;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChickenFarm.Api.PublicApi.Antipasto
{
    public class OrleansStarter : BaseStarter
    {
        public OrleansStarter(IConfiguration configuration) : base(configuration)
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IClusterClient>(CreateClusterClient);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {

            }
            
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            //TODO: DWS: Replace with your connection string
            const string connectionString = "YOUR_CONNECTION_STRING_HERE";

            var client = new ClientBuilder()
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "chicken-farm-docker";
                            options.ServiceId = "ChickenFarmApi";
                        })
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ITenant).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IUser).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IPropertyList).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IProperty).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IChickenHouse).Assembly).WithReferences())
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IChicken).Assembly).WithReferences())
                        .ConfigureLogging(logging => logging.AddConsole())
                        .Build();

            StartClientWithRetries(client).Wait();

            return client;
        }
        
        private static async Task StartClientWithRetries(IClusterClient client)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    await client.Connect();
                    return;
                }
                catch (Exception)
                { }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
