using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.HPP;
using System.Web;

namespace TCG.PaymentGateways.Providers
{
    public class gwPayFastHPP : IHostedPPStrategy
    {
        string posturl { get; set; }
        string validateurl { get; set; }

        string MerchantID = "";
        string MerchantKey = "";

        #region Properties
        public ProviderType GatewayType { get { return Classes.ProviderType.gwPayFastHPP; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "PayFast",
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

            if (isTestMode)
            {
                posturl = "https://sandbox.payfast.co.za/eng/process";
                validateurl = "https://sandbox.payfast.co.za/eng/query/validate";
            }
            else
            {
                posturl = "https://www.payfast.co.za/eng/process";
                validateurl = "https://www.payfast.co.za/eng/query/validate";
            }
        }

        public void LoginTest()
        {
            Login(true, new MerchantConfigValue[] { new MerchantConfigValue("MerchantID", "10001123"), new MerchantConfigValue("MerchantKey", "unnokc7xb61cx") });
        }

        public HPP_PostInfo SalePost(HPP_Details details)
        {
            //construct bill again url
            StringBuilder payfastUrl = new StringBuilder(posturl + "?");
            payfastUrl.Append("merchant_id=" + HttpUtility.UrlEncode(MerchantID));
            payfastUrl.Append("&merchant_key=" + HttpUtility.UrlEncode(MerchantKey));
            payfastUrl.Append("&return_url=" + HttpUtility.UrlEncode(details.SuccessUrl));
            payfastUrl.Append("&cancel_url=" + HttpUtility.UrlEncode(details.CancelUrl));
            payfastUrl.Append("&notify_url=" + HttpUtility.UrlEncode(details.CompleteUrl));
            payfastUrl.Append("&m_payment_id=" + HttpUtility.UrlEncode(details.TransactionReference));
            payfastUrl.Append("&amount=" + HttpUtility.UrlEncode(details.ItemAmount.ToString("F2")));
            payfastUrl.Append("&item_name=" + HttpUtility.UrlEncode(details.ItemDescription));
            payfastUrl.Append("&custom_str1=" + details.CustomerID);

            return new HPP_PostInfo
            {
                customerID = details.customerID,
                ExternalUrl = payfastUrl.ToString(),
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
                    default:
                        {
                            extraInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                }
            }

            fullInfo += "<ExtraInfo>" + extraInfo + "</ExtraInfo>";

            return new HPP_Result
            {
                hasError = payment_status != "COMPLETE",
                isPending = false,
                isSuccessful = payment_status == "COMPLETE",
                orderID = pf_payment_id,
                reference = m_payment_id,
                status = payment_status,
                fullResult=fullInfo
            };
        }

        public Transaction_Result_HPP SaleVerify(HPP_Result details) //returns confirmation of receipt data
        {
            throw new NotImplementedException();
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
