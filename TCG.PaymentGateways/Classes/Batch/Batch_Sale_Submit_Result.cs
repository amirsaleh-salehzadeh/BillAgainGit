using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Sale_Submit_Result
    {
        public string TransactionIdentifier { get; set; } //use in the case of code release

        public bool isSubmitSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }
    }
}
