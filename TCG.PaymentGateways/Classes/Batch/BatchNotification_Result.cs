using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    [Serializable()]
    public class BatchNotification_Result
    {
        public bool downloadSuccess { get; set; }
        public string errorDesc { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string DebitAmount { get; set; }
        public string ActionDate { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        public string RejectionCode { get; set; }
        public string RejectionReason { get; set; }
    }
}
