using System.ComponentModel.DataAnnotations;

namespace Domin.DTOS.Auth_DTO
{
    public class ExternalLoginRequest
    {
        [Required]
        public string? IdToken { get; set; }
    }
}