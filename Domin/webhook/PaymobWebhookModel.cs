using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.webhook
{
    public class PaymobWebhookModel
    {

        public string type { get; set; }
        public PaymobObj obj { get; set; }
    }
}
