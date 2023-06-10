using Drones.Enums;
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

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 _-]+$", ErrorMessage = "Invalid!. Name may only contain letters, numbers, ‘-‘, ‘_’")]   //(allowed only letters, numbers, ‘-‘, ‘_’);       
        public string Name { get; set; }

        [Required]
        public int Weight { get; set; }
        [RegularExpression(@"^([A-Z0-9_]+)$", ErrorMessage = "Invalid!. Code may only contain upper case letters,underscore and numbers")]   // (allowed only upper case letters, underscore and numbers);

        [Required]
        public string Code { get; set; }
             
        public Byte[]  Image { get; set; }

        [Required]
        public int DroneId { get; set; }

        public int DeliveryStatus { get; set; } = (int)MedicationDeliveryStatus.Loaded;        

        public DateTime DateTimeCreated { get; set; } = DateTime.Now;


    }




}
