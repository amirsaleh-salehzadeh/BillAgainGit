using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class AuthCapture_Details
    {
        public string TransactionIndex { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; }

        public string CardNumber { get; set; }
        public int ExpYear { get; set; }
        public int ExpMonth { get; set; }
        public string CVV { get; set; }
    }
}
