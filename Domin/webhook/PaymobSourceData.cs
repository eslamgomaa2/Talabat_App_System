namespace Domin.webhook
{
    public class PaymobSourceData
    {

        public string pan { get; set; }            // آخر 4 أرقام من الكارت
        public string sub_type { get; set; }       // نوع الكارت (MasterCard, Visa)
        public string type { get; set; }           // طريقة الدفع (card, wallet)
    }
}
