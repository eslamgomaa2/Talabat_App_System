namespace Domin.webhook
{
    public class PaymobObj
    {
        public int Transactionid { get; set; }
        public bool success { get; set; }
        public int amount_cents { get; set; }
        public string currency { get; set; }
        public PaymobOrder order { get; set; }
        public PaymobSourceData source_data { get; set; }
        public string created_at { get; set; }
    }
}
