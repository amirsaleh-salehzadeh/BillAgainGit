using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Verify_Build_Details_Item
    {
        public string LineIdentifier { get; set; }
        public string CustomerIdentifier { get; set; }

        //customer bank account details
        public string BankName { get; set; }
        public string BankBranchCode { get; set; }

        public string BankAccountType { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }

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

    public class Batch_Verify_Build_Details
    {
        public string BatchIdentifier { get; set; }
        public List<Batch_Verify_Build_Details_Item> items = new List<Batch_Verify_Build_Details_Item>();
    }
}
