using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.HPP
{
    public class HPP_Details
    {
        public long CustomerID { get; set; }
        public long customerID { get; set; }

        public string CurrencyCode { get; set; }
        public string CurrencyCodeNumeric { get; set; }

        public string FailUrl { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
        public string CompleteUrl { get; set; }

        public string TransactionReference { get; set; }

        public string ItemDescription { get; set; }
        public decimal ItemAmount { get; set; }
        public int ItemQuantity { get; set; }

        public NameValueCollection ExtraParams { get; set; }
    }
}
