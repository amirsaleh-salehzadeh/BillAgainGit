using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class Authenticate3dSecure_Result
    {
        public bool isSuccess { get; set; }
        public bool isEnrolled { get; set; }
        public bool isNotSupported { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string CardToken { get; set; }
        public string TransactionIdentifier { get; set; } //AKA UTI
        public string IPAddress { get; set; }

        public string EnrollmentStatus { get; set; }
        public string ThreeDSecureEci { get; set; }
        public string ThreeDSecureXid { get; set; }
        public string ThreeDSecureCavv { get; set; }
        public string RetrievalReferenceNumber { get; set; }
        
        public string CustomResponse { get; set; }

        public string TransactionToken { get; set; }
        public string ExternalIdentifier1 { get; set; }
        public string ExternalIdentifier2 { get; set; }
        public string ExternalIdentifier3 { get; set; }
        public string ExternalIdentifier4 { get; set; }
        public string ExternalIdentifier5 { get; set; }

        public string FullResult { get; set; }

    }
}
