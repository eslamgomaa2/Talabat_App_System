using System.ComponentModel.DataAnnotations;

namespace Domin.DTOS.Auth_DTO
{
    public class AuthenticationRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
