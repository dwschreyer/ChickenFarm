using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class ChickenHouse : Grain, IChickenHouse
    {
        public int Size { get; set; }

        public string Name { get; set; }

        public Task Initialise(int size)
        {
            Size = size;
            Console.WriteLine($"Chicken House {this.GetGrainIdentity()} prepared for {size} Chickens!");
            return Task.CompletedTask;
        }
    }
}
