using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class Resaurant_Owner
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string? FName { get; set; }
        [Required]
        [MaxLength(15)]
        public string? LName { get; set; }
        [Required]
        [MaxLength(11)]
        public string? Phone_Numbber { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [ForeignKey("User")]
        [Required]
        public string? UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        [JsonIgnore]
        public ICollection<Restaurant>? Restaurants { get; set; } 
    }
}
