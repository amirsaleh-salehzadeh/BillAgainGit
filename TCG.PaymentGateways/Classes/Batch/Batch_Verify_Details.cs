using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Verify_Details
    {
        public string TransactionIdentifier { get; set; }
        public string CSVString { get; set; } //used to process the verify recon when in csv format
    }
}
