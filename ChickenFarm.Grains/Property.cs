using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class Property : Grain, IProperty
    {
        public string Name { get; set; }

        public Task Initialise(string name)
        {
            Name = name;
            //var rnd = new Random();
            //var houseDict = new Dictionary<Guid, int>();
            //var houseCount = rnd.Next(1, 2);
            //for (int i = 0; i < houseCount; i++)
            //{
            //    houseDict.Add(Guid.NewGuid(), rnd.Next(1, 30));
            //};

            //Console.WriteLine($"Farm {this.GetGrainIdentity()} with {houseDict.Count} chicken houses!");

            //foreach (var item in houseDict)
            //{
            //    var chickenHouse = GrainFactory.GetGrain<IChickenHouse>(item.Key);
            //    chickenHouse.Initialise(this, item.Value);
            //}

            return Task.CompletedTask;
        }

        public Task<string> GetName()
        {
            return Task.FromResult(Name);
        }
    }
}
