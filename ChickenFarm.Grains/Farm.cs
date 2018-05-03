using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class Farm : Grain, IFarm
    {
        public string Name { get; set; }

        public async Task InitialiseFarm()
        {
            var houseDict = new Dictionary<Guid, int>()
            {
                {  Guid.NewGuid(), 1000 },
                {  Guid.NewGuid(), 30000 }
            };

            Console.WriteLine($"Farm {this.GetGrainIdentity()} with {houseDict.Count} chicken houses!");

            foreach (var item in houseDict)
            {
                var chickenHouse = GrainFactory.GetGrain<IChickenHouse>(item.Key);
                await chickenHouse.Initialise(item.Value);
            }
        }
    }
}
