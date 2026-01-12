using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.DTOS.DTO
{
    public class OrderItemDto
    {

        public int DishId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceAtOrder { get; set; }

    }
}
