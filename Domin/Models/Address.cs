using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domin.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

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

        [Required]
        [MaxLength(100)]
        public string? Country { get; set; }
        [ForeignKey("User")]
        [Required]
        public string? UserId { get; set; }

        [ForeignKey("Restaurant")]
        [Required]
        public int? RestaurantId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}
