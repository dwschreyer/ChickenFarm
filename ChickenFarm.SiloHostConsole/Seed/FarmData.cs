using System;
using System.Collections.Generic;
using System.Text;

namespace ChickenFarm.SiloHostConsole.Seed
{
    public class FarmData
    {
        public List<FarmInfo> Farms { get; set; }
        public List<HouseInfo> Houses { get; set; }
    }

    public class FarmInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class HouseInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
    }
}
