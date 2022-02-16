using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.ViewModels
{
    public class DroneViewModel
    {
                
        public string serialNumber { get; set; }

        public int model { get; set; }
        
        public double weightLimit { get; set; }

        public double batteryCapacity { get; set; }
        public int state { get; set; }
        
        

    }
}
