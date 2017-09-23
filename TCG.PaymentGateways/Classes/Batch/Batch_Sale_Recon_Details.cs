using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Sale_Recon_Details
    {
        public string TransactionIdentifier { get; set; } //used when we recon by transaction
        public DateTimeOffset ReconStartDate { get; set; } //used when we recon by date
        public DateTimeOffset ReconEndDate { get; set; } //used when we recon by date

        public string File { get; set; } //used to process the recon when in csv format
        public byte[] FileBytes { get; set; }
        public string FileType { get; set; }
    }
}
