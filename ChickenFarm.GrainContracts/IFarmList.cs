using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.GrainContracts
{
    public interface IFarmList : IGrainWithGuidKey
    {
        Task Reset();

        Task AddFarmId(Guid farmId);

        Task<List<Guid>> GetList();
    }
}
