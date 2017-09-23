using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;
using TCG.PaymentGateways.Utils;
using TCG.PaymentGatewayLibrary;
using TCG.Crypto;

namespace TCG.PaymentGateways.Providers
{
    public class gwVCS : IPaymentStrategy
    {
        private string userID;
        private string UserName;
        private string Password;
        private bool isTestMode;

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwVCS; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "VCS",
                        WebUrl: "https://www.vcs.co.za/",
                        Description: "Virtual Card Services (VCS) has become a recognized player in the online payment gateway solution market enabling the e-commerce web site to deal with credit card payments effectively, securely and in real-time.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "UserID" },
                        Currencies: new[] { "ZAR", "USD" },
                        Countries: new[] { "US", "ZA", },
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
                        requires_CVV: false,
                        PaymentTokenize: true,
                        PaymentTokenize_requires_CVV: true,
                        PaymentTokenize_external:false,
                        ThreeDSecure: true,
                        Verify: false,
                        Auth: false,
                        AuthCapture: false,
                        AuthCapturePartial: false,
                        Sale: true,
                        Refund: false,
                        RefundPartial: false,
                        Credit: false,
                        Void: false
                    );
            }
        }
        #endregion

        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            this.isTestMode = isTestMode;
            userID = MerchantConfigValues.Where(r => r.Key.Equals("UserID")).FirstOrDefault().Value;
            UserName = MerchantConfigValues.Where(r => r.Key.Equals("UserName")).FirstOrDefault().Value;
            Password = MerchantConfigValues.Where(r => r.Key.Equals("Password")).FirstOrDefault().Value;
        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="UserID",
                    Value="9659"
                },
                new MerchantConfigValue
                {
                    Key="UserName",
                    Value="Mohamed"
                },
                new MerchantConfigValue
                {
                    Key="Password",
                    Value="CO18evI3"
                }
            };

            Login(true, config);
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            var client = new PaymentGatewayLibrary.VCSTokenisation.Svc_VirtualRecur();

            var token = ShortStringCreator.Create(15);

            var result = client.AddCCTransaction(
                new PaymentGatewayLibrary.VCSTokenisation.AddCCRequest
                {
                    UserID = userID,
                    UserName = UserName,
                    Password = Password,
                    ReferenceNumber = token, //the tokenization token
                    CardNumber = details.CardNumber,
                    CardExpiryYY = details.CardExpiryYear.ToString(),
                    CardExpiryMM = details.CardExpiryMonth.ToString(),
                    CVC = details.CardCVV,
                    CardHolderName = details.CardHolderFullName,
                    StartDate = DateTime.Now.AddDays(1).ToString("yyyy'/'MM'/'dd"), //"2016/04/29",//otherwise complains that it should be in future if set to DateTime.Now
                    Frequency = "O",//on demand
                    OccurCount = "U",//unlimited
                    MerchantVar1 = details.ClientIdentifier,
                });

            var return_result = new StorePaymentMethod_Result { CardToken = token, ErrorCode = result.ResultCode, ErrorMessage = result.ResultMessage, isSuccess = int.Parse(result.ResultCode) == 0 };

            //if (paymentOptions.ThreeDSecure && details.doThreeDSecure)
            //{
            //    return_result.ThreeDSecurePost = Lookup3dSecure(new Lookup3dSecure_Details { ProviderToken = return_result.CardToken, ExternalIdentifier1 = details.ClientIdentifier, ExternalIdentifier2 = details.CardIdentifier }).ThreeDSecurePost;
            //}

            return return_result;
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {
            var client = new PaymentGatewayLibrary.VCSTokenisation.Svc_VirtualRecur();
            var result = client.DeleteCCTransaction(new PaymentGatewayLibrary.VCSTokenisation.DeleteCCRequest { UserID = userID, UserName = UserName, Password = Password, ReferenceNumber = details.CardToken });

            return new RevokePaymentMethod_Result { ErrorCode = result.ResultCode, ErrorMessage = result.ResultMessage, isSuccess = int.Parse(result.ResultCode) == 0 };
        }

        public Lookup3dSecure_Result Lookup3dSecure(Lookup3dSecure_Details details)
        {
            var result = new Lookup3dSecure_Result();

            if (paymentOptions.ThreeDSecure)
            {
                var stringPost = "";

                if (details.isPayment)
                {
                    stringPost += @"<form action='https://www.vcs.co.za/vvonline/vcspay.aspx' method='post' target='_self'>";
                    stringPost += @"<input type='hidden' name='p1' value='" + userID + "' >";
                    stringPost += @"<input type='hidden' name='p2' value='" + details.SaleData.ExtRef + "'>";
                    stringPost += @"<input type='hidden' name='p3' value='"+ details.SaleData.ProductDescription + "' >";
                    stringPost += @"<input type='hidden' name='p4' value='"+ details.SaleData.Amount.ToString("F2") +"' >";
                    stringPost += @"<input type='hidden' name='p5' value='" + details.SaleData.CurrencyCode + "' >";
                    if (!details.isOnceoff)
                    {
                        
                        //stringPost += @"<input type='hidden' name='p6' value='U' >";
                        //stringPost += @"<input type='hidden' name='p7' value='" + "O" + "' >";
                        stringPost += @"<input type='hidden' name='p12' value='" + "N" + "' >";
                        stringPost += @"<input type='hidden' name='p13' value='" + "0" + "' >";
                        stringPost += @"<input type='hidden' name='RecurReference' value='" + details.ProviderToken + "'>";//recur ref
                    }
                    stringPost += @"<input type='hidden' name='m_1' value='" + details.TransactionToken + "'>";//transactionID                                       
                    stringPost += @"<input type='hidden' name='m_2' value='" + details.ExternalIdentifier1 + "'>";//extref - AKA BA AccountID
                    stringPost += @"<input type='hidden' name='m_3' value='" + details.ExternalIdentifier2 + "'>";//payment method token
                    stringPost += @"<input type='hidden' name='m_4' value='" + details.ExternalIdentifier3 + "'>";//customerID
                    stringPost += @"<input type='hidden' name='m_5' value='" + details.ExternalIdentifier4 + "'>";//hash
                    stringPost += @"<input type='hidden' name='m_6' value='" + details.ExternalIdentifier5 + "'>";//verify
                    stringPost += @"<input type='hidden' name='PaymentMethod' value='cc'>";
                    if(details.isOnceoff)
                    {
                        stringPost += @"<input type='hidden' name='CardNumber' value='" + details.SaleData.CardNumber + "'>";
                    }                    
                    stringPost += @"<input type='hidden' name='ExpiryDate' value='" + GatewayUtils.formatExpiryDateYYmm(details.SaleData.CardExpiryMonth, details.SaleData.CardExpiryYear) + "'>";
                    stringPost += @"<input type='hidden' name='CVV' value='" + details.SaleData.CardCCV + "'>";
                    stringPost += @"<input type='hidden' name='CardholderName' value='" + details.SaleData.CardHolderName + " " + details.SaleData.CardHolderLastName + "'>";
                    stringPost += @"<input type='submit' value='Proceed'>";
                    stringPost += @"</form>";
                }
                else
                {
                    stringPost += @"<form action='https://www.vcs.co.za/vvonline/vcspay.aspx' method='post' target='_self'>";
                    stringPost += @"<input type='hidden' name='p1' value='" + userID + "' >";
                    stringPost += @"<input type='hidden' name='p2' value='" + details.ProviderToken + "'>";
                    stringPost += @"<input type='hidden' name='p3' value='Enrollment' >";
                    stringPost += @"<input type='hidden' name='p4' value='1' >";
                    stringPost += @"<input type='hidden' name='IsRegistration' value='Y'>";
                    stringPost += @"<input type='hidden' name='m_1' value='" + details.TransactionToken + "'>";//transactionID                                       
                    stringPost += @"<input type='hidden' name='m_2' value='" + details.ExternalIdentifier1 + "'>";//extref - AKA BA AccountID
                    stringPost += @"<input type='hidden' name='m_3' value='" + details.ExternalIdentifier2 + "'>";//payment method token
                    stringPost += @"<input type='hidden' name='m_4' value='" + details.ExternalIdentifier3 + "'>";//customerID
                    stringPost += @"<input type='hidden' name='m_5' value='" + details.ExternalIdentifier4 + "'>";//hash
                    stringPost += @"<input type='hidden' name='m_6' value='" + details.ExternalIdentifier5 + "'>";//verify
                    stringPost += @"<input type='hidden' name='PaymentMethod' value='cc'>";
                    stringPost += @"<input type='hidden' name='CardNumber' value='" + details.SaleData.CardNumber + "'>";
                    stringPost += @"<input type='hidden' name='ExpiryDate' value='" + GatewayUtils.formatExpiryDateYYmm(details.SaleData.CardExpiryMonth, details.SaleData.CardExpiryYear) + "'>";
                    if(!isTestMode) //put this here as VCS did not fix their testing sandbox bug but said we should omit in test mode
                    {
                        stringPost += @"<input type='hidden' name='CVV' value='" + details.SaleData.CardCCV + "'>";
                    }                    
                    stringPost += @"<input type='hidden' name='CardholderName' value='" + details.SaleData.CardHolderName + " " + details.SaleData.CardHolderLastName + "'>";
                    stringPost += @"<input type='submit' value='Proceed'>";
                    stringPost += @"</form>";
                }
                
                result.ThreeDSecurePost = stringPost;
            }

            return result;
        }

        public Authenticate3dSecure_Result Authenticate3dSecure(Authenticate3dSecure_Details details)
        {
            if (string.IsNullOrWhiteSpace(details.GatewayPayloadRaw) && !string.IsNullOrWhiteSpace(details.GatewayQueryStringRaw))
            {
                details.GatewayPayloadRaw = Uri.UnescapeDataString(details.GatewayQueryStringRaw); //get data from querystring and unescape
            }

            var xml = new XmlDocument();
            xml.LoadXml(details.GatewayPayloadRaw);

            var Token = xml.SelectSingleNode("/AuthorisationResponse/Reference") ?? xml.SelectSingleNode("/DemandResponse/Reference");
            var UTI = xml.SelectSingleNode("/AuthorisationResponse/Uti") ?? xml.SelectSingleNode("/DemandResponse/Uti");
            var error_code = xml.SelectSingleNode("/AuthorisationResponse/ResponseCode") ?? xml.SelectSingleNode("/DemandResponse/ResponseCode");
            var error_message = xml.SelectSingleNode("/AuthorisationResponse/Response") ?? xml.SelectSingleNode("/DemandResponse/Response");
            var ip = xml.SelectSingleNode("/AuthorisationResponse/CardHolderIpAddr") ?? xml.SelectSingleNode("/DemandResponse/CardHolderIpAddr");

            var threeDSecureEnrolled = xml.SelectSingleNode("/AuthorisationResponse/threeDsecureEnrolled") ?? xml.SelectSingleNode("/DemandResponse/threeDsecureEnrolled");
            var threeDSecureEci = xml.SelectSingleNode("/AuthorisationResponse/threeDsecureEci") ?? xml.SelectSingleNode("/DemandResponse/threeDsecureEci");
            var threeDSecureXid = xml.SelectSingleNode("/AuthorisationResponse/threeDsecureXid") ?? xml.SelectSingleNode("/DemandResponse/threeDsecureXid");
            var threeDSecureCavv = xml.SelectSingleNode("/AuthorisationResponse/threeDsecureCavv") ?? xml.SelectSingleNode("/DemandResponse/threeDsecureCavv");
            var RetrievalReferenceNumber = xml.SelectSingleNode("/AuthorisationResponse/RetrievalReferenceNumber") ?? xml.SelectSingleNode("/DemandResponse/RetrievalReferenceNumber");

            var m_1 = xml.SelectSingleNode("/AuthorisationResponse/m_1") ?? xml.SelectSingleNode("/DemandResponse/m_1");
            var m_2 = xml.SelectSingleNode("/AuthorisationResponse/m_2") ?? xml.SelectSingleNode("/DemandResponse/m_2");
            var m_3 = xml.SelectSingleNode("/AuthorisationResponse/m_3") ?? xml.SelectSingleNode("/DemandResponse/m_3");
            var m_4 = xml.SelectSingleNode("/AuthorisationResponse/m_4") ?? xml.SelectSingleNode("/DemandResponse/m_4");
            var m_5 = xml.SelectSingleNode("/AuthorisationResponse/m_5") ?? xml.SelectSingleNode("/DemandResponse/m_5");
            var m_6 = xml.SelectSingleNode("/AuthorisationResponse/m_6") ?? xml.SelectSingleNode("/DemandResponse/m_6");

            return new Authenticate3dSecure_Result
            {
                isSuccess = error_message.InnerText.Contains("APPROVED") && !error_message.InnerText.Contains("NOT"),
                isEnrolled = threeDSecureEnrolled.InnerText != "U",
                isNotSupported = threeDSecureEnrolled.InnerText == "N",
                ErrorCode = error_code.InnerText,
                ErrorMessage = error_message.InnerText,
                CardToken = Token.InnerText,
                TransactionIdentifier = UTI.InnerText,
                IPAddress = ip.InnerText,
                EnrollmentStatus = threeDSecureEnrolled.InnerText,
                ThreeDSecureEci = threeDSecureEci.InnerText,
                ThreeDSecureXid = threeDSecureXid.InnerText,
                ThreeDSecureCavv = threeDSecureCavv.InnerText,
                RetrievalReferenceNumber = RetrievalReferenceNumber.InnerText,
                CustomResponse = "<CallbackResponse>Accepted</CallbackResponse>",
                TransactionToken = m_1.InnerText,
                ExternalIdentifier1 = m_2.InnerText,
                ExternalIdentifier2 = m_3.InnerText,
                ExternalIdentifier3 = m_4.InnerText,
                ExternalIdentifier4 = m_5.InnerText,
                ExternalIdentifier5 = m_6.InnerText,
                FullResult= details.GatewayPayloadRaw
            };
        }

        public Transaction_Result Verify(Sale_Details details)
        {
            throw new NotSupportedException();
        }

        public Transaction_Result Auth(Sale_Details details)
        {
            try
            {
                var xmlString = getAuthXML
                    (
                        userID,
                        details.ExtRef,
                        "NA",
                        details.Amount.ToString("F2"),
                        details.CustomerFirstName + " " + details.CustomerLastName,
                        details.CardNumber,
                        details.CardExpiryMonth.ToString(),
                        details.CardExpiryYear.ToString(),
                        details.CardCCV, "",
                        details.CurrencyCode
                    );

                var xml = PostXMLTransaction("https://www.vcs.co.za/vvonline/ccxmlauth.asp", xmlString);

                XmlNode authResponse = xml.GetElementsByTagName("AuthorisationResponse").Item(0);
                var responseNode = authResponse.SelectNodes("Response").Item(0);
                var refNode = authResponse.SelectNodes("Reference").Item(0);
                var addResNode = authResponse.SelectNodes("AdditionalResponseData").Item(0);
                var resCodeNode = authResponse.SelectNodes("ResponseCode").Item(0);

                var approved = responseNode.InnerText.Contains("APPROVED") && !responseNode.InnerText.Contains("NOT");

                var result = new Transaction_Result
                {
                    isApproved = approved,
                    ApprovalCode = approved ? responseNode.InnerText : "",
                    TransactionIndex = refNode.InnerText,
                    ProcessorCode = resCodeNode.InnerText,
                    FullRequest = xmlString,
                    FullResponse = xml.OuterXml,
                    hasServerError = false,
                    ErrorCode = !approved ? responseNode.InnerText : "",
                    ErrorText = addResNode.InnerText
                };

                return result;
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
                var xmlString = getCaptureXML(userID, details.TransactionIndex);
                var xml = PostXMLTransaction("https://www.vcs.co.za/vvonline/ccxmlsettle.asp", xmlString);

                XmlNode authResponse = xml.GetElementsByTagName("SettlementResponse").Item(0);
                var responseNode = authResponse.SelectNodes("Response").Item(0);
                var refNode = authResponse.SelectNodes("Reference").Item(0);
                var addResNode = authResponse.SelectNodes("AdditionalResponseData").Item(0);

                var approved = responseNode.InnerText.Contains("APPROVED") && !responseNode.InnerText.Contains("NOT");

                var result = new Transaction_Result
                {
                    isApproved = approved,
                    ApprovalCode = approved ? responseNode.InnerText : "",
                    TransactionIndex = refNode.InnerText,
                    ProcessorCode = addResNode.InnerText,
                    FullRequest = xmlString,
                    FullResponse = xml.OuterXml,
                    hasServerError = false,
                    ErrorCode = !approved ? responseNode.InnerText : "",
                    ErrorText = addResNode.InnerText
                };

                return result;
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
                if (string.IsNullOrWhiteSpace(details.ProviderToken) && paymentOptions.PaymentTokenize_requires_CVV)
                    throw new FormatException("Gateway cannot process untokenised cards, as CVV is required");

                XElement root = new XElement("DemandRequest");

                XElement elem_userID = new XElement("UserId", userID);
                XElement elem_ref = new XElement("Reference", details.ProviderToken);
                XElement elem_desc = new XElement("Description", details.ProductDescription);
                XElement elem_amount = new XElement("Amount", details.Amount);
                XElement elem_m1 = new XElement("m_1", details.CustomerIdentifier);
                XElement elem_m2 = new XElement("m_2", details.transactionID);
                XElement elem_m3 = new XElement("m_3", details.accountID);
                XElement elem_m4 = new XElement("m_4", details.ExtRef);

                root.Add(elem_userID, elem_ref, elem_desc, elem_amount, elem_m1, elem_m2, elem_m3, elem_m4);

                var xmlString = root.ToString();

                var xml = PostXMLTransaction("https://www.vcs.co.za/vvonline/ccxmldemand.asp", "?xmlMessage=" + xmlString);

                XmlNode authResponse = xml.GetElementsByTagName("DemandResponse").Item(0);
                var responseNode = authResponse.SelectNodes("Response").Item(0);
                var refNode = authResponse.SelectNodes("Reference").Item(0);
                var addResNode = authResponse.SelectNodes("AdditionalResponseData").Item(0);
                var resCodeNode = authResponse.SelectNodes("ResponseCode").Item(0);

                var approved = responseNode.InnerText.Contains("APPROVED") && !responseNode.InnerText.Contains("NOT");

                var result = new Transaction_Result
                {
                    isApproved = approved,
                    ApprovalCode = approved ? responseNode.InnerText : "",
                    TransactionIndex = refNode.InnerText,
                    ProcessorCode = resCodeNode.InnerText,
                    FullRequest = xmlString,
                    FullResponse = xml.OuterXml,
                    hasServerError = false,
                    ErrorCode = !approved ? responseNode.InnerText : "",
                    ErrorText = addResNode.InnerText,
                    ProviderToken = details.ProviderToken
                };

                return result;
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
                var xmlString = getRefundXML(userID, details.TransactionIndex, "NA", details.Amount.Value.ToString("F2"));
                var xml = PostXMLTransaction("https://www.vcs.co.za/vvonline/ccxmlauth.asp", xmlString);

                XmlNode authResponse = xml.GetElementsByTagName("RefundResponse").Item(0);
                var responseNode = authResponse.SelectNodes("Response").Item(0);
                var refNode = authResponse.SelectNodes("Reference").Item(0);
                var addResNode = authResponse.SelectNodes("AdditionalResponseData").Item(0);

                var approved = responseNode.InnerText.Contains("APPROVED") && !responseNode.InnerText.Contains("NOT");

                var result = new Transaction_Result
                {
                    isApproved = approved,
                    ApprovalCode = approved ? responseNode.InnerText : "",
                    TransactionIndex = refNode.InnerText,
                    ProcessorCode = addResNode.InnerText,
                    FullRequest = xmlString,
                    FullResponse = xml.OuterXml,
                    hasServerError = false,
                    ErrorCode = !approved ? responseNode.InnerText : "",
                    ErrorText = addResNode.InnerText
                };

                return result;
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

        public Transaction_Result Credit(Credit_Details details)
        {
            throw new NotSupportedException();
        }

        public Transaction_Result Void(Void_Details details)
        {
            throw new NotSupportedException();
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Helpers
        private void newClient()
        {

        }

        internal static XmlDocument PostXMLTransaction(string v_strURL, String v_objXMLDoc)
        {
            try
            {
                XmlDocument xmldoc = null;
                var xml = Uri.EscapeUriString(v_objXMLDoc);
                WebRequest webrequest = WebRequest.Create(v_strURL + xml);
                webrequest.ContentType = "application/x-www-form-urlencoded";
                webrequest.Method = "POST";
                webrequest.ContentLength = 0;
                var response = (HttpWebResponse)webrequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var stream = response.GetResponseStream();

                    XmlTextReader objXMLReader = new XmlTextReader(stream);

                    //Declare XMLDocument
                    xmldoc = new XmlDocument();
                    xmldoc.Load(objXMLReader);

                    //Close XMLReader
                    objXMLReader.Close();
                }

                return xmldoc;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">VCS Terminal ID allocated by VCS</param>
        /// <param name="reference">
        /// Unique Transaction Reference Number with NO SPACES and no special characters. 
        /// If for some reason you did not get a response, then re-try the transaction with the same reference number
        /// The reference number in the transaction table can be a max length of 25 chars.
        /// The maximum length of reference numbers for RECURRING transactions is 15 chars.
        /// </param>
        /// <param name="description">Description of Goods / Product.</param>
        /// <param name="amount">Transaction Amount with a decimal point.</param>
        /// <param name="cardholder">Cardholder Name.</param>
        /// <param name="cardnumber">Card Number, with no spaces and no zero filling for short numbers. Left justified, right space filled.</param>
        /// <param name="expmonth">Expiry Month, format MM i.e. 01, 02 etc.</param>
        /// <param name="expyear">Expiry Year, format YY i.e. 10=2010</param>
        /// <param name="cvc">Card Verification Code / Value (CVC / CVV). Last 3 digits printed on the signature panel of the card.</param>
        /// <param name="budgetperiod">00 Straight authorisation (default option). i.e. 06 Budget period.</param>
        /// <param name="currency">ISO Currency, i.e. zar, usd, gbp, etc. If you do not send a currency then our system will default to the merchant’s default currency.</param>
        /// <returns>XML Response</returns>
        internal static string getAuthXML(string userID, string reference, string description, string amount, string cardholder, string cardnumber, string expmonth, string expyear, string cvc, string budgetperiod, string currency)
        {
            XElement root = new XElement("AuthorisationRequest");

            XElement elem_userID = new XElement("UserId", userID);
            XElement elem_ref = new XElement("Reference", reference);
            XElement elem_desc = new XElement("Description", description);
            XElement elem_amount = new XElement("Amount", amount);
            XElement elem_cardholder = new XElement("CardholderName", cardholder);
            XElement elem_cardnumber = new XElement("CardNumber", cardnumber);
            XElement elem_expmonth = new XElement("ExpiryMonth", expmonth);
            XElement elem_expyear = new XElement("ExpiryYear", expyear);
            XElement elem_cvc = new XElement("CardValidationCode", cvc);
            XElement elem_budgetperiod = new XElement("BudgetPeriod", budgetperiod);
            XElement elem_currency = new XElement("Currency", currency);
            XElement elem_delaysettlement = new XElement("DelaySettlement", 'Y'); //no settlement must be done, just authorisation - N for settlement

            if (String.IsNullOrEmpty(cvc)) //dont add CVC element if no CVV provided (as per documentation) 
            {
                root.Add(elem_userID, elem_ref, elem_desc, elem_amount, elem_cardholder, elem_cardnumber, elem_expmonth, elem_expyear, elem_budgetperiod, elem_currency, elem_delaysettlement);
            }
            else
            {
                root.Add(elem_userID, elem_ref, elem_desc, elem_amount, elem_cardholder, elem_cardnumber, elem_expmonth, elem_expyear, elem_cvc, elem_budgetperiod, elem_currency, elem_delaysettlement);
            }

            return "?xmlMessage=" + root.ToString();
        }

        internal static string getCaptureXML(string userID, string reference)
        {
            DateTime settlementDate = DateTime.Now;
            string date = settlementDate.ToString("yyyy") + "/" + settlementDate.ToString("MM") + "/" + settlementDate.ToString("dd");

            XElement root = new XElement("SettlementRequest");

            XElement elem_userID = new XElement("UserId", userID);
            XElement elem_ref = new XElement("Reference", reference);
            XElement elem_date = new XElement("SettlementDate", date);

            root.Add(elem_userID, elem_ref, elem_date);

            return "?xmlMessage=" + root.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">VCS Terminal ID allocated by VCS</param>
        /// <param name="reference">
        /// Unique Transaction Reference Number with NO SPACES and no special characters. 
        /// If for some reason you did not get a response, then re-try the transaction with the same reference number
        /// The reference number in the transaction table can be a max length of 25 chars.
        /// The maximum length of reference numbers for RECURRING transactions is 15 chars.
        /// </param>
        /// <param name="description">Description of Goods / Product.</param>
        /// <param name="amount">Transaction Amount with a decimal point.</param>
        /// <param name="cardholder">Cardholder Name.</param>
        /// <param name="cardnumber">Card Number, with no spaces and no zero filling for short numbers. Left justified, right space filled.</param>
        /// <param name="expmonth">Expiry Month, format MM i.e. 01, 02 etc.</param>
        /// <param name="expyear">Expiry Year, format YY i.e. 10=2010</param>
        /// <param name="cvc">Card Verification Code / Value (CVC / CVV). Last 3 digits printed on the signature panel of the card.</param>
        /// <param name="budgetperiod">00 Straight authorisation (default option). i.e. 06 Budget period.</param>
        /// <param name="currency">ISO Currency, i.e. zar, usd, gbp, etc. If you do not send a currency then our system will default to the merchant’s default currency.</param>
        /// <returns>XML Response</returns>
        internal static string getSaleXML(string userID, string reference, string description, string amount, string cardholder, string cardnumber, string expmonth, string expyear, string cvc, string budgetperiod, string currency)
        {
            XElement root = new XElement("AuthorisationRequest");

            XElement elem_userID = new XElement("UserId", userID);
            XElement elem_ref = new XElement("Reference", reference);
            XElement elem_desc = new XElement("Description", description);
            XElement elem_amount = new XElement("Amount", amount);
            XElement elem_cardholder = new XElement("CardholderName", cardholder);
            XElement elem_cardnumber = new XElement("CardNumber", cardnumber);
            XElement elem_expmonth = new XElement("ExpiryMonth", expmonth);
            XElement elem_expyear = new XElement("ExpiryYear", expyear);
            XElement elem_cvc = new XElement("CardValidationCode", cvc);
            XElement elem_budgetperiod = new XElement("BudgetPeriod", budgetperiod);
            XElement elem_currency = new XElement("Currency", currency);
            XElement elem_delaysettlement = new XElement("DelaySettlement", 'N'); //no settlement must be done, just authorisation - N for settlement

            root.Add(elem_userID, elem_ref, elem_desc, elem_amount, elem_cardholder, elem_cardnumber, elem_expmonth, elem_expyear, elem_cvc, elem_budgetperiod, elem_currency, elem_delaysettlement);

            //return "<?xml version='1.0' ?> " + root.ToString();
            return "?xmlMessage=" + root.ToString();

        }

        internal static string getRefundXML(string userID, string reference, string description, string amount)
        {
            XElement root = new XElement("RefundRequest");

            XElement elem_userID = new XElement("UserId", userID);
            XElement elem_ref = new XElement("Reference", reference);
            XElement elem_desc = new XElement("Description", description); //refund reason 
            XElement elem_amount = new XElement("Amount", amount);

            root.Add(elem_userID, elem_ref, elem_desc, elem_amount);

            //return "<?xml version='1.0' ?> " + root.ToString();
            return "?xmlMessage=" + root.ToString();
        }
        #endregion
    }
}
