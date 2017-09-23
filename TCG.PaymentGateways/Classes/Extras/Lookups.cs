using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TCG.PaymentGateways
{
    public static class Lookups
    {
        public static Dictionary<string, string> DefaultBranchCodes = new Dictionary<string, string>
        {
            {
                "ABSA",
                "632005"
            },
            {
                "African Bank",
                "430000"
            },
            {
                "Albaraka Bank",
                "800000"
            },
            {
                "Capitec Bank",
                "470010"
            },
            {
                "First National Bank",
                "250655"
            },
            {
                "FNB",
                "250655"
            },
            {
                "HSBC",
                "587000"
            },
            {
                "Investec Bank",
                "580105"
            },
            {
                "MTN Banking",
                "490991"
            },
            {
                "Nedbank",
                "198765"
            },
            {
                "Postbank",
                "460005"
            },
            {
                "Sasfin",
                "683000"
            },
            {
                "Standard Bank",
                "051001"
            },
            {
                "Std Bank",
                "051001"
            },
            {
                "Standard Chartered Bank",
                "730045"
            },
            {
                "State Bank of India",
                "801000"
            },
            {
                "VBS Mutual",
                "588000"
            }
        };
        
    }
}
