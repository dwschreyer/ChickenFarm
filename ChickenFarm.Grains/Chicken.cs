using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class Chicken : Grain, IChicken
    {
        public async Task Initialise()
        {
            Console.WriteLine($"Chicken {this.GetGrainIdentity()} initialised!");
            await Task.CompletedTask;
        }
    }
}
