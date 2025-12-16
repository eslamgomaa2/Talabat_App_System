using Domin.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.DTOS.DTO
{
    public class DriverDto
    {
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

    }
}
