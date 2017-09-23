using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_Sale_UploadReport_Result_batch_line
    {
        //customer specifics
        public bool isLineSuccess { get; set; } //here in case both error and success lines are provided
        public string LineIdentifier { get; set; }
        public int LineNumber { get; set; }
        public string LineError { get; set; }
    }

    public class Batch_Sale_UploadReport_Result_batch
    {
        public bool isBatchSuccess { get; set; }
        public string BatchIdentifier { get; set; }

        public List<Batch_Sale_UploadReport_Result_batch_line> lines { get; set; }
    }

    public class Batch_Sale_UploadReport_Result
    {
        public string TransactionIdentifier { get; set; }

        public bool isRequestSuccess { get; set; }

        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        //the batches
        public List<Batch_Sale_UploadReport_Result_batch> Batches { get; set; }
    }
}
