using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class Credit_Details
    {
        // Tracking Field
        public long accountID { get; set; }
        public long customerID { get; set; }
        public long transactionID { get; set; }
        public string InvoiceNumber { get; set; }

        // Transaction Info
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string CardCCV { get; set; }
        public CardTypeEnum CardType { get; set; }
        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }
        public string CurrencyCodeNumeric { get; set; }

        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
    }
}
