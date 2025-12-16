using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Helper
{
    public class PaymobSettings
    {
        public string ApiKey { get; set; }
        public string IntegrationId { get; set; }
        public string HmacHash { get; set; }
        public string IframeId { get; set; }
        public string BaseUrl { get; set; }
    }
}
