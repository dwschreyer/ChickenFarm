using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenFarm.Api.PublicApi.Antipasto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChickenFarm.Api.PublicApi
{
    public class Startup
    {
        private MvcStarter _mvcStarter;
        private OrleansStarter _orleansStarter;

        public Startup(IConfiguration configuration)
        {
            _mvcStarter = new MvcStarter(configuration);
            _orleansStarter = new OrleansStarter(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _mvcStarter.ConfigureServices(services);
            _orleansStarter.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _mvcStarter.Configure(app, env);
            _orleansStarter.Configure(app, env);
        }
    }
}
