using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class ChickenHouse : Grain, IChickenHouse
    {
        private IFarm _farm;

        public int Size { get; set; }

        public string Name { get; set; }

        public List<Guid> ChickenIds { get; set; } = new List<Guid>();

        public Task Initialise(IFarm farm, int size)
        {
            _farm = farm;
            Size = size;
            Console.WriteLine($"Chicken House {this.GetGrainIdentity()} prepared for {Size} Chickens!");

            var tasks = new List<Task>();
            for (int i = 0; i < Size; i++)
            {
                var chickenId = Guid.NewGuid();
                ChickenIds.Add(chickenId);
                var chicken = GrainFactory.GetGrain<IChicken>(chickenId);
                //chicken.Initialise();
                tasks.Add(chicken.Initialise());
            }

            //var t = tasks.ToArray();
            //Task.WaitAll(t);

            return Task.CompletedTask;
        }
        
    }
}
