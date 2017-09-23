using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    [Serializable()]
    public class BatchNotifications_Details
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string BatchRef { get; set; } //for gateways that take in specific batch reference to get results for specific batch
    }
}
