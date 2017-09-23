using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.HPP;

namespace TCG.PaymentGateways.Providers
{
    public class gwMonsterPayHPP : IHostedPPStrategy
    {
        string MerchantIdentifier = "";
        string Username = "";
        string Password = "";

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwMonsterPayHPP; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "MonsterPay",
                        WebUrl: "http://www.monsterpay.co.za/",
                        Description: "MonsterPay allows you to securely accept credit card, debit card, bank transfer and MonsterPay balance. No sign-up or monthly fees.",
                        isActive: false,
                        isLive: false,
                        MerchantConfigValues: new[] { "MerchantID", "Username", "Password" },
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
            MerchantIdentifier = MerchantConfigValues.Where(r => r.Key == "MerchantID").FirstOrDefault().Value;
            Username = MerchantConfigValues.Where(r => r.Key == "Username").FirstOrDefault().Value;
            Password = MerchantConfigValues.Where(r => r.Key == "Password").FirstOrDefault().Value;
        }

        public void LoginTest()
        {
            Login(true, new MerchantConfigValue[] { new MerchantConfigValue { Key = "MerchantID", Value = "testseller20" }, new MerchantConfigValue { Key = "Username", Value = "testseller20@MonsterPay.com" }, new MerchantConfigValue { Key = "Password", Value = "testseller" } });
        }

        public HPP_PostInfo SalePost(HPP_Details details)
        {
            var stringPost = "";
            stringPost += @"<form action=""https://www.monsterpay.com/secure/"" method=""post"" target=""_self"">";
            stringPost += @"<input type=""hidden"" name=""ButtonAction"" value=""buynow"" >";
            stringPost += @"<input type=""hidden"" name=""MerchantIdentifier"" value=" + MerchantIdentifier + ">"; //merchant number
            stringPost += @"<input type=""hidden"" name=""CurrencyAlphaCode"" value=" + details.CurrencyCode + " >";
            stringPost += @"<input type=""hidden"" name=""MerchCustom"" value=" + details.TransactionReference + " >";
            stringPost += @"<input type=""hidden"" name=""LIDSKU"" value=" + details.TransactionReference + ">"; //SKU
            stringPost += @"<input type=""hidden"" name=""LIDDesc"" value=" + details.ItemDescription + ">"; //desc
            stringPost += @"<input type=""hidden"" name=""LIDPrice"" value=" + details.ItemAmount.ToString("F2") + ">";
            stringPost += @"<input type=""hidden"" name=""LIDQty"" value=" + details.ItemQuantity + ">"; //qty       
            stringPost += @"<input type=""hidden"" name=""ShippingRequired"" value=""0"">";
            stringPost += @"<input type=""submit"" value=""Continue"">";
            stringPost += @"</form>";

            return new HPP_PostInfo
            {
                customerID = details.customerID,
                ExternalUrl = "https://www.monsterpay.com/secure/",
                PostData = stringPost
            };
        }

        public HPP_Result SaleResult(NameValueCollection data)
        {
            string orderID = data["tnxid"];
            string checksum = data["checksum"];
            string parity = data["parity"];

            string getUrl = "https://www.monsterpay.com/secure/components/synchro.cfc?wsdl&method=order_synchro&usrname=" + System.Web.HttpUtility.UrlEncode(Username) + "&pwd=" + Password + "&identifier=" + MerchantIdentifier + "&tnxid=" + orderID + "&checksum=" + checksum + "&parity=" + parity;
            
            string resultXml = "";

            // parameters: name1=value1&name2=value2
            WebRequest webRequest = WebRequest.Create(getUrl);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "GET";
            // get the response 
            WebResponse webResponse = webRequest.GetResponse();
            if (webResponse == null)
            { return null; }
            StreamReader sr = new StreamReader
            (webResponse.GetResponseStream());
            resultXml = sr.ReadToEnd().Trim();

            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();

            //check to see if data returned
            if (resultXml != null)
            {
                xDoc.LoadXml(resultXml);

                if (xDoc != null)
                {
                    XElement doc = XElement.Parse(xDoc.InnerText);
                    //get details from xml 
                    var outcome = doc.Element("outcome"); //xDoc.SelectSingleNode("status").InnerXml;
                    var status = outcome.Element("status").Value;
                    bool isApproved = status == "Complete";
                    bool isPending = status == "Pending";
                    var errorCode = outcome.Element("error_code").Value; //xDoc.SelectSingleNode("error_code").InnerText;
                    var errorDescription = outcome.Element("error_desc") == null ? outcome.Element("error_description").Value : outcome.Element("error_desc").Value; //xDoc.SelectSingleNode("error_desc").InnerText;

                    bool hasError = !isPending && !isApproved;

                    if (hasError)
                    {
                        return new HPP_Result { errorReason = errorCode + " - " + errorDescription, hasError = hasError, isPending = isPending, isSuccessful = isApproved };
                    }

                    var order = outcome.Element("order");
                    var gatewayRef = order.Element("id").Value;

                    var seller = doc.Element("seller");
                    var reference = seller.Element("reference").Value;

                    var line_items = doc.Element("line_items");
                    var lid = line_items.Element("lid");
                    var Amount = lid.Element("price").Value;
                    var hash1 = lid.Element("option1value").Value;
                    var hash2 = lid.Element("option2value").Value;

                    //validate 
                    //validatePaymentDetails(validateDetails);
                    return new HPP_Result { errorReason = errorCode + " - " + errorDescription, hasError = hasError, isPending = isPending, isSuccessful = isApproved, orderID = gatewayRef, reference = reference, fullResult=resultXml };
                    //return new Transaction_Result { hasServerError = false, isApproved = isApproved, isPending = isPending, FullRequest = getUrl, FullResponse = resultXml, ErrorCode = errorCode, ErrorText = errorDescription, TransactionIndex = gatewayRef };
                }
                else
                {
                    return new HPP_Result { errorReason = "Validation Error", hasError = true, isPending = false, isSuccessful = false };
                }
            }
            else
            {
                return new HPP_Result { errorReason = "Validation Error", hasError = true, isPending = false, isSuccessful = false };
            }
        }

        public Transaction_Result_HPP SaleVerify(HPP_Result details)
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
