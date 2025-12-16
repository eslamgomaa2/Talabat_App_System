using Domin.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("order")]
        public int OrderId { get; set; }
        [Required]
        public int? PaymobTransactionId { get; set; } 
        [Required]
        public int? PaymobOrderId { get; set; } 
        [Required]
        
        public decimal Amount { get; set; }
        [Required]
        public string? Currency { get; set; } = "EGP"; 
        [Required]
        public PaymentStatus Status { get; set; }
        [Required]
        public TypeOfPaymentMethod PaymentMethod { get; set; }
        public string PaymentGateway { get; set; } = "Paymob";


        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public virtual Order? Order { get; set; }
    }
}
