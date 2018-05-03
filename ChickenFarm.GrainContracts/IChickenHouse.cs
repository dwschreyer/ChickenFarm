﻿using Orleans;
using System;
using System.Threading.Tasks;

namespace ChickenFarm.GrainContracts
{
    public interface IChickenHouse : IGrainWithGuidKey
    {
        Task Initialise(int size);
    }
}
