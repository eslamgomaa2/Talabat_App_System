using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.DTOS.Auth_DTO
{
    public class Register:RegisterAsDriver
    {
        [Required]
        [MaxLength(8)]
        public string? FName { get; set; }
        [Required]
        [MaxLength(20)]
        public string? LName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNUmber { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
