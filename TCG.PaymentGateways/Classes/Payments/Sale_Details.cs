using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
     [Serializable()]
    public class Sale_Details
    {
        // Tracking Field
        public long accountID { get; set; }
        public long appID { get; set; }
        //public long customerID { get; set; }
        //public string customerReference { get; set; }
        public long transactionID { get; set; }
        //public string InvoiceNumber { get; set; }
        public string ExtRef { get; set; }

        // Transaction Info
        public string CardHolderName { get; set; }
        public string CardHolderLastName { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string CardCCV { get; set; }
        public CardTypeEnum CardType { get; set; }
        public decimal Amount { get; set; }
        public string ProviderToken { get; set; }
        public string ProviderPIN { get; set; }

        public string CurrencyCode { get; set; }

        public string ProductDescription { get; set; }

         // Customer Info
        public string CustomerIdentifier { get; set; }
        public string IPAddress { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerCountryCodeTwoLetter { get; set; }
        public string CustomerCountryCodeThreeLetter { get; set; }
        public string CustomerCountryNumeric { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZip { get; set; }

        //3D Secure fields
        //public bool is3dSecureUsed { get; set; }
        //public Lookup3dSecure_Result lookup;
        //public Authenticate3dSecure_Result authResult;
        //public string AcsReturnUrl { get; set; }
        //public string CardHolderAuthenticationData { get; set; }
        //public string CardHolderAuthenticationID { get; set; }
        //public string ElectronicCommerceIndicator { get; set; }
    }
}
