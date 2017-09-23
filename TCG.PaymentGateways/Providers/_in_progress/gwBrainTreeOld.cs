using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Braintree;
using nsoftware.InPay;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;
using TCG.PaymentGateways.Utils;

namespace TCG.PaymentGateways.Providers._in_progress
{
    /// <summary>
    /// nsoftware notes:
    /// NOT USING NSOFTWARE
    /// MerchantLogin and MerchantPassword are required properties.
    /// The TransactionAmount is required to be represented as cents with a decimal point. For example, "1.00".
    /// The "CurrencyCode" configuration setting is available for this gateway. The default value is "USD".
    /// TestMode is not supported and when set to "True" an exception will be thrown by the component.
    /// </summary>
    public class gwBrainTreeOld : IPaymentStrategy
    {
        private string MerchantId;
        private string PublicKey;
        private string PrivateKey;
        private Braintree.Environment Environment;

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwBrainTree; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "BrainTree DirectPost",
                        WebUrl: "http://www.braintreepaymentsolutions.com/",
                        Description: "Braintree is available in the US, Europe, Australia, and Canada. Merchants can accept payments in 130+ currencies.",
                        isActive: true,
                        isLive: false,
                        MerchantConfigValues: new[] { "PrivateKey", "MerchantId", "PublicKey" },
                        Currencies: new[] {
                            "AFA","ALL","DZD","ARS","AMD","AWG","AUD","AZN","BSD","BHD",
                            "BDT","BBD","BYR","BZD","BMD","BOB","BWP","BRL","BND","BGN",
                            "BIF","KHR","CAD","CVE","KYD","XAF","XPF","CLP","CNY","COP",
                            "KMF","BAM","CRC","HRK","CUP","CYP","CZK","DKK","DJF","DOP",
                            "XCD","ECS","EGP","SVC","ERN","EEK","ETB","EUR","FKP","FJD",
                            "CDF","GMD","GEL","GHS","GIP","GTQ","GNF","GWP","GYD","HTG",
                            "HNL","HKD","HUF","ISK","INR","IDR","IRR","IQD","ILS","JMD",
                            "JPY","JOD","KZT","KES","KWD","AOA","KGS","KIP","LAK","LVL",
                            "LBP","LRD","LYD","LTL","LSL","MOP","MOP","MKD","MGF","MGA",
                            "MWK","MYR","MVR","MTL","MRO","MUR","MXN","MDL","MNT","MAD",
                            "MZM","MMK","NAD","NPR","ANG","PGK","TWD","TRY","NZD","NIO",
                            "NGN","KPW","NOK","PKR","PAB","PYG","PEN","PHP","PLN","QAR",
                            "OMR","RON","RUB","RWF","WST","STD","SAR","RSD","SCR","SLL",
                            "SGD","SKK","SIT","SBD","SOS","ZAR","KRW","LKR","SHP","SDD",
                            "SRD","SZL","SEK","CHF","SYP","TJS","TZS","THB","TOP","TTD",
                            "TND","TMM","UGX","UAH","AED","GBP","USD","UYU","UZS","VUV",
                            "VEF","VND","XOF","YER","YUM","ZMK","ZWD",
                        },
                        Countries: new[] { "USA", "CAN" },      // USA, Canada, Aus, Europe - need full list
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
                        PaymentTokenize: false,
                        PaymentTokenize_requires_CVV: false,
                        PaymentTokenize_external: false,
                        ThreeDSecure: false,
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
            MerchantId = MerchantConfigValues.Where(r => r.Key.Equals("MerchantId")).FirstOrDefault().Value;
            PublicKey = MerchantConfigValues.Where(r => r.Key.Equals("PublicKey")).FirstOrDefault().Value;
            PrivateKey = MerchantConfigValues.Where(r => r.Key.Equals("PrivateKey")).FirstOrDefault().Value;

            if (isTestMode)
                Environment = Braintree.Environment.SANDBOX;
            else
                Environment = Braintree.Environment.PRODUCTION;
        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                //This corresponds to the x_login field and is your API Login ID
                new MerchantConfigValue
                {
                    Key="MerchantId",
                    Value="ffsxw99q6z4bcg87"
                },
                //This correspond to the x_tran_key field and is your Transaction Key value
                new MerchantConfigValue
                {
                    Key="PublicKey",
                    Value="xjrf3g4ndfrrn6q2"
                },
                new MerchantConfigValue 
                { 
                    Key = "PrivateKey", 
                    Value = "74063683b81c1c3654ed1cf80ce0eafc" 
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
            try
            {
                var gateway = newClient();

                TransactionRequest request = new TransactionRequest
                {
                    Amount = details.Amount,
                    OrderId = details.ExtRef,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = details.CardNumber,
                        ExpirationDate = formatExpiryDate(details.CardExpiryMonth, details.CardExpiryYear),
                        CardholderName = details.CustomerFirstName + " " + details.CustomerLastName,
                        CVV = details.CardCCV,
                    },
                    Customer = new CustomerRequest
                    {
                        FirstName = details.CustomerFirstName,
                        LastName = details.CustomerLastName,
                        Phone = details.CustomerPhone,
                        Email = details.CustomerEmail
                    },
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = false
                    },
                };

                Result<Transaction> Tresult = gateway.Transaction.Sale(request);

                Transaction_Result result = formatResult(request, Tresult);
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
                var gateway = newClient();

                Result<Transaction> Tresult;
                if (details.Amount.HasValue)
                    Tresult = gateway.Transaction.SubmitForSettlement(details.TransactionIndex, details.Amount.Value);
                else
                    Tresult = gateway.Transaction.SubmitForSettlement(details.TransactionIndex);

                Transaction_Result result = formatResult(null, Tresult);
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
                //var icharge = newClient();
                //nSoftwareUtils.fill_SaleRequest(ref icharge, details, false);
                //icharge.Sale();
                //var result = nSoftwareUtils.parse_SaleResponse(ref icharge);

                var gateway = newClient();

                TransactionRequest request = new TransactionRequest
                {
                    Amount = details.Amount,
                    OrderId = details.ExtRef,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = details.CardNumber,
                        ExpirationDate = formatExpiryDate(details.CardExpiryMonth, details.CardExpiryYear),
                        CardholderName = details.CustomerFirstName + " " + details.CustomerLastName,
                        CVV = details.CardCCV,
                    },
                    Customer = new CustomerRequest
                    {
                        FirstName = details.CustomerFirstName,
                        LastName = details.CustomerLastName,
                        Phone = details.CustomerPhone,
                        Email = details.CustomerEmail
                    },
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    },
                };

                Result<Transaction> Tresult = gateway.Transaction.Sale(request);

                Transaction_Result result = formatResult(request, Tresult);
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
                var gateway = newClient();

                Result<Transaction> Tresult;
                if (details.Amount.HasValue)
                    Tresult = gateway.Transaction.Refund(details.TransactionIndex, details.Amount.Value);
                else
                    Tresult = gateway.Transaction.Refund(details.TransactionIndex);

                Transaction_Result result = formatResult(null, Tresult);
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
            try
            {
                var gateway = newClient();

                Result<Transaction> Tresult = gateway.Transaction.Void(details.TransactionIndex);

                Transaction_Result result = formatResult(null, Tresult);
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

        public void runTests()
        {
            throw new NotImplementedException();
        }
        #endregion            

        #region Helpers
        private BraintreeGateway newClient()
        {
            var gateway = new BraintreeGateway
            {
                Environment = Environment,
                MerchantId = MerchantId,
                PublicKey = PublicKey,
                PrivateKey = PrivateKey
            };
            return gateway;
        }

        private string formatExpiryDate(int month, int year)
        {
            if (month < 10)
                return "0" + month + "/" + year;
            return month + "/" + year;
        }

        private Transaction_Result formatResult(Request request, Result<Transaction> Tresult)
        {
            Transaction_Result result;
            if (Tresult.IsSuccess())
            {
                Transaction transaction = Tresult.Target;

                result = new Transaction_Result
                {
                    isApproved = true,
                    ApprovalCode = transaction.ProcessorAuthorizationCode,

                    ResultCode = transaction.ProcessorResponseCode,
                    ResultText = Tresult.Message,
                    TransactionIndex = transaction.Id,
                    ProcessorCode = transaction.ProcessorAuthorizationCode,

                    FullRequest = request == null ? "" : request.ToXml(),
                    //FullResponse = Tresult._node.OuterXml(),

                    hasServerError = false,
                    ErrorCode = "",
                    ErrorText = ""
                };
            }
            else if (Tresult.Transaction != null)
            {
                Transaction transaction = Tresult.Transaction;

                result = new Transaction_Result
                {
                    isApproved = false,
                    ApprovalCode = transaction.ProcessorAuthorizationCode,

                    ResultCode = transaction.ProcessorResponseCode,
                    ResultText = Tresult.Message,
                    TransactionIndex = transaction.Id,
                    ProcessorCode = transaction.ProcessorAuthorizationCode,

                    FullRequest = request == null ? "" : request.ToXml(),
                    //FullResponse = Tresult._node.OuterXml(),

                    hasServerError = false,
                    ErrorCode = transaction.ProcessorResponseCode,
                    ErrorText = transaction.ProcessorResponseText
                };
            }
            else
            {
                string ErrorCode = "";
                string ErrorText = "";
                foreach (ValidationError error in Tresult.Errors.DeepAll())
                {
                    ErrorCode += error.Code + ",";
                    ErrorText += error.Attribute + ": " + error.Message + ",";
                }

                result = new Transaction_Result
                {
                    isApproved = false,
                    ApprovalCode = "",

                    ResultCode = "",
                    ResultText = Tresult.Message,
                    TransactionIndex = "",
                    ProcessorCode = "",

                    FullRequest = request == null ? "" : request.ToXml(),
                    //FullResponse = Tresult._node.OuterXml(),

                    hasServerError = false,
                    ErrorCode = ErrorCode,
                    ErrorText = ErrorText
                };
            }

            return result;
        }
        #endregion
    }
}
