using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.HPP;

namespace TCG.PaymentGateways.Providers
{
    public class gwSagePayNetCashHPP : IHostedPPStrategy
    {
        string ServiceKey = "";

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwSagePayNetCashHPP; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "SagePay (NetCash) Pay Now",
                        WebUrl: "http://www.netcash.co.za/",
                        Description: "Sage Pay supports businesses with online salary and creditor payments, debit order collections, eCommerce gateway and risk services.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "ServiceKey" },
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
            ServiceKey = MerchantConfigValues.Where(r => r.Key == "ServiceKey").FirstOrDefault().Value;

        }

        public void LoginTest()
        {
            ServiceKey = "fcd2808f-3cbd-4383-a1bd-a184ca70127c";
        }

        public HPP_PostInfo SalePost(HPP_Details details)
        {
            var stringPost = "";
            stringPost += @"<form action=""https://paynow.sagepay.co.za/site/paynow.aspx"" method=""post"" target=""_self"">";

            stringPost += @"<input type=""hidden"" name=""m1"" value=" + ServiceKey + ">"; //merchant number
            stringPost += @"<input type=""hidden"" name=""m2"" value=" + "24ade73c-98cf-47b3-99be-cc7b867b3080" + " >";
            stringPost += @"<input type=""hidden"" name=""p2"" value=" + details.TransactionReference + " >";
            stringPost += @"<input type=""hidden"" name=""p3"" value=" + details.ItemDescription + ">"; //desc
            stringPost += @"<input type=""hidden"" name=""p4"" value=" + details.ItemAmount.ToString("F2") + ">";
            stringPost += @"<input type=""hidden"" name=""Budget"" value=" + "Y" + ">";   
            stringPost += @"<input type=""hidden"" name=""m4"" value=" + (details.customerID <=0 ? details.CustomerID : details.customerID) + ">";
               
            stringPost += @"<input type=""submit"" value=""Continue"">";
            stringPost += @"</form>";

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
