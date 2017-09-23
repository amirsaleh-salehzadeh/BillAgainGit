using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Providers
{
    public class gwCashFlows : IPaymentStrategy
    {
        private string authID;
        private string authPass;
        private string Mode;// 1 => test, 0 => live

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwVCS; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "Cash Flows",
                        WebUrl: "http://www.cashflows.com/",
                        Description: "CashFlows provides a wealth of experience and a wide range of services to help business of all size to grow and start accepting and making payments.",
                        isActive: false,
                        isLive: false,
                        MerchantConfigValues: new[] { "auth_id", "auth_pass" },
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
                        requires_CVV: true,
                        PaymentTokenize: true,
                        PaymentTokenize_requires_CVV: true,
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
        #endregion

        public void Login(bool isTestMode, params Classes.MerchantConfigValue[] MerchantConfigValues)
        {
            authID = MerchantConfigValues.Where(r => r.Key.Equals("auth_id")).FirstOrDefault().Value;
            authPass = MerchantConfigValues.Where(r => r.Key.Equals("auth_pass")).FirstOrDefault().Value;

            if (isTestMode)
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
            throw new NotImplementedException();
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add("auth_id", authID);
            parameters.Add("auth_pass", authPass);
            parameters.Add("card_num", details.CardNumber);
            parameters.Add("card_cvv", details.CardCVV);
            parameters.Add("card_expiry", GatewayUtils.formatExpiryDateYY(details.CardExpiryMonth, details.CardExpiryYear));
            parameters.Add("cust_name", "");
            parameters.Add("cust_address", "");
            parameters.Add("cust_postcode", "");
            parameters.Add("cust_country", "");
            parameters.Add("cust_ip", "");
            parameters.Add("cust_email", "");
            parameters.Add("cust_tel", "");
            parameters.Add("tran_ref", details.ClientIdentifier);
            parameters.Add("tran_amount", "1.00");
            parameters.Add("tran_currency", "");
            parameters.Add("tran_testmode", Mode);
            parameters.Add("tran_type", "verify");
            parameters.Add("tran_class", "ecom");

            var result = DoUrlEncodedFormPost("https://secure.cashflows.com/gateway/remote", parameters);

            var result_set = result.Split('|');

            if (result_set.Length != 5)
            {
                throw new Exception("Response format invalid");
            }

            var status = result_set[0];
            var transactionIdentifier = result_set[1];
            var avs = result_set[2];
            var auth_code = result_set[3];
            var message = result_set[4];

            bool isSuccess = status == "A";

            return new StorePaymentMethod_Result
            {
                CardToken = transactionIdentifier,
                ErrorCode = !isSuccess ? auth_code : "",
                ErrorMessage = !isSuccess ? message : "",
                isSuccess = isSuccess
            };
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(Classes.Payments.RevokePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public Lookup3dSecure_Result Lookup3dSecure(Classes.Payments.Lookup3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Authenticate3dSecure_Result Authenticate3dSecure(Classes.Payments.Authenticate3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Verify(Classes.Payments.Sale_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Auth(Sale_Details details)
        {
            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add("auth_id", authID);
            parameters.Add("auth_pass", authPass);
            parameters.Add("card_num", details.CardNumber);
            parameters.Add("card_cvv", details.CardCCV);
            parameters.Add("card_expiry", GatewayUtils.formatExpiryDateYY(details.CardExpiryMonth, details.CardExpiryYear));
            parameters.Add("cust_name", details.CustomerFirstName);
            parameters.Add("cust_address", details.CustomerAddress);
            parameters.Add("cust_postcode", details.CustomerZip);
            parameters.Add("cust_country", details.CustomerCountryCodeTwoLetter);
            parameters.Add("cust_ip", details.IPAddress);
            parameters.Add("cust_email", details.CustomerEmail);
            parameters.Add("cust_tel", details.CustomerPhone);
            parameters.Add("tran_ref", details.ExtRef);
            parameters.Add("tran_amount", details.Amount.ToString("F2"));
            parameters.Add("tran_currency", details.CurrencyCode);
            parameters.Add("tran_testmode", Mode);
            parameters.Add("tran_type", "verify");
            parameters.Add("tran_class", "ecom");

            var result = DoUrlEncodedFormPost("https://secure.cashflows.com/gateway/remote", parameters);

            var result_set = result.Split('|');

            if (result_set.Length != 5)
            {
                throw new Exception("Response format invalid");
            }

            var status = result_set[0];
            var transactionIdentifier = result_set[1];
            var avs = result_set[2];
            var auth_code = result_set[3];
            var message = result_set[4];

            bool isSuccess = status == "A";

            return new Transaction_Result
            {
                ApprovalCode = isSuccess ? auth_code : "",
                ErrorCode = !isSuccess ? auth_code : "",
                ErrorText = !isSuccess ? message : "",
                FullRequest = KeyValueToQueryString(parameters),
                FullResponse = result,
                hasServerError = false,
                isApproved = isSuccess,
                isPending = false,
                ResultCode = avs,
                ResultText = message,
                TransactionIndex = transactionIdentifier

            };
        }

        public Transaction_Result Capture(AuthCapture_Details details)
        {
            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add("auth_id", authID);
            parameters.Add("auth_pass", authPass);
            parameters.Add("cust_name", "");
            parameters.Add("cust_address", "");
            parameters.Add("cust_postcode", "");
            parameters.Add("cust_country", "");
            parameters.Add("cust_ip", "");
            parameters.Add("cust_email", "");
            parameters.Add("cust_tel", "");
            parameters.Add("tran_ref", "");
            parameters.Add("tran_amount", details.Amount.Value.ToString("F2"));
            parameters.Add("tran_currency", details.CurrencyCode);
            parameters.Add("tran_testmode", Mode);
            parameters.Add("tran_type", "sale");
            parameters.Add("tran_class", "cont");
            parameters.Add("tran_orig_id", details.TransactionIndex);

            var result = DoUrlEncodedFormPost("https://secure.cashflows.com/gateway/remote", parameters);

            var result_set = result.Split('|');

            if (result_set.Length != 5)
            {
                throw new Exception("Response format invalid");
            }

            var status = result_set[0];
            var transactionIdentifier = result_set[1];
            var avs = result_set[2];
            var auth_code = result_set[3];
            var message = result_set[4];

            bool isSuccess = status == "A";

            return new Transaction_Result
            {
                ApprovalCode = isSuccess ? auth_code : "",
                ErrorCode = !isSuccess ? auth_code : "",
                ErrorText = !isSuccess ? message : "",
                FullRequest = KeyValueToQueryString(parameters),
                FullResponse = result,
                hasServerError = false,
                isApproved = isSuccess,
                isPending = false,
                ResultCode = avs,
                ResultText = message,
                TransactionIndex = transactionIdentifier

            };
        }

        public Transaction_Result Sale(Sale_Details details)
        {
            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add("auth_id", authID);
            parameters.Add("auth_pass", authPass);
            parameters.Add("card_num", details.CardNumber);
            parameters.Add("card_cvv", details.CardCCV);
            parameters.Add("card_expiry", GatewayUtils.formatExpiryDateYY(details.CardExpiryMonth, details.CardExpiryYear));
            parameters.Add("cust_name", details.CustomerFirstName);
            parameters.Add("cust_address", details.CustomerAddress);
            parameters.Add("cust_postcode", details.CustomerZip);
            parameters.Add("cust_country", details.CustomerCountryCodeTwoLetter);
            parameters.Add("cust_ip", details.IPAddress);
            parameters.Add("cust_email", details.CustomerEmail);
            parameters.Add("cust_tel", details.CustomerPhone);
            parameters.Add("tran_ref", details.ExtRef);
            parameters.Add("tran_amount", details.Amount.ToString("F2"));
            parameters.Add("tran_currency", details.CurrencyCode);
            parameters.Add("tran_testmode", Mode);
            parameters.Add("tran_type", "sale");
            parameters.Add("tran_class", "cont");

            var result = DoUrlEncodedFormPost("https://secure.cashflows.com/gateway/remote", parameters);

            var result_set = result.Split('|');

            if (result_set.Length != 5)
            {
                throw new Exception("Response format invalid");
            }

            var status = result_set[0];
            var transactionIdentifier = result_set[1];
            var avs = result_set[2];
            var auth_code = result_set[3];
            var message = result_set[4];

            bool isSuccess = status == "A";

            return new Transaction_Result
            {
                ApprovalCode = isSuccess ? auth_code : "",
                ErrorCode = !isSuccess ? auth_code : "",
                ErrorText = !isSuccess ? message : "",
                FullRequest = KeyValueToQueryString(parameters),
                FullResponse = result,
                hasServerError = false,
                isApproved = isSuccess,
                isPending = false,
                ResultCode = avs,
                ResultText = message,
                TransactionIndex = transactionIdentifier

            };
        }

        public Transaction_Result Refund(Classes.Payments.Refund_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Credit(Classes.Payments.Credit_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Void(Classes.Payments.Void_Details details)
        {
            throw new NotImplementedException();
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }

        public string DoUrlEncodedFormPost(string url, System.Collections.Specialized.NameValueCollection parameters)
        {
            using (var client = new System.Net.WebClient())
            {
                client.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Encoding = Encoding.UTF8;

                var responsebytes = client.UploadValues(url, "POST", parameters);
                var result = Encoding.UTF8.GetString(responsebytes);

                return result;
            }
        }

        public string KeyValueToQueryString(System.Collections.Specialized.NameValueCollection collection)
        {
            var query_string = "";
            foreach (string key in collection)
            {
                query_string += key + "=" + collection[key] + "&";
            }
            if (query_string.EndsWith("&")) query_string = query_string.Remove(query_string.Length - 1); //remove last ampersand

            return query_string;
        }

        public string KeyValueToXML(System.Collections.Specialized.NameValueCollection collection, string RootName)
        {
            var XML = "<" + RootName + ">";
            foreach (string key in collection)
            {
                var node = "<" + key + ">" + collection[key] + "</" + key + ">";
                XML += node;
            }
            XML += "</" + RootName + ">";

            return XML;
        }
    }
}
