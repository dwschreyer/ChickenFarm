using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChickenFarm.Api.PublicApi.Antipasto
{
    public abstract class BaseStarter
    {
        public IConfiguration Configuration { get; }

        public BaseStarter(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
