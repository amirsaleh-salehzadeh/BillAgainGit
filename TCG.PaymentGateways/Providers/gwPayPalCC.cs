using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;
using TCG.PaymentGateways.Utils;
using System.Net;

namespace TCG.PaymentGateways.Providers
{
    class gwPayPalCC : IPaymentStrategy
    {

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public GatewayOptions gatewayOptions
        {
            get
            {

                //For currencies and countrues please see https://developer.paypal.com/docs/classic/api/currency_codes/
                return new GatewayOptions
                    (
                        DisplayName: "PayPal Direct",
                        WebUrl: "https://www.paypal.com",
                        Description: "Specialising in online payments.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "ClientId", "ClientSecret" },
                        Currencies: new[] { "AUD", "USD", "THB", "TWD", "CHF", "SEK", "RUB", "SGD", "SEK", "CHF", "TWD", "THB", "USD" },
                        Countries: new[] { "AU", "CA", "FR", "HK", "IT", "JP", "SG", "ES", "GB", "US" },
                        CardTypes: new[]
                        {
                             CardTypeEnum.VISA,
                            CardTypeEnum.MASTERCARD
                        }
                    );
            }
        }

        public ProviderType GatewayType { get { return Classes.ProviderType.gwPayPalCC; } }

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


        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            ClientId = MerchantConfigValues.Where(r => r.Key.Equals("ClientId")).FirstOrDefault().Value;
            ClientSecret = MerchantConfigValues.Where(r => r.Key.Equals("ClientSecret")).FirstOrDefault().Value;

        }

        public void LoginTest()
        {
            Login(true, new MerchantConfigValue[] {
                new MerchantConfigValue("ClientId", "ASShty4eoP2beWl8Yq4eHOj5mb1AM9REFmNZcIjGiriavKA4jfV0W9AFZF1C6WdBlBOeVdv7PuL-aLjN"),
                new MerchantConfigValue("ClientSecret", "ECzH1ZnUK41NmODwSYydM3ErCZbC0V_0RrzjBw95JXafm0PM_291TO7fydb4HF8OhClogSJql1VJTKH9")
            });
        }



        public void runTests()
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Sale(Sale_Details details)
        {

            try
            {
                var Payment = PayPalDirect.Sale(ClientId, ClientSecret, new Sale_DetailsModel
                {
                    accountID = details.accountID,
                    transactionID = details.transactionID,
                    appID = details.appID,

                    Amount = details.Amount,
                    CurrencyCode = details.CurrencyCode,

                    CardNumber = details.CardNumber,
                    CardType = details.CardType.ToString().ToLower(),
                    CardCCV = details.CardCCV,
                    CardExpiryMonth = details.CardExpiryMonth,
                    CardExpiryYear = details.CardExpiryYear,
                    CardHolderLastName = details.CardHolderLastName,
                    CardHolderName = details.CardHolderName,

                    CustomerCity = details.CustomerCity,
                    CustomerCountry = details.CustomerCountry,
                    CustomerCountryCodeThreeLetter = details.CustomerCountryCodeThreeLetter,
                    CustomerCountryCodeTwoLetter = details.CustomerCountryCodeTwoLetter,
                    CustomerCountryNumeric = details.CustomerCountryNumeric,
                    CustomerEmail = details.CustomerEmail,
                    CustomerFirstName = details.CustomerFirstName,
                    CustomerIdentifier = details.CustomerIdentifier,
                    CustomerLastName = details.CustomerLastName,
                    CustomerPhone = details.CustomerPhone,
                    CustomerZip = details.CustomerZip,
                    CustomerState = details.CustomerState,
                    CustomerAddress = details.CustomerAddress,

                    ExtRef = details.ExtRef,
                    ProductDescription = details.ProductDescription,
                    ProviderPIN = details.ProviderPIN,
                    ProviderToken = details.ProviderToken,
                    IPAddress = details.IPAddress
                });

                if (Payment.state == "approved")
                {
                    return new Transaction_Result
                    {
                        isApproved = true,
                        ApprovalCode = "200",
                        FullRequest = Payment.FullRequest,
                        FullResponse = Payment.FullResponse,
                        hasServerError = false,
                        ErrorCode = "",
                        ErrorText = "",
                        ProviderToken = details.ProviderToken
                    };
                }
                else
                {
                    return new Transaction_Result
                    {
                        isApproved = false,
                        FullRequest = Payment.FullRequest,
                        FullResponse = Payment.FullResponse,
                        hasServerError = true,
                        ErrorCode = "",
                        ErrorText = ""
                    };
                }
            }
            catch (WebException e)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    FullRequest = "",
                    FullResponse = e.ToString(),
                    hasServerError = true,
                    ErrorText = "An unknown error has occurred, return object not in correct format"
                };
            }



            throw new NotImplementedException();
        }

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
        #endregion      
    }
}
