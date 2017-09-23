using System;
using System.Linq;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;
using System.Xml.Serialization;
using TCG.PaymentGatewayLibrary.PayUEnterpriseService;

namespace TCG.PaymentGateways.Providers
{
    public class gwPayU : IPaymentStrategy
    {
        private string userName;
        private string password;
        private string SafeKey;
        private string Url;

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwPayU; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "PayU Enterprise",
                        WebUrl: "https://www.payu.co.za/",
                        Description: "Sell online with PayU, SA's leading payment gateway. Securely accept credit card payments on your website. Hassle-free, easy and secure.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "SafeKey", "UserName", "Password" },
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
            SafeKey = MerchantConfigValues.Where(r => r.Key.Equals("SafeKey")).FirstOrDefault().Value;
            userName = MerchantConfigValues.Where(r => r.Key.Equals("Username")).FirstOrDefault().Value;
            password = MerchantConfigValues.Where(r => r.Key.Equals("Password")).FirstOrDefault().Value;

            if (isTestMode)
            {
                Url = "https://staging.payu.co.za/service/PayUAPI";
            }
            else
            {
                Url = "https://www.payu.co.za/service/PayUAPI";
            }

        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="SafeKey",
                    Value="{E7A333D4-CC48-4463-BEC6-A4BC1F16DC30}"
                },
                new MerchantConfigValue
                {
                    Key="Username",
                    Value="Staging Enterprise Integration Store 1"
                },
                new MerchantConfigValue
                {
                    Key="Password",
                    Value="j3w8swi5"
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
            // Supported but linked to Customer on BrainTree
            throw new NotSupportedException();
        }

        public Transaction_Result Auth(Sale_Details details)
        {
            try
            {
                var gateway = newClient();

                DoTransactionResponseMessage gateWayResult = gateway.doTransaction(
                    "ONE_ZERO",
                    SafeKey,
                    transactionType.RESERVE,
                    authenticationType.NA,
                    new additionalInfo { merchantReference = details.ExtRef },
                    new customer
                    {
                        address1 = details.CustomerAddress,
                        addressCity = details.CustomerCity,
                        countryCode = details.CustomerCountryNumeric,
                        countryOfResidence = details.CustomerCountry,
                        firstName = details.CustomerFirstName,
                        lastName = details.CustomerLastName
                    },
                    new basket { amountInCents = (details.Amount * 100).ToString(), currencyCode = details.CurrencyCode, description = details.ExtRef },
                    null,
                    new creditCard[] 
                    { 
                        new creditCard 
                        { 
                            amountInCents = (details.Amount*100).ToString(), 
                            cardExpiry = GatewayUtils.formatExpiryDate(details.CardExpiryMonth, details.CardExpiryYear), 
                            cardNumber = details.CardNumber, 
                            cvv = details.CardCCV, 
                            nameOnCard = details.CardHolderName + " " + details.CardHolderLastName
                        } 
                    },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                    );

                var results = gateWayResult;


                if (results.successful)
                {
                    return new Transaction_Result
                    {
                        isApproved = true,
                        ApprovalCode = results.resultCode,
                        ResultCode = results.resultCode,
                        ResultText = results.resultMessage,
                        TransactionIndex = results.payUReference,
                        hasServerError = false,
                        FullRequest = getXmlString(details),
                        FullResponse = getXmlString(results)
                    };
                }
                else
                {
                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = results.resultCode,
                        ErrorText = results.resultMessage,
                        FullResponse = getXmlString(results),
                        FullRequest = getXmlString(details)
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
                var gateway = newClient();

                DoTransactionResponseMessage gateWayResult = gateway.doTransaction(
                    "ONE_ZERO",
                    SafeKey,
                    transactionType.FINALIZE,
                    authenticationType.NA,
                    new additionalInfo { merchantReference = "FINALIZE-" + details.TransactionIndex, payUReference = details.TransactionIndex },
                    null,
                    new basket { amountInCents = (details.Amount * 100).ToString(), currencyCode = details.CurrencyCode },
                    null,
                    new creditCard[] 
                    { 
                        new creditCard 
                        { 
                            amountInCents = (details.Amount*100).ToString(),
                        } 
                    },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                    );

                var results = gateWayResult;


                if (results.successful)
                {
                    return new Transaction_Result
                    {
                        isApproved = true,
                        ApprovalCode = results.resultCode,
                        ResultCode = results.resultCode,
                        ResultText = results.resultMessage,
                        TransactionIndex = results.payUReference,
                        hasServerError = false,
                        FullRequest = getXmlString(details),
                        FullResponse = getXmlString(results)
                    };
                }
                else
                {
                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = results.resultCode,
                        ErrorText = results.resultMessage,
                        FullResponse = getXmlString(results),
                        FullRequest = getXmlString(details)
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
                var gateway = newClient();

                DoTransactionResponseMessage gateWayResult = gateway.doTransaction(
                    "ONE_ZERO",
                    SafeKey,
                    transactionType.PAYMENT,
                    authenticationType.NA,
                    new additionalInfo { merchantReference = details.ExtRef },
                    new customer
                    {
                        address1 = details.CustomerAddress,
                        addressCity = details.CustomerCity,
                        
                        countryOfResidence = details.CustomerCountry,
                        firstName = details.CustomerFirstName,
                        lastName = details.CustomerLastName
                    },
                    new basket { amountInCents = (details.Amount * 100).ToString("F0"), currencyCode = details.CurrencyCode },
                    null,
                    new creditCard[] 
                    { 
                        new creditCard 
                        { 
                            amountInCents = (details.Amount*100).ToString("F0"), 
                            cardExpiry = GatewayUtils.formatExpiryDate(details.CardExpiryMonth, details.CardExpiryYear), 
                            cardNumber = details.CardNumber, 
                            cvv = details.CardCCV, 
                            nameOnCard = details.CustomerFirstName + " " + details.CustomerLastName
                        } 
                    },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                    );

                var results = gateWayResult;


                if (results.successful)
                {
                    return new Transaction_Result
                    {
                        isApproved = true,
                        ApprovalCode = results.resultCode,
                        ResultCode = results.resultCode,
                        ResultText = results.resultMessage,
                        TransactionIndex = results.payUReference,
                        hasServerError = false,
                        FullRequest = getXmlString(details),
                        FullResponse = getXmlString(results)
                    };
                }
                else
                {
                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = results.resultCode,
                        ErrorText = results.resultMessage,
                        FullResponse = getXmlString(results),
                        FullRequest = getXmlString(details)
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

        public Transaction_Result Refund(Refund_Details details)
        {
            try
            {
                var gateway = newClient();

                DoTransactionResponseMessage gateWayResult = gateway.doTransaction(
                    "ONE_ZERO",
                    SafeKey,
                    transactionType.CREDIT,
                    authenticationType.NA,
                    new additionalInfo { merchantReference = "REFUND-" + details.TransactionIndex, payUReference = details.TransactionIndex },
                    null,
                    new basket { amountInCents = (details.Amount * 100).ToString(), currencyCode = details.CurrencyCode },
                    null,
                    new creditCard[] 
                    { 
                        new creditCard 
                        { 
                            amountInCents = (details.Amount*100).ToString(),
                        } 
                    },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                    );

                var results = gateWayResult;


                if (results.successful)
                {
                    return new Transaction_Result
                    {
                        isApproved = true,
                        ApprovalCode = results.resultCode,
                        ResultCode = results.resultCode,
                        ResultText = results.resultMessage,
                        TransactionIndex = results.payUReference,
                        hasServerError = false,
                        FullRequest = getXmlString(details),
                        FullResponse = getXmlString(results)
                    };
                }
                else
                {
                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = results.resultCode,
                        ErrorText = results.resultMessage,
                        FullResponse = getXmlString(results),
                        FullRequest = getXmlString(details)
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
            throw new NotImplementedException();
            //try
            //{

            //    Transaction_Result auth_result = AuthOnly(new Sale_Details
            //    {
            //        accountID = details.accountID,
            //        Amount = details.Amount,
            //        CardCCV = details.CardCCV,
            //        CardExpiryMonth = details.CardExpiryMonth,
            //        CardExpiryYear = details.CardExpiryYear,
            //        CardNumber = details.CardNumber,
            //        CardType = details.CardType,
            //        CurrencyCode = details.CurrencyCode,
            //        CurrencyCodeNumeric = details.CurrencyCodeNumeric,
            //        CustomerFirstName = details.CustomerFirstName,
            //        CustomerLastName = details.CustomerLastName,
            //        InvoiceNumber = details.InvoiceNumber,
            //        transactionID = details.transactionID,
            //        customerID = details.customerID
            //    });

            //    if (auth_result.isApproved)
            //    {
            //        var gateway = newClient();

            //        string cardtypestring = ((string)Enum.GetName(typeof(CardTypeEnum), details.CardType)).ToLower();
            //        string cardtypeval = ""; //convert from string to mygate number
            //        switch (cardtypestring)
            //        {
            //            case ("american express"):
            //                {
            //                    cardtypeval = "1";
            //                }
            //                break;
            //            case ("discover"):
            //                {
            //                    cardtypeval = "2";
            //                }
            //                break;
            //            case ("mastercard"):
            //                {
            //                    cardtypeval = "3";
            //                }
            //                break;
            //            case ("visa"):
            //                {
            //                    cardtypeval = "4";
            //                }
            //                break;
            //            case ("diners"):
            //                {
            //                    cardtypeval = "5";
            //                }
            //                break;
            //            default:
            //                {
            //                    cardtypeval = "-1";
            //                }
            //                break;
            //        }

            //        object[] arrResults = gateway.fProcess(
            //     "01",                                                       //GatewayID
            //     MerchantUID,                                                //MerchantUID
            //     ApplicationUID,                                             //ApplicationUID
            //     "1",    //1 Authorization Request                           //Action
            //     "",                                                         //TransactionIndex
            //     "Default",                                                  //Terminal
            //     Mode,                                                       //Mode
            //     details.InvoiceNumber,                                      //MerchantReference
            //     details.Amount.ToString("F2"),                              //Amount
            //     details.CurrencyCode,                                       //Currency
            //     "",                                                         //CashBackAmount                                        
            //     cardtypeval,                                                //CardType
            //     "",                                                         //AccountType
            //     details.CardNumber,                                         //CardNumber
            //     details.CustomerFirstName + " " + details.CustomerLastName,  //CardHolder
            //     details.CardCCV,                                            //CCVNumber
            //     details.CardExpiryMonth.ToString(),                         //ExpiryMonth
            //     details.CardExpiryYear.ToString(),                          //ExpiryYear
            //     "0",                                                        //Budget - 0 Straight, 1 budget
            //     "",                                                         //BudgetPeriod
            //     "",                                                         //AuthorizationNumber
            //     "",                                                         //PIN
            //     "",                                                         //DebugMode
            //     "",                                                         //eCommerceIndicator                            
            //     "",                                                         //verifiedByVisaXID
            //     "",                                                         //verifiedByVisaCAFF
            //     "",                                                         //secureCodeUCAF
            //     "",                                                         //UCI
            //     "",                                          //IP Address
            //     "",                                //Shipping Country Code,
            //     ""                                                          //Purchase Items ID
            //     );


            //        var result2 = formatResult(arrResults);

            //        if (result2.isApproved)
            //        {
            //            return result2;
            //        }
            //        else
            //        {
            //            result2.ResultText = "Credit Error || " + auth_result.ResultText;
            //            return result2;
            //        }
            //    }

            //    auth_result.ResultText = "Authorisation Error || " + auth_result.ResultText;
            //    return auth_result;
            //}
            //catch (Exception ex)
            //{
            //    return new Transaction_Result
            //    {
            //        isApproved = false,
            //        hasServerError = true,
            //        ErrorText = ex.Message
            //    };
            //}
        }

        public Transaction_Result Void(Void_Details details)
        {
            try
            {
                var gateway = newClient();

                DoTransactionResponseMessage gateWayResult = gateway.doTransaction(
                    "ONE_ZERO",
                    SafeKey,
                    transactionType.RESERVE_CANCEL,
                    authenticationType.NA,
                    new additionalInfo { merchantReference = "CANCEL-" + details.TransactionIndex, payUReference = details.TransactionIndex },
                    null,
                    new basket { amountInCents = (details.Amount * 100).ToString(), currencyCode = details.CurrencyCode },
                    null,
                    new creditCard[] 
                    { 
                        new creditCard 
                        { 
                            amountInCents = (details.Amount*100).ToString(),
                        } 
                    },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                    );

                var results = gateWayResult;


                if (results.successful)
                {
                    return new Transaction_Result
                    {
                        isApproved = true,
                        ApprovalCode = results.resultCode,
                        ResultCode = results.resultCode,
                        ResultText = results.resultMessage,
                        TransactionIndex = results.payUReference,
                        hasServerError = false,
                        FullRequest = getXmlString(details),
                        FullResponse = getXmlString(results)
                    };
                }
                else
                {
                    return new Transaction_Result
                    {
                        isApproved = false,
                        hasServerError = false,
                        ErrorCode = results.resultCode,
                        ErrorText = results.resultMessage,
                        FullResponse = getXmlString(results),
                        FullRequest = getXmlString(details)
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

        public void runTests()
        {
            var ran = new Random();

            var sale_details_payu_success = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "5471196125289392", CardExpiryMonth = 10, CardExpiryYear = 2020, CardCCV = "698", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            var sale_details_payu_expired = new Sale_Details { accountID = -1, appID = -1, transactionID = -1, ExtRef = "myref" + ran.Next(), CardNumber = "5420287453066155", CardExpiryMonth = 11, CardExpiryYear = 2011, CardCCV = "327", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };
            //var sale_details_payu_cvv = new Sale_Details { accountID = -1, customerID = -1, transactionID = -1, InvoiceNumber = "test1", CardNumber = "5471196125289392", CardExpiryMonth = 10, CardExpiryYear = 2020, CardCCV = "123", CardType = CardTypeEnum.VISA, Amount = 1, CurrencyCode = "ZAR", CurrencyCodeNumeric = "840", IPAddress = "192.168.1.150", CustomerFirstName = "John", CustomerLastName = "Doe", CustomerAddress = "50 Pickering", CustomerCity = "Port Elizabeth", CustomerCountry = "South Africa", CustomerCountryCodeTwoLetter = "ZA", CustomerEmail = "developer@thecodegroup.co.za", CustomerPhone = "+27413655888", CustomerState = "EC", CustomerZip = "6045", };

            LoginTest();

            // Store Payment Method

            // Revoke Payment Method

            // 3D Secure

            // Verify Card

            // AUTH
            var auth_tr = Auth(sale_details_payu_success).testApproved("Auth");
            Auth(sale_details_payu_expired).testNotApproved("Auth");
            //Auth(sale_details_payu_cvv).testNotApproved("Auth");
            

            //CAPTURE
            var capture_tr = Capture(new AuthCapture_Details
            {
                Amount = 1,
                CurrencyCode = "ZAR",
                TransactionIndex = auth_tr.TransactionIndex
            }).testApproved("Capture");

            //SALE
            var sale_tr = Sale(sale_details_payu_success).testApproved("Sale");

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
            var auth_tr_void = Auth(sale_details_payu_success).testApproved("Auth");
            var void_tr = Void(new Void_Details
            {
                Amount = 1,
                CurrencyCode = "ZAR",
                TransactionIndex = auth_tr_void.TransactionIndex
            }).testApproved("Void");



        }
        #endregion

        #region Helpers
        private TCG.PaymentGatewayLibrary.PayUEnterpriseService.EnterpriseAPISoapClient newClient()
        {
            var client = TCG.PaymentGatewayLibrary.PayUEnterprise.getClient(userName, password, Url);

            return client;
        }

        private string getXmlString(object source)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(source.GetType());
            serializer.Serialize(stringwriter, source);
            return stringwriter.ToString();
        }

        private Transaction_Result formatResult(object[] arrResults)
        {
            Transaction_Result results = new Transaction_Result();

            foreach (string result in arrResults)
            {
                results.FullResponse += result;
                //unpack result array
                int delimiter = result.IndexOf("||");
                string resultDefn = result.Substring(0, delimiter);
                string resultValue = result.Substring(delimiter + 2);

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

            return results;
        }

        #endregion
    }
}
