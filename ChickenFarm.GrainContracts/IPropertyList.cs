using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChickenFarm.GrainContracts
{
    public interface IPropertyList : IGrainWithGuidKey
    {
        Task Reset();

        Task AddPropertyId(Guid propertyId);

        Task<List<Guid>> GetList();
    }
}
