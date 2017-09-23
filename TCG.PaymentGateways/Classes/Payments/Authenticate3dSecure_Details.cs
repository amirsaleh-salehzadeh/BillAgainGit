using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Payments
{
    public class Authenticate3dSecure_Details
    {
        public string GatewayQueryStringRaw { get; set; }
        public string GatewayPayloadRaw { get; set; }
    }
}
