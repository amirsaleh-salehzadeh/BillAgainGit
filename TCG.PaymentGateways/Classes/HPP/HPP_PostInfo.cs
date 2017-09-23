using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.HPP
{
    public class HPP_PostInfo
    {
        public long customerID { get; set; }
        public string ExternalUrl { get; set; }
        public string PostData { get; set; }
    }
}
