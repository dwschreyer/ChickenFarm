using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class FarmList : Grain, IFarmList
    {
        private List<Guid> _farmListIds = new List<Guid>();

        public async Task Reset()
        {
            _farmListIds = new List<Guid>();
        }

        public Task AddFarmId(Guid farmId)
        {
            _farmListIds.Add(farmId);
            return Task.CompletedTask;
        }

        public Task<List<Guid>> GetList()
        {
            return Task.FromResult(_farmListIds);
        }
    }
}
