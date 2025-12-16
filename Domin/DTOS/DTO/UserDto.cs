using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.DTOS.DTO
{
    public class UserDto
    {
        [Required]
        [MaxLength(9)]
        public string? FName { get; set; }
        [Required]
        [MaxLength(10)]
        public string? LName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }


    }
}
