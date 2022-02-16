using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Models
{
    public class Drone
    {
        [Key]
        public int DroneId { get; set; }

        [MaxLength(100)]
        public string SerialNumber { get; set; }

        public int Model { get; set; }

        public  double WeightLimit { get; set; }

        public double BatteryCapacity { get; set; }
        public int State { get; set; }
        public double BatteryLevel { get; set; }



    }





}
