using ChickenFarm.GrainContracts;
using Orleans;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.SiloHostConsole.Seed
{
    internal class SeedData
    {
        private IGrainFactory _grainFactory;

        public SeedData(IGrainFactory grainFactory)
        {

            _grainFactory = grainFactory;
        }

        public async Task Initialise()
        {
            await SpawnFarms(50);
        }

        private async Task SpawnFarms(int farmCount)
        {
            var tasks = new List<Task>();

            var farmId = new Guid("3A55870F-3DBC-4D2F-B1DC-160E2D964DFB");
            var farm = _grainFactory.GetGrain<IFarm>(farmId);
            await farm.Initialise("Darrel's Farm");

            for (int i = 0; i < farmCount; i++)
            {
                var farmId = Guid.NewGuid();
                farm = _grainFactory.GetGrain<IFarm>(farmId);
                await farm.Initialise(farmId.ToString());
            }

            
            


            await Task.CompletedTask;
        }

        private Task SpawnFarm(Guid farmId, string name)
        {
            var farm = _grainFactory.GetGrain<IFarm>(farmId);
            return farm.Initialise(name);
        }
    }
}
