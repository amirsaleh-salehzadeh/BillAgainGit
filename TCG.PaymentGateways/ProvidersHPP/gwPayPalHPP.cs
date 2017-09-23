using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.HPP;

namespace TCG.PaymentGateways.Providers
{
    public class gwPayPalHPP : IHostedPPStrategy
    {

        public string BusinessEmail { get; set; }
        public string PDTIdentityToken { get; set; }
        private string url;
        private string Mode;// 0 => test, 1 => live

        #region Properties
        public GatewayOptions gatewayOptions
        {
            get
            {

                //For currencies and countrues please see https://developer.paypal.com/docs/classic/api/currency_codes/
                return new GatewayOptions
                    (
                        DisplayName: "PayPal HPP",
                        WebUrl: "https://www.paypal.com",
                        Description: "Specialising in online payments.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "BusinessEmail", "PDTIdentityToken" },
                        Currencies: new[] { "AUD", "USD", "THB", "TWD", "CHF", "SEK", "RUB", "SGD", "SEK", "CHF", "TWD", "THB", "USD" },
                        Countries: new[] { "AU", "CA", "FR", "HK", "IT", "JP", "SG", "ES", "GB", "US" },
                        CardTypes: new[]
                        {
                             CardTypeEnum.Unknown
                        }
                    );
            }
        }
        public ProviderType GatewayType { get { return ProviderType.gwPayPalHPP; } }
        #endregion

        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {

            BusinessEmail = MerchantConfigValues.Where(r => r.Key.Equals("BusinessEmail")).FirstOrDefault().Value;
            PDTIdentityToken = MerchantConfigValues.Where(r => r.Key.Equals("PDTIdentityToken")).FirstOrDefault().Value;

            if (!isTestMode)
            {
                Mode = "1";
                url = "https://www.paypal.com/us/cgi-bin/webscr";
            }
            else
            {
                Mode = "0";
                url = "https://www.sandbox.paypal.com/us/cgi-bin/webscr";
            }

        }

        public void LoginTest()
        {
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="BusinessEmail",
                    Value="ropperman-facilitator@thecodegroup.co.za"
                },
                new MerchantConfigValue
                {
                    Key="PDTIdentityToken",
                    Value="qeEqf9k6qyMUJJzw1Gp_MFhfk0fKw0J8QQ62p3AKHKTwLPBCFKivnwrCqRy"
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
            var Url = BuildURL("SaleUrl", details);

            return new HPP_PostInfo
            {
                customerID = details.customerID,
                ExternalUrl = Url
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

            string transaction = "";
            string GatewayToken = "";
            string mc_gross = "";
            string protection_eligibility = "";
            string address_status = "";
            string payer_id = "";
            string address_street = "";
            string payment_date = "";
            string payment_status = "";
            string charset = "";
            string address_zip = "";
            string first_name = "";
            string mc_fee = "";
            string address_country_code = "";
            string address_name = "";
            string notify_version = "";
            string custom = "";
            string payer_status = "";
            string business = "";
            string address_country = "";
            string address_city = "";
            string quantity = "";
            string verify_sign = "";
            string payer_email = "";
            string txn_id = "";
            string payment_type = "";
            string last_name = "";
            string address_state = "";
            string receiver_email = "";
            string payment_fee = "";
            string receiver_id = "";
            string txn_type = "";
            string item_name = "";
            string mc_currency = "";
            string item_number = "";
            string residence_country = "";
            string test_ipn = "";
            string transaction_subject = "";
            string payment_gross = "";
            string ipn_track_id = "";
            string extraInfo = "";
            string fullInfo = "";

            foreach (var entry in PostedValues)
            {
                switch (entry.Key)
                {
                    case ("transaction"):
                        {
                            transaction = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("GatewayToken"):
                        {
                            GatewayToken = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("mc_gross"):
                        {
                            mc_gross = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("protection_eligibility"):
                        {
                            protection_eligibility = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_status"):
                        {
                            address_status = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payer_id"):
                        {
                            payer_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_street"):
                        {
                            address_street = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payment_date"):
                        {
                            payment_date = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payment_status"):
                        {
                            payment_status = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("charset"):
                        {
                            charset = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_zip"):
                        {
                            address_zip = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("first_name"):
                        {
                            first_name = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("mc_fee"):
                        {
                            mc_fee = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_country_code"):
                        {
                            address_country_code = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_name"):
                        {
                            address_name = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("notify_version"):
                        {
                            notify_version = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("custom"):
                        {
                            custom = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payer_status"):
                        {
                            payer_status = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("business"):
                        {
                            business = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_country"):
                        {
                            address_country = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_city"):
                        {
                            address_city = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("quantity"):
                        {
                            quantity = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("verify_sign"):
                        {
                            verify_sign = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payer_email"):
                        {
                            payer_email = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("txn_id"):
                        {
                            txn_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payment_type"):
                        {
                            payment_type = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("last_name"):
                        {
                            last_name = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("address_state"):
                        {
                            address_state = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("receiver_email"):
                        {
                            receiver_email = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payment_fee"):
                        {
                            payment_fee = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("receiver_id"):
                        {
                            receiver_id = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("txn_type"):
                        {
                            txn_type = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("item_name"):
                        {
                            item_name = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("mc_currency"):
                        {
                            mc_currency = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("item_number"):
                        {
                            item_number = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("residence_country"):
                        {
                            residence_country = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("test_ipn"):
                        {
                            test_ipn = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("transaction_subject"):
                        {
                            transaction_subject = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("payment_gross"):
                        {
                            payment_gross = entry.Value;
                            fullInfo += "<" + entry.Key + ">" + entry.Value + "</" + entry.Key + ">";
                        }
                        break;
                    case ("ipn_track_id"):
                        {
                            ipn_track_id = entry.Value;
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


            //Payment status as seen here https://developer.paypal.com/webapps/developer/docs/classic/ipn/integration-guide/IPNandPDTVariables/
            return new HPP_Result
            {
                hasError = payment_status != "Completed",
                isPending = false,
                isSuccessful = payment_status == "Completed",
                //orderID = pf_payment_id,
                reference = custom,
                status = payment_status,
                fullResult = fullInfo
            };
        }

        public Transaction_Result_HPP SaleVerify(HPP_Result details)
        {
            throw new NotImplementedException();
        }

        public string BuildURL(string urlType, HPP_Details details)
        {

            var sb = new StringBuilder();
            sb.Append(url);

            //details.ExtraParams = new NameValueCollection { { "address1", "3315 S Kentucky Ave" }, { "address2", "" }, { "city", "Oklahoma City" }, { "state", "OK" }, { "country", "US" }, { "zip", "73119" } };
            switch (urlType)
            {
                case "SaleUrl":
                    //As seen on https://developer.paypal.com/docs/classic/paypal-payments-standard/integration-guide/Appx_websitestandard_htmlvariables

                    sb.Append("?cmd=_xclick"); //The button that the person clicked was a Buy Now button.
                    sb.Append("&business=" + HttpUtility.UrlEncode(BusinessEmail));
                    sb.Append("&item_name=" + HttpUtility.UrlEncode(details.ItemDescription));
                    sb.Append("&amount=" + HttpUtility.UrlEncode(details.ItemAmount.ToString("F2")));
                    sb.Append("&custom=" + new Guid(details.TransactionReference));
                    sb.Append("&charset=utf-8");
                    sb.Append("&no_note=1"); //For Subscribe buttons, always set no_note to 1
                    sb.Append("&currency_code=" + HttpUtility.UrlEncode(details.CurrencyCode));
                    //sb.Append("&invoice=" + HttpUtility.UrlEncode(details.TransactionReference));
                    sb.Append("&rm=2"); //2 — the buyer's browser is redirected to the return URL by using the POST method, and all payment variables are included
                    sb.Append("&no_shipping=1"); //2 — prompt for an address, and require one
                    sb.Append("&return=" + HttpUtility.UrlEncode(details.SuccessUrl));
                    sb.Append("&cancel_return=" + HttpUtility.UrlEncode(details.CancelUrl));
                    sb.Append("&notify_url=" + HttpUtility.UrlEncode(details.CompleteUrl));

                    if (details.ExtraParams != null)
                    {
                        sb.Append("&address_override=1");
                        sb.Append("&first_name=" + HttpUtility.UrlEncode(details.ExtraParams["first_name"]));
                        sb.Append("&last_name=" + HttpUtility.UrlEncode(details.ExtraParams["last_name"]));
                        sb.Append("&address1=" + HttpUtility.UrlEncode(details.ExtraParams["address1"]));
                        sb.Append("&address2=" + HttpUtility.UrlEncode(details.ExtraParams["address2"]));
                        sb.Append("&city=" + HttpUtility.UrlEncode(details.ExtraParams["city"]));
                        sb.Append("&state=" + HttpUtility.UrlEncode(details.ExtraParams["state"]));
                        sb.Append("&country=" + HttpUtility.UrlEncode(details.ExtraParams["country"]));
                        sb.Append("&zip=" + HttpUtility.UrlEncode(details.ExtraParams["zip"]));
                    }

                    break;
                default:
                    break;
            }

            return sb.ToString();

        }
        #endregion
    }
}
