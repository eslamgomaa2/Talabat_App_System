using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public DateTime ExpiryDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByToken { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

        public bool IsActive => !IsRevoked && !IsExpired;
    }
}