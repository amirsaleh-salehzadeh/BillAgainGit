using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Providers
{
    public class gwPayFastCC : IPaymentStrategy
    {

        string posturl { get; set; }
        string validateurl { get; set; }
        string apiurl { get; set; }
        bool isTestMode { get; set; }

        string MerchantID = "";
        string MerchantKey = "";
        string Passphrase = "";

        string apiVersion = "v1";

        #region Properties

        public ProviderType GatewayType { get { return Classes.ProviderType.gwPayFastCC; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "PayFast Recurring",
                        WebUrl: "http://www.payfast.co.za/",
                        Description: "The Fastest, Safest Online Payments System. No Setup or Monthly Fees!",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "MerchantID", "MerchantKey" },
                        Currencies: new[] { "ZAR" },
                        Countries: new[] { "ZA", },
                        CardTypes: new[]
                        {
                            CardTypeEnum.VISA,
                            CardTypeEnum.MASTERCARD
                        }
                    );
            }
        }

        #endregion


        #region Methods

        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            MerchantID = MerchantConfigValues.Where(r => r.Key.Equals("MerchantID")).FirstOrDefault().Value;
            MerchantKey = MerchantConfigValues.Where(r => r.Key.Equals("MerchantKey")).FirstOrDefault().Value;
            Passphrase = MerchantConfigValues.Where(r => r.Key.Equals("Passphrase")).FirstOrDefault().Value;

            if (isTestMode)
            {
                posturl = "https://sandbox.payfast.co.za/eng/process";
                validateurl = "https://sandbox.payfast.co.za/eng/query/validate";
                apiurl = "https://api.payfast.co.za/subscriptions/{0}/{1}?testing=true"; //{0} token, {1} action
            }
            else
            {
                posturl = "https://www.payfast.co.za/eng/process";
                validateurl = "https://www.payfast.co.za/eng/query/validate";
                apiurl = "https://api.payfast.co.za/subscriptions/{0}/{1}?testing=false"; //{0} token, {1} action
            }
        }

        public void LoginTest()
        {
            Login(true, new MerchantConfigValue[] { new MerchantConfigValue("MerchantID", "10001123"), new MerchantConfigValue("MerchantKey", "unnokc7xb61cx") });
        }

        #endregion

        public PaymentOptions paymentOptions
        {
            get
            {
                return new PaymentOptions
                    (
                        requires_CVV: false,
                        PaymentTokenize: true,
                        PaymentTokenize_requires_CVV: false,
                        PaymentTokenize_external: true,
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
            var url = string.Format(apiurl, details.CardToken, "cancel");

            //do PUT request to URL - header needs to include merchant_id, version, timestamp, signature - body needs to be blank 

            var ISO8601_timestamp = DateTimeOffset.Now.ToOffset(new TimeSpan(2, 0, 0)).DateTime.ToString("o");

            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            paramDict.Add("merchant_id", MerchantID);
            paramDict.Add("version", apiVersion);
            paramDict.Add("timestamp", ISO8601_timestamp);
            paramDict.Add("passphrase", Passphrase);

            WebRequest purchase_request = WebRequest.Create(url);

            purchase_request.Headers.Add("merchant_id", paramDict["merchant_id"]);
            purchase_request.Headers.Add("version", paramDict["version"]);
            purchase_request.Headers.Add("timestamp", paramDict["timestamp"]);

            var ordered_keys = paramDict.Keys.OrderBy(r => r).ToArray();

            var signature_string = "";

            for (int i = 0; i < ordered_keys.Length; i++)
            {
                if (i != 0)
                {
                    signature_string += "&";
                }

                var key = ordered_keys[i];

                signature_string += key + "=" + paramDict[key];
            }

            var signature_hash = System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(signature_string));
            var signature_hash_string = Convert.ToBase64String(signature_hash);

            purchase_request.Headers.Add("signature", signature_hash_string);

            purchase_request.Method = "PUT";
            purchase_request.ContentType = "application/x-www-form-urlencoded";

            WebResponse response = purchase_request.GetResponse();
            var data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string serverResponse = reader.ReadToEnd();
            reader.Close();

            if (string.IsNullOrWhiteSpace(serverResponse) || serverResponse.Trim().StartsWith("{"))
            {
                return new RevokePaymentMethod_Result
                {
                    isSuccess = false,
                    ErrorMessage = "An unknown error has occurred, return object not in correct format"
                };
            }

            dynamic json_serverResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(serverResponse);

            var code = json_serverResponse.code;
            var status = json_serverResponse.status ?? "";
            var dataobj = json_serverResponse.data ?? null;
            var responseitem = dataobj != null ? dataobj.response : false;
            var messageitem = dataobj != null ? dataobj.message ?? "" : "";

            /*
            
            {
	            "code": 200,  The HTTP status code for the result
	            "status": "success", A more verbose description of the result
	            "data": 
                {
		            "response": true, //approved or not
		            "message": "Success" //error message
	            }
            }

            */

            if (code < 200 || code > 299)
            {
                return new RevokePaymentMethod_Result
                {
                    isSuccess = false,
                    ErrorCode = code.ToString(),
                    ErrorMessage = status
                };
            }

            return new RevokePaymentMethod_Result
            {
                isSuccess = responseitem,
                ErrorCode = code.ToString(),
                ErrorMessage = messageitem
            };

        }

        public void runTests()
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Sale(Sale_Details details)
        {
            var url = string.Format(apiurl, details.ProviderToken, "adhoc");

            //do POST request to URL - header needs to include merchant-id, version, timestamp, signature - body needs to include amount, item_name

            var ISO8601_timestamp = DateTimeOffset.Now.ToOffset(new TimeSpan(2, 0, 0)).DateTime.ToString("yyyy'-'MM'-'dd'T'hh:mm:ss");
            var body_query_string = "";

            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            paramDict.Add("merchant-id", MerchantID);
            //paramDict.Add("merchant_key", MerchantKey);
            paramDict.Add("version", apiVersion);
            paramDict.Add("timestamp", ISO8601_timestamp);
            paramDict.Add("amount", (details.Amount * 100).ToString("F0"));
            paramDict.Add("item_name", details.ProductDescription);
            paramDict.Add("passphrase", Passphrase);

            body_query_string = "amount=" + paramDict["amount"];
            body_query_string += "&item_name="+ paramDict["item_name"];    

            byte[] postData = Encoding.ASCII.GetBytes(body_query_string);
            HttpWebRequest purchase_request = (HttpWebRequest)WebRequest.Create(url);
            purchase_request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            purchase_request.Headers.Add( "merchant-id", paramDict["merchant-id"]);
            //purchase_request.Headers.Add("merchant_key", paramDict["merchant_key"]);
            purchase_request.Headers.Add("version", paramDict["version"]);
            purchase_request.Headers.Add("timestamp", paramDict["timestamp"]);

            var ordered_keys = paramDict.Keys.OrderBy(r => r).ToArray();

            var signature_string = "";

            for (int i=0;i<ordered_keys.Length;i++)
            {
                var key = ordered_keys[i];

                if (i != 0)
                {
                    signature_string += "&";
                }
                
                signature_string += key + "=" + Uri.EscapeDataString(paramDict[key]);
            }

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            var signature_hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(signature_string));
            
            var signature_hash_string = BitConverter.ToString(signature_hash);
            signature_hash_string = signature_hash_string.Replace("-", "").ToLower();

            purchase_request.Headers.Add("signature", signature_hash_string);

            purchase_request.Method = "POST";
            purchase_request.ContentType = "application/x-www-form-urlencoded";
            purchase_request.ContentLength = postData.Length;
            Stream data = purchase_request.GetRequestStream();
            data.Write(postData, 0, postData.Length);
            data.Close();

            WebResponse response=null;

            try
            {
                response = purchase_request.GetResponse();
            }
            catch(WebException e)
            {
                using (WebResponse response2 = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse) response2;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data2 = response2.GetResponseStream())
                    using (var reader2 = new StreamReader(data2))
                    {
                        string text = reader2.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
            }

            
            data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string serverResponse = reader.ReadToEnd();
            reader.Close();

            if (string.IsNullOrWhiteSpace(serverResponse) || !serverResponse.Trim().StartsWith("{"))
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    FullRequest = signature_string,
                    FullResponse = serverResponse,
                    hasServerError = true,
                    ErrorText = "An unknown error has occurred, return object not in correct format"
                };
            }

            dynamic json_serverResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(serverResponse);

            var code = json_serverResponse.code;
            var status = json_serverResponse.status ?? "";
            var dataobj = json_serverResponse.data ?? null;
            var responseitem = dataobj != null ? dataobj.response : false;
            var messageitem = dataobj != null ? dataobj.message ?? "" : "";

            /*
            
            {
	            "code": 200,  The HTTP status code for the result
	            "status": "success", A more verbose description of the result
	            "data": 
                {
		            "response": true, //approved or not
		            "message": "Success" //error message
	            }
            }

            */

            if(code<200 || code > 299)
            {
                return new Transaction_Result
                {
                    isApproved = false,
                    FullRequest = signature_string,
                    FullResponse = serverResponse,
                    hasServerError = true,
                    ErrorCode = code.ToString(),
                    ErrorText = status
                };
            }

            return new Transaction_Result
            {
                isApproved = responseitem,
                ApprovalCode = code.ToString(),
                FullRequest = signature_string,
                FullResponse = serverResponse,
                hasServerError = false,
                ErrorCode = code.ToString(),
                ErrorText = messageitem,
                ProviderToken = details.ProviderToken
            };
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            //generate TokenisePost
            StringBuilder payfastUrl = new StringBuilder(posturl + "?");
            payfastUrl.Append("merchant_id=" + HttpUtility.UrlEncode(MerchantID));
            payfastUrl.Append("&merchant_key=" + HttpUtility.UrlEncode(MerchantKey));
            payfastUrl.Append("&return_url=" + HttpUtility.UrlEncode(details.HPP_Details.SuccessUrl));
            payfastUrl.Append("&cancel_url=" + HttpUtility.UrlEncode(details.HPP_Details.CancelUrl));
            payfastUrl.Append("&notify_url=" + HttpUtility.UrlEncode(details.HPP_Details.CompleteUrl));
            payfastUrl.Append("&m_payment_id=" + HttpUtility.UrlEncode(details.HPP_Details.TransactionReference));
            payfastUrl.Append("&amount=" + HttpUtility.UrlEncode(details.HPP_Details.ItemAmount.ToString("F2")));
            payfastUrl.Append("&item_name=" + HttpUtility.UrlEncode(details.HPP_Details.ItemDescription));
            payfastUrl.Append("&custom_str1=" + details.HPP_Details.CustomerID);
            payfastUrl.Append("&subscription_type=" + "2");

            return new StorePaymentMethod_Result
            {
                TokeniseURL = payfastUrl.ToString()
            };
        }

        public Transaction_Result Verify(Sale_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Void(Void_Details details)
        {
            throw new NotImplementedException();
        }

        public static Classes.HPP.HPP_Result SaleResult(NameValueCollection data) //custom method just for payfast
        {
            //Get Posted Values. Store them in dictionary.
            Dictionary<string, string> PostedValues = new Dictionary<string, string>();
            var Keys = data.Keys;
            foreach (var key in Keys)
            {
                string keyString = key.ToString();
                PostedValues.Add(keyString, data.GetValues(keyString).FirstOrDefault());
            }

            //Read dictionary here to get values for usage
            string m_payment_id = "";
            string pf_payment_id = "";
            string payment_status = "";
            string item_name = "";
            string item_description = "";
            string amount_gross = "";
            string amount_fee = "";
            string amount_net = "";
            string merchant_id = "";
            string signature = "";
            string custID = "";
            string token = "";
            string extraInfo = "";
            string fullInfo = "";

            foreach (var entry in PostedValues)
            {
                switch (entry.Key)
                {
                    case ("m_payment_id"):
                        {
                            m_payment_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("pf_payment_id"):
                        {
                            pf_payment_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payment_status"):
                        {
                            payment_status = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("item_name"):
                        {
                            item_name = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("item_description"):
                        {
                            item_description = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("amount_gross"):
                        {
                            amount_gross = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("amount_fee"):
                        {
                            amount_fee = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("amount_net"):
                        {
                            amount_net = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("merchant_id"):
                        {
                            merchant_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("signature"):
                        {
                            signature = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("custom_str1"): //customer ID
                        {
                            custID = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("token"): //token
                        {
                            token = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    default:
                        {
                            extraInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                }
            }

            fullInfo += "<ExtraInfo>" + extraInfo + "</ExtraInfo>";

            return new Classes.HPP.HPP_Result
            {
                hasError = payment_status != "COMPLETE",
                isPending = false,
                isSuccessful = payment_status == "COMPLETE",
                orderID = pf_payment_id,
                reference = m_payment_id,
                status = payment_status,
                fullResult = fullInfo,
                RecurrenceToken = token
            };
        }
    }
}
