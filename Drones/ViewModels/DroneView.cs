using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.ViewModels
{
    public class DroneView
    {
                
        public string serialNumber { get; set; }

        public int model { get; set; }
        
        //public double weightLimit { get; set; }

        public double batteryCapacity { get; set; }       

    }
}
