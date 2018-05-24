using ChickenFarm.GrainContracts;
using Newtonsoft.Json;
using Orleans;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Console.WriteLine("Seed Data Completed");
        }

        private async Task SpawnFarms()
        {
            var sw = new Stopwatch();
            sw.Start();
            var farmList = _client.GetGrain<IPropertyList>(Guid.Empty);
            var list = await farmList.GetList();
            var startFarmIds = list.Count;

            foreach (var farmInfo in _farmData.Farms)
            {
                var farm = _client.GetGrain<IProperty>(farmInfo.Id);
                await farm.Initialise(farmInfo.Name);
                await farmList.AddPropertyId(farmInfo.Id);
            }

            for (int i = 0; i < 1000; i++)
            {
                var farmId = Guid.NewGuid();
                var farm = _client.GetGrain<IProperty>(farmId);
                await farm.Initialise($"F {farmId}");
                await farmList.AddPropertyId(farmId);
            }

            list = await farmList.GetList();
            var endFarmIds = list.Count;

            sw.Stop();
            Console.WriteLine($"Farms spawned in {sw.ElapsedMilliseconds}ms.");

            await Task.CompletedTask;
        }
    }
}
