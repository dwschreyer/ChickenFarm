using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChickenFarm.Grains
{
    public class Tenant : Grain, ITenant
    {
    }
}
