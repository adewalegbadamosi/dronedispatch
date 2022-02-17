using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.ViewModels
{
    public class DroneAvailability
    {
        public int droneId{ get; set; }
        public string availability { get; set; }
        public int Model { get; set; }

        public double WeightLimit { get; set; }


    }
}
