using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class RevokePaymentMethod_Details
    {
        public string ClientIdentifier { get; set; }
        public string CardToken { get; set; } //if required
        public string CardPIN { get; set; } //if required
    }
}
