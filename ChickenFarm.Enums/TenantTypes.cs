using System;
using System.ComponentModel;

namespace ChickenFarm.Enums
{
    [Flags]
    public enum TenantTypes
    {
        [Description("An unclassified tenant.")]
        None = 0,

        [Description("A commercial business.")]
        Company = 1,

        [Description("An association of companies.")]
        Consortium = 2,

        [Description("A seller of companies.")]
        Broker = 4
    }
}
