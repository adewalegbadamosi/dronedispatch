using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Enums
{
    
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

    public enum MedicationDeliveryStatus
    {       
        
        Loaded = 1,
        InTransit = 2,
        Delivered = 3,
        
    }



}
