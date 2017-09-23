using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TCG.PaymentGateways;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Batch;

namespace TCG.PaymentGateways.Providers
{
    public class gwSagePayNetCashDD : IBatchStrategy
    {
        /*
         NOTES
         * Same day service - Submit by 11am today, will be submitted today (if today a working day) - instruction F
         * Two day service - Submit by 11am today, will be submitted AFTER 2 working days have passed - instruction B
         * Can only take money from a credit card CANNOT deposit into a credit card
         */

        public string ServiceKey { get; set; }
        public string AccountServiceKey { get; set; }

        /// <summary>
        /// Either SameDay or TwoDay will be accepted
        /// </summary>
        public string ServiceType { get; set; }

        public bool isTest { get; set; }

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwSagePayNetCashDD; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "SagePay NetCash Batching",
                        WebUrl: "http://www.netcash.co.za/",
                        Description: "Sage Pay provides your business with a simple and secure way to collect money from your customers.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "ServiceKey", "ServiceType", "AccountServiceKey" },
                        Currencies: new[] { "ZAR" },
                        Countries: new[] { "ZA", },
                        CardTypes: new CardTypeEnum[]
                        {

                        }
                    );
            }
        }
        public BatchOptions batchOptions
        {
            get
            {
                return new BatchOptions
                    (
                        Bank_Batching: true,
                        CreditCard_Batching: false,

                        Bank_Batching_Type: "debit_order",

                        BankPaymentTokenize: false,
                        CCPaymentTokenize: false,
                        CCThreeDSecure: false,

                        require_BankPaymentTokenize: false,
                        require_CCPaymentTokenize: false,

                        is_NotifyPull: true,
                        is_AutoRelease: false, //manual release through external interface (false) or via our code (true)
                        is_AutoRecon: true, //manual reconciliation through external interface (false) or via code (true)

                        Verify: false,
                        Sale: true,
                        Refund: false,
                        UsesExternalIdentifier: false
                    );
            }
        }

        #endregion

        #region Methods

        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            ServiceKey = MerchantConfigValues.Where(r => r.Key.Equals("ServiceKey")).FirstOrDefault().Value;
            ServiceType = MerchantConfigValues.Where(r => r.Key.Equals("ServiceType")).FirstOrDefault().Value;
            AccountServiceKey = MerchantConfigValues.Where(r => r.Key.Equals("AccountServiceKey")).FirstOrDefault().Value;

            isTest = isTestMode;
        }

        /// <summary>
        /// Logs in using test credentials, need to pre-set BankAccountEnabled and CreditCardEnabled for stuff to work
        /// </summary>
        public void LoginTest()
        {
            isTest = true;
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="ServiceKey",
                    Value="80630e7e-64ba-46a8-a9fb-915c864dcd8d"
                },
                new MerchantConfigValue
                {
                    Key="ServiceType",
                    Value="TWODAY"
                },
                new MerchantConfigValue
                {
                    Key="AccountServiceKey",
                    Value="fd7868e3-875e-41dc-b7f0-67d052e4bf2c"
                },
            };

            Login(true, config);
        }

        public Batch_Sale_Build_Result Sale_Build(Batch_Sale_Build_Details details)
        {
            if (details.Lines.Count <= 0)
            {
                throw new Exception("Cannot create an empty batch");
            }

            //pre validation
            StringBuilder batch_csv_string_V = new StringBuilder(); //validation csv


            var fileheader = string.Format("H\t{0}\t1\t{1}\t{2}\t{3}\t24ade73c-98cf-47b3-99be-cc7b867b3080\r\n",
                                        ServiceKey, ServiceType, details.BatchIdentifier, details.ActionDate.ToString("yyyyMMdd"));

            var filekey = string.Format("K\t101\t102\t131\t132\t133\t134\t135\t136\t162\t301\t302\t303\r\n");

            StringBuilder batch_csv_string_lines = new StringBuilder();

            #region Build Lines

            foreach (var row in details.Lines)
            {
                string BankBranchCode = "0";

                if (!String.IsNullOrEmpty(row.BankBranch))
                {
                    BankBranchCode = row.BankBranch;

                    if (BankBranchCode.Length > 6)
                    {
                        throw new Exception("Branch code MUST contain 6 digits");
                    }

                    while (BankBranchCode.Length < 6)
                    {
                        BankBranchCode = "0" + BankBranchCode;
                    }
                }

                string AccRef = row.ExtRef;
                string AccName = row.CardBankName;
                string BankAccName = row.CardBankName;
                string AccType = "0";
                string BranchCode = BankBranchCode;
                string AccNum = !String.IsNullOrEmpty(row.CardBankNumber) ? row.CardBankNumber : "0";
                string ContractAmt = row.Amount >= 0 ? ((int)(row.Amount * 100)).ToString("F0") : "0";
                string BatchAmount = ContractAmt;
                string Extra1 = row.LineIdentifier; //line (paymentID)
                string Extra2 = row.ExtRef; //customer (customerID)
                string Extra3 = details.BatchIdentifier; //batch (batchID)

                if (!row.isCreditCard)
                {
                    switch (row.BankAccountType)
                    {
                        case ("Current"):
                        case ("Checking"):
                        case ("Cheque"):
                            {
                                AccType = "1";
                            }
                            break;
                        case ("Savings"):
                            {
                                AccType = "2";
                            }
                            break;
                        case ("Transmission"):
                            {
                                AccType = "3";
                            }
                            break;
                        default:
                            {
                                throw new Exception("Account Type Invalid");
                            }
                    }
                }

                var transaction = string.Format("T\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\r\n",
                                                AccRef, AccName, 1, BankAccName, AccType, BranchCode, 0, AccNum, ContractAmt, Extra1, Extra2, Extra3);

                batch_csv_string_lines.Append(transaction);
            }
            #endregion

            var filefooter = string.Format("F\t{0}\t{1}\t{2}\r\n",
                                        details.Lines.Count, details.Lines.Sum(r => r.Amount * 100).ToString("F0"), 9999);

            //validation CSV
            batch_csv_string_V.Append(fileheader);
            batch_csv_string_V.Append(filekey);
            batch_csv_string_V.Append(batch_csv_string_lines.ToString());
            batch_csv_string_V.Append(filefooter);

            return new Batch_Sale_Build_Result { RequestFile = batch_csv_string_V.ToString(), isBuildSuccess = true };
        }

        public Batch_Sale_Submit_Result Sale_Submit(Batch_Sale_Submit_Details details)
        {
            var client = getNIWSNIFClient();
            var result = client.BatchFileUpload(ServiceKey, details.RequestFile);

            var uploadReport = client.RequestFileUploadReport(ServiceKey, result);

            var isSuccess = !uploadReport.ToLower().Contains("unsuccessful");

            if (isSuccess)
            {

                return new Batch_Sale_Submit_Result { isSubmitSuccess = isSuccess, RequestXml = details.RequestFile, ResponseXml = uploadReport, TransactionIdentifier = result };
            }
            else
            {
                return new Batch_Sale_Submit_Result { ErrorMessage = uploadReport, isSubmitSuccess = isSuccess, RequestXml = details.RequestFile, ResponseXml = uploadReport, TransactionIdentifier = result };
            }
        }

        /// <summary>
        /// Converts a delimited string to a datatable
        /// </summary>
        /// <param name="text">the string to convert</param>
        /// <param name="col_delimiter">the delimiter indicating the column separation as a char eg. '\t' for tab separated</param>
        /// <param name="row_delimiter_string">the delimiters indicating the row separation as a list of strings eg. ["\r", "\r\n"] for carriage return separated</param>
        /// <returns>a datatable containing a structured form of the data</returns>
        public static System.Data.DataTable TextToTable(string text, char col_delimiter, string row_delimiter)
        {
            System.Data.DataTable DT = new System.Data.DataTable();

            string[] lines = text.Split(new string[] { row_delimiter }, StringSplitOptions.RemoveEmptyEntries);

            List<string[]> LineCells = new List<string[]>();
            int maxCells = 0; //we get max so that we can deal with variations in number of cells per row
            foreach (var line in lines)
            {
                string[] cells = line.Split(col_delimiter);
                LineCells.Add(cells);
                maxCells = cells.Length > maxCells ? cells.Length : maxCells;
            }

            // add cols
            for (int i = 0; i < maxCells; i++)
                DT.Columns.Add(i.ToString());

            foreach (var line in LineCells)
            {
                System.Data.DataRow DR = DT.NewRow();

                for (int i = 0; i < line.Length; i++)
                {
                    DR[i] = line[i];
                }

                DT.Rows.Add(DR);
            }

            return DT;
        }

        public Batch_Sale_UploadReport_Result Sale_UploadReport()
        {
            //var client = getClient();
            //var result = client.Request_UploadReport(Username, Password, Pin);

            string result = "###BEGIN \t B 20 \t UNSUCCESSFULL \t 08:40:09 \r\n ACC REF :1106711 \t LINE : 3 \t HAS A CDV ERROR NO : 1 - INVALID BRANCH CODE \r\n ###END \t 08:40:09 \r\n ###BEGIN \t B 21 \t UNSUCCESSFULL \t 08:40:09 \r\n ACC REF :1106711 \t LINE : 3 \t HAS A CDV ERROR NO : 1 - INVALID BRANCH CODE \r\n ###END \t 08:40:09 \r\n";

            System.Data.DataTable data = TextToTable(result, '\t', "\r\n"); //converts result to tabular format

            var firstrow = data.Rows[0];
            var status = firstrow[0].ToString();

            bool isRequestSuccess = !status.Contains("False");

            var return_result = new Batch_Sale_UploadReport_Result
            {
                isRequestSuccess = isRequestSuccess,
                ResponseXml = result
            };

            if (!isRequestSuccess)
            {
                return_result.ErrorCode = status;
                return_result.ErrorMessage = data.Columns.Count > 1 ? firstrow[1].ToString() : "";
            }
            else
            {
                DateTime now = DateTime.Now;

                List<System.Data.DataTable> subtables = new List<System.Data.DataTable>();
                var rows = data.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    //if begin is found
                    if (rows[i][0].ToString().Trim() == "###BEGIN")
                    {
                        System.Data.DataTable sub = new System.Data.DataTable(); //each begin / end set will be made into subtable containing the line errors 

                        for (int k = 0; k < data.Columns.Count; k++)
                            sub.Columns.Add(k.ToString());

                        sub.Rows.Add(rows[i].ItemArray); //add begin row

                        //traverse and add other rows to the subtable until we reach and end
                        for (int j = i + 1; j < rows.Count; j++)
                        {
                            string bla = rows[j][0].ToString().Trim();
                            if (rows[j][0].ToString().Trim() == "###END")
                            {
                                sub.Rows.Add(rows[j].ItemArray); //add end row
                                i = j; //set i to where it should resume i.e. it must not worry about rows that have been taken care of already (which will work even if nothing is left to process)
                                break; //break inner loop
                            }
                            else
                            {
                                sub.Rows.Add(rows[j].ItemArray); //add line
                            }
                        }

                        subtables.Add(sub);
                    }

                }

                List<Batch_Sale_UploadReport_Result_batch> batches = new List<Batch_Sale_UploadReport_Result_batch>();
                foreach (var subtable in subtables)
                {
                    var st_rows = subtable.Rows;
                    var begin_row = st_rows[0];
                    var end_row = st_rows[st_rows.Count - 1];

                    List<Batch_Sale_UploadReport_Result_batch_line> lines = new List<Batch_Sale_UploadReport_Result_batch_line>();
                    for (int i = 1; i < st_rows.Count - 1; i++) //start at 1 and end at count - 2 since we dont want first and last item (it wont run if there isnt any rows)
                    {
                        var line_row = st_rows[i].ItemArray;

                        var accRefCell = line_row.Where(r => r.ToString().Contains("ACC REF")).FirstOrDefault();
                        string accRef = "";
                        if (accRefCell != null)
                        {
                            accRef = accRefCell.ToString().Split(':')[1].Trim();
                        }

                        var lineNumCell = line_row.Where(r => r.ToString().Contains("LINE")).FirstOrDefault();
                        int lineNum = -1;
                        if (lineNumCell != null)
                        {
                            lineNum = int.Parse(lineNumCell.ToString().Split(':')[1].Trim());
                        }

                        string lineError = "";
                        var errors = line_row.Where(r => !r.ToString().Contains("ACC REF") && !r.ToString().Contains("LINE")).ToList();
                        foreach (var err in errors)
                        {
                            lineError += err.ToString() + " \t ";
                        }

                        lines.Add(new Batch_Sale_UploadReport_Result_batch_line
                        {
                            isLineSuccess = false,
                            LineIdentifier = accRef,
                            LineNumber = lineNum,
                            LineError = lineError
                        });
                    }

                    batches.Add(new Batch_Sale_UploadReport_Result_batch
                    {
                        BatchIdentifier = begin_row[1].ToString(),
                        isBatchSuccess = begin_row[2].ToString().Trim() == "SUCCESSFULL",
                        lines = lines
                    });


                }

                return_result.Batches = batches;
            }

            return return_result;
        }

        public Batch_Sale_Release_Result Sale_Release(Batch_Sale_Release_Details details)
        {
            throw new NotImplementedException();
        }

        public Batch_Sale_Recon_Result Sale_Recon(Batch_Sale_Recon_Details details)
        {

            var client = getNIWSNIFClient();
            var result = client.RequestMerchantStatement(ServiceKey, details.ReconStartDate.ToString("yyyyMMdd"));

            return new Batch_Sale_Recon_Result
            {
                isAsync = true,
                TransactionIdentifier = result
            };
        }

        public Batch_Sale_Recon_Result SaleReconComplete(string statement)
        {
            var statementdata = TextToTable(statement, '\t', "\r\n");
            List<Batch_Sale_Recon_Result_batch> Batches = new List<Batch_Sale_Recon_Result_batch>();
            List<Batch_Sale_Recon_Result_batch_line> lines = new List<Batch_Sale_Recon_Result_batch_line>();

            foreach (DataRow item in statementdata.Rows)
            {
                if (item.ItemArray[1].ToString() == "DRU")
                {
                    var extra1 = item.ItemArray[8].ToString();
                    var extra2 = item.ItemArray[9].ToString();
                    var extra3 = item.ItemArray[10].ToString();
                    var desc = item.ItemArray[3].ToString();
                    var trid = item.ItemArray[2].ToString();
                    var amount = item.ItemArray[4].ToString();

                    lines.Add(new Batch_Sale_Recon_Result_batch_line
                    {
                        BatchIdentifier = extra3,
                        CustomerIdentifier = extra2,
                        LineIdentifier = extra1,
                        isLineSuccess = false,
                        isDebitedWithError = false,
                        LineError = desc,
                        LineNumber = int.Parse(trid),
                        Amount = decimal.Parse(amount)
                    });
                }
            }

            return new Batch_Sale_Recon_Result
            {
                Batches = lines.GroupBy(r => r.BatchIdentifier).Select(r => new Batch_Sale_Recon_Result_batch { BatchIdentifier = r.Key, lines = lines }).ToList(),
                isUnmappedData = true,
                ResponseXml = statement
            };
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region helpers
        public TCG.PaymentGatewayLibrary.SagePayNetCashNetFTP.Service getClient()
        {
            return new TCG.PaymentGatewayLibrary.SagePayNetCashNetFTP.Service();
        }

        public TCG.PaymentGatewayLibrary.SageNetcash.NIWS.Validation.NIWS_Validation getNIWSValidationClient()
        {
            return new TCG.PaymentGatewayLibrary.SageNetcash.NIWS.Validation.NIWS_Validation();
        }

        public TCG.PaymentGatewayLibrary.SageNetcash.NIWS.NIF.NIWS_NIF getNIWSNIFClient()
        {
            return new TCG.PaymentGatewayLibrary.SageNetcash.NIWS.NIF.NIWS_NIF();
        }

        public TCG.PaymentGatewayLibrary.SageNetcash.NIWS.Help.NIWS_Help getNIWSHelpClient()
        {
            return new TCG.PaymentGatewayLibrary.SageNetcash.NIWS.Help.NIWS_Help();
        }

        public TCG.PaymentGatewayLibrary.SagePayNetCashPartner.NIWS_Partner getNIWSPartnerClient()
        {
            return new TCG.PaymentGatewayLibrary.SagePayNetCashPartner.NIWS_Partner();
        }

        public TCG.PaymentGatewayLibrary.SagePayNetCashVault.cceService getVaultClient()
        {
            return new TCG.PaymentGatewayLibrary.SagePayNetCashVault.cceService();
        }
        #endregion

        public bool BankAccountIsValid(string AccountNumber, string BranchCode, string AccountType)
        {
            string AccType = "0";

            switch (AccountType)
            {
                case ("Current"):
                case ("Checking"):
                case ("Cheque"):
                    {
                        AccType = "1";
                    }
                    break;
                case ("Savings"):
                    {
                        AccType = "2";
                    }
                    break;
                case ("Transmission"):
                    {
                        AccType = "3";
                    }
                    break;
                default:
                    {
                        throw new Exception("Account Type Invalid");
                    }
            }

            var client = getNIWSValidationClient();
            var result = client.ValidateBankAccount(AccountServiceKey, AccountNumber, BranchCode, AccType);

            return result == "0";
        }

        public void TestMethod()
        {
            var client = getNIWSValidationClient();
            var banks = client.GetBankListWithDefaultBranchCode(AccountServiceKey);

        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public Batch_Verify_Result Verify(Batch_Verify_Details details)
        {
            throw new NotImplementedException();
        }
    }
}
