using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.HPP;

namespace TCG.PaymentGateways.Providers
{
    public class gwAliPayHPP : IHostedPPStrategy
    {

        private static string partner; //Partner pID provided by ali
        private static string security_code; //Key provided by ali
        private string seller_email; //Seller email registered by ali
        private string _input_charset = "utf-8";
        private string payment_type = "1";
        private string sign_type = "MD5";
        private string url;
        private string Mode;// 0 => test, 1 => live

        //private string body; //Item Description
        //private string currency; //Sale currency
        //private string mysign
        //{
        //    get
        //    {
        //        if(!string.IsNullOrEmpty(security_code))
        //        {
        //            return GatewayUtils.ConvertStringToMD5Hash(security_code); //MD encoded Key in MD5 used in query
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}
        //Preset items from AliExpress
        //private string service = "create_direct_pay_by_user";
        //private string transport = "POST";
        //private string return_url = "";
        //private string notify_url = "";
        //private string subject; //Item Name
        //private string out_trade_no = new DateTime().ToShortTimeString();
        //private string total_fee; //Item price

        public ProviderType GatewayType { get { return ProviderType.gwAliPayHPP; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "AliPay",
                        WebUrl: "https://intl.alipay.com/",
                        Description: "Specialising in online payments. Asia based gateway.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "partner", "security_code", "seller_email" },
                        Currencies: new[] { "USD", "EUR", "JPY", "GBP", "CAD", "AUD", "SGD", "CHF", "SEK", "DKK", "NOK", "HKD" },
                        Countries: new[] {  "ZA", "MU", "AT", "BE", "CY", "EE", "FI", "FR", "DE", "GR",
                                            "IE", "IT", "LV", "LU", "MT", "NL", "PT", "SK", "SI", "ES" },
                        CardTypes: new[]
                        {
                             CardTypeEnum.Unknown
                        }
                    );
            }
        }

        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            partner = MerchantConfigValues.Where(r => r.Key.Equals("partner")).FirstOrDefault().Value;
            security_code = MerchantConfigValues.Where(r => r.Key.Equals("security_code")).FirstOrDefault().Value;
            seller_email = MerchantConfigValues.Where(r => r.Key.Equals("seller_email")).FirstOrDefault().Value;

            //body = MerchantConfigValues.Where(r => r.Key.Equals("body")).FirstOrDefault()?.Value;
            //currency = MerchantConfigValues.Where(r => r.Key.Equals("currency")).FirstOrDefault().Value;

            //service = MerchantConfigValues.Where(r => r.Key.Equals("currency")).Count() == 0 ? service : MerchantConfigValues.Where(r => r.Key.Equals("service")).FirstOrDefault().Value;


            if (!isTestMode)
            {
                Mode = "1";
                //transport = "https";
                url = "https://mapi.alipay.com/gateway.do?";
            }
            else
            {
                Mode = "0";
                //transport = "http";
                url = "https://openapi.alipaydev.com/gateway.do?";
            }
        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="partner",
                    Value="2088102135220161"
                },
                new MerchantConfigValue
                {
                    Key="security_code",
                    Value="a5erdelradlhg3j9jw4d7nd2d442s0lw"
                },
                new MerchantConfigValue
                {
                    Key="seller_email",
                    Value="sandbox_national@alitest.com"
                }
                //new MerchantConfigValue
                //{
                //    Key="currency",
                //    Value="USD"
                //},
                // new MerchantConfigValue
                //{
                //    Key="total_fee",
                //    Value="0.01"
                //},
                //new MerchantConfigValue
                //{
                //    Key="body",
                //    Value="Test Item Body"
                //},
                
            };

            Login(true, config);
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }

        public HPP_PostInfo SalePost(HPP_Details details)
        {

            var Url = BuildUrl("create_direct_pay_by_user", details);

            return new HPP_PostInfo
            {
                customerID = details.customerID,
                ExternalUrl = Url,
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
            string body = "";
            string buyer_email = "";
            string buyer_id = "";
            string exterface = "";
            string is_success = "";
            string notify_id = "";
            string notify_time = "";
            string notify_type = "";
            string out_trade_no = "";
            string payment_type = "";
            string seller_email = "";
            string seller_id = "";
            string subject = "";
            string total_fee = "";
            string trade_no = "";
            string trade_status = "";
            string sign = "";
            string sign_type = "";

            string extraInfo = "";
            string fullInfo = "";

            foreach (var entry in PostedValues)
            {

                bool isExstra = false;

                switch (entry.Key)
                {
                    case ("body"):
                        {
                            body = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("buyer_email"):
                        {
                            buyer_email = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("buyer_id"):
                        {
                            buyer_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("exterface"):
                        {
                            exterface = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("is_success"):
                        {
                            is_success = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("notify_id"):
                        {
                            notify_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("notify_time"):
                        {
                            notify_time = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("notify_type"):
                        {
                            notify_type = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("out_trade_no"):
                        {
                            out_trade_no = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payment_type"):
                        {
                            payment_type = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("seller_email"): 
                        {
                            seller_email = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("seller_id"): 
                        {
                            seller_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("subject"): 
                        {
                            subject = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("total_fee"): 
                        {
                            total_fee = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("trade_no"): 
                        {
                            trade_no = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("trade_status"): 
                        {
                            trade_status = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("sign"): 
                        {
                            sign = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("sign_type"): 
                        {
                            sign_type = entry.Value;
                            extraInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    default:
                        {                            
                            extraInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                }


                fullInfo += "<ExtraInfo>" + extraInfo + "</ExtraInfo>";
            }

            fullInfo += "<ExtraInfo>" + extraInfo + "</ExtraInfo>";

            return new HPP_Result
            {
                hasError = trade_status != "TRADE_SUCCESS",
                isPending = false,
                isSuccessful = trade_status == "TRADE_SUCCESS",
                //orderID = pf_payment_id,
                reference = out_trade_no,
                status = trade_status,
                fullResult = fullInfo
            };
            
        }

        public Transaction_Result_HPP SaleVerify(HPP_Result details)
        {
            throw new NotImplementedException();
        }

        public string BuildUrl(string urlType, HPP_Details details)
        {
            string URL = "";

            Dictionary<string, string> sPara = new Dictionary<string, string>();

            switch (urlType)
            {
                case "create_direct_pay_by_user":

                    //Add Parameters
                    sPara.Add("_input_charset", _input_charset);
                    sPara.Add("body", details.ItemDescription);
                    sPara.Add("currency", details.CurrencyCode);
                    sPara.Add("error_notify_url", details.FailUrl);
                    sPara.Add("notify_url", details.CompleteUrl);
                    sPara.Add("out_trade_no", details.TransactionReference);
                    sPara.Add("partner", partner);
                    sPara.Add("payment_type", payment_type);
                    sPara.Add("quantity", details.ItemQuantity.ToString());
                    sPara.Add("return_url", details.SuccessUrl);
                    sPara.Add("seller_email", seller_email);
                    sPara.Add("service", urlType);
                    sPara.Add("subject", details.ItemDescription);
                    sPara.Add("total_fee", details.ItemAmount.ToString("F2"));

                    //Create Html
                    StringBuilder sbHtml = new StringBuilder();
                    sbHtml.Append(url);

                    int count = 0;
                    foreach (KeyValuePair<string, string> temp in sPara.OrderBy(R => R.Key))
                    {
                        if (String.IsNullOrEmpty(temp.Value))
                            continue;

                        if (count == 0)
                            sbHtml.Append(temp.Key + "=" + HttpUtility.UrlEncode((temp.Value)));
                        else
                            sbHtml.Append("&" + temp.Key + "=" + HttpUtility.UrlEncode((temp.Value)));

                        count++;
                    }

                    //Sign
                    string prestr = CreateLinkString(sPara);
                    string mysign = Sign(prestr, security_code, sign_type);

                    sbHtml.Append("&sign=" + mysign);
                    sbHtml.Append("&sign_type=" + sign_type);


                    URL = sbHtml.ToString();
                    break;
                default:
                    break;
            }

            return URL;
        }


        //Ali API Sign
        /// <summary>
        /// Signs the specified prestr.
        /// </summary>
        /// <param name="prestr">The prestr.</param>
        /// <param name="key">The key.</param>
        /// <param name="_input_charset">The _input_charset.</param>
        /// <returns></returns>
        /// <author>Rudi Opperman</author>
        /// <datetime>14/12/2016-3:02 PM</datetime>
        private static string Sign(string prestr, string key, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the link string.
        /// </summary>
        /// <param name="dicArray">The dic array.</param>
        /// <returns></returns>
        /// <author>Rudi Opperman</author>
        /// <datetime>14/12/2016-3:03 PM</datetime>
        private static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

    }
}
