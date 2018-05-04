using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class Chicken : Grain, IChicken
    {
        private Timer _eatTimer;

        public Task Initialise()
        {
            Console.WriteLine($"Chicken {this.GetGrainIdentity()} initialised!");

            //await Start();

            //await Task.Delay(6000);

            //await End();

            return Task.CompletedTask;
        }

        //public async Task Start()
        //{
        //    _eatTimer = new Timer(e => Eat(), null, 0, 200);
        //    await Task.CompletedTask;
        //}

        //public async Task End()
        //{
        //    _eatTimer.Dispose();
        //    await Task.CompletedTask;
        //}

        //private void Eat()
        //{
        //    Console.WriteLine($"Chicken {this.GetGrainIdentity()} pecked!");
        //}
    }
}
