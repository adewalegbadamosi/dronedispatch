using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.ViewModels.DTOs
{
    public class DroneDTO
    {
        public int droneId { get; set; }
        public string serialNumber { get; set; }

        public int model { get; set; }
        public string modelName { get; set; }
        public double weightLimit { get; set; }

        public double batteryCapacity { get; set; }
        public int state { get; set; }

        public string stateName { get; set; }
        public double batteryLevel { get; set; }

    }
}
