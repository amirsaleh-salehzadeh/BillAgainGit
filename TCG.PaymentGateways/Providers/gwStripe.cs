using System;
using System.Linq;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;
using System.Xml;
using TCG.PaymentGateways.Providers.Stripe;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace TCG.PaymentGateways.Providers
{

    public class gwStripe : IPaymentStrategy
    {
        // please see https://stripe.com/docs/dashboard#livemode-and-testing to generate an API KEY
        private string secretKey = "";
        private string publishableKey = "";


        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwStripe; } }
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
                            new TestCard(true, "Visa Success", "Joe", "Soap", "4242424242424242", 12, 2020, 5, "USD", null, null),
                            new TestCard(true, "Mastercard Success", "Joe", "Soap", "5555555555554444", 12, 2020, 5, "USD", null, null),
                            new TestCard(true, "Amex Success", "Joe", "Soap", "378282246310005", 12, 2020, 5, "USD", null, null),
                            new TestCard(true, "Amex Success", "Joe", "Soap", "378282246310005", 12, 2020, 5, "USD", null, null),
                            new TestCard(false, "Amex Fail Expiry", "Joe", "Soap", "378282246310005", 13, 2020, 5, "USD", null, null),
                            new TestCard(true, "Visa Success Token", null, null, null, 0, 0, 5, "USD", "tok_visa", null)
                        }
                    );
            }
        }
        #endregion

        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            publishableKey = MerchantConfigValues.Where(r => r.Key.Equals("PublishableKey")).FirstOrDefault().Value;
            secretKey = MerchantConfigValues.Where(r => r.Key.Equals("SecretKey")).FirstOrDefault().Value;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="PublishableKey",
                    Value = "pk_test_3OlSntr4fXcvqqXljqEXqiZM"
                },
                new MerchantConfigValue
                {
                    Key="SecretKey",
                    Value="sk_test_QKOge2uJCpgDwcA91NnaMl4W"
                }
            };
            Login(true, config);
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            try
            {
                StripeCustomer customer = createTCGStripeCustomer(details, null);
                return new StorePaymentMethod_Result()
                {
                    CardToken = customer.Id,
                    isSuccess = true,
                    TokenisePost = customer.StripeResponse.ObjectJson,

                };
            }
            catch (StripeException e)
            {
                return new StorePaymentMethod_Result()
                {
                    CardToken = null,
                    isSuccess = false,
                    ErrorCode = e.StripeError.Code,
                    ErrorMessage = e.StripeError.Message
                };
            }

        }

        public Transaction_Result Auth(Sale_Details details)
        {
            bool isApproved = false;
            bool isPending = false;
            bool failed = false;
            try
            {
                StripeConfiguration.SetApiKey(secretKey);
                if (details.ProviderToken == null || details.ProviderToken.Length <= 1)
                {
                    try
                    {

                        StripeSource source = createStripeSource(details.CardNumber, details.CardExpiryYear, details.CardExpiryMonth, details.CardCCV, details.CardHolderName + " " + details.CardHolderLastName, false);
                        details.ProviderToken = source.Id;
                    }
                    catch (StripeException e)
                    {
                        return new Transaction_Result()
                        {
                            isApproved = false,
                            hasServerError = true,
                            ErrorCode = e.StripeError.Code,
                            ErrorText = e.StripeError.Message,
                            FullResponse = e.StripeError.StripeResponse.ResponseJson
                        };
                    }
                }
                StripeCustomer customer = new StripeCustomer();
                if (details.ProviderToken.IndexOf("cus") > -1)
                {
                    customer = getTCGStripeCustomer(details.ProviderToken);
                }
                else
                {
                    customer = createTCGStripeCustomer(new StorePaymentMethod_Details()
                    {
                        CardCVV = details.CardCCV,
                        CardExpiryMonth = details.CardExpiryMonth,
                        CardExpiryYear = details.CardExpiryYear,
                        CardHolderName = details.CardHolderName,
                        CardNumber = details.CardNumber,
                        CardHolderSurname = details.CardHolderLastName
                    }, details.ProviderToken);
                }
                details.ProviderToken = customer.DefaultSourceId;
                var chargeOptions = new StripeChargeCreateOptions()
                {
                    Amount = calculateAmount(details.Amount, details.CurrencyCode),
                    Currency = details.CurrencyCode.ToLower(), //SHOULD BE LOWER CASE
                    Description = "Authentication for " + details.CustomerLastName + ", " + details.CustomerFirstName,
                    SourceTokenOrExistingSourceId = details.ProviderToken,
                    CustomerId = customer.Id,
                    Capture = false
                };
                var chargeService = new StripeChargeService();
                StripeCharge charge = chargeService.Create(chargeOptions);
                string status = charge.Status;
                if (status.Contains("succeeded"))
                    isApproved = true;
                else if (status.Contains("pending"))
                    isPending = true;
                else
                    failed = true;
                return new Transaction_Result
                {
                    isApproved = isApproved,
                    hasServerError = failed,
                    ErrorText = charge.FailureMessage,
                    ResultText = charge.Status,
                    isPending = isPending,
                    ProviderToken = charge.Id,
                    FullResponse = charge.StripeResponse.ResponseJson,
                    ApprovalCode = charge.Status
                };
            }
            catch (StripeException e)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    hasServerError = true,
                    ErrorText = e.StripeError.Message,
                    ProviderToken = null,
                    ErrorCode = e.StripeError.Code,
                    FullResponse = e.StripeError.StripeResponse.ResponseJson
                };
            }

        }

        public Transaction_Result Capture(AuthCapture_Details details)
        {
            bool isApproved = false;
            bool isPending = false;
            try
            {
                StripeConfiguration.SetApiKey(secretKey);
                var receivedTokenId = details.TransactionIndex;
                var chargeOptions = new StripeChargeCreateOptions();
                var chargeService = new StripeChargeService();
                StripeCharge charge = new StripeCharge();
                charge.Captured = true;
                //charge = chargeService.Get(receivedTokenId);
                charge = chargeService.Capture(receivedTokenId);
                string status = charge.Status;
                if (status.Contains("succeeded"))
                    isApproved = true;
                else if (status.Contains("pending"))
                    isPending = true;
                return new Transaction_Result
                {
                    isApproved = isApproved,
                    hasServerError = false,
                    isPending = isPending,
                    TransactionIndex = charge.BalanceTransactionId,
                    ProviderToken = charge.Id,
                    FullResponse = charge.StripeResponse.ResponseJson,
                    ApprovalCode = status,
                    ResultText = charge.Status
                };
            }
            catch (StripeException e)
            {
                //string errorMessage = handleStripeError(e);
                return new Transaction_Result
                {
                    isApproved = false,
                    hasServerError = true,
                    ErrorText = e.StripeError.Message,
                    ErrorCode = e.StripeError.Code,
                    ProviderToken = null,
                    FullResponse = e.StripeError.StripeResponse.ResponseJson
                };
            }
        }

        public Transaction_Result Sale(Sale_Details details)
        {
            StripeConfiguration.SetApiKey(secretKey);
            //ONCE-OFF PAYMENT 
            if (details.ProviderToken == null || details.ProviderToken.Length <= 1)
            {
                try
                {
                    StripeSource source = createStripeSource(details.CardNumber, details.CardExpiryYear, details.CardExpiryMonth, details.CardCCV, details.CardHolderName + " " + details.CardHolderLastName, false);
                    details.ProviderToken = source.Id;
                }
                catch (StripeException e)
                {
                    return new Transaction_Result()
                    {
                        isApproved = false,
                        hasServerError = true,
                        ErrorCode = e.StripeError.Code,
                        ErrorText = e.StripeError.Message,
                        FullResponse = e.StripeError.StripeResponse.ResponseJson
                    };
                }
            }

            //INITIATING A CHARGE
            var chargeOptions = new StripeChargeCreateOptions();
            var chargeService = new StripeChargeService();
            StripeCharge charge = new StripeCharge();
            chargeOptions.Capture = true;
            bool isApproved = false;
            bool isPending = false;
            try
            {

                //IF A SOURCE TOKEN IS PROVIDED >>>> ONCE-OFF PAYMENT

                if (details.ProviderToken.IndexOf("src") > -1)
                {
                    var sourceService = new StripeSourceService();
                    StripeSource source = sourceService.Get(details.ProviderToken);
                    chargeOptions.SourceTokenOrExistingSourceId = source.Id;
                    chargeOptions.Amount = calculateAmount(details.Amount, details.CurrencyCode); // $1.00 = 100 cents 
                    chargeOptions.Currency = details.CurrencyCode.ToLower();//SHOULD BE LOWER CASE
                    charge = chargeService.Create(chargeOptions);
                }

                //ONCE-OFF PAYMENT

                else if (details.ProviderToken.IndexOf("tok") > -1)
                {
                    var sourceService = new StripeSourceService();
                    chargeOptions.SourceTokenOrExistingSourceId = details.ProviderToken;
                    chargeOptions.Amount = calculateAmount(details.Amount, details.CurrencyCode);// $1.00 = 100 cents 
                    chargeOptions.Currency = details.CurrencyCode.ToLower();//SHOULD BE LOWER CASE
                    charge = chargeService.Create(chargeOptions);
                }

                //A REUSABLE CUSTOMER (OR A CARD)

                else if (details.ProviderToken.IndexOf("cus") > -1)
                {
                    var customerService = new StripeCustomerService();
                    StripeCustomer customer = customerService.Get(details.ProviderToken);
                    chargeOptions.SourceTokenOrExistingSourceId = customer.DefaultSourceId;
                    chargeOptions.CustomerId = details.ProviderToken;
                    chargeOptions.Amount = calculateAmount(details.Amount, details.CurrencyCode); // $1.00 = 100 cents 
                    chargeOptions.Currency = details.CurrencyCode.ToLower();//SHOULD BE LOWER CASE
                    charge = chargeService.Create(chargeOptions);
                }
                string status = charge.Status;
                if (status.Contains("succeeded"))
                    isApproved = true;
                else if (status.Contains("pending"))
                    isPending = true;
                return new Transaction_Result
                {
                    isApproved = isApproved,
                    hasServerError = isPending,
                    ErrorText = charge.FailureMessage,
                    ErrorCode = charge.FailureCode,
                    TransactionIndex = charge.BalanceTransactionId,
                    ProviderToken = charge.Id,
                    ResultText = charge.Status,
                    FullResponse = charge.StripeResponse.ResponseJson
                };
            }
            catch (StripeException e)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    hasServerError = true,
                    ErrorText = e.StripeError.Message,
                    ProviderToken = null,
                    ErrorCode = e.StripeError.Code,
                    FullResponse = e.StripeError.StripeResponse.ResponseJson
                };
            }

        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {

            StripeConfiguration.SetApiKey(secretKey);
            StripeDeleted delete = new StripeDeleted();
            try
            {
                var customerService = new StripeCustomerService();
                delete = customerService.Delete(details.CardToken);
            }
            catch (StripeException e)
            {
                return new RevokePaymentMethod_Result()
                {
                    ErrorCode = e.StripeError.Code,
                    ErrorMessage = e.StripeError.Message
                };
            }
            return new RevokePaymentMethod_Result()
            {
                ErrorMessage = delete.Deleted + "",
                isSuccess = delete.Deleted,
                TransactionIdentifier = delete.Id,
            };
        }

        public Lookup3dSecure_Result Lookup3dSecure(Lookup3dSecure_Details details)
        {
            var result = new Lookup3dSecure_Result();
            string style = @"<style>
                            .StripeElement {
                                    background - color: white;
                                padding: 8px 12px;
                                    border - radius: 4px;
                                border: 1px solid transparent;
                                    box - shadow: 0 1px 3px 0 #e6ebf1;
                                - webkit - transition: box - shadow 150ms ease;
                                transition: box - shadow 150ms ease;
                                }
                            .StripeElement--focus {
                                    box - shadow: 0 1px 3px 0 #cfd7df;
                            }

                            .StripeElement--invalid {
                                    border - color: #fa755a;
                            }

                            .StripeElement--webkit - autofill {
                                    background - color: #fefde5 !important;
                            }
                        </style >";
            string formHTML = @"<form id='payment-form' action='" + details.ExternalIdentifier1 + "' method='post' target='_self' ><div class='form-row'>";
            formHTML += "<label for='card-element'>Stripe Payment</label><div id='card-element'></div>";
            formHTML += "<div id='card-errors' role='alert'></div>";
            formHTML += "<input type='hidden' id='tokenVal' name='tokenVal'/>";
            formHTML += "<input type='text' name='currency' value='" + details.SaleData.CurrencyCode + "'/>";
            formHTML += "<input type='text' name='name' value='" + details.SaleData.CardHolderName + "'/>";
            formHTML += "<input type='text' name='amount' value='" + details.SaleData.Amount + "'/>";
            formHTML += "<input type='text' name='surname' value='" + details.SaleData.CardHolderLastName + "'/>";
            formHTML += "<button> Submit Payment </button></div></form>";

            string script = @"<script src='https://js.stripe.com/v3/'></script><script>";
            script += "var stripe = Stripe('" + publishableKey + "');";
            script += @"var elements = stripe.elements();
                                            var style = {
                                                base: {
                                                    color: '#32325d',
                                                    lineHeight: '24px',
                                                    fontSmoothing: 'antialiased',
                                                    fontSize: '16px',
                                                    '::placeholder': {
                                                color: '#aab7c4'
                                                    }
                                            },
                                                invalid: {
                                                    color: '#fa755a',
                                                    iconColor: '#fa755a'
                                                }
                                    };
                                    var card = elements.create('card', { style: style });
                                            card.mount('#card-element');
                                            card.addEventListener('change', function(event)
                                    {
                                        var displayError = document.getElementById('card-errors');
                                        if (event.error) {
                                            displayError.textContent = event.error.message;
                                        } else {
                                            displayError.textContent = '';
                                        }
                                    });
                                    var form = document.getElementById('payment-form');
                                    form.addEventListener('submit', function(event)
                                    {
                                        event.preventDefault();
                                        stripe.createToken(card).then(function(result) {
                                            if (result.error)
                                            {
                                                var errorElement = document.getElementById('card-errors');
                                                errorElement.textContent = result.error.message;
                                            }
                                            else
                                            {
                                                stripeTokenHandler(result.token);
                                            }
                                        });
                                    });
                                            function stripeTokenHandler(res)
                                    {
                                        document.getElementById('tokenVal').value = res.id;
                                        document.getElementById('payment-form').submit();
                                    }
                        </script>";
            if (paymentOptions.ThreeDSecure)
            {
                if (details.isPayment)
                {
                    result.ThreeDSecurePost = formHTML + script; //
                }
            }
            return result;
        }

        public Authenticate3dSecure_Result Authenticate3dSecure(Authenticate3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Verify(Sale_Details details)
        {
            throw new NotSupportedException();
        }

        //private string handleStripeError(StripeException e)
        //{
        //    String res = "";
        //    switch (e.StripeError.ErrorType)
        //    {
        //        case "card_error":
        //            Console.WriteLine("   Code: " + e.StripeError.Code);
        //            Console.WriteLine("Message: " + e.StripeError.Message);
        //            break;
        //        case "api_connection_error":
        //            break;
        //        case "api_error":
        //            break;
        //        case "authentication_error":
        //            break;
        //        case "invalid_request_error":
        //            break;
        //        case "rate_limit_error":
        //            break;
        //        case "validation_error":
        //            break;
        //        default:
        //            // Unknown Error Type
        //            break;
        //    }
        //    return res;
        //}

        public Transaction_Result Refund(Refund_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Void(Void_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Credit(Credit_Details details)
        {
            throw new NotImplementedException();
        }

        private StripeCustomer getTCGStripeCustomer(string customerToken)
        {
            StripeCustomer customer = new StripeCustomer();
            try
            {
                var customerService = new StripeCustomerService();
                customer = customerService.Get(customerToken);
            }
            catch (StripeException e)
            {
                throw new StripeException(e.HttpStatusCode, e.StripeError, e.Message);
            }
            return customer;
        }

        private StripeSource createStripeSource(string cardNo, int? expirationYear, int? expirationMonth, string ccv, string cardHolderFullName, bool isReusable)
        {
            StripeSource source = new StripeSource();
            try
            {
                var tokenOptions = new StripeTokenCreateOptions()
                {
                    Card = new StripeCreditCardOptions()
                    {
                        Number = cardNo,
                        ExpirationYear = expirationYear,
                        ExpirationMonth = expirationMonth,
                        Cvc = ccv
                    }
                };
                var tokenService = new StripeTokenService(secretKey);
                StripeToken stripeToken = tokenService.Create(tokenOptions);
                string usage = "reusable";
                if (!isReusable)
                    usage = "single_use";
                var sourceOptions = new StripeSourceCreateOptions()
                {
                    Type = StripeSourceType.Card,
                    Owner = new StripeSourceOwner()
                    {
                        Name = cardHolderFullName
                    },
                    Token = stripeToken.Id,
                    Usage = usage
                };
                var sourceService = new StripeSourceService(secretKey);
                //CREATE A SOURCE
                source = sourceService.Create(sourceOptions);
            }
            catch (StripeException e)
            {
                throw new StripeException(e.HttpStatusCode, e.StripeError, e.Message);
            }
            return source;
        }

        private int? calculateAmount(decimal amount, string currency)
        {
            var currencies = new List<string> { "mga", "bif", "pyg", "xaf", "xpf", "clp", "kmf", "rwf", "djf", "krw", "gnf", "jpy", "vuv", "vnd", "xof" };
            bool contains = currencies.Contains(currency.ToLower(), StringComparer.OrdinalIgnoreCase);
            int? amountResult = 0;
            string currencyTxt = amount + "";
            if (contains)
            {
                if (currencyTxt.Split('.').Length > 0)
                    currencyTxt = currencyTxt.Split('.')[0];
                amountResult = int.Parse(currencyTxt);
            }
            else
            {
                if (currencyTxt.Split('.').Length > 0)
                    currencyTxt = currencyTxt.Split('.')[0] + currencyTxt.Split('.')[1];
                amountResult = int.Parse(currencyTxt);
                //amountResult = Decimal.ToInt32(amount * 100);
            }
            return amountResult;
        }

        private StripeCustomer createTCGStripeCustomer(StorePaymentMethod_Details details, string sourceToken)
        {
            StripeConfiguration.SetApiKey(secretKey);
            StripeCustomer customer = new StripeCustomer();
            try
            {
                StripeSource source = new StripeSource();
                if (sourceToken == null)
                    source = createStripeSource(details.CardNumber, details.CardExpiryYear, details.CardExpiryMonth, details.CardCVV, details.CardHolderFullName, true);
                else
                    source.Id = sourceToken;
                //ATTACH THE SOURCE TO CUSTOMER
                var customerOptions = new StripeCustomerCreateOptions()
                {
                    SourceToken = source.Id
                };

                var customerService = new StripeCustomerService();
                customer = customerService.Create(customerOptions);
            }
            catch (StripeException e)
            {
                throw new StripeException(e.HttpStatusCode, e.StripeError, e.Message);
            }
            return customer;
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
