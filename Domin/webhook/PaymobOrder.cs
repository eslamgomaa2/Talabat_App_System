namespace Domin.webhook
{
    public class PaymobOrder
    {
        public int id { get; set; }                // رقم الأوردر في Paymob
        public string merchant_order_id { get; set; } // رقم الأوردر في نظامك (اختياري)
        public string created_at { get; set; }     // تا
    }
}
