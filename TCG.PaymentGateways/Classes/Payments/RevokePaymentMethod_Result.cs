using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class RevokePaymentMethod_Result
    {
        public string TransactionIdentifier { get; set; }
        public bool isSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
