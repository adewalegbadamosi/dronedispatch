using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9'_'-'\s]$")]   //(allowed only letters, numbers, ‘-‘, ‘_’);
        public string Name { get; set; }
        public  int Weight { get; set; }
        [RegularExpression(@"^[A-Z0-9\_]$")]   // (allowed only upper case letters, underscore and numbers);
        public string Code { get; set; }

        public byte[]  Image { get; set; }

        //blic string Image { get; set; }

        public int DroneId { get; set; }


    }




}
