using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Providers
{
    public class gwPayGate : IPaymentStrategy
    {
        private string PayGateID;
        private string Password;
        private string url = "https://www.paygate.co.za/payxml/process.trans";
        private string version = "4.0";

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwPayGate; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                    
                        DisplayName: "PayGate PayXML",
                        WebUrl: "https://www.paygate.co.za/",
                        Description: "With PayGate you can accept Visa™, MasterCard®, American Express™ and Diners™ cards from around the world securely online.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "payGateID", "password" },
                        Currencies: new[] { "AED", "AFA", "ALL", "AMD", "ANG", "AOA",  "ARS", "AUD", "AWG",  "AZM", 
                                            "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BRL", "BSD", "BWP", "BYR",  "BZD", 
                                            "CDF", "CHF", "CLP", "CNY", "COP", "CRC", "CUP", "CYP", "CZK", 
                                            "DEM", "DJF", "DKK", "DOP", "DZD",  
                                            "ECS", "EEK", "EGP", "ERN", "ETB", "EUR", 
                                            "FJD", "FKP",
                                            "GEL", "GHC", "GIP","GMD", "GNF", "GTQ",  "GWP", "GYD",
                                            "HKD", "HNL","HRK", "HTG", "HUF",
                                            "IDR", "INR", "IQD", "IRA", "IRR", "ISK", 
                                            "JMD", "JOD", "JPY",
                                            "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KZT",     
                                            "LAK", "LBP", "LKR", "LRD", "LTL", "LVL", "LYD", 
                                            "MAD", "MDL", "MGF", "MKD", "MMK", "MNT","MOP", "MRO", "MTL", "MUR", "MVR", "MWK", "MXN", "MYR", "MZM", 
                                            "NAD", "NGN", "NIO", "NOK", "NPR", "NZD",  
                                            "OMR",
                                            "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG",
                                            "QAR",
                                            "ROL", "RUB", "RUR", "RWF", 
                                            "SAR", "SBD", "SCR", "SDA", "SDD", "SEK", "SGD", "SHP", "SIT", "SKK", "SLL", "SOS", "SRG", "STD", "SYP", "SZL", 
                                            "THB", "TJS", "TMM", "TND", "TOP", "TPE", "TRL", "TTD", "TWD",       
                                            "UAH", "UGX", "USD","UYU", "UZS",
                                            "VEB", "VND", "VUV", 
                                            "WST",
                                            "XAF", "XCD", "XEU", "XOF", "XPF",
                                            "YER", "YUM", 
                                            "ZAR", "ZMK","ZWD" },
                        Countries: new[] {  "AF", "AL", "DZ", "AS", "AD", "AO", "AI", "AQ", "AG", "AR", "AM", "AW", "AU", "AT", "AZ", "BS", "BH", "BD", "BB", 
                                            "BY", "BE", "BZ", "BJ", "BM", "BT", "BO", "BA", "BW", "BV", "BR", "IO", "VG", "BN", "BG", "BF", "BI", "KH", "CM", 
                                            "CA", "CV", "KY", "CF", "TD", "CL", "CN", "CX", "CC", "CO", "KM", "CG", "CK", "CR", "CI", "HR", "CU", "CY", "CZ", 
                                            "CD", "DK", "DJ", "DM", "DO", "TP", "EC", "EG", "SV", "GQ", "ER", "EE", "ET", "FO", "FK", "FJ", "FI", "FR", "FX", 
                                            "GF", "PF", "TF", "GA", "GM", "GE", "DE", "GH", "GI", "GR", "GL", "GD", "GP", "GU", "GT", "GN", "GW", "GY", "HT", 
                                            "HM", "VA", "HN", "HK", "HU", "IS", "IN", "IR", "IQ", "IE", "IT", "JM", "JP", "JO", "KZ", "KE", "KI", "KP", "KR", 
                                            "KW", "KG", "LA", "LV", "LB", "LS", "LR", "LY", "LI", "LT", "LU", "MO", "MK", "MG", "MW", "MY", "MV", "ML", "MT", 
                                            "MH", "MQ", "MR", "MU", "YT", "MX", "FM", "MD", "MC", "MN", "MS", "MA", "MZ", "MM", "NA", "NR", "NP", "NL", "AN", 
                                            "NC", "NZ", "NI", "NE", "NG", "NU", "NF", "MP", "NO", "OM", "PK", "PW", "PA", "PG", "PY", "PE", "PH", "PN", "PL", 
                                            "PT", "PR", "QA", "RE", "RO", "RU", "RW", "WS", "SM", "ST", "SA", "SN", "SC", "SL", "SG", "SK", "SI", "GS", "SB", 
                                            "SO", "ZA", "ES", "LK", "SH", "KN", "LC", "PM", "VC", "SD", "SR", "SJ", "SZ", "SE", "CH", "SY", "TW", "TJ", "TZ", 
                                            "TH", "TG", "TK", "TO", "TT", "TN", "TR", "TM", "TC", "TV", "UM", "VI", "UG", "UA", "AE", "GB", "US", "UY", "UZ", 
                                            "VU", "VE", "VN", "WF", "EH", "YE", "YU", "ZM", "ZW" },
                        CardTypes: new[] {  CardTypeEnum.VISA, 
                                            CardTypeEnum.MASTERCARD, 
                                            CardTypeEnum.AMERICAN_EXPRESS, 
                                            CardTypeEnum.DINERS_CLUB }
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
                        ThreeDSecure: false,
                        Verify: false,
                        Auth: true,
                        AuthCapture: true,
                        AuthCapturePartial: false,
                        Sale: true,
                        Refund: true,
                        RefundPartial: true,
                        Credit: false,
                        Void: false
                    );
            }
        }
        #endregion

        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            PayGateID = MerchantConfigValues.Where(r => r.Key.Equals("PayGateID")).FirstOrDefault().Value;
            Password = MerchantConfigValues.Where(r => r.Key.Equals("Password")).FirstOrDefault().Value;
        }

        public void LoginTest()
        {
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

        //NOT IMPLEMENTED
        public Transaction_Result Verify(Sale_Details details)
        {
            throw new NotSupportedException();
        }

        public Transaction_Result Auth(Sale_Details details)
        {
            try
            {
                var xmlString = getAuthXML(
                    PayGateID,
                    Password,
                    details.ExtRef,
                    details.CustomerFirstName + " " + details.CustomerLastName,
                    details.CardNumber,
                    GatewayUtils.formatExpiryDate(details.CardExpiryMonth, details.CardExpiryYear),
                    "0",                                    //budget period 0 == Straight 
                    (details.Amount * 100).ToString("F0"),      //takes amounts in cents
                    details.CurrencyCode,
                    details.CardCCV
                    );

                var xml = GatewayUtils.PostXMLTransaction(url, xmlString);
                XmlNode protocol = xml.GetElementsByTagName("protocol").Item(0);
                XmlNode errorNode = protocol.SelectNodes("errorrx").Item(0);

                string cardnumber = details.CardNumber;
                Regex rgx = new Regex(@"[^\d]");
                cardnumber = rgx.Replace(cardnumber, String.Empty); //removes dashes spaces, etc
                string last4digits = cardnumber.Substring(details.CardNumber.Length - 4);

                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(details.GetType());
                serializer.Serialize(stringwriter, details);
                var fullrequest = stringwriter.ToString();

                fullrequest = fullrequest.Replace(cardnumber, last4digits);
                fullrequest = fullrequest.Replace(details.CardCCV, "***");

                var respstringwriter = new System.IO.StringWriter();
                xml.Save(respstringwriter);

                if (errorNode == null)
                {
                    XmlNode successNode = protocol.SelectNodes("authrx").Item(0);
                    string transactionID = successNode.Attributes.GetNamedItem("tid").Value; //The unique reference number assign by PayGate to the original transaction.
                    string customerReference = successNode.Attributes.GetNamedItem("cref").Value; //This is your reference number for use by your internal systems. Must be different per transaction
                    string status = successNode.Attributes.GetNamedItem("stat").Value; //Transaction status. 
                    string statusDesc = successNode.Attributes.GetNamedItem("sdesc").Value; //Transaction status description.
                    string resultCode = successNode.Attributes.GetNamedItem("res").Value; //Result Code
                    string resultDesc = successNode.Attributes.GetNamedItem("rdesc").Value; //Result Code description.
                    string authCode = successNode.Attributes.GetNamedItem("auth").Value; //The Authorisation code returned by the acquirer (bank).
                    string risk = successNode.Attributes.GetNamedItem("risk").Value; //X - NA, A - Authenticated, N - Not Authenticated This is a 2-character field containing a risk indicator for this transaction.
                    string cardTypeCode = successNode.Attributes.GetNamedItem("ctype").Value; //Card type code 0 - Unknown, 1 - Visa, 2 - MasterCard, 3 - Amex, 4 - Diners
                    return new Transaction_Result
                    {
                        TransactionIndex = transactionID,
                        isApproved = status != "0" && status != "2",
                        hasServerError = false,
                        ApprovalCode = statusDesc,
                        ResultCode = resultCode,
                        ResultText = resultDesc,
                        ProcessorCode = authCode,
                        FullRequest = fullrequest,
                        FullResponse = xml.OuterXml
                    };
                }
                else
                {
                    string errorCode = errorNode.Attributes.GetNamedItem("ecode").Value;
                    string errorDesc = errorNode.Attributes.GetNamedItem("edesc").Value;

                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = errorCode,
                        ErrorText = errorDesc,
                    };
                }
            }
            catch (Exception ex)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    hasServerError = true,
                    ErrorText = ex.Message
                };
            }
        }

        public Transaction_Result Capture(AuthCapture_Details details)
        {
            try
            {
                var xmlString = getCaptureXML(PayGateID, Password, details.TransactionIndex);
                var xml = GatewayUtils.PostXMLTransaction(url, xmlString);
                XmlNode protocol = xml.GetElementsByTagName("protocol").Item(0);
                XmlNode errorNode = protocol.SelectNodes("errorrx").Item(0);

                var reqXmlDoc = new XmlDocument();
                reqXmlDoc.LoadXml(xmlString);

                var reqstringwriter = new System.IO.StringWriter();
                reqXmlDoc.Save(reqstringwriter);
                
                var respstringwriter = new System.IO.StringWriter();
                xml.Save(respstringwriter);

                if (errorNode == null)
                {
                    XmlNode successNode = protocol.SelectNodes("settlerx").Item(0);
                    string transactionID = successNode.Attributes.GetNamedItem("tid").Value; //The unique reference number assign by PayGate to the original transaction.
                    string customerReference = successNode.Attributes.GetNamedItem("cref").Value; //This is your reference number for use by your internal systems. Must be different per transaction
                    string status = successNode.Attributes.GetNamedItem("stat").Value; //Transaction status. 
                    string statusDesc = successNode.Attributes.GetNamedItem("sdesc").Value; //Transaction status description.
                    string resultCode = successNode.Attributes.GetNamedItem("res").Value; //Result Code
                    string resultDesc = successNode.Attributes.GetNamedItem("rdesc").Value; //Result Code description.
                    return new Transaction_Result
                    {
                        TransactionIndex = transactionID,
                        isApproved = status != "0" && status != "2",
                        ApprovalCode = statusDesc,
                        ResultCode = resultCode,
                        ResultText = resultDesc,
                        FullRequest = reqstringwriter.ToString(),
                        FullResponse = protocol.InnerXml
                    };
                }
                else
                {
                    string errorCode = errorNode.Attributes.GetNamedItem("ecode").Value;
                    string errorDesc = errorNode.Attributes.GetNamedItem("edesc").Value;

                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = errorCode,
                        ErrorText = errorDesc,
                    };
                }
            }
            catch (Exception ex)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    hasServerError = true,
                    ErrorText = ex.Message
                };
            }
        }

        public Transaction_Result Sale(Sale_Details details)
        {
            try
            {
                Transaction_Result authResult = Auth(details);

                if (!authResult.isApproved)
                {
                    authResult.ErrorText = "Authorisation Error - " + authResult.ErrorText;
                    return authResult;
                }

                Transaction_Result captureResult = Capture(new AuthCapture_Details { TransactionIndex = authResult.TransactionIndex });

                if (!captureResult.isApproved)
                {
                    captureResult.ErrorText = "Capture Error - " + authResult.ErrorText;
                    return captureResult;
                }
                captureResult.TransactionIndex = authResult.TransactionIndex;
                return captureResult;
            }
            catch (Exception ex)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    hasServerError = true,
                    ErrorText = ex.Message
                };
            }
        }

        public Transaction_Result Refund(Refund_Details details)
        {
            try
            {
                var xmlString = getRefundXML(PayGateID, Password, details.TransactionIndex, details.Amount.HasValue ? (details.Amount * 100).ToString() : "");
                var xml = GatewayUtils.PostXMLTransaction(url, xmlString);
                XmlNode protocol = xml.GetElementsByTagName("protocol").Item(0);
                XmlNode errorNode = protocol.SelectNodes("errorrx").Item(0);

                var reqXmlDoc = new XmlDocument();
                reqXmlDoc.LoadXml(xmlString);

                var reqstringwriter = new System.IO.StringWriter();
                reqXmlDoc.Save(reqstringwriter);

                var respstringwriter = new System.IO.StringWriter();
                xml.Save(respstringwriter);

                if (errorNode == null)
                {
                    XmlNode successNode = protocol.SelectNodes("refundrx").Item(0);
                    string transactionID = successNode.Attributes.GetNamedItem("tid").Value; //The unique reference number assign by PayGate to the original transaction.
                    string customerReference = successNode.Attributes.GetNamedItem("cref").Value; //This is your reference number for use by your internal systems. Must be different per transaction
                    string status = successNode.Attributes.GetNamedItem("stat").Value; //Transaction status. 
                    string statusDesc = successNode.Attributes.GetNamedItem("sdesc").Value; //Transaction status description.
                    string resultCode = successNode.Attributes.GetNamedItem("res").Value; //Result Code
                    string resultDesc = successNode.Attributes.GetNamedItem("rdesc").Value; //Result Code description.
                    return new Transaction_Result
                    {
                        TransactionIndex = transactionID,
                        isApproved = status != "0" && status != "2",
                        ApprovalCode = statusDesc,
                        ResultCode = resultCode,
                        ResultText = resultDesc,
                        FullRequest = reqstringwriter.ToString(),
                        FullResponse = respstringwriter.ToString()
                    };
                }
                else
                {
                    string errorCode = errorNode.Attributes.GetNamedItem("ecode").Value;
                    string errorDesc = errorNode.Attributes.GetNamedItem("edesc").Value;

                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = errorCode,
                        ErrorText = errorDesc,
                    };
                }
            }
            catch (Exception ex)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    hasServerError = true,
                    ErrorText = ex.Message
                };
            }
        }

        //NOT IMPLEMENTED
        public Transaction_Result Credit(Credit_Details details)
        {
            throw new NotSupportedException();
        }

        //NOT IMPLEMENTED
        public Transaction_Result Void(Void_Details details)
        {
            throw new NotSupportedException();
        }

        public void runTests()
        {
            var ran = new Random();
            // classes
            var sale_details_visa_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef="myref" + ran.Next(), CardNumber = "4000000000000002", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_visa_fail = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef="myref" + ran.Next(), CardNumber = "4000000000000036", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_visa_nofunds = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "4000000000000028", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_visa_fail_expiry = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "4000000000000002", CardExpiryMonth = 11, CardExpiryYear = 2011, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };

            var sale_details_master_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "5200000000000015", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.MASTERCARD, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_master_fail = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "5200000000000023", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.MASTERCARD, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };


            LoginTest();

            // Store Payment Method

            // Revoke Payment Method

            // 3D Secure

            // Verify Card

            // AUTH
            var auth_tr = Auth(sale_details_visa_success).testApproved("Auth");
            Auth(sale_details_visa_fail).testNotApproved("Auth");
            Auth(sale_details_visa_nofunds).testNotApproved("Auth");
            Auth(sale_details_visa_fail_expiry).testNotApproved("Auth");
            Auth(sale_details_master_success).testApproved("Auth");
            Auth(sale_details_master_fail).testNotApproved("Auth");

            //CAPTURE
            var capture_tr = Capture(new AuthCapture_Details
            {
                Amount = 1,
                CurrencyCode = "ZAR",
                TransactionIndex = auth_tr.TransactionIndex
            }).testApproved("Capture");

            //SALE
            sale_details_visa_success.ExtRef = "test" + ran.Next();
            var sale_tr = Sale(sale_details_visa_success).testApproved("Sale");

            //REFUND
            Transaction_Result refund_tr = Refund(new Refund_Details
            {
                Amount = 1,
                CurrencyCode = "ZAR",
                TransactionIndex = sale_tr.TransactionIndex
            }).testApproved("Refund");

            // Credit
            // NOT IMPLEMENTED

            //VOID
            // NOT IMPLEMENTED
        }
        #endregion

        #region Helpers
        internal string getAuthXML(string pgid, string pwd, string custRef, string cardHolder, string cardNumber, string expDate, string budgetPeriod, string amt, string currency, string cvv)
        {
            return string.Format
                (
                "<protocol ver='{0}' pgid='{1}' pwd='{2}'><authtx cref='{3}' cname='{4}' cc='{5}' exp='{6}' budp='{7}' amt='{8}' cur='{9}' cvv='{10}' bno='' /></protocol> ",
                version, pgid, pwd, custRef, cardHolder, cardNumber, expDate, budgetPeriod, amt, currency, cvv
                );
        }

        internal string getCaptureXML(string pgid, string pwd, string transactionID)
        {
            return string.Format
                (
                "<protocol ver='{0}' pgid='{1}' pwd='{2}'><settletx tid='{3}' bno='' /></protocol> ",
                version, pgid, pwd, transactionID
                );
        }

        internal string getRefundXML(string pgid, string pwd, string transactionID, string Amount)
        {
            return string.Format
                (
                "<protocol ver='{0}' pgid='{1}' pwd='{2}'><refundtx tid='{3}' amt='{4}' bno='' /></protocol> ",
                version, pgid, pwd, transactionID, Amount
                );
        }
        #endregion
    }
}
