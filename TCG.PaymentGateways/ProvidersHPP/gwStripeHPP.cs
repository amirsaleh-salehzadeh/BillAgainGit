using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.HPP;
using System.Web;

namespace TCG.PaymentGateways.Providers
{
    public class gwStripeHPP : IHostedPPStrategy
    {
        string ServiceKey = "";
        // please see https://stripe.com/docs/dashboard#livemode-and-testing to generate an API KEY
        static string SecretKey = "sk_test_BQokikJOvBiI2HlWgH4olfQ2";
        static string PublishableKey = "pk_test_6pRNASCoBOKtIshFeQd4XMUh";
        string JsonStripeConfig = "\"Stripe\": {\"SecretKey\": \"" + SecretKey + "\",\"PublishableKey\": \""
            + PublishableKey +"\"}";
    #region Properties
    public ProviderType GatewayType { get { return ProviderType.gwSagePayNetCashHPP; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "Stripe",
                        WebUrl: "https://stripe.com",
                        Description: "Sage Pay supports businesses with online salary and creditor payments, debit order collections, eCommerce gateway and risk services.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "SecretKey", "PublishableKey" },
                        Currencies: new[] { "ZAR", "AFN", "ALL", "DZD", "AOA", "ARS", "AMD", "AWG", "AUD", "AZN", "BSD",
                            "BDT", "BBD", "BZD", "BMD", "BOB", "BAM", "BWP", "BRL", "GBP", "BND", "BGN" },//110 more items at https://stripe.com/docs/currencies
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
            ServiceKey = MerchantConfigValues.Where(r => r.Key == "ServiceKey").FirstOrDefault().Value;
        }

        public void LoginTest()
        {
            ServiceKey = "fcd2808f-3cbd-4383-a1bd-a184ca70127c";
        }

        public HPP_PostInfo SalePost(HPP_Details details)
        {
            var stringPost = "";
            stringPost += @"<script src='https://checkout.stripe.com/v2/checkout.js' class='stripe-button' data-key='"+PublishableKey+"' ";
            stringPost += "data-locale='auto' ";
            stringPost += "data-zip-code='true' ";
            stringPost += "data-amount='" + details.ItemAmount.ToString()+ "' ";
            stringPost += "data-name='" + details.customerID.ToString()+ "' ";
            stringPost += "data-description='" + details.ItemDescription+ "' ";
            stringPost += "data-image='" + HttpUtility.UrlEncode(details.ExtraParams["image_file_source"]) + " ></script>";

            return new HPP_PostInfo
            {
                customerID = details.customerID,
                PostData = stringPost
            };
        }

        public HPP_Result SaleResult(NameValueCollection data)
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
            bool TransactionAccepted = false;
            string CardHolderIpAddr = "";
            string RequestTrace = "";
            string Reference = "";
            string Reason = "";
            string extraInfo = "";
            string fullInfo = "";

            foreach (var entry in PostedValues)
            {
                switch (entry.Key)
                {
                    case ("TransactionAccepted"):
                        {
                            TransactionAccepted = bool.Parse(entry.Value);
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("CardHolderIpAddr"):
                        {
                            CardHolderIpAddr = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("RequestTrace"):
                        {
                            RequestTrace = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("Reference"):
                        {
                            Reference = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("Reason"):
                        {
                            Reason = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("Amount"):
                        {
                            Reason = entry.Value;
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

            //VERIFY

            var vdata = new WebClient().DownloadString("https://ws.sagepay.co.za/PayNow/TransactionStatus/Check?RequestTrace=" + RequestTrace);
            
            return new HPP_Result
            {
                hasError = !TransactionAccepted,
                errorReason = Reason,
                fullResult = fullInfo,
                isPending = false,
                isSuccessful = TransactionAccepted,
                orderID = RequestTrace,
                reference = Reference,
                status = !TransactionAccepted ? Reason : "Success"
            };
        }

        //order id == request trace
        //re verify transaction
        public Transaction_Result_HPP SaleVerify(HPP_Result details)
        {
            var reqTrace = details.orderID;

            var url = "https://gateway.sagepay.co.za/transactionstatus?RequestTrace=" + reqTrace;

            string resultJson = "";

            // parameters: name1=value1&name2=value2
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "GET";
            // get the response 
            WebResponse webResponse = webRequest.GetResponse();
            if (webResponse == null)
            { return null; }
            StreamReader sr = new StreamReader
            (webResponse.GetResponseStream());
            resultJson = sr.ReadToEnd().Trim();

            var type = new { RequestTrace = "", Amount = -1M, TransactionAccepted = false, Reference = "", Reason = "" };

            var desJson = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, type);

            return new Transaction_Result_HPP { FullRequest = url, FullResponse = resultJson, isApproved = desJson.TransactionAccepted, hasServerError = false, TransactionIndex = desJson.Reference, ResultCode = desJson.RequestTrace, ErrorText = !String.IsNullOrEmpty(desJson.Reason) ? desJson.Reason : "" };
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
