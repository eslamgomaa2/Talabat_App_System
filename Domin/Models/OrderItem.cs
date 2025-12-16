using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{


    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Dish")]
        public int DishId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceAtOrder { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Order? Order { get; set; }
        public Dish? Dish { get; set; }
    }
}
