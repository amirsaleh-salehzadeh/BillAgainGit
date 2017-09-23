using Newtonsoft.Json;
using PayPal;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TCG.PaymentGatewayLibrary;

namespace TCG.PaymentGateways.Utils
{
    public class PayPalDirect
    {
        public static string ApiClientId = "";
        public static string ClientSecret = "";

        /// <summary>
        /// Does the direct payment sale. Capture and Authorize
        /// </summary>
        /// <param name="apiClientId">The API client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        /// <author>Rudi Opperman</author>
        /// <datetime>02/02/2017-10:34 AM</datetime>
        public static PaymentResult Sale(string apiClientId, string clientSecret, Sale_DetailsModel details)
        {
            //Get Credentials, can be found in https://developer.paypal.com/developer/applications 
            ApiClientId = apiClientId;
            ClientSecret = clientSecret;

            //Using Paypal.Api from sdk
            var apiContext = Configuration.GetAPIContext();

            // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    currency = details.CurrencyCode,
                    total = details.Amount.ToString("f2")
                },
                description = details.ProductDescription,
                //item_list = new ItemList()
                //{
                //    items = new List<Item>()
                //    {
                //        new Item()
                //        {
                //            name = "Item Name",
                //            currency = "USD",
                //            price = "1",
                //            quantity = "5",
                //            sku = "sku"
                //        }
                //    },
                //    shipping_address = new ShippingAddress
                //    {
                //        city = "Johnstown",
                //        country_code = "US",
                //        line1 = "52 N Main ST",
                //        postal_code = "43210",
                //        state = "OH",
                //        recipient_name = "Joe Buyer"
                //    }
                //},
                invoice_number = details.transactionID.ToString()
            };

            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "credit_card",
                funding_instruments = new List<FundingInstrument>()
                {
                    new FundingInstrument()
                    {
                        credit_card = new CreditCard()
                        {
                            billing_address = new Address()
                            {
                                city = details.CustomerCity,
                                country_code =  details.CustomerCountryCodeTwoLetter, //details.CustomerCountryCodeTwoLetter,
                                line1 = details.CustomerAddress,
                                postal_code = details.CustomerZip,
                                state = details.CustomerState
                            },
                            cvv2 = details.CardCCV,
                            expire_month = details.CardExpiryMonth,
                            expire_year = details.CardExpiryYear,
                            first_name = details.CustomerFirstName,
                            last_name = string.IsNullOrWhiteSpace(details.CardHolderLastName)?details.CustomerFirstName:details.CardHolderLastName,
                            number = details.CardNumber,
                            type = details.CardType.ToString().ToLower()
                        }
                    }
                },
                payer_info = new PayerInfo
                {
                    email = details.CustomerEmail
                }
            };

            // A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = new List<Transaction>() { transaction }
            };

            try
            {
                // Create a payment using a valid APIContext
                var createdPayment = payment.Create(apiContext);

                return new PaymentResult
                {
                    state = createdPayment.state,
                    FullRequest = XmlSerialize<Payment>(payment),
                    FullResponse = XmlSerialize<Payment>(createdPayment),
                    ErrorCode = "",
                    ErrorText = ""
                };
            }
            catch (PaymentsException e) //Special Exception thrown by PayPal will contain error
            {
                return new PaymentResult
                {
                    state = "Fail",
                    FullRequest = XmlSerialize<Payment>(payment),
                    FullResponse = JsonConvert.DeserializeXmlNode(e.Response, "root").ToString(),
                    ErrorCode = "",
                    ErrorText = ""
                };
            }
            catch (Exception e)
            {
                return new PaymentResult
                {
                    state = "Fail",
                    FullRequest = XmlSerialize<Payment>(payment),
                    FullResponse = XmlSerialize<Exception>(e),
                    ErrorCode = "",
                    ErrorText = ""
                };

            }

        }

        /// <summary>
        /// Authorizes the specified API client identifier.
        /// </summary>
        /// <param name="apiClientId">The API client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        /// <author>Rudi Opperman</author>
        /// <datetime>02/02/2017-11:06 AM</datetime>
        private PaymentResult Authorize(string apiClientId, string clientSecret, Sale_DetailsModel details)
        {

            //Big Said that for now I must not code this
            throw new NotImplementedException();

            //Get Credentials, can be found in https://developer.paypal.com/developer/applications 
            ApiClientId = apiClientId;
            ClientSecret = clientSecret;

            //Using Paypal.Api from sdk
            var apiContext = Configuration.GetAPIContext();

            // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    currency = details.CurrencyCode,
                    total = details.Amount.ToString()
                },
                description = details.ProductDescription,
                //item_list = new ItemList()
                //{
                //    items = new List<Item>()
                //    {
                //        new Item()
                //        {
                //            name = "Item Name",
                //            currency = "USD",
                //            price = "1",
                //            quantity = "5",
                //            sku = "sku"
                //        }
                //    },
                //    shipping_address = new ShippingAddress
                //    {
                //        city = "Johnstown",
                //        country_code = "US",
                //        line1 = "52 N Main ST",
                //        postal_code = "43210",
                //        state = "OH",
                //        recipient_name = "Joe Buyer"
                //    }
                //},
                invoice_number = details.transactionID.ToString()
            };

            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "credit_card",
                funding_instruments = new List<FundingInstrument>()
                {
                    new FundingInstrument()
                    {
                        credit_card = new CreditCard()
                        {
                            billing_address = new Address()
                            {
                                city = details.CustomerCity,
                                country_code =  details.CustomerCountryCodeTwoLetter, //details.CustomerCountryCodeTwoLetter,
                                line1 = details.CustomerAddress,
                                postal_code = details.CustomerZip,
                                state = details.CustomerState
                            },
                            cvv2 = details.CardCCV,
                            expire_month = details.CardExpiryMonth,
                            expire_year = details.CardExpiryYear,
                            first_name = details.CustomerFirstName,
                            last_name = details.CardHolderLastName,
                            number = details.CardNumber,
                            type = details.CardType.ToString().ToLower()
                        }
                    }
                },
                payer_info = new PayerInfo
                {
                    email = details.CustomerEmail
                }
            };

            // A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = "authorize",
                payer = payer,
                transactions = new List<Transaction>() { transaction }
            };

            try
            {
                // Create a payment using a valid APIContext
                var createdPayment = payment.Create(apiContext);

                return new PaymentResult
                {
                    state = createdPayment.state,
                    FullRequest = XmlSerialize<Payment>(payment),
                    FullResponse = XmlSerialize<Payment>(createdPayment),
                    ErrorCode = "",
                    ErrorText = ""
                };
            }
            catch (PaymentsException e) //Special Exception thrown by PayPal will contain error
            {
                return new PaymentResult
                {
                    state = "Fail",
                    FullRequest = XmlSerialize<Payment>(payment),
                    FullResponse = JsonConvert.DeserializeXmlNode(e.Response, "root").ToString(),
                    ErrorCode = "",
                    ErrorText = ""
                };
            }
            catch (Exception e)
            {
                return new PaymentResult
                {
                    state = "Fail",
                    FullRequest = XmlSerialize<Payment>(payment),
                    FullResponse = XmlSerialize<Exception>(e),
                    ErrorCode = "",
                    ErrorText = ""
                };

            }

        }       

        /// <summary>
        /// XMLs the serialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataToSerialize">The data to serialize.</param>
        /// <returns></returns>
        /// <author>Rudi Opperman</author>
        /// <datetime>02/02/2017-10:36 AM</datetime>
        private static string XmlSerialize<T>(T dataToSerialize)
        {
            try
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stringwriter, dataToSerialize);

                    var Result = stringwriter.ToString();

                    return Result;
                }                  
            }
            catch
            {
                throw;
            }
        }
    }

    //PayPal Class required for Configuration
    public static class Configuration
    {
        public static string ClientId;
        public static string ClientSecret;

        // Static constructor for setting the readonly static members.
        static Configuration()
        {

            if (string.IsNullOrEmpty(PayPalDirect.ApiClientId) || string.IsNullOrEmpty(PayPalDirect.ClientSecret))
                throw new NullReferenceException("Please call PayPalDirect.DoDirectPayment");

            //var config = GetConfig();
            ClientId = PayPalDirect.ApiClientId;
            ClientSecret = PayPalDirect.ClientSecret;
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        // Create accessToken
        private static string GetAccessToken()
        {
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string accessToken = "")
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken);
            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
        }

    }

    public class Sale_DetailsModel
    {
        // Tracking Field
        public long accountID { get; set; }
        public long appID { get; set; }
        //public long customerID { get; set; }
        //public string customerReference { get; set; }
        public long transactionID { get; set; }
        //public string InvoiceNumber { get; set; }
        public string ExtRef { get; set; }

        // Transaction Info
        public string CardHolderName { get; set; }
        public string CardHolderLastName { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string CardCCV { get; set; }
        public string CardType { get; set; }
        public decimal Amount { get; set; }
        public string ProviderToken { get; set; }
        public string ProviderPIN { get; set; }

        public string CurrencyCode { get; set; }

        public string ProductDescription { get; set; }

        // Customer Info
        public string CustomerIdentifier { get; set; }
        public string IPAddress { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerCountryCodeTwoLetter { get; set; }
        public string CustomerCountryCodeThreeLetter { get; set; }
        public string CustomerCountryNumeric { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZip { get; set; }
    }

    public class PaymentResult
    {
        public string state { get; set; }
        public string FullRequest { get; set; }
        public string FullResponse { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }


}
