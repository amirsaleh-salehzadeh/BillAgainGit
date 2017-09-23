using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Batch;

namespace TCG.PaymentGateways.Providers
{
    public class gwPayGateDD : IBatchStrategy
    {
        string PayGateID = "";
        string Password = "";

        public bool BankAccountEnabled { get; set; }
        public bool CreditCardEnabled { get; set; }

        public bool isTest { get; set; }

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwPayGateDD; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "PayGate Credit Card Batching",
                        WebUrl: "http://www.paygate.co.za/",
                        Description: "Offering secure and convenient online merchant payment processing solutions to the South African and African market. Credit Card Batching Only.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "PayGateID", "Password" },
                        Currencies: new[] { "ZAR" },
                        Countries: new[] { "ZA", },
                        CardTypes: new[] 
                        {
                            CardTypeEnum.VISA, 
                            CardTypeEnum.MASTERCARD, 
                            CardTypeEnum.AMERICAN_EXPRESS, 
                            CardTypeEnum.DINERS_CLUB 
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
                        CCPaymentTokenize: false,
                        CCThreeDSecure: false,

                        require_BankPaymentTokenize: false,
                        require_CCPaymentTokenize: false,

                        is_NotifyPull: false,
                        is_AutoRelease: true,
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
            PayGateID = MerchantConfigValues.Where(r => r.Key.Equals("PayGateID")).FirstOrDefault().Value;
            Password = MerchantConfigValues.Where(r => r.Key.Equals("Password")).FirstOrDefault().Value;

            isTest = isTestMode;

            BankAccountEnabled = bool.Parse(MerchantConfigValues.Where(r => r.Key.Equals("BankAccountEnabled")).FirstOrDefault().Value);
            CreditCardEnabled = bool.Parse(MerchantConfigValues.Where(r => r.Key.Equals("CreditCardEnabled")).FirstOrDefault().Value);
        }

        public void LoginTest()
        {
            isTest = true;
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="PayGateID",
                    Value="10011021600"
                },
                new MerchantConfigValue
                {
                    Key="Password",
                    Value="test"
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

        public Batch_Sale_Build_Result Sale_Build(Batch_Sale_Build_Details details)
        {
            // Transaction Type, Transaction Ref, Card Holder, Card Number, Expiry Date MMYYYY, Budget Period (00 -> 60), Amount (cents)
            string cdata = "<![CDATA[";
            foreach (var detail in details.Lines)
            {
                string expdate = (detail.CardExpiryMonth < 10 ? "0" + detail.CardExpiryMonth : detail.CardExpiryMonth.ToString()) + detail.CardExpiryYear.ToString();
                cdata += string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}' {7}", "A", 
                                        detail.LineIdentifier, detail.CardBankName, detail.CardBankNumber, expdate, 0, ((int)(detail.Amount*100)).ToString(), Environment.NewLine);
            }
            cdata += "]]>";

            var file = string.Format
                (
                "<protocol ver='4.0' pgid='{0}' pwd='{1}'><batchtx bref='{2}' cur='{3}'> {4} </batchtx></protocol> ",
                                    PayGateID, Password, details.BatchIdentifier, details.BatchCurrency, cdata
                );

            return new Batch_Sale_Build_Result 
            {
                isBuildSuccess=true,
                RequestFile=file
            };
        }

        public Batch_Sale_Submit_Result Sale_Submit(Batch_Sale_Submit_Details details)
        {
            var returnXml = PostXMLTransaction("https://www.paygate.co.za/payxml/process.trans", details.RequestFile);

            //process results
            XmlNode protocol = returnXml.GetElementsByTagName("protocol").Item(0);
            XmlNode errorNode = protocol.SelectNodes("errorrx").Item(0);

            if (errorNode == null)
            {
                XmlNode successNode = protocol.SelectNodes("authrx").Item(0);

                if (successNode == null)
                {
                    throw new Exception("Invalid results from gateway");
                }

                return new Batch_Sale_Submit_Result
                {
                    RequestXml = details.RequestFile,
                    ResponseXml = returnXml.InnerXml.ToString(),
                    isSubmitSuccess = true,
                    TransactionIdentifier = successNode.Attributes.GetNamedItem("tid").Value
                };
            }
            else
            {
                return new Batch_Sale_Submit_Result
                {
                    RequestXml = details.RequestFile,
                    ResponseXml = returnXml.InnerXml.ToString(),
                    isSubmitSuccess = false,
                    ErrorCode = errorNode.Attributes.GetNamedItem("ecode").Value,
                    ErrorMessage = errorNode.Attributes.GetNamedItem("edesc").Value
                };
            }

            
        }

        public Batch_Sale_Release_Result Sale_Release(Batch_Sale_Release_Details details)
        {
            throw new NotImplementedException();
        }

        public Batch_Sale_Recon_Result Sale_Recon(Batch_Sale_Recon_Details details)
        {
            throw new NotImplementedException();
        }


        public void runTests()
        {
            throw new NotImplementedException();
        }
        #endregion


        #region TO SORT AND REMOVE


        ////BUILD BATCH

        //public string BuildSaleBatchFile(List<DDSale_Details> details)
        //{

        //    // Transaction Type, Transaction Ref, Card Holder, Card Number, Expiry Date MMYYYY, Budget Period (00 -> 60), Amount (cents)
        //    string cdata = "<![CDATA[";
        //    foreach (var detail in details)
        //    {
        //        cdata += string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}' {7}", "A", detail.Reference, detail.Name + " " + detail.LastName, detail.CardNumber, detail.ExpDate, detail.BudgetPeriod, detail.AmountInCents.ToString(), Environment.NewLine);
        //    }
        //    cdata += "]]";

        //    return string.Format
        //        (
        //        "<protocol ver='4.0' pgid='{0}' pwd='{1}'><batchtx bref='{2}' cur='{3}'> {4} </batchtx></protocol> ",
        //        PayGateID, Password, "batchID", "currency", cdata
        //        );
        //}

        ////POST BATCH

        //public Transaction_Result_Batch Sale(string file)
        //{
        //    var returnXml = PostXMLTransaction("https://www.paygate.co.za/payxml/process.trans", file);

        //    return new Transaction_Result_Batch();
        //}

        ////RELEASE BATCH



        ////PROCESS NOTIFICATIONS

        //// OLD

        ////public Transaction_Result Auth(DDSale_Details details)
        ////{
        ////    string postData = getAuthXML(PayGateID, Password, details.Reference, details.CurrencyCode, details.Reference, details.Name + " " + details.LastName, details.CardNumber, formatExpiryDate(details.ExpMonth, details.ExpYear), "00", (details.Amount * 100).ToString());

        ////    var returnXml = PostXMLTransaction("https://www.paygate.co.za/payxml/process.trans", postData);

        ////    return new Transaction_Result();
        ////}

        //public Transaction_Result Capture(DDSale_Details details)
        //{
        //    string postData = getCaptureXML(PayGateID, Password, details.Reference, details.CurrencyCode, details.Reference, details.TransactionIndex);

        //    var returnXml = PostXMLTransaction("https://www.paygate.co.za/payxml/process.trans", postData);

        //    return new Transaction_Result();
        //}

        //internal static string getCaptureXML(string pgid, string pwd, string batchRef, string currency, string pgRef, string authCode)
        //{
        //    // Transaction Type, PayGate Ref, Auth Code, Amount (cents)
        //    string cdata = string.Format("<![CDATA['{0}','{1}','{2}']]", "S", pgRef, authCode);

        //    return string.Format
        //        (
        //        "<protocol ver='4.0' pgid='{0}' pwd='{1}'><batchtx bref='{2}' cur='{3}'> {4} </batchtx></protocol>",
        //        pgid, pwd, batchRef, currency, cdata
        //        );
        //}

        //public Transaction_Result Refund(DDRefund_Details details)
        //{
        //    string postData = getRefundXML(PayGateID, Password, details.InvoiceNumber, details.CurrencyCode, details.InvoiceNumber, details.TransactionIndex, (details.Amount * 100).ToString());

        //    var returnXml = PostXMLTransaction("https://www.paygate.co.za/payxml/process.trans", postData);

        //    return new Transaction_Result();
        //}

        //internal static string getRefundXML(string pgid, string pwd, string batchRef, string currency, string pgRef, string authCode, string amountInCents)
        //{
        //    // Transaction Type, PayGate Ref, Auth Code, Amount (cents)
        //    string cdata = string.Format("<![CDATA['{0}','{1}','{2}','{3}']]", "R", pgRef, authCode, amountInCents);

        //    return string.Format
        //        (
        //        "<protocol ver='4.0' pgid='{0}' pwd='{1}'><batchtx bref='{2}' cur='{3}'> {4} </batchtx></protocol>",
        //        pgid, pwd, batchRef, currency, cdata
        //        );
        //}



        #endregion


        #region Helpers

        private string formatExpiryDate(int month, int year)
        {
            if (month < 10)
                return "0" + month + "" + year;
            return month + "" + year;
        }

        public TCG.PaymentGatewayLibrary.PayGatePayBatch.PayBatchHelper.paybatch getClient()
        {
            var gateway = new TCG.PaymentGatewayLibrary.PayGatePayBatch.PayBatchHelper.paybatch();
            //gateway._customHeaders.Add("login", "10011013800");
            // gateway._customHeaders.Add("password", "password");

            //CredentialCache cache = new CredentialCache();
            //cache.Add(new Uri(gateway.Url), // Web service URL
            //           "Negotiate",  // Kerberos or NTLM
            //           new NetworkCredential(PayGateID, Password, "paygate.co.za"));
            //gateway.Credentials = cache;

            //NetworkCredential netCredential = new NetworkCredential(PayGateID, Password);
            //CredentialCache myCache = new CredentialCache();
            //myCache.Add(new Uri(gateway.Url), "NTLM", netCredential);
            //gateway.Credentials = myCache;
            //gateway.UnsafeAuthenticatedConnectionSharing = true;
            //gateway.ConnectionGroupName = PayGateID;
            //NetworkCredential myCred = new NetworkCredential(username, password, domainName);
            //CredentialCache myCache = new CredentialCache();
            //myCache.Add(new Uri(si.Url), "NTLM", myCred);
            //si.Credentials = myCache;
            //si.UnsafeAuthenticatedConnectionSharing = true;
            //si.ConnectionGroupName = username;

            //CredentialCache cache = new CredentialCache();
            //cache.Add(new Uri(proxy.Url), // Web service URL
            //           "Negotiate",  // Kerberos or NTLM
            //           new NetworkCredential("username", "password", "domainname"));
            //proxy.Credentials = cache;

            //var gateway = new PayGatePayBatch.paybatch();

            //NetworkCredential netCredential = new NetworkCredential(PayGateID, Password);
            //Uri uri = new Uri(gateway.Url);
            //ICredentials credentials = netCredential.GetCredential(uri, "Basic");
            //gateway.Credentials = credentials;
            //gateway.PreAuthenticate = true;
            var x = gateway.Auth("test001", "testurl", new TCG.PaymentGatewayLibrary.PayGatePayBatch.PayBatchHelper.BatchData { BatchLine = new string[] { "" } });
            return gateway;
        }

        internal static XmlDocument PostXMLTransaction(string v_strURL, String v_objXMLDoc)
        {
            //Declare XMLResponse document
            XmlDocument XMLResponse = null;

            //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebRequest objHttpWebRequest;

            //Declare an HTTP-specific implementation of the WebResponse class
            HttpWebResponse objHttpWebResponse = null;

            //Declare a generic view of a sequence of bytes
            Stream objRequestStream = null;
            Stream objResponseStream = null;

            //Declare XMLReader
            XmlTextReader objXMLReader;

            //Creates an HttpWebRequest for the specified URL.
            objHttpWebRequest = (HttpWebRequest)WebRequest.Create(v_strURL);

            try
            {
                //---------- Start HttpRequest 

                //Set HttpWebRequest properties
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(v_objXMLDoc);
                objHttpWebRequest.Method = "POST";
                objHttpWebRequest.ContentLength = bytes.Length;
                objHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                //Get Stream object 
                objRequestStream = objHttpWebRequest.GetRequestStream();

                //Writes a sequence of bytes to the current stream 
                objRequestStream.Write(bytes, 0, bytes.Length);

                //Close stream
                objRequestStream.Close();

                //---------- End HttpRequest

                //Sends the HttpWebRequest, and waits for a response.
                objHttpWebResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();

                //---------- Start HttpResponse
                if (objHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //Get response stream 
                    objResponseStream = objHttpWebResponse.GetResponseStream();

                    //Load response stream into XMLReader
                    objXMLReader = new XmlTextReader(objResponseStream);

                    //Declare XMLDocument
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(objXMLReader);

                    //Set XMLResponse object returned from XMLReader
                    XMLResponse = xmldoc;

                    //Close XMLReader
                    objXMLReader.Close();
                }

                //Close HttpWebResponse
                objHttpWebResponse.Close();
            }
            catch (WebException we)
            {
                //TODO: Add custom exception handling
                throw we;
                throw new Exception(we.Message);
            }
            catch (Exception ex)
            {
                throw ex;
                throw new Exception(ex.Message);
            }
            finally
            {
                //Close connections
                objRequestStream.Close();
                objResponseStream.Close();
                objHttpWebResponse.Close();

                //Release objects
                objXMLReader = null;
                objRequestStream = null;
                objResponseStream = null;
                objHttpWebResponse = null;
                objHttpWebRequest = null;
            }

            //Return
            return XMLResponse;
        }

        #endregion
    }
}
