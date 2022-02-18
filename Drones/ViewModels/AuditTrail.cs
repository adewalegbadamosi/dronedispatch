using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.ViewModels
{
    public class AuditTrail
    {
       
        public string auditType { get; set; }        
        public string task { get; set; }
        public string detail { get; set; }
        public int droneId { get; set; }
        public double droneBatteryLevel { get; set; }
        public DateTime timeCreated { get; set; }

    }
}
