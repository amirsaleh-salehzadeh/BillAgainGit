using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Batch;
using TCG.PaymentGateways.Classes.Extras;

namespace TCG.PaymentGateways.Providers
{
    public class gwThreePeaksDD : IBatchStrategy
    {
        private string DevToken { get; set; }
        private int devID { get; set; }
        private string cref { get; set; }

        private bool isTest { get; set; }
        private static string TestURL
        {
            get
            {
                return "http://demo.threepeaks.co.za/api/index.php";
            }
        }

        private static string LiveURL
        {
            get
            {
                return "https://wp.threepeaks.co.za/api/index.php";
            }
        }

        private CookieContainer cookieJar { get; set; }

        private string URL
        {
            get
            {

                return isTest ? TestURL : LiveURL;
            }
        }

        //public bool BankAccountEnabled { get; set; }
        //public bool CreditCardEnabled { get; set; }

        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwThreePeaksDD; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "Three Peaks Debitsure",
                        WebUrl: "http://www.threepeaks.co.za/debitsure-debit-order-collection.html",
                        Description: "Debitsure is a personalised, secure, and effective debit order collection service.",
                        isActive: true,
                        isLive: true,
                        MerchantConfigValues: new[] { "DevToken", "devID", "cref" },
                        Currencies: new[] { "ZAR" },
                        Countries: new[] { "ZA" },
                        CardTypes: new CardTypeEnum[]
                        {

                        }
                    );
            }
        }
        public BatchOptions batchOptions
        {
            get
            {
                return new BatchOptions
                    (
                        Bank_Batching: true,
                        CreditCard_Batching: false,

                        Bank_Batching_Type: "debit_order",

                        BankPaymentTokenize: false,
                        CCPaymentTokenize: false,
                        CCThreeDSecure: false,

                        require_BankPaymentTokenize: false,
                        require_CCPaymentTokenize: false,

                        is_NotifyPull: true,
                        is_AutoRelease: true,
                        is_AutoRecon: true,

                        Verify: false,
                        Sale: true,
                        Refund: false,
                        UsesExternalIdentifier: true,

                        DailyCutoff: new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 10, 0, 0), new TimeSpan(2, 0, 0)),
                        InvalidSubmitDays: new string[] { "Sunday" }
                    );
            }
        }

        #endregion

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public Batch_Verify_Result Verify(Batch_Verify_Details details)
        {
            throw new NotImplementedException();
        }

        private string CleanUpRef(string MRef)
        {
            var validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (int i = 0; i < MRef.Length; i++)
            {
                if (!validChars.Contains(MRef[i]))
                {
                    MRef = MRef.Replace(MRef[i], '0');
                }
            }

            return MRef;
        }

        private string CleanUpSymbols(string text)
        {
            var validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

            text = string.Concat(text.Where(r => validChars.Contains(r)));

            return text;
        }

        //public string GenerateDebitRef(string ExtRef)
        //{
        //    string Ref = ExtRef;

        //    var total = ExtRef.Length;

        //    if (total < 10)
        //    {
        //        var amountofpadding = 10 - total;
        //        for (int i = 0; i < amountofpadding; i++)
        //        {
        //            Ref += "0";
        //        }
        //    }

        //    if (Ref.Length > 10)
        //    {
        //        Ref = Ref.Substring(0, 10);
        //    }

        //    return Ref;
        //}

        public Batch_Sale_Build_Result Sale_Build(Batch_Sale_Build_Details details)
        {
            foreach (var item in details.Lines)
            {
                if (string.IsNullOrWhiteSpace(item.ExtRef))
                {
                    item.ExtRef = item.LineIdentifier;
                }
            }

            details.ComputeMerchantExtRefs(true, 10, true, true);

            // validate Action dates
            var validate_result = batchOptions.ActionDate_Validator(details.InvalidActionDates).Validate_Date(details.ActionDate);

            if (!validate_result.isValid)
            {
                return new Batch_Sale_Build_Result
                {
                    isBuildSuccess = false,
                    ErrorMessage = validate_result.ErrorText
                };
            }

            // build xml

            string item_xml = "<?xml version='1.0' encoding='UTF-8'?>";
            item_xml += "<newsub>";

            foreach (var item in details.Lines)
            {
                var descrip = item.Memo;
                if (string.IsNullOrEmpty(descrip))
                    if (item.ExtRef != item.MerchantExtRef)
                        descrip = item.ExtRef;

                item_xml += "<debit>";

                item_xml += "<adate>" + details.ActionDate.ToString("ddMMyyyy") + "</adate>";
                item_xml += "<debitref>" + CleanUpRef(item.MerchantExtRef) + "</debitref>";
                item_xml += "<accholder>" + CleanUpSymbols(item.CardBankName) + "</accholder>";
                item_xml += "<description>" + CleanUpSymbols(item.LineIdentifier) + "</description>";
                item_xml += "<bankname>" + CleanUpSymbols(item.Bank) + "</bankname>";
                item_xml += "<bankacc>" + CleanUpSymbols(item.CardBankNumber) + "</bankacc>";
                item_xml += "<bankbranch>" + "" + "</bankbranch>";
                item_xml += "<bankcode>" + CleanUpSymbols(item.BankBranch) + "</bankcode>";
                item_xml += "<bankacctype>0</bankacctype>"; //0 means auto determine
                item_xml += "<amount>" + item.Amount.ToString("F2") + "</amount>";
                item_xml += "<hmessage>" + CleanUpSymbols(item.LineIdentifier) + "</hmessage>";

                item_xml += "</debit>";
            }

            item_xml += "</newsub>";

            Dictionary<string, string> qs_data = new Dictionary<string, string>();

            qs_data.Add("f", "newsub");
            qs_data.Add("p", "1");
            qs_data.Add("cref", cref);
            qs_data.Add("debtorref", details.BatchIdentifier);
            qs_data.Add("adate", details.ActionDate.ToString("ddMMyyyy"));
            qs_data.Add("records", details.Lines.Count.ToString());
            qs_data.Add("amount", details.Lines.Sum(r => r.Amount).ToString("F2"));
            qs_data.Add("xmltype", "1");

            var jdata = new JavaScriptSerializer().Serialize(qs_data);

            return new Batch_Sale_Build_Result
            {
                isBuildSuccess = true,
                RequestParams = jdata,
                RequestFile = item_xml
            };
        }

        public Batch_Sale_Submit_Result Sale_Submit(Batch_Sale_Submit_Details details)
        {

            var RequestParams = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(details.RequestParams);

            var xml = new XmlDocument();
            xml.LoadXml(details.RequestFile);

            var result = GatewayUtils.PostMultiPartXMLTransaction(URL, RequestParams, new XmlDocument[] { xml }, cookieJar);

            var res_xml = new XmlDocument();
            res_xml.LoadXml(result);
            if (res_xml.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                XmlDeclaration xmlDeclaration = (XmlDeclaration)res_xml.FirstChild;
                xmlDeclaration.Encoding = "UTF-16";
            }
            //*[@note]
            string errors = "";
            bool hasAuthError = false;

            XmlNodeList auth_nodes = res_xml.SelectNodes("/API/auth/*[@note]");
            foreach (XmlNode tag in auth_nodes)
            {
                var note = tag.Attributes["note"].Value.ToLower();
                if (note.Contains("invalid") || note.Contains("fail"))
                {
                    hasAuthError = true;
                    errors += note;
                }
            }

            if (hasAuthError)
            {
                return new Batch_Sale_Submit_Result
                {
                    isSubmitSuccess = false,
                    ErrorCode = "AUTH",
                    ErrorMessage = errors
                };
            }

            XmlNodeList nodes = res_xml.SelectNodes("/API/sub/*[@note]");
            foreach (XmlNode tag in nodes)
            {
                var note = tag.Attributes["note"].Value.ToLower();
                if (note.Contains("invalid") || note.Contains("fail"))
                {
                    errors += tag.Name + ": " + note + ",";
                }
            }

            if (errors.Length > 0)
            {
                errors.Remove(errors.Length - 1);
            }

            var adate = res_xml.SelectNodes("/API/sub/adate")[0];
            var xml_node = res_xml.SelectNodes("/API/sub/xml")[0];
            var records = res_xml.SelectNodes("/API/sub/records")[0];
            var amount = res_xml.SelectNodes("/API/sub/amount")[0];
            var subid_S = res_xml.SelectNodes("/API/sub/subid")[0];

            int subid = 0;
            int.TryParse(subid_S.InnerText, out subid);

            if (subid <= 0)
            {
                return new Batch_Sale_Submit_Result
                {
                    RequestXml = details.RequestFile + "<params>" + details.RequestParams + "</params>",
                    ResponseXml = res_xml.InnerXml,
                    isSubmitSuccess = false,
                    ErrorMessage = errors,
                    TransactionIdentifier = subid.ToString()
                };
            }
            else
            {
                Dictionary<string, string> ValidationReqParams = new Dictionary<string, string>();
                ValidationReqParams.Add("f", "subi");
                ValidationReqParams.Add("p", "1");
                ValidationReqParams.Add("cref", cref);
                ValidationReqParams.Add("subid", subid.ToString());

                int i = 0;
                while (i < 3)
                {
                    //check validation status

                    var status = GatewayUtils.PostMultiPartXMLTransaction(URL, ValidationReqParams, new XmlDocument[] { }, cookieJar);
                    var stat_xml = new XmlDocument();
                    stat_xml.LoadXml(status);

                    var apiProcessingNode = stat_xml.SelectNodes("/subs/sub/apiprocessing")[0];
                    var apiProcessing = int.Parse(apiProcessingNode.InnerText);

                    if (apiProcessing < 3)
                    {
                        i++;
                    }
                    else
                    {
                        i = 3; //kill while loop
                        if (apiProcessing == 3)
                        {
                            return new Batch_Sale_Submit_Result
                            {
                                RequestXml = details.RequestFile + "<params>" + details.RequestParams + "</params>",
                                ResponseXml = res_xml.InnerXml,
                                isSubmitSuccess = true,
                                ErrorMessage = subid_S.Attributes["note"].InnerText,
                                TransactionIdentifier = subid.ToString()
                            };
                        }
                        else
                        {
                            return new Batch_Sale_Submit_Result
                            {
                                RequestXml = details.RequestFile + "<params>" + details.RequestParams + "</params>",
                                ResponseXml = res_xml.InnerXml,
                                isSubmitSuccess = false,
                                ErrorMessage = "Failed Validation",
                                TransactionIdentifier = subid.ToString()
                            };
                        }
                    }
                }


                return new Batch_Sale_Submit_Result
                {
                    RequestXml = (details.RequestFile + "<params>" + details.RequestParams + "</params>").Replace("UTF-8", "UTF-16"),
                    ResponseXml = res_xml.InnerXml,
                    isSubmitSuccess = subid > 0,
                    ErrorMessage = "Validation took too long, success was assumed.",
                    TransactionIdentifier = subid.ToString()
                };

            }


        }

        public Batch_Sale_Release_Result Sale_Release(Batch_Sale_Release_Details details)
        {
            throw new NotImplementedException();
        }

        public Batch_Sale_Recon_Result Sale_Recon(Batch_Sale_Recon_Details details)
        {

            var returnBatch = new Batch_Sale_Recon_Result();
            returnBatch.Batches = new List<Batch_Sale_Recon_Result_batch>();

            //string dummySubIDxml = "<subs><sub><subid>183</subid><comref>THECODEGRO</comref><adate>04072015</adate><records>15</records><amount>1932.30</amount><status>1</status><rbr>1234</rbr><rej_records>2</rej_records><rej_amount>310.94</rej_amount>";
            //dummySubIDxml += "<unpiad_records>0</unpiad_records><unpaid_amount>0.00</unpaid_amount><lunpaid_records>0</lunpaid_records><lunpaid_amount>0.00</lunpaid_amount><retamount>0.00</retamount><debitfee>0.00</debitfee>";
            //dummySubIDxml += "<unpaidfee>0.00</unpaidfee><apiprocessing>3</apiprocessing></sub></subs>";

            Dictionary<string, string> ValidationReqParams = new Dictionary<string, string>();
            ValidationReqParams.Add("f", "subi");
            ValidationReqParams.Add("p", "1");
            ValidationReqParams.Add("cref", cref);
            ValidationReqParams.Add("subid", details.TransactionIdentifier);

            var statusCheck = GatewayUtils.PostMultiPartXMLTransaction(URL, ValidationReqParams, new XmlDocument[] { }, cookieJar);
            var stat_xml = new XmlDocument();
            stat_xml.LoadXml(statusCheck);

            var subs = stat_xml.SelectNodes("/subs/sub");

            if (subs.Count == 0)
            {
                return returnBatch;
            }

            var apiProcessingNode = stat_xml.SelectNodes("/subs/sub/apiprocessing")[0];
            var apiProcessing = int.Parse(apiProcessingNode.InnerText);

            var statusNode = stat_xml.SelectNodes("/subs/sub/status")[0];
            var status = int.Parse(statusNode.InnerText);

            if (apiProcessing > 3)
            {

                returnBatch.Batches.Add(new Batch_Sale_Recon_Result_batch
                {
                    BatchIdentifier = details.TransactionIdentifier,
                    isBatchSuccess = false,
                    lines = new List<Batch_Sale_Recon_Result_batch_line>()
                });

                return returnBatch;
            }

            if (status > 3)
            {
                returnBatch.Batches.Add(new Batch_Sale_Recon_Result_batch
                {
                    BatchIdentifier = details.TransactionIdentifier,
                    isBatchSuccess = false,
                    lines = new List<Batch_Sale_Recon_Result_batch_line>()
                });

                return returnBatch;
            }

            if (status > 0)
            {

                //check cdv  0 Not done/Processing 3 Accepted 4 Rejected
                Dictionary<string, string> DebitsValidationReqParams = new Dictionary<string, string>();
                DebitsValidationReqParams.Add("f", "subid");
                DebitsValidationReqParams.Add("p", "1");
                DebitsValidationReqParams.Add("cref", cref);
                DebitsValidationReqParams.Add("subid", details.TransactionIdentifier);

                var debitCall = GatewayUtils.PostMultiPartXMLTransaction(URL, DebitsValidationReqParams, new XmlDocument[] { }, cookieJar);

                var debitXml = new XmlDocument();
                debitXml.LoadXml(debitCall);
                var debits = debitXml.SelectNodes("/sub/debit");


                var batch = new Batch_Sale_Recon_Result_batch
                {
                    BatchIdentifier = details.TransactionIdentifier,
                    isBatchSuccess = true,
                    lines = new List<Batch_Sale_Recon_Result_batch_line>()
                };

                returnBatch.Batches.Add(batch);

                foreach (XmlNode node in debits)
                {

                    var cdv = int.Parse(node.SelectNodes("cdv")[0].InnerText);

                    if(cdv==3)
                    {
                        var debitstatus = int.Parse(node.SelectNodes("debitstatus")[0].InnerText);

                        if(debitstatus == 1 || debitstatus == 3 || debitstatus == 6 || debitstatus == 7)
                        {
                            batch.lines.Add(new Batch_Sale_Recon_Result_batch_line
                            {
                                Amount = decimal.Parse(node.SelectNodes("amount")[0].InnerText),
                                isLineSuccess = false,
                                LineError = node.SelectNodes("hmessage")[0].InnerText,
                                LineIdentifier = node.SelectNodes("description")[0].InnerText
                            });
                        }

                    }

                    if (cdv == 4 || cdv == 99)
                    {
                        batch.lines.Add(new Batch_Sale_Recon_Result_batch_line
                        {
                            Amount = decimal.Parse(node.SelectNodes("amount")[0].InnerText),
                            isLineSuccess = false,
                            LineError = node.SelectNodes("hmessage")[0].InnerText,
                            LineIdentifier = node.SelectNodes("description")[0].InnerText
                        });
                    }

                }

                return returnBatch;
            }

            return returnBatch;
        }

        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            DevToken = MerchantConfigValues.Where(r => r.Key.Equals("DevToken")).FirstOrDefault().Value;
            devID = int.Parse(MerchantConfigValues.Where(r => r.Key.Equals("devID")).FirstOrDefault().Value);
            cref = MerchantConfigValues.Where(r => r.Key.Equals("cref")).FirstOrDefault().Value;
            isTest = isTestMode;

            var qs = "f=auth&p=1&devtoken=" + DevToken + "&devid=" + devID;
            cookieJar = new CookieContainer();

            string result = PostTransaction(URL, qs);
        }

        public void LoginTest()
        {
            isTest = true;
            MerchantConfigValue[] config = new MerchantConfigValue[]
            {
                new MerchantConfigValue
                {
                    Key="DevToken",
                    Value="64c00a5eabb836b0d322da98f5b5b5ce"
                },
                new MerchantConfigValue
                {
                    Key="devID",
                    Value="2"
                },
                new MerchantConfigValue
                {
                    Key="cref",
                    Value="THECODEGRO"
                }
                //new MerchantConfigValue
                //{
                //    Key="BankAccountEnabled",
                //    Value=BankAccountEnabled.ToString()
                //},
                //new MerchantConfigValue
                //{
                //    Key="CreditCardEnabled",
                //    Value=CreditCardEnabled.ToString()
                //}
            };

            Login(true, config);
        }

        public void runTests()
        {
            throw new NotImplementedException();
        }

        private string PostTransaction(string v_strURL, String v_objQSData)
        {
            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(v_objQSData.ToString());
                HttpWebRequest purchase_request = (HttpWebRequest)WebRequest.Create(v_strURL);
                purchase_request.CookieContainer = cookieJar; //used to keep session persistent
                purchase_request.Method = "POST";
                purchase_request.ContentType = "application/x-www-form-urlencoded";
                purchase_request.ContentLength = postData.Length;
                Stream data = purchase_request.GetRequestStream();
                data.Write(postData, 0, postData.Length);
                data.Close();
                WebResponse response = purchase_request.GetResponse();
                data = response.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                string serverResponse = reader.ReadToEnd();
                reader.Close();


                return serverResponse;
            }
            catch (WebException we)
            {
                //TODO: Add custom exception handling
                throw we;
                throw new Exception(we.Message);
            }
            catch (Exception ex)
            {
                throw ex;
                throw new Exception(ex.Message);
            }
        }

        //private string PostXMLMultiPartTransaction(string v_strURL, NameValueCollection qs_params, XmlDocument XmlFile)
        //{
        //    /* Reference http://www.asp.net/web-api/overview/advanced/sending-html-form-data,-part-2 */
        //    try
        //    {
        //        string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        //        string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());//needs to be any value that doesnt appear in data, used as a delimiter
        //        byte[] formDataBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + formDataBoundary + "\r\n");

        //        var purchase_request = (HttpWebRequest)System.Net.WebRequest.Create(v_strURL);
        //        purchase_request.CookieContainer = cookieJar; //used to keep session persistent
        //        purchase_request.Method = "POST";
        //        purchase_request.ContentType = "multipart/form-data; boundary=" + formDataBoundary; 
        //        purchase_request.KeepAlive = true;
        //        System.IO.Stream requestStream = purchase_request.GetRequestStream();

        //        foreach (string item in qs_params.Keys)
        //        {
        //            requestStream.Write(formDataBoundaryBytes, 0, formDataBoundaryBytes.Length);
        //            string formitem = string.Format(formdataTemplate, item, qs_params[item]);
        //            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
        //            requestStream.Write(formitembytes, 0, formitembytes.Length);
        //        }

        //        requestStream.Write(formDataBoundaryBytes, 0, formDataBoundaryBytes.Length);
        //        string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        //        string header = string.Format(headerTemplate, "xmlfile", "xmlfile.xml", "application/xml");
        //        byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

        //        requestStream.Write(headerbytes, 0, headerbytes.Length);

        //        XmlWriter writer = XmlWriter.Create(requestStream, new XmlWriterSettings { Encoding = System.Text.Encoding.UTF8 });
        //        XmlFile.Save(writer);
        //        writer.Flush();
        //        writer.Close();

        //        byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + formDataBoundary + "--\r\n");

        //        requestStream.Write(trailer, 0, trailer.Length);
        //        requestStream.Close();

        //        System.Net.WebResponse response = purchase_request.GetResponse();
        //        requestStream = response.GetResponseStream();
        //        System.IO.StreamReader reader = new System.IO.StreamReader(requestStream);
        //        string serverResponse = reader.ReadToEnd();
        //        reader.Close();

        //        return serverResponse;
        //    }
        //    catch (WebException we)
        //    {
        //        //TODO: Add custom exception handling
        //        throw we;
        //        throw new Exception(we.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        throw new Exception(ex.Message);
        //    }
        //}

        public Batch_InvalidActionDates_Result ActionDate_Validator(List<DateTime> InvalidSubmitDates)
        {
            throw new NotImplementedException();
        }

        public ExternalUser GetUserInfo(string UserIdentifier)
        {
            var qs = "f=getbillagainuser&p=1&userid=" + UserIdentifier;

            string result = PostTransaction(URL, qs);

            StringReader strReader = new StringReader(result);
            XmlSerializer serializer = new XmlSerializer(typeof(ThreePeaksUsers));
            XmlTextReader xmlReader = new XmlTextReader(strReader);
            var obj = (ThreePeaksUsers)serializer.Deserialize(xmlReader);

            if (obj.Count == 0)
            {
                return new ExternalUser { MerchantConfigValues = new MerchantConfigValue[0] };
            }
            else
            {
                var user = obj[0];
                return new ExternalUser
                {
                    ExternalIdentifier = user.bau_id,
                    Ref = user.cpr_ref,
                    CompName = user.cpr_name,
                    CustLastName = user.con_surname,
                    CustName = user.con_name,
                    Tel = user.cpr_tel,
                    Fax = user.cpr_fax,
                    Email = user.con_email,
                    Website = user.cpr_website,
                    Address = (user.cpr_address_1 + Environment.NewLine + user.cpr_address_2 + Environment.NewLine + user.cpr_address_3).Trim(),
                    Zip = user.cpr_address_zip,
                    CountryCodeTwoLetter = user.bau_country,
                    TimeZoneOffset = user.bau_timezone,
                    Currency = user.bau_currency,
                    MerchantConfigValues = new MerchantConfigValue[] {
                        new MerchantConfigValue { Key= "devID", Value=user.dev_id },
                        new MerchantConfigValue { Key= "DevToken", Value=user.dev_devtoken },
                        new MerchantConfigValue { Key= "cref", Value=user.cpr_ref }
                    },
                    Contacts = new List<ExternalUser_Contact> { new ExternalUser_Contact { Name = user.con_name, LastName = user.con_surname, Email = user.con_email } }
                };
            }
        }

        public List<ExternalUser> ListUsers()
        {
            var qs = "f=getlistofbillagainusers&p=1";

            string result = PostTransaction(URL, qs);

            StringReader strReader = new StringReader(result);
            XmlSerializer serializer = new XmlSerializer(typeof(ThreePeaksUsers));
            XmlTextReader xmlReader = new XmlTextReader(strReader);
            var obj = (ThreePeaksUsers)serializer.Deserialize(xmlReader);

            var return_result = new List<ExternalUser>();

            for (int i = 0; i < obj.Count; i++)
            {
                var user = obj[i];
                return_result.Add(new ExternalUser
                {
                    ExternalIdentifier = user.bau_id,
                    Ref = user.cpr_ref,
                    CompName = user.cpr_name,
                    Tel = user.cpr_tel,
                    Fax = user.cpr_fax,
                    Website = user.cpr_website,
                    Address = (user.cpr_address_1 + Environment.NewLine + user.cpr_address_2 + Environment.NewLine + user.cpr_address_3).Trim(),
                    Zip = user.cpr_address_zip,
                    CountryCodeTwoLetter = user.bau_country,
                    TimeZoneOffset = user.bau_timezone,
                    Currency = user.bau_currency,
                    MerchantConfigValues = new MerchantConfigValue[] {
                        new MerchantConfigValue { Key= "devID", Value=user.dev_id },
                        new MerchantConfigValue { Key= "DevToken", Value=user.dev_devtoken },
                        new MerchantConfigValue { Key= "cref", Value=user.cpr_ref }
                    },
                    Contacts = new List<ExternalUser_Contact> { new ExternalUser_Contact { Name = user.con_name, LastName = user.con_surname, Email = user.con_email } }
                });
            }

            return return_result;
        }

        public void ConfirmAcceptanceOfTerms(string UserIdentifier, string cref, bool isAccept)
        {
            var qs = "f=signupbillagainuser&p=1&cref=" + cref + "&userid=" + UserIdentifier + "&signup=" + (isAccept ? "YES" : "NO");

            string result = PostTransaction(URL, qs);
        }      

        public DevInfos GetDevToken(string UserIdentifier)
        {

            var qs = "f=getbillagaindevtoken&p=1&userid=" + UserIdentifier;

            string result = PostTransaction(URL, qs);

            StringReader strReader = new StringReader(result);

            XmlSerializer serializer = new XmlSerializer(typeof(DevInfos));
            XmlTextReader xmlReader = new XmlTextReader(strReader);
            var obj = (DevInfos)serializer.Deserialize(xmlReader);

            var return_result = new DevInfos();

            for (int i = 0; i < obj.Count; i++)
            {
                var Info = obj[i];
                return_result.Add(new DevInfo
                {
                    dev_devtoken = Info.dev_devtoken,
                    dev_id = Info.dev_id
                });
            }

            return return_result;
        }

    }
}
