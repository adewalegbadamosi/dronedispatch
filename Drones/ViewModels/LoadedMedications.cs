using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.ViewModels
{
    public class LoadedMedications
    {
        public int medicationId { get; set; }
        public string name { get; set; }
        public int weight { get; set; }        
        public string code { get; set; }
        //public byte[] image { get; set; }

        public string Image { get; set; }

        public int droneId { get; set; }
        //public string droneSerialNumber { get; set; }

        //public int droneModel { get; set; }

       
    }
}
