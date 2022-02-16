using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Enums
{
    //public enum LoanType
    //{
    //    [Description("Working Capital")]
    //    WorkingCapital = 1,

    //    [Description("Assest Finance")]
    //    AssestFinance = 2,

    //    [Description("Temporary Overdraft")]
    //    TemporaryOverdraft = 3,
    //}

    public enum DroneModels
    {
        
        Lightweight = 1,
        Middleweight = 2,
        Cruiserweight = 3,
        Heavyweight = 4,
      
    }

    public enum DroneState
    {
        Idle = 0,
        Loading = 1,
        Loaded = 2,
        Delivering = 3,
        Delivered = 4,
        Returning = 5
    }


    
}
