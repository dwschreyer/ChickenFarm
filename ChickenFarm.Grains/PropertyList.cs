using ChickenFarm.GrainContracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.Grains
{
    public class PropertyList : Grain, IPropertyList
    {
        private List<Guid> _propertyListIds = new List<Guid>();

        public async Task Reset()
        {
            _propertyListIds = new List<Guid>();
        }

        public Task AddPropertyId(Guid propertyId)
        {
            _propertyListIds.Add(propertyId);
            return Task.CompletedTask;
        }

        public Task<List<Guid>> GetList()
        {
            return Task.FromResult(_propertyListIds);
        }
    }
}
