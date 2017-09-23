using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Sale_Recon_Result_batch_line
    {
        //customer specifics
        public bool isLineSuccess { get; set; } //here in case both error and success lines are provided
        public bool isDebitedWithError { get; set; } //used when debit has been done even though there was error
        public string LineIdentifier { get; set; }
        public string CustomerIdentifier { get; set; }
        public int LineNumber { get; set; }
        public string LineError { get; set; }
        public decimal Amount { get; set; }

        public string BatchIdentifier { get; set; } //for grouping purposes
    }

    public class Batch_Sale_Recon_Result_batch
    {
        public bool isBatchSuccess { get; set; }
        public string BatchIdentifier { get; set; }

        public DateTime BatchDate { get; set; }
        public int NumberOfRecords { get; set; }
        public decimal Total { get; set; }

        public List<Batch_Sale_Recon_Result_batch_line> lines { get; set; }
    }

    public class Batch_Sale_Recon_Result
    {
        public bool isAsync { get; set; }
        public bool isUnmappedData { get; set; } //will only apply if an explicit mapping does not exist
        public List<Dictionary<string, string>> rawRowValues { get; set; } //will only apply if an explicit mapping does not exist

        public string TransactionIdentifier { get; set; }

        public bool isRequestSuccess {get;set;}

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }

        //the batches
        public List<Batch_Sale_Recon_Result_batch> Batches { get; set; }
    }
}
