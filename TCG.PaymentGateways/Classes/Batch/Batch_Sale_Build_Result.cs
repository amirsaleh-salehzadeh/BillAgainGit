using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Sale_Build_Result
    {
        public string TransactionIdentifier { get; set; } //used in case where validations are done during build

        public bool isBuildSuccess { get; set; } //will be false if validation is done during build and it fails

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string RequestParams { get; set; }
        public string RequestFile { get; set; }
        public string ResponseFile { get; set; } //used in case where validations are done during build
    }
}
