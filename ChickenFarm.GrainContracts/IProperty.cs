﻿using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.GrainContracts
{
    public interface IProperty : IGrainWithGuidKey
    {
        Task Initialise(string name);

        Task<string> GetName();
    }
}
