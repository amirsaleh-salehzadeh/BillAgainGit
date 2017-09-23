using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes
{
    [Serializable()]
    public enum CardTypeEnum
    {
        [Description("Visa")]
        VISA,
        [Description("Mastercard")]
        MASTERCARD,
        [Description("American Express")]
        AMERICAN_EXPRESS,
        [Description("Diners Club")]
        DINERS_CLUB,
        [Description("Discover")]
        DISCOVER,
        [Description("JCB")]
        JCB,
        [Description("Dankort")]
        DANKORT,
        [Description("Maestro")]
        MAESTRO,
        [Description("Forbrugsforeningen")]
        FORBRUGSFORENINGEN,
        [Description("Unknown")]
        Unknown
    }
}
