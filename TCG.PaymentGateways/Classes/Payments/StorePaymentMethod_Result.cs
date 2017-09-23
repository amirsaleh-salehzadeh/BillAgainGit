using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class StorePaymentMethod_Result
    {
        public bool isSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string CardToken { get; set; } //if required
        public string CardPIN { get; set; } //if required

        public string ThreeDSecureURL { get; set; }
        public string ThreeDSecurePost { get; set; }

        public string TokeniseURL { get; set; } //used for external tokenise gateways like payfast
        public string TokenisePost { get; set; } //used for external tokenise gateways like payfast
    }
}
