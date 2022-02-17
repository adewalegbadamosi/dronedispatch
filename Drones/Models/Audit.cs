using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Models
{
    public class Audit
    {
        [Key]
        public int AuditId { get; set; }
        public string AuditType { get; set; }        
        public string CurrentTask { get; set; }
        public string Detail { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.Now;

    }
}
