using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class StorePaymentMethod_Details
    {
        public string ClientIdentifier { get; set; }

        //credit card details
        public CardTypeEnum CardType { get; set; }

        public string CardHolderName { get; set; }
        public string CardHolderSurname { get; set; }
        public string CardHolderFullName
        {
            get
            {
                return CardHolderName + " " + CardHolderSurname;
            }
        }

        public string CardNumber { get; set; }
        public string CardCVV { get; set; }

        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
    }
}
