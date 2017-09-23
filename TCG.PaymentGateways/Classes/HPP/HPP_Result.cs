using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.HPP
{
    public class HPP_Result
    {
        //our data - somehow embed into response from gateway
        public string reference { get; set; } //the reference we use in our system

        //gateway data
        public string orderID { get; set; } //the transaction identifier on gateways system
        public string processorID { get; set; } //any external processors identification reference
        public string merchantID { get; set; } //the gateways merchantID
        
        //result data
        public string status { get; set; } //text status
        public bool isSuccessful { get; set; } //whether successful or not
        public bool isPending { get; set; } //whether pending or not
        
        public bool hasError { get; set; } //whether has error or not
        public string errorReason { get; set; } //failure reason 
        
        public string fullResult { get; set; } //complete result
        
        //validation data
        public string checksum { get; set; }
        public string parity { get; set; }

        public string RecurrenceToken { get; set; }
    }
}
