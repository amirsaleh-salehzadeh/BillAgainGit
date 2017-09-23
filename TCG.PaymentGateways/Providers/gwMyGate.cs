using System;
using System.Linq;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;

namespace TCG.PaymentGateways.Providers
{
    public class gwMyGate : IPaymentStrategy
    {
        private string MerchantUID;
        private string PaymentApplicationUID;
        private string VaultApplicationUID;
        private string GatewayBank;
        private string Mode;// 0 => test, 1 => live

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwMyGate; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "MyGate MyEnterprise",
                        WebUrl: "http://mygate.co.za/products/my-enterprise",
                        Description: "Specialising in online payments. South African based gateway.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "MerchantUID", "ApplicationUID", "GatewayBank" },
                        Currencies: new[] { "ZAR", "EUR", "MUR" },
                        Countries: new[] {  "ZA", "MU", "AT", "BE", "CY", "EE", "FI", "FR", "DE", "GR", 
                                            "IE", "IT", "LV", "LU", "MT", "NL", "PT", "SK", "SI", "ES" },
                        CardTypes: new[] 
                        {
                            CardTypeEnum.VISA, 
                            CardTypeEnum.MASTERCARD, 
                            CardTypeEnum.AMERICAN_EXPRESS, 
                            CardTypeEnum.DINERS_CLUB,
                            CardTypeEnum.MAESTRO
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
                        RefundPartial: false,
                        Credit: false,
                        Void: true
                    );
            }
        }
        #endregion

        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            MerchantUID = MerchantConfigValues.Where(r => r.Key.Equals("MerchantUID")).FirstOrDefault().Value;
            PaymentApplicationUID = MerchantConfigValues.Where(r => r.Key.Equals("PaymentApplicationUID")).FirstOrDefault().Value;
            //VaultApplicationUID = MerchantConfigValues.Where(r => r.Key.Equals("VaultApplicationUID")).FirstOrDefault().Value;
            GatewayBank = MerchantConfigValues.Where(r => r.Key.Equals("GatewayBank")).FirstOrDefault().Value;

            if (!isTestMode)
            {
                Mode = "1";
            }
            else
            {
                Mode = "0";
            }

        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="MerchantUID",
                    Value="9a6646ab-5e5e-40b8-aa06-015ec6031bd0"
                },
                new MerchantConfigValue
                {
                    Key="PaymentApplicationUID",
                    Value="bbd74b25-51a3-4bdc-8c90-d64f2c23b9ef"
                },
                //new MerchantConfigValue
                //{
                //    Key="VaultApplicationUID",
                //    Value=""
                //},
                new MerchantConfigValue
                {
                    Key="GatewayBank",
                    Value="01"
                }
            };

            Login(true, config);
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            var client = newVaultClient();

            string cardTypeNumber = details.CardType == CardTypeEnum.VISA ? "7" : "8";

            var result = client.fCreateTokenCC(MerchantUID, VaultApplicationUID, details.ClientIdentifier, details.CardHolderFullName, details.CardNumber, details.CardExpiryMonth.ToString(), details.CardExpiryYear.ToString());

            var resultArray = result.ToList();

            bool isSuccess = resultArray[0].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() == "0";

            return new StorePaymentMethod_Result
            {
                isSuccess = isSuccess,
                CardToken = isSuccess ? resultArray[1].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "",
                ErrorCode = !isSuccess ? resultArray[2].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "",
                ErrorMessage = !isSuccess ? resultArray[2].ToString() : ""
            };
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {
            var client = newVaultClient();

            var result = client.fDeregisterTokenCC(MerchantUID, VaultApplicationUID, details.ClientIdentifier);

            var resultArray = result.ToList();

            bool isSuccess = resultArray[0].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() == "0";

            return new RevokePaymentMethod_Result
            {
                isSuccess = isSuccess,
                TransactionIdentifier = isSuccess ? resultArray[1].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "",
                ErrorCode = !isSuccess ? resultArray[2].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : "",
                ErrorMessage = !isSuccess ? resultArray[2].ToString() : ""
            };
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
            // Supported but linked to Customer on BrainTree
            throw new NotSupportedException();
        }

        public Transaction_Result Auth(Sale_Details details)
        {
            try
            {
                var gateway = newClient();
                string cardtypestring = ((string)Enum.GetName(typeof(CardTypeEnum), details.CardType)).ToLower();
                string cardtypeval = ""; //convert from string to mygate number
                switch (cardtypestring)
                {
                    case ("american_express"):
                        {
                            cardtypeval = "1";
                        }
                        break;
                    case ("discover"):
                        {
                            cardtypeval = "2";
                        }
                        break;
                    case ("mastercard"):
                        {
                            cardtypeval = "3";
                        }
                        break;
                    case ("visa"):
                        {
                            cardtypeval = "4";
                        }
                        break;
                    case ("diners_club"):
                        {
                            cardtypeval = "5";
                        }
                        break;
                    default:
                        {
                            cardtypeval = "-1";
                        }
                        break;
                }

                object[] arrResults = gateway.fProcess(
                "01",                                                       //GatewayID
                MerchantUID,                                                //MerchantUID
                PaymentApplicationUID,                                      //ApplicationUID
                "1",    //1 Authorization Request                           //Action
                "",                                                         //TransactionIndex
                "Default",                                                  //Terminal
                Mode,                                                       //Mode
                details.ExtRef,                                      //MerchantReference
                details.Amount.ToString("F2"),                              //Amount
                details.CurrencyCode,                                       //Currency
                "",                                                         //CashBackAmount                                        
                cardtypeval,                                                //CardType
                "",                                                         //AccountType
                details.CardNumber,                                         //CardNumber
                details.CustomerFirstName + " " + details.CustomerLastName, //CardHolder
                details.CardCCV,                                            //CCVNumber
                details.CardExpiryMonth.ToString(),                         //ExpiryMonth
                details.CardExpiryYear.ToString(),                          //ExpiryYear
                "0",                                                        //Budget - 0 Straight, 1 budget
                "",                                                         //BudgetPeriod
                "",                                                         //AuthorizationNumber
                "",                                                         //PIN
                "",                                                         //DebugMode
                "",                                                         //eCommerceIndicator                            
                "",                                                         //verifiedByVisaXID
                "",                                                         //verifiedByVisaCAFF
                "",                                                         //secureCodeUCAF
                "",                                                         //UCI
                details.IPAddress,                                          //IP Address
                details.CustomerCountryCodeTwoLetter,                       //Shipping Country Code,
                ""                                                          //Purchase Items ID
                );

                var results = formatResult(arrResults);

                string cardnumber = details.CardNumber;
                Regex rgx = new Regex(@"[^\d]");
                cardnumber = rgx.Replace(cardnumber, String.Empty); //removes dashes spaces, etc
                string last4digits = cardnumber.Substring(cardnumber.Length - 4);

                var stringwriter = new System.IO.StringWriter();
                var xmlwriter = XmlWriter.Create(stringwriter, new XmlWriterSettings { NewLineHandling=NewLineHandling.None });
                var serializer = new XmlSerializer(details.GetType());
                serializer.Serialize(xmlwriter, details);
                var fullrequest = stringwriter.ToString();

                if (cardnumber.Length > 0) //added if statement because crashes if string length 0
                {
                    fullrequest = fullrequest.Replace(cardnumber, last4digits);
                }
                if (details.CardCCV.Length > 0) //added if statement because crashes if string length 0
                {
                    fullrequest = fullrequest.Replace(details.CardCCV, "***");
                }

                results.FullRequest = fullrequest;

                return results;
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
                var gateway = newClient();

                object[] arrResults = gateway.fProcess(
                "01",                                                                   //GatewayID
                MerchantUID,                                                            //MerchantUID
                PaymentApplicationUID,                                                  //ApplicationUID
                "3",    //3 Capture Request                                             //Action
                details.TransactionIndex,                                               //TransactionIndex
                "",                                                                     //Terminal
                Mode,                                                                   //Mode
                "",                                                                     //MerchantReference
                details.Amount.HasValue ? details.Amount.Value.ToString("F2") : "",     //Amount
                "",                                                                     //Currency
                "",                                                                     //CashBackAmount                                        
                "",                                                                     //CardType
                "",                                                                     //AccountType
                "",                                                                     //CardNumber
                "",                                                                     //CardHolder
                "",                                                                     //CCVNumber
                "",                                                                     //ExpiryMonth
                "",                                                                     //ExpiryYear
                "",                                                                     //Budget - 0 Straight, 1 budget
                "",                                                                     //BudgetPeriod
                "",                                                                     //AuthorizationNumber
                "",                                                                     //PIN
                "",                                                                     //DebugMode
                "",                                                                     //eCommerceIndicator                            
                "",                                                                     //verifiedByVisaXID
                "",                                                                     //verifiedByVisaCAFF
                "",                                                                     //secureCodeUCAF
                "",                                                                     //UCI
                "",                                                                     //IP Address
                "",                                                                     //Shipping Country Code,
                ""                                                                      //Purchase Items ID
                );


                var results = formatResult(arrResults);

                string cardnumber = details.CardNumber;
                string last4digits = "";
                if (!String.IsNullOrEmpty(cardnumber))
                {
                    Regex rgx = new Regex(@"[^\d]");
                    cardnumber = rgx.Replace(cardnumber, String.Empty); //removes dashes spaces, etc
                    last4digits = cardnumber.Substring(details.CardNumber.Length - 4);
                }


                var stringwriter = new System.IO.StringWriter();
                var xmlwriter = XmlWriter.Create(stringwriter, new XmlWriterSettings { NewLineHandling=NewLineHandling.None });
                var serializer = new XmlSerializer(details.GetType());
                serializer.Serialize(xmlwriter, details);
                var fullrequest = stringwriter.ToString();

                if (!String.IsNullOrEmpty(cardnumber))
                {
                    fullrequest = fullrequest.Replace(cardnumber, last4digits);
                }

                if (!String.IsNullOrEmpty(details.CVV))
                {
                    fullrequest = fullrequest.Replace(details.CVV, "***");
                }


                results.FullRequest = fullrequest;

                return results;
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

                Transaction_Result auth_result = Auth(details);

                if (auth_result.isApproved)
                {
                    Transaction_Result capture_result = Capture(new AuthCapture_Details { Amount = details.Amount, TransactionIndex = auth_result.TransactionIndex });
                    if (capture_result.isApproved)
                    {
                        
                        var authDoc = new XmlDocument();
                        authDoc.LoadXml(auth_result.FullRequest);

                        var captureDoc = new XmlDocument();
                        captureDoc.LoadXml(capture_result.FullRequest);

                        var mergedDoc = new XmlDocument();
                        var root = mergedDoc.CreateElement("Requests");
                        mergedDoc.AppendChild(root);

                        var authNode = mergedDoc.CreateElement("AuthRequest");
                        root.AppendChild(authNode);

                        var captureNode = mergedDoc.CreateElement("CaptureRequest");
                        root.AppendChild(captureNode);

                        foreach (XmlNode node in authDoc.ChildNodes[1])
                        {
                            var imported_node = mergedDoc.ImportNode(node, true);
                            authNode.AppendChild(imported_node);
                        }

                        foreach (XmlNode node in captureDoc.ChildNodes[1])
                        {
                            var imported_node = mergedDoc.ImportNode(node, true);
                            captureNode.AppendChild(imported_node);
                        }

                        //authNode.InnerXml = authDoc.InnerXml;
                        //captureNode.InnerXml = captureDoc.InnerXml;

                        var stringwriter = new System.IO.StringWriter();
                        mergedDoc.Save(stringwriter);

                        capture_result.FullRequest = stringwriter.ToString();

                        return capture_result;
                    }
                    else
                    {
                        capture_result.ResultText = "Capture Error || " + auth_result.ResultText;
                        capture_result.FullRequest = auth_result.FullRequest + capture_result.FullRequest;
                        return capture_result;
                    }
                }

                auth_result.ResultText = "Authorisation Error || " + auth_result.ResultText;

                return auth_result;
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
                var gateway = newClient();

                object[] arrResults = gateway.fProcess(
                "01",                                                                   //GatewayID
                MerchantUID,                                                            //MerchantUID
                PaymentApplicationUID,                                                         //ApplicationUID
                "4",    //4 Credit Request                                              //Action
                details.TransactionIndex,                                               //TransactionIndex
                "",                                                                     //Terminal
                "",                                                                     //Mode
                "",                                                                     //MerchantReference
                details.Amount.HasValue ? details.Amount.Value.ToString("F2") : "",     //Amount
                "",                                                                     //Currency
                "",                                                                     //CashBackAmount                                        
                "",                                                                     //CardType
                "",                                                                     //AccountType
                "",                                                                     //CardNumber
                "",                                                                     //CardHolder
                "",                                                                     //CCVNumber
                "",                                                                     //ExpiryMonth
                "",                                                                     //ExpiryYear
                "",                                                                     //Budget - 0 Straight, 1 budget
                "",                                                                     //BudgetPeriod
                "",                                                                     //AuthorizationNumber
                "",                                                                     //PIN
                "",                                                                     //DebugMode
                "",                                                                     //eCommerceIndicator                            
                "",                                                                     //verifiedByVisaXID
                "",                                                                     //verifiedByVisaCAFF
                "",                                                                     //secureCodeUCAF
                "",                                                                     //UCI
                "",                                                                     //IP Address
                "",                                                                     //Shipping Country Code,
                ""                                                                      //Purchase Items ID
                );


                var results = formatResult(arrResults);

                string cardnumber = details.CardNumber;
                string last4digits = "";
                if (!String.IsNullOrEmpty(cardnumber))
                {
                    Regex rgx = new Regex(@"[^\d]");
                    cardnumber = rgx.Replace(cardnumber, String.Empty); //removes dashes spaces, etc
                    last4digits = cardnumber.Substring(details.CardNumber.Length - 4);
                }


                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(details.GetType());
                serializer.Serialize(stringwriter, details);
                var fullrequest = stringwriter.ToString();

                if (!String.IsNullOrEmpty(cardnumber))
                {
                    fullrequest = fullrequest.Replace(cardnumber, last4digits);
                }

                if (!String.IsNullOrEmpty(details.CVV))
                {
                    fullrequest = fullrequest.Replace(details.CVV, "***");
                }


                results.FullRequest = fullrequest;

                return results;
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

        public Transaction_Result Void(Void_Details details)
        {
            try
            {
                var gateway = newClient();

                object[] arrResults = gateway.fProcess(
                "01",                                                                   //GatewayID
                MerchantUID,                                                            //MerchantUID
                PaymentApplicationUID,                                                         //ApplicationUID
                "2",    //2 Authorization Reversal Request                              //Action
                details.TransactionIndex,                                               //TransactionIndex
                "",                                                                     //Terminal
                "",                                                                     //Mode
                "",                                                                     //MerchantReference
                "",     //Amount
                "",                                                                     //Currency
                "",                                                                     //CashBackAmount                                        
                "",                                                                     //CardType
                "",                                                                     //AccountType
                "",                                                                     //CardNumber
                "",                                                                     //CardHolder
                "",                                                                     //CCVNumber
                "",                                                                     //ExpiryMonth
                "",                                                                     //ExpiryYear
                "",                                                                     //Budget - 0 Straight, 1 budget
                "",                                                                     //BudgetPeriod
                "",                                                                     //AuthorizationNumber
                "",                                                                     //PIN
                "",                                                                     //DebugMode
                "",                                                                     //eCommerceIndicator                            
                "",                                                                     //verifiedByVisaXID
                "",                                                                     //verifiedByVisaCAFF
                "",                                                                     //secureCodeUCAF
                "",                                                                     //UCI
                "",                                                                     //IP Address
                "",                                                                     //Shipping Country Code,
                ""                                                                      //Purchase Items ID
                );


                var results = formatResult(arrResults);

                string cardnumber = details.CardNumber;
                string last4digits = "";
                if (!String.IsNullOrEmpty(cardnumber))
                {
                    Regex rgx = new Regex(@"[^\d]");
                    cardnumber = rgx.Replace(cardnumber, String.Empty); //removes dashes spaces, etc
                    last4digits = cardnumber.Substring(details.CardNumber.Length - 4);
                }


                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(details.GetType());
                serializer.Serialize(stringwriter, details);
                var fullrequest = stringwriter.ToString();

                if (!String.IsNullOrEmpty(cardnumber))
                {
                    fullrequest = fullrequest.Replace(cardnumber, last4digits);
                }

                if (!String.IsNullOrEmpty(details.CVV))
                {
                    fullrequest = fullrequest.Replace(details.CVV, "***");
                }


                results.FullRequest = fullrequest;
                return results;
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
            try
            {
                throw new NotImplementedException();
                //Transaction_Result auth_result = AuthOnly(new Sale_Details
                //{
                //    accountID = details.accountID,
                //    Amount = details.Amount,
                //    CardCCV = details.CardCCV,
                //    CardExpiryMonth = details.CardExpiryMonth,
                //    CardExpiryYear = details.CardExpiryYear,
                //    CardNumber = details.CardNumber,
                //    CardType = details.CardType,
                //    CurrencyCode = details.CurrencyCode,
                //    CurrencyCodeNumeric = details.CurrencyCodeNumeric,
                //    CustomerFirstName = details.CustomerFirstName,
                //    CustomerLastName = details.CustomerLastName,
                //    InvoiceNumber = details.InvoiceNumber,
                //    transactionID = details.transactionID,
                //    customerID = details.customerID
                //});

                //if (auth_result.isApproved)
                //{
                //    var gateway = newClient();

                //    string cardtypestring = ((string)Enum.GetName(typeof(CardTypeEnum), details.CardType)).ToLower();
                //    string cardtypeval = ""; //convert from string to mygate number
                //    switch (cardtypestring)
                //    {
                //        case ("american express"):
                //            {
                //                cardtypeval = "1";
                //            }
                //            break;
                //        case ("discover"):
                //            {
                //                cardtypeval = "2";
                //            }
                //            break;
                //        case ("mastercard"):
                //            {
                //                cardtypeval = "3";
                //            }
                //            break;
                //        case ("visa"):
                //            {
                //                cardtypeval = "4";
                //            }
                //            break;
                //        case ("diners"):
                //            {
                //                cardtypeval = "5";
                //            }
                //            break;
                //        default:
                //            {
                //                cardtypeval = "-1";
                //            }
                //            break;
                //    }

                //    object[] arrResults = gateway.fProcess(
                // "01",                                                       //GatewayID
                // MerchantUID,                                                //MerchantUID
                // ApplicationUID,                                             //ApplicationUID
                // "1",    //1 Authorization Request                           //Action
                // "",                                                         //TransactionIndex
                // "Default",                                                  //Terminal
                // Mode,                                                       //Mode
                // details.InvoiceNumber,                                      //MerchantReference
                // details.Amount.ToString("F2"),                              //Amount
                // details.CurrencyCode,                                       //Currency
                // "",                                                         //CashBackAmount                                        
                // cardtypeval,                                                //CardType
                // "",                                                         //AccountType
                // details.CardNumber,                                         //CardNumber
                // details.CustomerFirstName + " " + details.CustomerLastName,  //CardHolder
                // details.CardCCV,                                            //CCVNumber
                // details.CardExpiryMonth.ToString(),                         //ExpiryMonth
                // details.CardExpiryYear.ToString(),                          //ExpiryYear
                // "0",                                                        //Budget - 0 Straight, 1 budget
                // "",                                                         //BudgetPeriod
                // "",                                                         //AuthorizationNumber
                // "",                                                         //PIN
                // "",                                                         //DebugMode
                // "",                                                         //eCommerceIndicator                            
                // "",                                                         //verifiedByVisaXID
                // "",                                                         //verifiedByVisaCAFF
                // "",                                                         //secureCodeUCAF
                // "",                                                         //UCI
                // "",                                          //IP Address
                // "",                                //Shipping Country Code,
                // ""                                                          //Purchase Items ID
                // );


                //    var result2 = formatResult(arrResults);

                //    if (result2.isApproved)
                //    {
                //        return result2;
                //    }
                //    else
                //    {
                //        result2.ResultText = "Credit Error || " + auth_result.ResultText;
                //        return result2;
                //    }
                //}

                //auth_result.ResultText = "Authorisation Error || " + auth_result.ResultText;
                //return auth_result;
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

        public void runTests()
        {
            var ran = new Random();

            // classes
            var sale_details_visa_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "4111 1111 1111 1111", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_visa_fail = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "4242424242424242", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_visa_fail_expiry = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "4111 1111 1111 1111", CardExpiryMonth = 11, CardExpiryYear = 2011, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };

            var sale_details_master_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "5100080000000000", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.MASTERCARD, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_master_fail = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "5404000000000001", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.MASTERCARD, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_master_fail_expiry = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "5100080000000000", CardExpiryMonth = 11, CardExpiryYear = 2011, CardCCV = "123", CardType = CardTypeEnum.MASTERCARD, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };

            var sale_details_amex_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "370000200000000", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.AMERICAN_EXPRESS, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_amex_fail = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "374200000000004", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.AMERICAN_EXPRESS, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_amex_fail_expiry = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "370000200000000", CardExpiryMonth = 11, CardExpiryYear = 2011, CardCCV = "123", CardType = CardTypeEnum.AMERICAN_EXPRESS, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };

            var sale_details_diners_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "362135898197781", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.DINERS_CLUB, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            //mygate diners decline card does not work i.e. it approves
            //var sale_details_diners_fail = new Sale_Details { accountID = -1, customerID = -1, transactionID = -1, InvoiceNumber = "test1", CardNumber = "360569309025904", CardExpiryMonth = 11, CardExpiryYear = 2016, CardCCV = "123", CardType = CardTypeEnum.DINERS_CLUB, Amount = 1, CurrencyCode = "ZAR", CurrencyCodeNumeric = "840", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_diners_fail_expiry = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "362135898197781", CardExpiryMonth = 11, CardExpiryYear = 2011, CardCCV = "123", CardType = CardTypeEnum.DINERS_CLUB, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };

            LoginTest();

            // Store Payment Method

            // Revoke Payment Method

            // 3D Secure

            // Verify Card

            // AUTH
            var auth_tr = Auth(sale_details_visa_success).testApproved("Auth");
            Auth(sale_details_visa_fail).testNotApproved("Auth");
            Auth(sale_details_visa_fail_expiry).testNotApproved("Auth");
            Auth(sale_details_master_success).testApproved("Auth");
            Auth(sale_details_master_fail).testNotApproved("Auth");
            Auth(sale_details_master_fail_expiry).testNotApproved("Auth");
            Auth(sale_details_amex_success).testApproved("Auth");
            Auth(sale_details_amex_fail).testNotApproved("Auth");
            Auth(sale_details_amex_fail_expiry).testNotApproved("Auth");
            Auth(sale_details_diners_success).testApproved("Auth");
            //Auth(sale_details_diners_fail).testNotApproved("Auth"); //-mygate diners decline card does not work i.e. it approves
            Auth(sale_details_diners_fail_expiry).testNotApproved("Auth");

            //CAPTURE
            var capture_tr = Capture(new AuthCapture_Details
            {
                Amount = 500,
                CurrencyCode = "ZAR",
                TransactionIndex = auth_tr.TransactionIndex
            }).testApproved("Capture");

            //SALE
            var sale_tr = Sale(sale_details_visa_success).testApproved("Sale");

            //REFUND
            Transaction_Result refund_tr = Refund(new Refund_Details
            {
                Amount = 500,
                CurrencyCode = "ZAR",
                TransactionIndex = sale_tr.TransactionIndex
            }).testApproved("Refund");

            //VOID
            var auth_tr_2 = Auth(sale_details_visa_success).testApproved("Auth Void");

            var void_tr = Void(new Void_Details
            {
                Amount = 500,
                CurrencyCode = "ZAR",
                TransactionIndex = auth_tr_2.TransactionIndex
            }).testApproved("Void");

            // Credit
            // NOT IMPLEMENTED
        }
        #endregion

        #region Helpers
        private TCG.PaymentGatewayLibrary.MyGateGlobalEnterprise.ePayService5x0x0 newClient()
        {
            var gateway = new TCG.PaymentGatewayLibrary.MyGateGlobalEnterprise.ePayService5x0x0();

            if (Mode == "1")
            {
                gateway.Url = "https://enterprise.mygateglobal.com/5x0x0/epayservice.cfc";
            }
            else
            {
                gateway.Url = "https://dev-enterprise.mygateglobal.com/5x0x0/epayservice.cfc";
            }

            return gateway;
        }

        private TCG.PaymentGatewayLibrary.MyGateEnterpriseVault.tokenizationService newVaultClient()
        {
            var gateway = new TCG.PaymentGatewayLibrary.MyGateEnterpriseVault.tokenizationService();

            return gateway;
        }

        private Transaction_Result formatResult(object[] arrResults)
        {
            Transaction_Result results = new Transaction_Result();
            XmlDocument xdoc = new XmlDocument();
            var root = xdoc.CreateElement("response");
            xdoc.AppendChild(root);

            foreach (string result in arrResults)
            {
                if (result == null)
                    continue;

                
                //unpack result array
                int delimiter = result.IndexOf("||");
                string resultDefn = result.Substring(0, delimiter);
                string resultValue = result.Substring(delimiter + 2);

                
                var elem = xdoc.CreateElement(resultDefn);
                elem.InnerText = resultValue;
                root.AppendChild(elem);


                if (resultDefn == "Result")
                {
                    switch (resultValue)
                    {
                        case ("0"):
                            {
                                //successful
                                results.isApproved = true;
                            }
                            break;
                        case ("1"):
                            {
                                //successful with warning
                                results.isApproved = true;
                            }
                            break;
                        case ("-1"):
                            {
                                //fail
                                results.isApproved = false;
                            }
                            break;
                    }
                }

                if (resultDefn == "Error")
                {
                    results.ErrorText += resultValue + " || ";
                }

                if (resultDefn == "TransactionIndex") results.TransactionIndex = resultValue;

            }

            results.hasServerError = false;

            var stringwriter = new System.IO.StringWriter();
            xdoc.Save(stringwriter);

            results.FullResponse = stringwriter.ToString();

            return results;
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
