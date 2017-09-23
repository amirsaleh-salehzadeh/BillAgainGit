using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Batch;
using TCG.PaymentGatewayLibrary;
using System.Text.RegularExpressions;

namespace TCG.PaymentGateways.Providers
{
    public class gwMyGateDD : IBatchStrategy
    {
        /*
            NOTES:  
            *   ClientUCI = internal token
            *   ClientUID = external token
            *   ClientPIN = mg.tdo.64368
         */

        private string MerchantNumber;
        private string MerchantUID;
        private string PaymentApplicationUID;
        private string VaultApplicationUID; //will only apply if using credit card batching
        private string ServiceType;

        public bool isTest { get; set; }

        public bool BankAccountEnabled { get; set; }
        public bool CreditCardEnabled { get; set; }

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwMyGateDD; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "MyGate Batching",
                        WebUrl: "http://mygate.co.za/",
                        Description: "Specialising in online payments. South African based gateway. Supports either credit card or debit order batching.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "MerchantNumber", "MerchantUID", "ApplicationUID", "ServiceType", "BankAccountEnabled", "CreditCardEnabled" },
                        Currencies: new[] { "ZAR", "MUR" },
                        Countries: new[] { "ZA" },
                        CardTypes: new[] 
                        {
                            CardTypeEnum.VISA, 
                            CardTypeEnum.MASTERCARD
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
                        CreditCard_Batching: true,

                        Bank_Batching_Type: "debit_order",

                        BankPaymentTokenize: false,
                        CCPaymentTokenize: true,
                        CCThreeDSecure:false,

                        require_BankPaymentTokenize: false,
                        require_CCPaymentTokenize: true,

                        is_NotifyPull: true,
                        is_AutoRelease: false,
                        is_AutoRecon: true,

                        Verify: true,
                        Sale: true,
                        Refund: true,
                        UsesExternalIdentifier:false
                    );
            }
        }

        #endregion

        #region Methods

        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            MerchantUID = MerchantConfigValues.Where(r => r.Key.Equals("MerchantUID")).FirstOrDefault().Value;
            PaymentApplicationUID = MerchantConfigValues.Where(r => r.Key.Equals("PaymentApplicationUID")).FirstOrDefault().Value;
            VaultApplicationUID = MerchantConfigValues.Where(r => r.Key.Equals("VaultApplicationUID")).FirstOrDefault().Value;
            MerchantNumber = MerchantConfigValues.Where(r => r.Key.Equals("MerchantNumber")).FirstOrDefault().Value;
            ServiceType = MerchantConfigValues.Where(r => r.Key.Equals("ServiceType")).FirstOrDefault().Value;

            isTest = isTestMode;

            BankAccountEnabled = bool.Parse(MerchantConfigValues.Where(r => r.Key.Equals("BankAccountEnabled")).FirstOrDefault().Value);
            CreditCardEnabled = bool.Parse(MerchantConfigValues.Where(r => r.Key.Equals("CreditCardEnabled")).FirstOrDefault().Value);
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
                    Key="MerchantNumber",
                    Value="AB99999"
                },
                new MerchantConfigValue
                {
                    Key="MerchantUID",
                    Value="9a6646ab-5e5e-40b8-aa06-015ec6031bd0"
                },
                new MerchantConfigValue
                {
                    Key="ApplicationUID",
                    Value="bbd74b25-51a3-4bdc-8c90-d64f2c23b9ef"
                },
                new MerchantConfigValue
                {
                    Key="ServiceType",
                    Value="TwoDay"
                },
                new MerchantConfigValue
                {
                    Key="BankAccountEnabled",
                    Value=BankAccountEnabled.ToString()
                },
                new MerchantConfigValue
                {
                    Key="CreditCardEnabled",
                    Value=CreditCardEnabled.ToString()
                }

            };

            Login(true, config);
        }

        public Batch_Verify_Result Verify(Batch_Verify_Details details)
        {
            throw new NotImplementedException();
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            var client = getCollectionsVaultClient();
            string cardTypeNumber=details.CardType == CardTypeEnum.VISA ? "7" : "8";

            string pin = generateClientPin(8);
            var result = client.fLoadPinCC(MerchantUID, VaultApplicationUID, details.CardNumber, details.CardHolderFullName, details.CardExpiryMonth.ToString(), details.CardExpiryYear.ToString(), cardTypeNumber, pin, details.ClientIdentifier);

            var resultArray = result.ToList();

            bool isSuccess = resultArray[0].ToString().Split(new string[]{"||"}, StringSplitOptions.RemoveEmptyEntries)[1].Trim() == "0";

            return new StorePaymentMethod_Result 
            { 
                isSuccess=isSuccess, 
                CardToken=isSuccess ? resultArray[1].ToString().Split(new string[]{"||"}, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "", 
                CardPIN=isSuccess ? pin : "",
                ErrorCode = !isSuccess ? resultArray[2].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "",
                ErrorMessage = !isSuccess ? resultArray[2].ToString() : "" 
            };
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {
            var client = getCollectionsVaultClient();
            
            var result = client.fDeletePinCC(MerchantUID, VaultApplicationUID, details.CardToken, details.ClientIdentifier);

            var resultArray = result.ToList();

            bool isSuccess = resultArray[0].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() == "0";

            return new RevokePaymentMethod_Result
            {
                isSuccess=isSuccess,
                TransactionIdentifier = isSuccess ? resultArray[1].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "",
                ErrorCode = !isSuccess ? resultArray[2].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "",
                ErrorMessage = !isSuccess ? resultArray[2].ToString() : ""
            };
        }

        public Batch_Sale_Build_Result Sale_Build(Batch_Sale_Build_Details details)
        {
            #region Error Condition Checking
            if (BankAccountEnabled && CreditCardEnabled)
            {
                throw new Exception("Mixed batching not allowed by gateway");
            }

            if (!BankAccountEnabled && !CreditCardEnabled)
            {
                throw new Exception("Gateway not set up for any service");
            }
            #endregion

            //root element
            XElement root = new XElement("debitorder");

            #region Common Detail Processing
            string ServiceTypeNumber = "";
            switch (ServiceType)
            {
                case ("OneDay"):
                    {
                        ServiceTypeNumber = "2";
                    }
                    break;
                case ("TwoDay"):
                    {
                        ServiceTypeNumber = "3";
                    }
                    break;
                case ("SameDay"): //not required here but put here just for understanding purposes
                default: //auto assume same day
                    {
                        ServiceTypeNumber = "1";
                    }
                    break;
            }

            int totalTransactions = details.Lines.Count;

            if (totalTransactions <= 0)
            {
                throw new Exception("Cannot create empty batch");
            }

            string ActionDateS = details.ActionDate.ToString("yyMMdd");
            #endregion

            #region Header Creation
            //start header creation
            XElement header = new XElement("header");
            header.Add(new XElement("merchantno", MerchantUID));
            header.Add(new XElement("applicationid", PaymentApplicationUID));
            header.Add(new XElement("servicetype", ServiceTypeNumber));
            header.Add(new XElement("totaltransactions", totalTransactions));
            header.Add(new XElement("firstactiondate", ActionDateS));
            header.Add(new XElement("lastactiondate", ActionDateS));
            #endregion

            root.Add(header);

            //start items creation
            var items = details.Lines;
            for (int i = 1; i <= totalTransactions; i++) //create for loop starting at 1 for sequencing
            {
                var item = items[i - 1];
                XElement transaction = new XElement("transaction");

                if (BankAccountEnabled)
                {
                    #region Using Debit Order XML 1.3.4
                    string AccTypeNumber = "";
                    switch (item.BankAccountType)
                    {
                        case ("Current"):
                        case ("Checking"):
                        case ("Cheque"):
                            {
                                AccTypeNumber = "1";
                            }
                            break;
                        case ("Savings"):
                            {
                                AccTypeNumber = "2";
                            }
                            break;
                        case ("Transmission"):
                            {
                                AccTypeNumber = "3";
                            }
                            break;
                        default:
                            {
                                throw new Exception("Account Type Invalid");
                            }
                    }

                    transaction.Add(new XElement("sequencenumber", i));
                    transaction.Add(new XElement("branchcode", item.BankBranch));
                    transaction.Add(new XElement("accounttype", AccTypeNumber));
                    transaction.Add(new XElement("accountno", item.CardBankNumber));
                    transaction.Add(new XElement("debitamount", item.Amount.ToString("F2")));
                    transaction.Add(new XElement("debitdate", ActionDateS));
                    transaction.Add(new XElement("debitreference", item.LineIdentifier));
                    transaction.Add(new XElement("accountholder", item.CardBankName));
                    transaction.Add(new XElement("transactionrefno", item.LineIdentifier));
                    #endregion
                }
                else if (CreditCardEnabled)
                {
                    #region Using Collections 2.2.2
                    transaction.Add(new XElement("sequencenumber", i));
                    transaction.Add(new XElement("clientpin", item.TokenPIN));
                    transaction.Add(new XElement("clientuci", item.Token));
                    transaction.Add(new XElement("clientuid", item.ExtRef));
                    transaction.Add(new XElement("debitamount", item.Amount.ToString("F2")));
                    transaction.Add(new XElement("debitdate", ActionDateS));
                    transaction.Add(new XElement("debitreference", item.LineIdentifier));
                    transaction.Add(new XElement("creditcardexpirymonth", item.CardExpiryMonth));
                    transaction.Add(new XElement("creditcardexpiryyear", item.CardExpiryYear));
                    transaction.Add(new XElement("transactionrefno", item.LineIdentifier));
                    #endregion
                }

                //add transaction
                root.Add(transaction);
            }

            #region Footer Creation
            //start footer creation
            XElement footer = new XElement("footer");
            footer.Add(new XElement("totaltransactions", totalTransactions));
            footer.Add(new XElement("firstactiondate", ActionDateS));
            footer.Add(new XElement("lastactiondate", ActionDateS));
            footer.Add(new XElement("debittotal", details.Lines.Sum(s => s.Amount).ToString("F2")));
            #endregion

            root.Add(footer);

            return new Batch_Sale_Build_Result 
            { 
                isBuildSuccess = true, 
                RequestFile = root.ToString(SaveOptions.DisableFormatting)
                /* 
                   Disable formatting used to avoid unnecessary adding of carriage returns and spaces, 
                   which if there will cause errors posting batch
                 */
            }; 
             
        }

        public Batch_Sale_Submit_Result Sale_Submit(Batch_Sale_Submit_Details details)
        {
            #region Error Condition Checking
            if (BankAccountEnabled && CreditCardEnabled)
            {
                throw new Exception("Mixed batching not allowed by gateway");
            }

            if (!BankAccountEnabled && !CreditCardEnabled)
            {
                throw new Exception("Gateway not set up for any service");
            }

            if (String.IsNullOrEmpty(details.RequestFile))
            {
                throw new Exception("Batch has to be built before submission");
            }
            #endregion

            string result = "";

            if (BankAccountEnabled)
            {
                var client = getDebitOrderClient();
                result = client.uploadDebitFile(details.RequestFile);
            }
            else if (CreditCardEnabled)
            {
                var client = getCollectionsClient();
                result = client.uploadDebitFile(details.RequestFile);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            XmlNodeList errorNodes = doc.GetElementsByTagName("debituploaderror");
            if (errorNodes.Count > 0)
            {
                //since the whole file wont go through if theres errors theres no need for us to process the error here
                return new Batch_Sale_Submit_Result { isSubmitSuccess = false, RequestXml = details.RequestFile, ResponseXml = result, ErrorMessage = errorNodes.ToString() };
            }
            else
            {
                XmlNodeList doResultNodes = doc.GetElementsByTagName("debitorderresult");
                string TransactionIdentifier = "";
                if (doResultNodes.Count > 0) //process successes
                {
                    var refNumNode = doResultNodes[0].SelectNodes("transactionreference").Item(0);
                    TransactionIdentifier = refNumNode.InnerText;
                }
                else
                {
                    throw new Exception("Invalid response from gateway");
                }

                return new Batch_Sale_Submit_Result { isSubmitSuccess = true, RequestXml = details.RequestFile, ResponseXml = result, TransactionIdentifier = TransactionIdentifier };
            }
        }

        public Batch_Sale_Release_Result Sale_Release(Batch_Sale_Release_Details details)
        {
            #region Error Condition Checking
            if (BankAccountEnabled && CreditCardEnabled)
            {
                throw new Exception("Mixed batching not allowed by gateway");
            }

            if (!BankAccountEnabled && !CreditCardEnabled)
            {
                throw new Exception("Gateway not set up for any service");
            }

            if (String.IsNullOrEmpty(details.TransactionIdentifier))
            {
                throw new Exception("Batch has to be submitted before release");
            }
            #endregion

            string result = "";

            XElement dorelease = new XElement("dorelease");
            dorelease.Add(new XElement("merchantno", MerchantNumber));
            dorelease.Add(new XElement("referenceno", details.TransactionIdentifier));

            if (BankAccountEnabled)
            {
                var client = getDebitOrderClient();
                result = client.releaseDebitFile(dorelease.ToString());
            }
            else if (CreditCardEnabled)
            {
                var client = getCollectionsClient();
                result = client.releaseDebitFile(dorelease.ToString());
            }

            XmlDocument rdoc = new XmlDocument();
            rdoc.LoadXml(result);

            XmlNodeList doReleaseNodes = rdoc.GetElementsByTagName("dorelease_result");

            if (doReleaseNodes.Count == 0)
            {
                throw new Exception("Invalid response from gateway");
            }

            var releaseResNode = doReleaseNodes[0].SelectNodes("result").Item(0);
            var releaseDescNode = doReleaseNodes[0].SelectNodes("description").Item(0);

            bool approved = releaseResNode.InnerText == "1";

            return new Batch_Sale_Release_Result { isReleaseSuccess=approved, RequestXml=dorelease.ToString(), ResponseXml=result, ErrorCode=releaseResNode.InnerText, ErrorMessage=releaseDescNode.InnerText };
        }

        public Batch_Sale_Recon_Result Sale_Recon(Batch_Sale_Recon_Details details)
        {
            #region Error Condition Checking
            if (BankAccountEnabled && CreditCardEnabled)
            {
                throw new Exception("Mixed batching not allowed by gateway");
            }

            if (!BankAccountEnabled && !CreditCardEnabled)
            {
                throw new Exception("Gateway not set up for any service");
            }
            #endregion

            string result = "";

            XElement doRDRequest = new XElement("rd_request");
            doRDRequest.Add(new XElement("merchantno", MerchantNumber));
            doRDRequest.Add(new XElement("merchantuid", MerchantUID));
            doRDRequest.Add(new XElement("fromdate", details.ReconStartDate.ToString("yyMMdd")));
            doRDRequest.Add(new XElement("todate", details.ReconEndDate.ToString("yyMMdd")));

            if (BankAccountEnabled)
            {
                var client = getDebitOrderClient();
                result = client.downloadRDFiles(doRDRequest.ToString());
            }
            else if (CreditCardEnabled)
            {
                var client = getCollectionsClient();
                result = client.downloadRDFiles(doRDRequest.ToString());
            }

            var return_object = new Batch_Sale_Recon_Result
            {
                RequestXml=doRDRequest.ToString(),
                ResponseXml=result
            };

            XmlDocument rdoc = new XmlDocument();
            rdoc.LoadXml(result);
            
            XmlNodeList errorNodes = rdoc.GetElementsByTagName("rd_download_error"); //each file represents a batch
            if (errorNodes.Count > 0)
            {
                return_object.isRequestSuccess = false;
                return_object.ErrorMessage = errorNodes.ToString();
                return return_object;
            }            

            XmlNodeList fileNodes = rdoc.GetElementsByTagName("rd_file"); //each file represents a batch 

            List<Batch_Sale_Recon_Result_batch> batches = new List<Batch_Sale_Recon_Result_batch>();
            foreach (XmlElement file in fileNodes)
            {
                Batch_Sale_Recon_Result_batch batch = new Batch_Sale_Recon_Result_batch 
                {
                    BatchDate = DateTime.Parse(file.SelectNodes("date_received").Item(0).InnerText),
                    BatchIdentifier = file.SelectNodes("file_name").Item(0).InnerText,
                    isBatchSuccess=true,
                    NumberOfRecords = int.Parse(file.SelectNodes("num_records").Item(0).InnerText),
                    Total = decimal.Parse(file.SelectNodes("rd_amount").Item(0).InnerText)
                };

                var records = file.GetElementsByTagName("rd_record");
                List<Batch_Sale_Recon_Result_batch_line> lines = new List<Batch_Sale_Recon_Result_batch_line>();
                foreach (XmlNode record in records)
                {
                    var recordNodes = record.ChildNodes;

                    string branchcode = record.SelectNodes("branchcode").Item(0).InnerText;
                    string accountno = record.SelectNodes("accountno").Item(0).InnerText;
                    string accounttype = record.SelectNodes("accounttype").Item(0).InnerText;
                    string accountname = record.SelectNodes("accountname").Item(0).InnerText;
                    string debitamount = record.SelectNodes("debitamount").Item(0).InnerText;
                    string actiondate = record.SelectNodes("actiondate").Item(0).InnerText;
                    string statement_reference = record.SelectNodes("statement_reference").Item(0).InnerText;
                    string status = record.SelectNodes("status").Item(0).InnerText;
                    string rejection_code = record.SelectNodes("rejection_code").Item(0).InnerText;
                    string rejection_reason = record.SelectNodes("rejection_reason").Item(0).InnerText;
                    string original_sequencenumber = record.SelectNodes("original_sequencenumber").Item(0).InnerText;
                    string transaction_reference = record.SelectNodes("transaction_reference").Item(0).InnerText;

                    lines.Add(new Batch_Sale_Recon_Result_batch_line
                    {
                        isLineSuccess = false,
                        isDebitedWithError = status != "Rejected",
                        LineError = rejection_code + " - " + rejection_reason,
                        LineIdentifier = transaction_reference,
                        LineNumber = int.Parse(original_sequencenumber)
                    });
                }

                batch.lines = lines;
                batches.Add(batch);
            }

            return_object.Batches = batches;
            return_object.isRequestSuccess = true;

            return return_object;
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region TO SORT AND REMOVE

        ////POST BATCH


        //public Transaction_Result_Batch Refund(string file)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    //ACTUAL
        //    //    //var gateway = newClient();

        //    //    if (allows_CreditCard && allows_DebitOrder)
        //    //    {
        //    //        if (!String.IsNullOrEmpty(details.AccountNo))
        //    //        {
        //    //            return CCRefund(details);
        //    //        }
        //    //        else
        //    //        {
        //    //            return DDRefund(details);
        //    //        }
        //    //    }

        //    //    if (allows_CreditCard)
        //    //    {
        //    //        return CCRefund(details);
        //    //    }

        //    //    if (allows_DebitOrder)
        //    //    {
        //    //        return DDRefund(details);
        //    //    }


        //    //    return DDRefund(details);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return new Transaction_Result
        //    //    {
        //    //        isApproved = false,
        //    //        hasServerError = true,
        //    //        ErrorText = ex.Message
        //    //    };
        //    //}
        //}

        //public Transaction_Result VerifyBankDetails(DDSale_Details details)
        //{
        //    throw new NotImplementedException();
        //    if (String.IsNullOrEmpty(details.AccountNumber))
        //    {
        //        throw new NotSupportedException("Can only do bank account verification if account number is provided.");
        //    }

        //    try
        //    {
        //        //ACTUAL
        //        var gateway = newAccVerifyClient();

        //        DateTime now = DateTime.UtcNow.AddHours(2); //a south african based gateway therefore we add 2 hours for UTC +2 (SAST)

        //        TimeSpan tenOClock = new TimeSpan(10, 0, 0);

        //        if (now.TimeOfDay > new TimeSpan(10, 0, 0)) //if it is after 10am, then set the date to tomorrows date (10am is cut off time)
        //        {
        //            now = now.AddDays(1);
        //            now = new DateTime(now.Year, now.Month, now.Day, tenOClock.Hours, tenOClock.Minutes, tenOClock.Seconds); //set tomorrows time to 10am so it falls within bound
        //        }

        //        string actionDate = now.ToString("yyMMdd");

        //        XElement root = new XElement("accountvalidation");

        //        XElement header = new XElement("header");
        //        header.Add(new XElement("merchantno", MerchantUID));
        //        header.Add(new XElement("applicationid", ApplicationUID));
        //        header.Add(new XElement("totalaccounts", "1"));

        //        //this can be multiple
        //        XElement transaction = new XElement("account");
        //        transaction.Add(new XElement("sequencenumber", "1"));
        //        transaction.Add(new XElement("accountholder", details.Name + " " + details.LastName));
        //        transaction.Add(new XElement("idno", details.IDNumber));
        //        transaction.Add(new XElement("accountno", details.AccountNumber));
        //        transaction.Add(new XElement("accounttype", details.AccountType)); //account type might need to be converted to number
        //        transaction.Add(new XElement("branchcode", details.BranchCode));
        //        transaction.Add(new XElement("unique_reference", details.Reference));

        //        XElement footer = new XElement("footer");
        //        footer.Add(new XElement("totalaccounts", "1"));

        //        root.Add(header, transaction, footer);

        //        //ACTUAL
        //        //string result = gateway.ImportAccountsToValidate(root.ToString());

        //        //SUCCESS SIMULATION
        //        string result = "<accountvalidation_result><batchreferenceno>4592032</batchreferenceno><totaluploadedno>1</totaluploadedno></accountvalidation_result>";

        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(result);

        //        XmlNodeList errorNodes = doc.GetElementsByTagName("accountverifyerror");

        //        if (errorNodes.Count > 0) //error has occurred
        //        {
        //            var sectionNode = errorNodes[0].SelectNodes("section").Item(0);
        //            var seqNumberNode = errorNodes[0].SelectNodes("sequenceno").Item(0);
        //            var descriptionNode = errorNodes[0].SelectNodes("description").Item(0);

        //            string errorCode = sectionNode.InnerText + " #" + seqNumberNode.InnerText;
        //            string errorDesc = descriptionNode.InnerText;

        //            return new Transaction_Result
        //            {
        //                isApproved = false,
        //                hasServerError = false,
        //                ErrorCode = errorCode,
        //                ErrorText = errorDesc,
        //                FullRequest = root.ToString(),
        //                FullResponse = result
        //            };
        //        }
        //        else
        //        {
        //            XmlNodeList doResultNodes = doc.GetElementsByTagName("accountvalidation_result");

        //            var totUploadedNoNode = doResultNodes[0].SelectNodes("totaluploadedno").Item(0);
        //            var refNumNode = doResultNodes[0].SelectNodes("batchreferenceno").Item(0);

        //            XElement dodownload = new XElement("rf_request");
        //            dodownload.Add(new XElement("merchantno", MerchantNumber));
        //            dodownload.Add(new XElement("merchantuid", MerchantUID));
        //            dodownload.Add(new XElement("batchreferenceno", refNumNode.InnerText));

        //            //ACTUAL
        //            //string releaseResult = gateway.downloadResponseFiles(dodownload.ToString());

        //            //SUCCESS SIMULATION
        //            string releaseResult = "<rf_download_result><response_file><uploadfile_refno>0807021455_1</uploadfile_refno><downloadfile_refno>0807021455_1_23</downloadfile_refno>";
        //            releaseResult += "<uploadfile_name></uploadfile_name><date_downloaded>02-Jul-2008 14:55</date_downloaded><bank_name>Standard Bank S.A.</bank_name><negative_match>1</negative_match>";
        //            releaseResult += "<positive_match>1</positive_match><total_records>2</total_records><rf_record><date_uploaded>02-Jul-2008 14:55</date_uploaded><branch_code>025109</branch_code><account_number>876876876876</account_number>";
        //            releaseResult += "<id_number>8705451250089</id_number><account_holder>Soap</account_holder><unique_reference>Unique Ref 1</unique_reference><idno_validated>1</idno_validated><accountno_validated>0</accountno_validated>";
        //            releaseResult += "<accholder_validated>1</accholder_validated><accactive_validated>1</accactive_validated><periodactive_validated>0</periodactive_validated><accepts_debits>0</accepts_debits><accepts_credits >1</accepts_credits>";
        //            releaseResult += "<matched>1</matched><status>Complete</status><bank_name>Standard Bank S.A.</bank_name></rf_record>";
        //            releaseResult += "</response_file></rf_download_result>";

        //            XmlDocument rdoc = new XmlDocument();
        //            rdoc.LoadXml(releaseResult);

        //            XmlNodeList doDownloadNodes = rdoc.GetElementsByTagName("rf_download_result");
        //            var resFile = doDownloadNodes[0].SelectNodes("response_file").Item(0);
        //            var record = resFile.SelectNodes("rf_record").Item(0);

        //            var date_uploaded = record.SelectNodes("date_uploaded").Item(0).InnerText;
        //            var branch_code = record.SelectNodes("branch_code").Item(0).InnerText;
        //            var account_number = record.SelectNodes("account_number").Item(0).InnerText;
        //            var id_number = record.SelectNodes("id_number").Item(0).InnerText;
        //            var account_holder = record.SelectNodes("account_holder").Item(0).InnerText;
        //            var unique_reference = record.SelectNodes("unique_reference").Item(0).InnerText;
        //            var idno_validated = record.SelectNodes("idno_validated").Item(0).InnerText;
        //            var accountno_validated = record.SelectNodes("accountno_validated").Item(0).InnerText;
        //            var accholder_validated = record.SelectNodes("accholder_validated").Item(0).InnerText;
        //            var accactive_validated = record.SelectNodes("accactive_validated").Item(0).InnerText;
        //            var periodactive_validated = record.SelectNodes("periodactive_validated").Item(0).InnerText;
        //            var accepts_debits = record.SelectNodes("accepts_debits").Item(0).InnerText;
        //            var accepts_credits = record.SelectNodes("accepts_credits").Item(0).InnerText;
        //            var matched = record.SelectNodes("matched").Item(0).InnerText;
        //            var status = record.SelectNodes("status").Item(0).InnerText;
        //            var bank_name = record.SelectNodes("bank_name").Item(0).InnerText;

        //            bool approved = matched == "1";

        //            return new Transaction_Result
        //            {
        //                isApproved = approved,
        //                hasServerError = false,
        //                ResultCode = approved ? status : "",
        //                ErrorText = !approved ? status : "",
        //                FullRequest = root.ToString(),
        //                FullResponse = releaseResult
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new Transaction_Result
        //        {
        //            isApproved = false,
        //            hasServerError = true,
        //            ErrorText = ex.Message
        //        };
        //    }
        //}

        //#region DEPRECATED

        //public Transaction_Result_Batch CCRefund(BatchRefund_Details details)
        //{
        //    throw new NotImplementedException();
        //    //var gateway = newTokenDOClient();

        //    DateTime now = DateTime.UtcNow.AddHours(2); //a south african based gateway therefore we add 2 hours for UTC +2 (SAST)

        //    TimeSpan tenOClock = new TimeSpan(10, 0, 0);

        //    if (now.TimeOfDay > new TimeSpan(10, 0, 0)) //if it is after 10am, then set the date to tomorrows date (10am is cut off time)
        //    {
        //        now = now.AddDays(1);
        //        now = new DateTime(now.Year, now.Month, now.Day, tenOClock.Hours, tenOClock.Minutes, tenOClock.Seconds); //set tomorrows time to 10am so it falls within bound
        //    }

        //    string actionDate = now.ToString("yyMMdd");

        //    XElement root = new XElement("debitorder");
        //    //begin xml creation
        //    XElement header = new XElement("header");
        //    header.Add(new XElement("merchantno", MerchantUID)); //the merchants mygate client number
        //    header.Add(new XElement("applicationid", ApplicationUID)); //the debit order application ID
        //    header.Add(new XElement("servicetype", "0")); //0 - Same day service
        //    header.Add(new XElement("totaltransactions", "1")); //the number of transactions in batch - Hard Coded to 1
        //    header.Add(new XElement("firstactiondate", actionDate)); //the min action date
        //    header.Add(new XElement("lastactiondate", actionDate)); //the max action date

        //    //this can be multiple
        //    XElement transaction = new XElement("transaction");
        //    transaction.Add(new XElement("sequencenumber", "1"));
        //    transaction.Add(new XElement("branchcode", "999999")); //branch code Hard Coded as per documentation
        //    transaction.Add(new XElement("accounttype", "7")); //7 = Credit Card
        //    transaction.Add(new XElement("accountno", details.CardNumber)); //credit card number
        //    transaction.Add(new XElement("creditcardexpirymonth", details.CardExpMonth)); //the month the credit card expires
        //    transaction.Add(new XElement("creditcardexpiryyear", details.CardExpYear)); //the year the credit card expires
        //    transaction.Add(new XElement("creditamount", details.Amount.ToString("F2"))); //amount to debit
        //    transaction.Add(new XElement("creditdate", actionDate)); //the date the transaction should take place
        //    transaction.Add(new XElement("debitreference", details.InvoiceNumber)); //the invoice number for reference
        //    transaction.Add(new XElement("accountholder", details.CardHolderName)); //the month the credit card expires
        //    transaction.Add(new XElement("transactionrefno", details.TransactionIndex)); //the year the credit card expires

        //    XElement footer = new XElement("footer");
        //    footer.Add(new XElement("totaltransactions", "1")); //the number of transactions in batch - Hard Coded to 1
        //    footer.Add(new XElement("firstactiondate", actionDate)); //the min action date
        //    footer.Add(new XElement("lastactiondate", actionDate)); //the max action date
        //    footer.Add(new XElement("debittotal", details.Amount.ToString("F2"))); //the total of debits

        //    root.Add(header, transaction, footer);
        //    //end xml creation

        //    //DO TRANSACTION
        //    //------------------------------------------------------------------//

        //    //ACTUAL
        //    //string result = gateway.uploadRefundFile(root.ToString());

        //    //FAIL SIMULATION
        //    //string result = "<debitorderresult>";
        //    //result += "<debituploaderror><section>transaction</section><sequenceno>1</sequenceno><description>Transaction reference cannot be blank</description></debituploaderror>";
        //    //result += "<totaluploadedno>0</totaluploadedno><totaluploadedamount>0</totaluploadedamount>";
        //    //result += "</debitorderresult>";

        //    //SUCCESS SIMULATION
        //    string result = "<debitorderresult><totaluploadedno>2</totaluploadedno><totaluploadedamount>271.98</totaluploadedamount><transactionreference>080418153218_213</transactionreference></debitorderresult>";

        //    //------------------------------------------------------------------//

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(result);

        //    XmlNodeList errorNodes = doc.GetElementsByTagName("debituploaderror");

        //    if (errorNodes.Count > 0) //error has occurred
        //    {
        //        var sectionNode = errorNodes[0].SelectNodes("section").Item(0);
        //        var seqNumberNode = errorNodes[0].SelectNodes("sequenceno").Item(0);
        //        var descriptionNode = errorNodes[0].SelectNodes("description").Item(0);

        //        string errorCode = sectionNode.InnerText + " #" + seqNumberNode.InnerText;
        //        string errorDesc = descriptionNode.InnerText;

        //        return new Transaction_Result
        //        {
        //            isApproved = false,
        //            hasServerError = false,
        //            ErrorCode = errorCode,
        //            ErrorText = errorDesc,
        //            FullRequest = root.ToString(),
        //            FullResponse = result
        //        };
        //    }
        //    else
        //    {
        //        XmlNodeList doResultNodes = doc.GetElementsByTagName("debitorderresult");

        //        var totUploadedNoNode = doResultNodes[0].SelectNodes("totaluploadedno").Item(0);
        //        var totUploadedAmtNode = doResultNodes[0].SelectNodes("totaluploadedamount").Item(0);
        //        var refNumNode = doResultNodes[0].SelectNodes("transactionreference").Item(0);

        //        XElement dorelease = new XElement("dorelease");
        //        dorelease.Add(new XElement("merchantno", MerchantNumber));
        //        dorelease.Add(new XElement("referenceno", refNumNode.InnerText));

        //        //ACTUAL
        //        //string releaseResult = gateway.releaseDebitFile(dorelease.ToString());

        //        //FAIL SIMULATION
        //        //string releaseResult = "<dorelease_result> <result>‐1</result><description>The specified merchant account number (AB999999) is not recognised. Release process aborted.</description></dorelease_result>";

        //        //SUCCESS SIMULATION
        //        string releaseResult = "<dorelease_result><result>1</result><description>OK</description></dorelease_result>";

        //        XmlDocument rdoc = new XmlDocument();
        //        rdoc.LoadXml(releaseResult);

        //        XmlNodeList doReleaseNodes = rdoc.GetElementsByTagName("dorelease_result");
        //        var releaseResNode = doReleaseNodes[0].SelectNodes("result").Item(0);
        //        var releaseDescNode = doReleaseNodes[0].SelectNodes("description").Item(0);

        //        bool approved = releaseResNode.InnerText == "1";

        //        return new Transaction_Result
        //        {
        //            isApproved = approved,
        //            hasServerError = false,
        //            ResultCode = approved ? releaseDescNode.InnerText : "",
        //            ErrorText = !approved ? releaseDescNode.InnerText : "",
        //            FullRequest = root.ToString(),
        //            FullResponse = result
        //        };
        //    }
        //}

        //public Transaction_Result_Batch DDRefund(BatchRefund_Details details)
        //{
        //    throw new NotImplementedException();
        //    //var gateway = newTokenDOClient();

        //    DateTime now = DateTime.UtcNow.AddHours(2); //a south african based gateway therefore we add 2 hours for UTC +2 (SAST)

        //    TimeSpan tenOClock = new TimeSpan(10, 0, 0);

        //    if (now.TimeOfDay > new TimeSpan(10, 0, 0)) //if it is after 10am, then set the date to tomorrows date (10am is cut off time)
        //    {
        //        now = now.AddDays(1);
        //        now = new DateTime(now.Year, now.Month, now.Day, tenOClock.Hours, tenOClock.Minutes, tenOClock.Seconds); //set tomorrows time to 10am so it falls within bound
        //    }

        //    string actionDate = now.ToString("yyMMdd");

        //    XElement root = new XElement("debitorder");
        //    //begin xml creation
        //    XElement header = new XElement("header");
        //    header.Add(new XElement("merchantno", MerchantUID)); //the merchants mygate client number
        //    header.Add(new XElement("applicationid", ApplicationUID)); //the debit order application ID
        //    header.Add(new XElement("servicetype", "0")); //0 - Same day service
        //    header.Add(new XElement("totaltransactions", "1")); //the number of transactions in batch - Hard Coded to 1
        //    header.Add(new XElement("firstactiondate", actionDate)); //the min action date
        //    header.Add(new XElement("lastactiondate", actionDate)); //the max action date

        //    //this can be multiple
        //    XElement transaction = new XElement("transaction");
        //    transaction.Add(new XElement("sequencenumber", "1"));
        //    transaction.Add(new XElement("branchcode", details.BranchCode)); //branch code Hard Coded as per documentation
        //    transaction.Add(new XElement("accounttype", details.AccountType)); //7 = Credit Card
        //    transaction.Add(new XElement("accountno", details.AccountNo)); //credit card number
        //    transaction.Add(new XElement("creditamount", details.Amount.ToString("F2"))); //amount to debit
        //    transaction.Add(new XElement("creditdate", actionDate)); //the date the transaction should take place
        //    transaction.Add(new XElement("debitreference", details.InvoiceNumber)); //the invoice number for reference
        //    transaction.Add(new XElement("accountholder", details.AccountHolder)); //the month the credit card expires
        //    transaction.Add(new XElement("transactionrefno", details.TransactionIndex)); //the year the credit card expires

        //    XElement footer = new XElement("footer");
        //    footer.Add(new XElement("totaltransactions", "1")); //the number of transactions in batch - Hard Coded to 1
        //    footer.Add(new XElement("firstactiondate", actionDate)); //the min action date
        //    footer.Add(new XElement("lastactiondate", actionDate)); //the max action date
        //    footer.Add(new XElement("debittotal", details.Amount.ToString("F2"))); //the total of debits

        //    root.Add(header, transaction, footer);
        //    //end xml creation

        //    //DO TRANSACTION
        //    //------------------------------------------------------------------//

        //    //ACTUAL
        //    //string result = gateway.uploadRefundFile(root.ToString());

        //    //FAIL SIMULATION
        //    //string result = "<debitorderresult>";
        //    //result += "<debituploaderror><section>transaction</section><sequenceno>1</sequenceno><description>Transaction reference cannot be blank</description></debituploaderror>";
        //    //result += "<totaluploadedno>0</totaluploadedno><totaluploadedamount>0</totaluploadedamount>";
        //    //result += "</debitorderresult>";

        //    //SUCCESS SIMULATION
        //    string result = "<debitorderresult><totaluploadedno>2</totaluploadedno><totaluploadedamount>271.98</totaluploadedamount><transactionreference>080418153218_213</transactionreference></debitorderresult>";

        //    //------------------------------------------------------------------//

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(result);

        //    XmlNodeList errorNodes = doc.GetElementsByTagName("debituploaderror");

        //    if (errorNodes.Count > 0) //error has occurred
        //    {
        //        var sectionNode = errorNodes[0].SelectNodes("section").Item(0);
        //        var seqNumberNode = errorNodes[0].SelectNodes("sequenceno").Item(0);
        //        var descriptionNode = errorNodes[0].SelectNodes("description").Item(0);

        //        string errorCode = sectionNode.InnerText + " #" + seqNumberNode.InnerText;
        //        string errorDesc = descriptionNode.InnerText;

        //        return new Transaction_Result
        //        {
        //            isApproved = false,
        //            hasServerError = false,
        //            ErrorCode = errorCode,
        //            ErrorText = errorDesc,
        //            FullRequest = root.ToString(),
        //            FullResponse = result
        //        };
        //    }
        //    else
        //    {
        //        XmlNodeList doResultNodes = doc.GetElementsByTagName("debitorderresult");

        //        var totUploadedNoNode = doResultNodes[0].SelectNodes("totaluploadedno").Item(0);
        //        var totUploadedAmtNode = doResultNodes[0].SelectNodes("totaluploadedamount").Item(0);
        //        var refNumNode = doResultNodes[0].SelectNodes("transactionreference").Item(0);

        //        XElement dorelease = new XElement("dorelease");
        //        dorelease.Add(new XElement("merchantno", MerchantNumber));
        //        dorelease.Add(new XElement("referenceno", refNumNode.InnerText));

        //        //ACTUAL
        //        //string releaseResult = gateway.releaseDebitFile(dorelease.ToString());

        //        //FAIL SIMULATION
        //        //string releaseResult = "<dorelease_result> <result>‐1</result><description>The specified merchant account number (AB999999) is not recognised. Release process aborted.</description></dorelease_result>";

        //        //SUCCESS SIMULATION
        //        string releaseResult = "<dorelease_result><result>1</result><description>OK</description></dorelease_result>";

        //        XmlDocument rdoc = new XmlDocument();
        //        rdoc.LoadXml(releaseResult);

        //        XmlNodeList doReleaseNodes = rdoc.GetElementsByTagName("dorelease_result");
        //        var releaseResNode = doReleaseNodes[0].SelectNodes("result").Item(0);
        //        var releaseDescNode = doReleaseNodes[0].SelectNodes("description").Item(0);

        //        bool approved = releaseResNode.InnerText == "1";

        //        return new Transaction_Result
        //        {
        //            isApproved = approved,
        //            hasServerError = false,
        //            ResultCode = approved ? releaseDescNode.InnerText : "",
        //            ErrorText = !approved ? releaseDescNode.InnerText : "",
        //            FullRequest = root.ToString(),
        //            FullResponse = result
        //        };
        //    }
        //}

        //#endregion

        #endregion

        #region Helpers
        private TCG.PaymentGatewayLibrary.MyGateDebitOrder.MyGate_DebitOrder_WebServiceService getDebitOrderClient()
        {
            var gateway = new TCG.PaymentGatewayLibrary.MyGateDebitOrder.MyGate_DebitOrder_WebServiceService();

            return gateway;
        }

        private TCG.PaymentGatewayLibrary.MyGateCollections.MyGate_DebitOrder_TokenWebServiceService getCollectionsClient()
        {
            var gateway = new TCG.PaymentGatewayLibrary.MyGateCollections.MyGate_DebitOrder_TokenWebServiceService();

            return gateway;
        }

        private TCG.PaymentGatewayLibrary.MyGateCollectionsVault.PinManagement getCollectionsVaultClient()
        {
            var gateway = new TCG.PaymentGatewayLibrary.MyGateCollectionsVault.PinManagement();

            return gateway;
        }

        private TCG.PaymentGatewayLibrary.MyGateBankAccountVerification.Mygate_accverification_webservicesService getAccVerifyClient()
        {
            var gateway = new TCG.PaymentGatewayLibrary.MyGateBankAccountVerification.Mygate_accverification_webservicesService();

            return gateway;
        }
        
        public string generateClientPin(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
        #endregion

    }
}
