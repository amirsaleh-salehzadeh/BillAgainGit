using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Sale_Release_Result
    {
        public string TransactionIdentifier { get; set; }

        public bool isReleaseSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }
    }
}
