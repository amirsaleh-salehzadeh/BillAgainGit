using System;
using System.Collections;
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
    public class gwTenPayHPP : IHostedPPStrategy
    {

        private static string partner; //Partner pID provided by ali
        private static string key; //Key provided by ali
        private string url;
        private string Mode;// 0 => test, 1 => live

        public ProviderType GatewayType { get { return ProviderType.gwTenPayHPP; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "TenPay",
                        WebUrl: "http://global.tenpay.com/",
                        Description: "WeChat keeps transactions secure and simple. Customers simply scan the QR Code on their mobile phone with WeChat, and they can quickly make a payment to your website. WeChat Payments is convenient and more secure than other forms of online payment.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "partner", "key" },
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
            key = MerchantConfigValues.Where(r => r.Key.Equals("key")).FirstOrDefault().Value;


            if (!isTestMode)
            {
                Mode = "1";
                //transport = "https";
                url = "https://gw.tenpay.com/intl/gateway/pay.htm?";
            }
            else
            {
                Mode = "0";
                //transport = "http";
                url = "https://gw.tenpay.com/intl/gateway/pay.htm?";
            }
        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="partner",
                    Value="1900000109"
                },
                new MerchantConfigValue
                {
                    Key="key",
                    Value="8934e7d15453e97507ef794cf7b0519d"
                }
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
            string trade_state = "";
            string out_trade_no = "";


            string extraInfo = "";
            string fullInfo = "";

            foreach (var entry in PostedValues)
            {

                switch (entry.Key)
                {
                    case ("trade_state"):
                        {
                            trade_state = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;

                    case ("out_trade_no"):
                        {
                            out_trade_no = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
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
                hasError = trade_state == "1",
                isPending = false,
                isSuccessful = trade_state == "0",
                //orderID = pf_payment_id,
                reference = out_trade_no,
                status = trade_state,
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
                    sPara.Add("total_fee", details.ItemAmount.ToString("F0"));
                    sPara.Add("spbill_create_ip", "41.0.80.130");
                    sPara.Add("return_url", details.SuccessUrl);
                    sPara.Add("partner", partner);
                    sPara.Add("out_trade_no", details.TransactionReference);
                    sPara.Add("notify_url", details.CompleteUrl);
                    sPara.Add("attach", details.TransactionReference);
                    sPara.Add("body", details.ItemDescription);
                    sPara.Add("bank_type", "DEFAULT");
                    sPara.Add("sign_type", "MD5");
                    sPara.Add("service_version", "1.0");
                    sPara.Add("input_charset", "GBK");
                    sPara.Add("sign_key_index", "1");

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
                    string mysign = Sign(sPara);

                    sbHtml.Append("&sign=" + mysign);

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
        private static string Sign(Dictionary<string, string> keys)
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(keys.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)keys[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + key);
            string sign = GetMD5(sb.ToString(), "GB2312").ToLower();

            return sign;
        }

        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //´´½¨md5¶ÔÏó
            byte[] inputBye;
            byte[] outputBye;

            //Ê¹ÓÃGB2312±àÂë·½Ê½°Ñ×Ö·û´®×ª»¯Îª×Ö½ÚÊý×é£®
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
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
