using Domin.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        public Vehicles VehicleType { get; set; }

        [Required]
        [MaxLength(100)]
        public string? VehicleRegistration { get; set; }

        public bool IsAvailable { get; set; } = true;
        [Required]
        public DateTime CreatedAt { get; set; }
        public ICollection<DeliveryDetail>? DeliveryDetails { get; set; }



    }
}
