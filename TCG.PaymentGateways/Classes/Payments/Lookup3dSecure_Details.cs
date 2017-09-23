using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class Lookup3dSecure_Details
    {
        public Sale_Details SaleData { get; set; }
        public string TransactionToken { get; set; }
        public bool isPayment { get; set; }
        public bool isOnceoff { get; set; }
        public string ProviderToken { get; set; }
        public string ProviderPIN { get; set; }
        public string CVV { get; set; }
        public string ExternalIdentifier1 { get; set; }
        public string ExternalIdentifier2 { get; set; }
        public string ExternalIdentifier3 { get; set; }
        public string ExternalIdentifier4 { get; set; }
        public string ExternalIdentifier5 { get; set; }
        
    }
}
