using System.ComponentModel.DataAnnotations;

namespace Domin.DTOS.Auth_DTO
{
    public class RefreshTokenRequest
    {
        [Required]
        public string? RefreshToken { get; set; }
    }
}