using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class ChickenHouse : Grain, IChickenHouse
    {
        public int Size { get; set; }

        public string Name { get; set; }

        public List<Guid> ChickenIds { get; set; } = new List<Guid>();

        public async Task Initialise(int size)
        {
            Size = size;
            Console.WriteLine($"Chicken House {this.GetGrainIdentity()} prepared for {size} Chickens!");

            var tasks = new List<Task>();
            for (int i = 0; i < size; i++)
            {
                var chickenId = Guid.NewGuid();
                ChickenIds.Add(chickenId);
                var chicken = GrainFactory.GetGrain<IChicken>(chickenId);
                tasks.Add(chicken.Initialise());
            }

            Task.WaitAll(tasks.ToArray());

            await Task.CompletedTask;
        }
        
    }
}
