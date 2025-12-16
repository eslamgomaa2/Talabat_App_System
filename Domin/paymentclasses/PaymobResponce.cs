using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.paymentclasses
{
    public class PaymobResponce
    {
        public bool Success { get; set; }
        public int Paymob_Orderid { get; set; }
        public int Amount_cents { get; set; }
        public string Currency { get; set; }
        public string Created_at { get; set; }
    }
}
