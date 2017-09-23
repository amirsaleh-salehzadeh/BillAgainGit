using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Providers
{
    public class gwSetcom : IPaymentStrategy
    {
        private string CompanyID;
        private string Outlet;
        private string Username;
        private string Password;
        private bool isAutoSettleOn { get; set; }

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwSetcom; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "Setcom",
                        WebUrl: "https://www.setcom.co.za/",
                        Description: "Setcom Offers Secure Online Credit Card Processing and Fraud Prevention Solutions for South African Businesses.",
                        isActive: false,
                        isLive: false,
                        MerchantConfigValues: new[] { "CompanyID", "Outlet", "Username", "Password", "isAutoSettleOn" },
                        Currencies: new[] { "ZAR" },
                        Countries: new[] { "ZA" },
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
        public PaymentOptions paymentOptions
        {
            get
            {
                return new PaymentOptions
                    (
                        requires_CVV: true,
                        PaymentTokenize: false,
                        PaymentTokenize_requires_CVV: false,
                        PaymentTokenize_external: false,
                        ThreeDSecure: true,
                        Verify: false,
                        Auth: true,
                        AuthCapture: true,
                        AuthCapturePartial: true,
                        Sale: true,
                        Refund: true,
                        RefundPartial: true,
                        Credit: false,
                        Void: true
                    );
            }
        }
        #endregion

        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            CompanyID = MerchantConfigValues.Where(r => r.Key.Equals("CompanyID")).FirstOrDefault().Value;
            Outlet = MerchantConfigValues.Where(r => r.Key.Equals("Outlet")).FirstOrDefault().Value;
            Username = MerchantConfigValues.Where(r => r.Key.Equals("Username")).FirstOrDefault().Value;
            Password = MerchantConfigValues.Where(r => r.Key.Equals("Password")).FirstOrDefault().Value;
            
            bool _isAutoSettleOn = true;
            if (MerchantConfigValues.Where(r => r.Key.Equals("isAutoSettleOn")).FirstOrDefault() != null)
            {
                isAutoSettleOn = bool.TryParse(MerchantConfigValues.Where(r => r.Key.Equals("isAutoSettleOn")).FirstOrDefault().Value, out _isAutoSettleOn);
            }          
            isAutoSettleOn = _isAutoSettleOn;
        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="CompanyID",
                    Value="testaccount"
                },
                new MerchantConfigValue
                {
                    Key="Outlet",
                    Value="testaccount"
                },
                new MerchantConfigValue
                {
                    Key="Username",
                    Value="testaccount"
                },
                new MerchantConfigValue
                {
                    Key="Password",
                    Value="testaccount"
                }
                ,
                new MerchantConfigValue
                {
                    Key="isAutoSettleOn",
                    Value="true"
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

        public Lookup3dSecure_Result Lookup3dSecure(Lookup3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Authenticate3dSecure_Result Authenticate3dSecure(Authenticate3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Verify(Sale_Details details)
        {
            // Supported but linked to Customer on BrainTree
            throw new NotSupportedException();
        }

        public Transaction_Result Auth(Sale_Details details)
        {
            if (isAutoSettleOn)
            {
                throw new Exception("Cannot do an authorisation only since gateway is configured to automatically mark authorisations to be settled");
            }

            string cardType = (string)Enum.GetName(typeof(CardTypeEnum), details.CardType);
            string qs = getAuthQS(CompanyID, Outlet, details.ExtRef, details.Amount.ToString("F2"), cardType, details.CustomerFirstName + " " + details.CustomerLastName, details.CardNumber, details.CardExpiryYear.ToString(), details.CardExpiryMonth.ToString(), details.CardCCV);

            return PostTransaction("https://secure.setcom.co.za/server.cfm", qs);
        }

        //wont work with auto settle on
        public Transaction_Result Capture(AuthCapture_Details details)
        {
            if (isAutoSettleOn)
            {
                throw new Exception("Cannot do an capture only since gateway is configured to automatically mark authorisations to be settled");
            }

            string qs = getCaptureQS(CompanyID, Outlet, details.TransactionIndex, details.Amount.Value.ToString("F2"), Username, Password, details.CardNumber, details.ExpYear.ToString(), details.ExpMonth.ToString(), details.CVV);

            return PostManagerTransaction("https://manager.setcom.co.za/captures2s.cfm", qs);
        }

        public Transaction_Result Sale(Sale_Details details)
        {
            if (isAutoSettleOn)
            {
                string cardType = (string)Enum.GetName(typeof(CardTypeEnum), details.CardType);
                string qs = getAuthQS(CompanyID, Outlet, details.ExtRef, details.Amount.ToString("F2"), cardType, details.CustomerFirstName + " " + details.CustomerLastName, details.CardNumber, details.CardExpiryYear.ToString(), details.CardExpiryMonth.ToString(), details.CardCCV);

                return PostTransaction("https://secure.setcom.co.za/server.cfm", qs);
            }
            else
            {
                var auth = Auth(details);
                return Capture(new AuthCapture_Details
                {
                    Amount = details.Amount,
                    CurrencyCode = details.CurrencyCode,
                    TransactionIndex = auth.TransactionIndex
                });
            }
            

        }

        public Transaction_Result Refund(Refund_Details details)
        {
            string qs = getRefundQS(CompanyID, Outlet, details.TransactionIndex, details.Amount.Value.ToString("F2"), Username, Password, details.CardNumber, details.ExpYear.ToString(), details.ExpMonth.ToString(), details.CVV);

            return PostManagerTransaction("https://manager.setcom.co.za/captures2s.cfm", qs);
        }

        //NOT IMPLEMENTED
        public Transaction_Result Credit(Credit_Details details)
        {
            throw new NotImplementedException();
        }

        //wont work with auto settle on
        public Transaction_Result Void(Void_Details details)
        {
            string qs = getVoidQS(CompanyID, Outlet, details.TransactionIndex, details.Amount.Value.ToString("F2"), Username, Password, details.CardNumber, details.ExpYear.ToString(), details.ExpMonth.ToString(), details.CVV);

            return PostManagerTransaction("https://manager.setcom.co.za/captures2s.cfm", qs);
        }

        public void runTests()
        {
            var ran = new Random();

            var sale_details_visa_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "4444333322221111", CardExpiryMonth = 11, CardExpiryYear = 2020, CardCCV = "111", CardType = CardTypeEnum.VISA, Amount = 10, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            //var sale_details_visa_expired = new Sale_Details { accountID = -1, customerID = -1, transactionID = -1, InvoiceNumber = "test1" + ran.Next(), CardNumber = "4000000000000010", CardExpiryMonth = 10, CardExpiryYear = 2011, CardCCV = "111", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", CurrencyCodeNumeric = "840", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            //var sale_details_visa_cvv = new Sale_Details { accountID = -1, customerID = -1, transactionID = -1, InvoiceNumber = "test1" + ran.Next(), CardNumber = "4000000000000010", CardExpiryMonth = 10, CardExpiryYear = 2011, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", CurrencyCodeNumeric = "840", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };

            LoginTest();

            // Store Payment Method

            // Revoke Payment Method

            // 3D Secure

            // Verify Card

            // AUTH
            if (!isAutoSettleOn)
            {
                var auth_tr = Auth(sale_details_visa_success).testApproved("Auth");

                //Auth(sale_details_visa_expired).testNotApproved("Auth");
                //Auth(sale_details_visa_cvv).testNotApproved("Auth");

                //CAPTURE
                if (!isAutoSettleOn)
                {
                    var capture_tr = Capture(new AuthCapture_Details
                    {
                        Amount = 1,
                        CurrencyCode = "ZAR",
                        TransactionIndex = auth_tr.TransactionIndex
                    }).testApproved("Capture");
                }
            }
            
            //SALE
            var sale_tr = Sale(sale_details_visa_success).testApproved("Sale");

            //REFUND
            Transaction_Result refund_tr = Refund(new Refund_Details
            {
                Amount = 5,
                CurrencyCode = "ZAR",
                TransactionIndex = sale_tr.TransactionIndex,
                CardNumber=sale_details_visa_success.CardNumber,
                ExpMonth=sale_details_visa_success.CardExpiryMonth,
                ExpYear=sale_details_visa_success.CardExpiryYear,
                CVV=sale_details_visa_success.CardCCV

            }).testApproved("Refund");

            // Credit
            // NOT IMPLEMENTED

            //VOID
            if (!isAutoSettleOn)
            {
                var auth_tr_void = Auth(sale_details_visa_success).testApproved("Auth");
                var void_tr = Void(new Void_Details
                {
                    Amount = 1,
                    CurrencyCode = "ZAR",
                    TransactionIndex = auth_tr_void.TransactionIndex
                }).testApproved("Void");
            }
            
        }
        #endregion

        #region Helpers
        private string getAuthQS(string CO_ID, string OUTLET, string Reference, string CC_Amount, string CCType, string CCName, string CCNumber, string ExYear, string ExMonth, string CCCVV)
        {
            string i_CO_ID = CO_ID;
            string i_OUTLET = OUTLET;
            string i_Reference = Reference;
            string i_CC_Amount = CC_Amount;
            string i_CCType = CCType;
            string i_CCName = CCName;
            string i_CCNumber = CCNumber;
            string i_ExYear = ExYear;
            string i_ExMonth = ExMonth;
            string i_CCCVV = CCCVV;

            /*------------Purchase Request------------*/
            StringBuilder sb_purchase_data = new StringBuilder();
            sb_purchase_data.Append(string.Format("CO_ID={0}", i_CO_ID));
            sb_purchase_data.Append(string.Format("&OUTLET={0}", i_OUTLET));
            sb_purchase_data.Append(string.Format("&Reference={0}", i_Reference));
            sb_purchase_data.Append(string.Format("&CC_Amount={0}", i_CC_Amount));
            sb_purchase_data.Append(string.Format("&CCType={0}", i_CCType));
            sb_purchase_data.Append(string.Format("&CCName={0}", i_CCName));
            sb_purchase_data.Append(string.Format("&CCNumber={0}", i_CCNumber));
            sb_purchase_data.Append(string.Format("&ExYear={0}", i_ExYear));
            sb_purchase_data.Append(string.Format("&ExMonth={0}", i_ExMonth));
            sb_purchase_data.Append(string.Format("&CCCVV={0}", i_CCCVV));

            return sb_purchase_data.ToString();
        }

        private string getCaptureQS(string CO_ID, string OUTLET, string orderID, string amount, string username, string password, string CCNumber, string ExYear, string ExMonth, string CCCVV)
        {
            string i_CO_ID = CO_ID;
            string i_OUTLET = OUTLET;
            string i_orderID = orderID;
            string i_TnxType = "SHIP";
            string i_CCAmount = amount;
            string i_Username = username;
            string i_Password = password;
            string i_CCNumber = CCNumber;
            string i_ExYear = ExYear;
            string i_ExMonth = ExMonth;
            string i_CCCVV = CCCVV;

            /*------------Purchase Request------------*/
            StringBuilder sb_purchase_data = new StringBuilder();
            sb_purchase_data.Append(string.Format("CO_ID={0}", i_CO_ID));
            sb_purchase_data.Append(string.Format("&OUTLET={0}", i_OUTLET));
            sb_purchase_data.Append(string.Format("&OrderID={0}", i_orderID));
            sb_purchase_data.Append(string.Format("&TnxType={0}", i_TnxType));
            sb_purchase_data.Append(string.Format("&Amount={0}", i_CCAmount));
            sb_purchase_data.Append(string.Format("&Username={0}", i_Username));
            sb_purchase_data.Append(string.Format("&Password={0}", i_Password));
            sb_purchase_data.Append(string.Format("&CardNumber={0}", i_CCNumber));
            sb_purchase_data.Append(string.Format("&CardExYear={0}", i_ExYear));
            sb_purchase_data.Append(string.Format("&CardExMonth={0}", i_ExMonth));
            sb_purchase_data.Append(string.Format("&CardCVV={0}", i_CCCVV));

            return sb_purchase_data.ToString();

        }

        private string getRefundQS(string CO_ID, string OUTLET, string orderID, string amount, string username, string password, string CCNumber, string ExYear, string ExMonth, string CCCVV)
        {
            string i_CO_ID = CO_ID;
            string i_OUTLET = OUTLET;
            string i_orderID = orderID;
            string i_TnxType = "REFUND";
            string i_CCAmount = amount;
            string i_Username = username;
            string i_Password = password;
            string i_CCNumber = CCNumber;
            string i_ExYear = ExYear;
            string i_ExMonth = ExMonth;
            string i_CCCVV = CCCVV;

            /*------------Purchase Request------------*/
            StringBuilder sb_purchase_data = new StringBuilder();
            sb_purchase_data.Append(string.Format("CO_ID={0}", i_CO_ID));
            sb_purchase_data.Append(string.Format("&OUTLET={0}", i_OUTLET));
            sb_purchase_data.Append(string.Format("&OrderID={0}", i_orderID));
            sb_purchase_data.Append(string.Format("&TnxType={0}", i_TnxType));
            sb_purchase_data.Append(string.Format("&Amount={0}", i_CCAmount));
            sb_purchase_data.Append(string.Format("&Username={0}", i_Username));
            sb_purchase_data.Append(string.Format("&Password={0}", i_Password));
            sb_purchase_data.Append(string.Format("&CardNumber={0}", i_CCNumber));
            sb_purchase_data.Append(string.Format("&CardExYear={0}", i_ExYear));
            sb_purchase_data.Append(string.Format("&CardExMonth={0}", i_ExMonth));
            sb_purchase_data.Append(string.Format("&CardCVV={0}", i_CCCVV));

            return sb_purchase_data.ToString();
        }

        private string getVoidQS(string CO_ID, string OUTLET, string orderID, string amount, string username, string password, string CCNumber, string ExYear, string ExMonth, string CCCVV)
        {
            string i_CO_ID = CO_ID;
            string i_OUTLET = OUTLET;
            string i_orderID = orderID;
            string i_TnxType = "CANCEL";
            string i_CCAmount = amount;
            string i_Username = username;
            string i_Password = password;
            string i_CCNumber = CCNumber;
            string i_ExYear = ExYear;
            string i_ExMonth = ExMonth;
            string i_CCCVV = CCCVV;

            /*------------Purchase Request------------*/
            StringBuilder sb_purchase_data = new StringBuilder();
            sb_purchase_data.Append(string.Format("CO_ID={0}", i_CO_ID));
            sb_purchase_data.Append(string.Format("&OUTLET={0}", i_OUTLET));
            sb_purchase_data.Append(string.Format("&OrderID={0}", i_orderID));
            sb_purchase_data.Append(string.Format("&TnxType={0}", i_TnxType));
            sb_purchase_data.Append(string.Format("&Amount={0}", i_CCAmount));
            sb_purchase_data.Append(string.Format("&Username={0}", i_Username));
            sb_purchase_data.Append(string.Format("&Password={0}", i_Password));
            sb_purchase_data.Append(string.Format("&CardNumber={0}", i_CCNumber));
            sb_purchase_data.Append(string.Format("&CardExYear={0}", i_ExYear));
            sb_purchase_data.Append(string.Format("&CardExMonth={0}", i_ExMonth));
            sb_purchase_data.Append(string.Format("&CardCVV={0}", i_CCCVV));

            return sb_purchase_data.ToString();
        }

        private Transaction_Result PostTransaction(string v_strURL, String v_objQSData)
        {
            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(v_objQSData.ToString());
                WebRequest purchase_request = WebRequest.Create(v_strURL);
                purchase_request.Method = "POST";
                purchase_request.ContentType = "application/x-www-form-urlencoded";
                purchase_request.ContentLength = postData.Length;
                Stream data = purchase_request.GetRequestStream();
                data.Write(postData, 0, postData.Length);
                data.Close();
                WebResponse response = purchase_request.GetResponse();
                data = response.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                string serverResponse = reader.ReadToEnd();
                reader.Close();
                string[] TrimStrings = serverResponse.Split(',');
                string ReturnString = string.Empty;
                for (int i = 0; i < TrimStrings.Length; i++)
                {
                    if (i != TrimStrings.Length - 1)
                        ReturnString = ReturnString + TrimStrings[i].Trim() + ",";
                    else
                        ReturnString = ReturnString + TrimStrings[i].Trim();
                }

                /*------------Handling purchase response------------*/
                List<string> ReturnedList = new List<string>(ReturnString.ToString().Split(','));
                string outcome = ReturnedList[0];
                string response_indicator = ReturnedList[1];
                string tnx_date = ReturnedList[2];
                string tnx_time = ReturnedList[3];
                string orderID = ReturnedList[4];
                string Merchant_Reference = ReturnedList[5];
                string tnx_Amount = ReturnedList[6];

                var stringwriter = new System.IO.StringWriter();
                var xmlwriter = XmlWriter.Create(stringwriter, new XmlWriterSettings { NewLineHandling = NewLineHandling.None });
                var serializer = new XmlSerializer(ReturnedList.GetType());
                serializer.Serialize(xmlwriter, ReturnedList);
                var fullresp = stringwriter.ToString();

                string statusText = "";
                bool isSuccessful = false;

                switch (outcome)
                {
                    case "APPROVED":
                    case "Approved":
                        statusText = "Thank you, your purchase has been Approved.";
                        isSuccessful = true;
                        break;
                    case "DECLINED":
                    case "Declined":
                        statusText = "Sorry, your purchase has been Declined.";
                        break;
                    case "ERROR":
                    case "Error":
                        statusText = "Sorry, an error occurred while processing your purchase request.";
                        break;
                    case "DUPLICATE":
                    case "Duplicate":
                        statusText = "This is a duplicate order. Please change the order amount or contact the merchant.";
                        break;
                    case "REVIEW":
                    case "Review":
                        statusText = "Your order has been stopped for review by order screening engine. Your card has not been charged.";
                        break;
                }

                StringWriter reqWriter = new StringWriter();
                XmlDocument reqDoc = new XmlDocument();
                var root = reqDoc.CreateElement("Request");
                reqDoc.AppendChild(root);
                List<string> FullRequestListSplit = v_objQSData.Split('&').ToList();
                foreach (var item in FullRequestListSplit)
                {
                    var item_split = item.Split('=');

                    var elem = reqDoc.CreateElement(item_split[0]);
                    elem.InnerText = item_split[1];

                    root.AppendChild(elem);
                }
                reqDoc.Save(reqWriter);

                return new Transaction_Result
                {
                    isApproved = isSuccessful,
                    ApprovalCode = isSuccessful ? response_indicator : "",
                    ResultCode = outcome,
                    ResultText = statusText,
                    TransactionIndex = orderID,
                    ProcessorCode = Merchant_Reference,
                    FullRequest = reqWriter.ToString(),
                    FullResponse = fullresp,
                    hasServerError = false,
                    ErrorCode = !isSuccessful ? response_indicator : "",
                    ErrorText = !isSuccessful ? statusText : ""
                };
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
        }

        private Transaction_Result PostManagerTransaction(string v_strURL, String v_objQSData)
        {
            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(v_objQSData.ToString());
                WebRequest purchase_request = WebRequest.Create(v_strURL);
                purchase_request.Method = "POST";
                purchase_request.ContentType = "application/x-www-form-urlencoded";
                purchase_request.ContentLength = postData.Length;
                Stream data = purchase_request.GetRequestStream();
                data.Write(postData, 0, postData.Length);
                data.Close();
                WebResponse response = purchase_request.GetResponse();
                data = response.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                string serverResponse = reader.ReadToEnd();
                reader.Close();
                string[] TrimStrings = serverResponse.Split(',');
                string ReturnString = string.Empty;
                for (int i = 0; i < TrimStrings.Length; i++)
                {
                    if (i != TrimStrings.Length - 1)
                        ReturnString = ReturnString + TrimStrings[i].Trim() + ",";
                    else
                        ReturnString = ReturnString + TrimStrings[i].Trim();
                }

                /*------------Handling purchase response------------*/
                List<string> ReturnedList = new List<string>(ReturnString.ToString().Split(','));
                string outcome = ReturnedList[0];
                string error_code = ReturnedList[1]; //For approved transactions this field will simply contain the text APPROVED
                string authorisation_number = ReturnedList[2]; //the bank authorisation number
                string tnx_date = ReturnedList[3];
                string tnx_time = ReturnedList[4];
                string orderID = ReturnedList[5];
                string transaction_key = ReturnedList[6]; //Transaction key as generated by the bank
                string transaction_type = ReturnedList[7];
                string tnx_Amount = ReturnedList[8];

                string statusText = "";
                bool isSuccessful = false;

                var stringwriter = new System.IO.StringWriter();
                var xmlwriter = XmlWriter.Create(stringwriter, new XmlWriterSettings { NewLineHandling = NewLineHandling.None });
                var serializer = new XmlSerializer(ReturnedList.GetType());
                serializer.Serialize(xmlwriter, ReturnedList);
                var fullresp = stringwriter.ToString();

                switch (outcome)
                {
                    case "APPROVED":
                    case "Approved":
                        statusText = "Thank you, your purchase has been Approved.";
                        isSuccessful = true;
                        break;
                    case "DECLINED":
                    case "Declined":
                        statusText = "Sorry, your purchase has been Declined.";
                        break;
                    case "ERROR":
                    case "Error":
                        statusText = "Sorry, an error occurred while processing your purchase request.";
                        break;
                    case "DUPLICATE":
                    case "Duplicate":
                        statusText = "This is a duplicate order. Please change the order amount or contact the merchant.";
                        break;
                    case "REVIEW":
                    case "Review":
                        statusText = "Your order has been stopped for review by order screening engine. Your card has not been charged.";
                        break;
                }

                return new Transaction_Result
                {
                    isApproved = isSuccessful,
                    ApprovalCode = isSuccessful ? error_code : "",
                    ResultCode = outcome,
                    ResultText = statusText,
                    TransactionIndex = orderID,
                    ProcessorCode = transaction_key,
                    FullRequest = v_objQSData,
                    FullResponse = fullresp,
                    hasServerError = false,
                    ErrorCode = !isSuccessful ? error_code : "",
                    ErrorText = !isSuccessful ? statusText : ""
                };
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
        }
        #endregion
    }
}
