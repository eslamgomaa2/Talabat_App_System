using System.ComponentModel.DataAnnotations;

namespace Domin.paymentclasses
{
    public class PayNowRequest
    {
        [Required]
        public decimal amount { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public int orderId { get; set; }



    }
}
