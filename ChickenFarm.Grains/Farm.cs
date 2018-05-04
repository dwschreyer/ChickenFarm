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
            var rnd = new Random();
            var houseDict = new Dictionary<Guid, int>();
            var houseCount = rnd.Next(1, 10);
            for (int i = 0; i < houseCount; i++)
            {
                houseDict.Add(Guid.NewGuid(), rnd.Next(1000, 30000));
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
