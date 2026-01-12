using System.ComponentModel.DataAnnotations;

namespace Domin.DTOS.Auth_DTO
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}
