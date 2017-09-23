using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Sale_Line
    {
        public string LineIdentifier { get; set; }

        public string ExtRef { get; set; }
        public string MerchantExtRef { get; set; }

        public decimal Amount { get; set; }
        public string Ref { get; set; }
        public string Memo { get; set; }

        public string BankBranch { get; set; }
        public string BankAccountType { get; set; }
        public string Bank { get; set; }

        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }

        public string CardBankType { get; set; }
        public string CardBankName { get; set; }
        public string CardBankNumber { get; set; }
        public string Token { get; set; }
        public string TokenPIN { get; set; }

        public bool isCreditCard { get; set; }
    }

    //public class Batch_Sale_Build_Details_line
    //{
    //    //customer specifics
    //    public string CustomerIdentifier { get; set; }
    //    public string CustomerFullName { get; set; }

    //    //line specifics
    //    public string CurrencyCode { get; set; }
    //    public decimal Amount { get; set; }
    //    public string LineIdentifier { get; set; }
    //    public bool isCreditCardLine { get; set; } //important since if true and no token is provided it needs to tokenise even if it means once off tokenisation

    //    public string AccDisplayReference { get; set; }

    //    //customer bank account details
    //    public string BankName { get; set; }
    //    public string BankBranchCode { get; set; }

    //    public string BankAccountType { get; set; }
    //    public string BankAccountName { get; set; }
    //    public string BankAccountNumber { get; set; }

    //    public string BankAccountPrimaryToken { get; set; } //if required
    //    public string BankAccountSecondaryToken { get; set; } //if required

    //    //credit card details
    //    public CardTypeEnum CardType { get; set; }

    //    public string CardHolderName { get; set; }
    //    public string CardHolderSurname { get; set; }
    //    public string CardHolderFullName
    //    {
    //        get
    //        {
    //            return CardHolderName + " " + CardHolderSurname;
    //        }
    //    }

    //    public string CardNumber { get; set; }
    //    //public string CardCVV { get; set; }

    //    public int CardExpiryMonth { get; set; }
    //    public int CardExpiryYear { get; set; }

    //    public string CardToken { get; set; } //if required
    //    public string CardPIN { get; set; } //if required

    //    public static Batch_Sale_Build_Details_line newBankLine(string CustomerIdentifier, string CustomerFullName,
    //        string CurrencyCode, decimal Amt, string LineIdentifier,
    //        string BankName, string BankBranchCode, string BankAccountType, string BankAccountName, string BankAccountNumber)
    //    {
    //        return new Batch_Sale_Build_Details_line
    //        {
    //            //customer specifics
    //            CustomerIdentifier = CustomerIdentifier,
    //            CustomerFullName = CustomerFullName,

    //            //line specifics
    //            CurrencyCode = CurrencyCode,
    //            Amount = Amt,
    //            LineIdentifier = LineIdentifier,
    //            isCreditCardLine = false,

    //            AccDisplayReference = "",

    //            //customer bank account details
    //            BankName = BankName,
    //            BankBranchCode = BankBranchCode,

    //            BankAccountType = BankAccountType,
    //            BankAccountName = BankAccountName,
    //            BankAccountNumber = BankAccountNumber,

    //            BankAccountPrimaryToken = "",
    //            BankAccountSecondaryToken = "",
    //        };
    //    }

    //    public static Batch_Sale_Build_Details_line newCCLine(string CustomerIdentifier, string CustomerFullName,
    //        string CurrencyCode, decimal Amt, string LineIdentifier,
    //        CardTypeEnum CardType, string CardHolderName, string CardHolderSurname, string CardNumber, int CardExpiryMonth, int CardExpiryYear,
    //        string CardToken, string CardPIN)
    //    {
    //        return new Batch_Sale_Build_Details_line
    //        {
    //            //customer specifics
    //            CustomerIdentifier = CustomerIdentifier,
    //            CustomerFullName = CustomerFullName,

    //            //line specifics
    //            CurrencyCode = CurrencyCode,
    //            Amount = Amt,
    //            LineIdentifier = LineIdentifier,
    //            isCreditCardLine = false,

    //            AccDisplayReference = "",

    //            //credit card details
    //            CardType = CardType,
    //            CardHolderName = CardHolderName,
    //            CardHolderSurname = CardHolderSurname,
    //            CardNumber = CardNumber,
    //            CardExpiryMonth = CardExpiryMonth,
    //            CardExpiryYear = CardExpiryYear,

    //            CardToken = CardToken,
    //            CardPIN = CardPIN,
    //        };
    //    }
    //}

    public class Batch_Sale_Build_Details
    {
        //the batch 
        public string BatchIdentifier { get; set; }
        public DateTimeOffset ActionDate { get; set; }
        public string BatchCurrency { get; set; }
        public List<Batch_Sale_Line> Lines { get; set; }
        public List<DateTime> InvalidActionDates { get; set; }

        //the items / lines
        //public List<Batch_Sale_Build_Details_line> Items { get; set; }

        public void ComputeMerchantExtRefs(bool allowsAlphaNumeric = true, int charlimit = -1, bool isReqFixedCharLimit = false, bool isExtRefUniqueWithinBatch = true)
        {
            if (allowsAlphaNumeric == false)
            {
                throw new NotImplementedException();
            }

            foreach (var line in Lines)
            {
                // Set Merchant Ext Ref
                if (string.IsNullOrEmpty(line.ExtRef))
                    line.MerchantExtRef = line.LineIdentifier;
                else
                    line.MerchantExtRef = line.ExtRef;

                // apply char limit
                if (charlimit > 0)
                    if (line.MerchantExtRef.Length > charlimit)
                        line.MerchantExtRef.Substring(0, charlimit);
            }

            // Check for uniqueness
            if (isExtRefUniqueWithinBatch)
            {
                var duplicates = Lines.GroupBy(n => n.ExtRef).Where(r => r.Count() > 1);

                if (duplicates.Count() > 0)
                {
                    // list of unique items
                    var uniqueValues = Lines.GroupBy(n => n.ExtRef).Select(r => r.Key).ToDictionary(k => k);

                    // deal with duplicates
                    foreach (var item in duplicates)
                    {
                        var dup = item.ToList();

                        // fix second onwards record
                        for (int i = 1; i < dup.Count; i++)
                        {
                            // make unique
                            var line = dup[i];

                            var newRef = createNewRef(line.MerchantExtRef, ref uniqueValues, charlimit);

                            line.MerchantExtRef = newRef;
                            uniqueValues.Add(newRef, newRef);
                        }
                    }
                }
            }

            if (charlimit > 0 && isReqFixedCharLimit)
            {
                // prepend 0's
                foreach (var line in Lines)
                {
                    while (line.MerchantExtRef.Length < charlimit)
                        line.MerchantExtRef = "0" + line.MerchantExtRef;
                }
            }

            // possibly recheck uniqueness
        }

        private string createNewRef(string OldRef, ref Dictionary<string, string> uniqueValues, int charlimit)
        {
            string newRef;

            // try adding up to 3 number
            for (int i = 1; i < 1000; i++)
            {
                newRef = OldRef + "-" + i;
                if (charlimit > 0)
                {
                    // has a char limit
                    if (newRef.Length > charlimit)
                    {
                        // exceeds limit
                        break;
                    }
                }

                // already in list - continue
                if (uniqueValues.ContainsKey(newRef))
                    continue;

                // return new unique value
                return newRef;
            }

            // replace with guid
            for (int i = 0; i < 100; i++)
			{
                newRef = Guid.NewGuid().ToString();

                if (charlimit > 0)
                    if (newRef.Length > charlimit)
                        newRef.Substring(0, charlimit);

                // already in list - continue
                if (uniqueValues.ContainsKey(newRef))
                    continue;

                // return new unique value
                return newRef;
            }

            // Could not find an optimal solution - Exception
            throw new FormatException("No Unique Reference possible");
        }
    }
}
