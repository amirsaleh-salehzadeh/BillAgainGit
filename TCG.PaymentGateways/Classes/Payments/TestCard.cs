using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class TestCard
    {
        public bool isSuccess { get; set; }
        public string Note { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Token { get; set; }
        public string Ref { get; set; }

        public TestCard(bool isSuccess, string Note, string Name, string Surname, string Number, int ExpiryMonth, int ExpiryYear, decimal Amount, string Currency, string Token, string Ref)
        {
            this.isSuccess = isSuccess;
            this.Note = Note;

            this.Name = Name;
            this.Surname = Surname;
            this.Number = Number;
            this.ExpiryMonth = ExpiryMonth;
            this.ExpiryYear = ExpiryYear;
            this.Amount = Amount;
            this.Currency = Currency;
            this.Token = Token;
            this.Ref = Ref;
        }
    }
}
