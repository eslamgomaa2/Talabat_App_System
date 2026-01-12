using System.ComponentModel.DataAnnotations;

namespace Domin.DTOS.DTO
{
    public class RestaurantDTO
    {
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

    }
}
