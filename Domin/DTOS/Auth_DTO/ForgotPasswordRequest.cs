using System.ComponentModel.DataAnnotations;

namespace Domin.DTOS.Auth_DTO
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
