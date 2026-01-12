using Domin.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.DTOS.DTO
{
    public class OrderDto
    {
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }
        [Required]
        public int DeliveryAddressId { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
