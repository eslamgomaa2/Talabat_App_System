using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.DTOS.DTO
{
    public struct DishDto
    {
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }
        public bool IsAvailable { get; set; }

        public IFormFile? ImageFile { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
