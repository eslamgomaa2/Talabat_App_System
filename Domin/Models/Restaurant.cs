using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class Restaurant
    {

        [Key]
        public int RestaurantId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [MaxLength(100)]
        public string? CuisineType { get; set; }

        [Required]
        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }  
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("resaurant_Owner")]
        public int OwnerId { get; set; }

        // Navigation properties
        public ICollection<Address>? Address { get; set; }
        public ICollection<Dish>? Dishes { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public Resaurant_Owner? resaurant_Owner { get; set; }
    }
}
