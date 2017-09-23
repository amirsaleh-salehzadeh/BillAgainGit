using Braintree;
using Braintree.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Providers
{
    public class gwBrainTree : IPaymentStrategy
    {
        public string Username;
        private BraintreeGateway gateway;
        public string merchantId;
        public string publicKey;
        public string privateKey;

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwBrainTree; } }

        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "Stripe",
                        WebUrl: "https://stripe.com",
                        Description: "",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "SecretKey", "PublishableKey" },
                        Currencies: new[] { "USD", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "LVL",
                            "BAM", "BBD", "BDT", "BGN", "BIF", "BMD", "BND", "BOB", "BRL", "BSD", "BWP", "BZD", "CAD", "CDF",
                            "CHF", "CLP", "CNY", "COP", "CRC", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ETB", "EUR",
                            "FJD", "FKP", "GBP", "GEL", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF",
                            "IDR", "ILS", "INR", "ISK", "JMD", "JPY", "KES", "KGS", "KHR", "KMF", "KRW", "KYD", "KZT", "LAK",
                            "LBP", "LKR", "LRD", "LSL", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR",
                            "MWK", "MXN", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "PAB", "PEN", "PGK", "PHP",
                            "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SEK", "SGD", "SHP",
                            "SLL", "SOS", "SRD", "STD", "SVC", "SZL", "THB", "TJS", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH",
                            "UGX", "UYU", "UZS", "VND", "VUV", "WST", "XAF", "XCD", "XOF", "XPF", "YER", "ZAR", "ZMW", "EEK", "VEF" },//https://stripe.com/docs/currencies
                        Countries: new[] { "ZA" },
                        CardTypes: new[]
                        {
                            CardTypeEnum.VISA,
                            CardTypeEnum.MASTERCARD,
                            CardTypeEnum.AMERICAN_EXPRESS,
                            CardTypeEnum.DINERS_CLUB,
                            CardTypeEnum.JCB,
                            CardTypeEnum.DISCOVER
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
                        PaymentTokenize: true,
                        PaymentTokenize_requires_CVV: true,
                        PaymentTokenize_external: true,
                        ThreeDSecure: true,
                        Verify: false,
                        Auth: true,
                        AuthCapture: true,
                        AuthCapturePartial: true,
                        Sale: true,
                        Refund: false,
                        RefundPartial: false,
                        Credit: false,
                        Void: false,
                        TestCards: new TestCard[] {
                            new TestCard(true, "American Express Success", "Joe", "Soap", "378282246310005", 12, 2020, 5, "USD", null, null),
                            new TestCard(true, "Diners Club Success", "Joe", "Soap", "36259600000004", 12, 2020, 5, "USD", null, null),
                            new TestCard(true, "Mastercard Success", "Joe", "Soap", "5555555555554444", 12, 2020, 5, "USD", null, null),
                            new TestCard(true, "Visa Success", "Joe", "Soap", "4111111111111111", 12, 2020, 5, "USD", null, null),
                            new TestCard(false, "Visa processor declined", "Joe", "Soap", "4000111111111115", 11, 2020, 5, "USD", null, null)
                        }
                    );
            }
        }
        #endregion

        #region Methods
        public Transaction_Result Auth(Sale_Details details)
        {
            throw new NotImplementedException();
        }

        public Authenticate3dSecure_Result Authenticate3dSecure(Authenticate3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Capture(AuthCapture_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Credit(Credit_Details details)
        {
            throw new NotImplementedException();
        }

        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            publicKey = MerchantConfigValues.Where(r => r.Key.Equals("PublicKey")).FirstOrDefault().Value;
            privateKey = MerchantConfigValues.Where(r => r.Key.Equals("PrivateKey")).FirstOrDefault().Value;
            merchantId = MerchantConfigValues.Where(r => r.Key.Equals("MerchantId")).FirstOrDefault().Value;
            var environment = Braintree.Environment.PRODUCTION;
            if (isTestMode)
                environment = Braintree.Environment.SANDBOX;
            gateway = new BraintreeGateway
            {
                Environment = environment,
                MerchantId = merchantId,
                PublicKey = publicKey,
                PrivateKey = privateKey
            };
        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="PublicKey",
                    Value = "xjrf3g4ndfrrn6q2"
                },
                new MerchantConfigValue
                {
                    Key="PrivateKey",
                    Value="74063683b81c1c3654ed1cf80ce0eafc"
                },
                new MerchantConfigValue
                {
                    Key="MerchantId",
                    Value="ffsxw99q6z4bcg87"
                }
            };

            Login(true, config);
        }

        public Lookup3dSecure_Result Lookup3dSecure(Lookup3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Refund(Refund_Details details)
        {
            throw new NotImplementedException();
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Sale(Sale_Details details)
        {
            Result<Customer> resCustomer = null;
            Customer customer = null;
            try
            {

                if (details.ProviderToken == null || details.ProviderToken.Length <= 1)
                {
                    resCustomer = createOnceOffCustomer(new Sale_Details()
                    {
                        CustomerFirstName = details.CustomerFirstName,
                        CustomerLastName = details.CustomerLastName,
                        CardCCV = details.CardCCV,
                        CardNumber = details.CardNumber,
                        CardExpiryYear = details.CardExpiryYear,
                        CardExpiryMonth = details.CardExpiryMonth
                    });
                    if (resCustomer.Target == null)
                    {
                        return new Transaction_Result()
                        {
                            isApproved = false,
                            ErrorText = resCustomer.Message,
                            FullResponse = new JavaScriptSerializer().Serialize(resCustomer),
                        };
                    }
                    else customer = resCustomer.Target;
                }
                else
                {
                    customer = gateway.Customer.Find(details.ProviderToken);
                }
                
            }
            catch (NotFoundException e)
            {
                return new Transaction_Result()
                {
                    isApproved = false,
                    ErrorText = e.Message,
                    ErrorCode = e.HResult + ""
                };
            }
            try
            {
                Result<PaymentMethodNonce> resultPMN = gateway.PaymentMethodNonce.Create("A_PAYMENT_METHOD_TOKEN");
                String nonce = resultPMN.Target.Nonce;
                var requestPMethod = new PaymentMethodRequest
                {
                    CustomerId = customer.Id,
                    PaymentMethodNonce = nonce
                };

                Result<PaymentMethod> resultPM = gateway.PaymentMethod.Create(requestPMethod);
                var request = new TransactionRequest
                {
                    Amount = details.Amount,
                    CustomerId = customer.Id,
                    PaymentMethodToken = customer.PaymentMethods[0].Token,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true,
                    }
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);
                return new Transaction_Result()
                {
                    isApproved = result.IsSuccess(),
                    TransactionIndex = result.Transaction.Id,
                    ResultText = result.Message,
                    FullResponse = new JavaScriptSerializer().Serialize(result)
                };
            }
            catch (NotFoundException e) {
                return new Transaction_Result()
                {
                    ErrorText = e.Message,
                    isApproved = false,
                    FullResponse = new JavaScriptSerializer().Serialize(e)
                };
            }
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Verify(Sale_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Void(Void_Details details)
        {
            throw new NotImplementedException();
        }

        private Result<Customer> createOnceOffCustomer(Sale_Details details)
        {
            //try
            //{
                CustomerRequest request = new CustomerRequest
                {
                    FirstName = details.CardHolderName,
                    LastName = details.CardHolderLastName,
                    CreditCard = new CreditCardRequest
                    {
                        Options = new CreditCardOptionsRequest
                        {
                            VerifyCard = true
                        },
                        CVV = details.CardCCV,
                        ExpirationMonth = details.CardExpiryMonth + "",
                        ExpirationYear = details.CardExpiryYear + "",
                        Number = details.CardNumber
                    }
                };
                Result<Customer> result = gateway.Customer.Create(request);
                return result;
            //}
            //catch (AuthenticationException e) {
            //    throw new AuthenticationException() {
            //        Source = e.Source,
            //        HelpLink = e.HelpLink
            //    };
            //}
            
        }
        #endregion
    }
}
