using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.DTOS.DTO
{
    public class AddressDTO
    {
        [Required]
        [MaxLength(255)]
        public string? AddressLine1 { get; set; }

        [MaxLength(255)]
        public string? AddressLine2 { get; set; }

        [Required]
        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [Required]
        [MaxLength(20)]
        public string? PostalCode { get; set; }

        
        [MaxLength(100)]
        public string? Country { get; set; }

    }
}
