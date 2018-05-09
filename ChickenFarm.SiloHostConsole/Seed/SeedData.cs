using ChickenFarm.GrainContracts;
using Newtonsoft.Json;
using Orleans;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.SiloHostConsole.Seed
{
    internal class SeedData
    {
        private IClusterClient _client;
        private string _seedDataPath;
        private FarmData _farmData;

        public SeedData(IClusterClient client, string seedDataPath)
        {
            _client = client;
            _seedDataPath = seedDataPath;
        }

        public async Task Initialise()
        {
            using (var jsonStream = new JsonTextReader(File.OpenText(_seedDataPath)))
            {
                var deserializer = new JsonSerializer();
                _farmData = deserializer.Deserialize<FarmData>(jsonStream);
            }

            await SpawnFarms();
        }

        private async Task SpawnFarms()
        {

            foreach (var farmInfo in _farmData.Farms)
            {
                var farm = _client.GetGrain<IFarm>(farmInfo.Id);
                
            }

            for (int i = 0; i < 10000; i++)
            {
                var farm = _client.GetGrain<IFarm>(Guid.NewGuid());
            }

            await Task.CompletedTask;
        }
    }
}
