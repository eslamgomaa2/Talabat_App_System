using Domin.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class Order
    {

        [Key]
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Restaurant? Restaurant { get; set; }
        public Address? Address { get; set; }
        public ICollection<OrderItem>? OrderItem { get; set; }
        public ICollection<PaymentTransaction>? PaymentTransactions { get; set; }
        public DeliveryDetail? DeliveryDetails { get; set; }
    }
}

