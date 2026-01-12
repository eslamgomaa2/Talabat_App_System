using Domin.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class DeliveryDetail
    {
        [Key]
        public int DeliveryId { get; set; }
        [Required]

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required]

        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        public DateTime? PickupTime { get; set; }
        public DateTime? DeliveredTime { get; set; }

        public DeliveryStatus Status { get; set; } = DeliveryStatus.Assigned;



        // Navigation properties
        public Order? Order { get; set; }
        public Driver? Driver { get; set; }
    }
}
