using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.ViewModels.DTOs
{
    public class MedicationReturnDTO
    {
        //public int medicationId { get; set; }
       
        public string name { get; set; }
        public int weight { get; set; }
        
        public string code { get; set; }

        //public string imageName { get; set; }
        //public string imageType { get; set; }
        public Byte[] image { get; set; }

        public int droneId { get; set; }
        public string deliveryStatus { get; set; }

        public DateTime dateTimeCreated { get; set; } 
    }
}
