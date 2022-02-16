using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Models
{
    public class Audit
    {
        public decimal AuditId { get; set; }
        public string AuditType { get; set; }        
        public string CurrentTask { get; set; }
        public string Detail { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.Now;

    }
}
