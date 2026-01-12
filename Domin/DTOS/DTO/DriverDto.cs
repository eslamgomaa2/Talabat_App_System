using Domin.Enum;
using System.ComponentModel.DataAnnotations;

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
