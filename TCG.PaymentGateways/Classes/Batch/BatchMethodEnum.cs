using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    [Serializable()]
    public enum BatchMethodEnum
    {
        DEBITORDER,
        ACH,
        SEPA,
    }
}
