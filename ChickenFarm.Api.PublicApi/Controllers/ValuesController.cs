using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenFarm.GrainContracts;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace ChickenFarm.Api.PublicApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IClusterClient _clusterClient;

        public ValuesController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var farmList = _clusterClient.GetGrain<IPropertyList>(Guid.Empty);
            var farmIds = await farmList.GetList();


            return farmIds.Select(id => id.ToString()).ToArray();
        }
    }
}
