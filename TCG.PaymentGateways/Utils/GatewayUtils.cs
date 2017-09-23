using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways
{
    public class GatewayUtils_file
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string MIMEType { get; set; }
        public byte[] bytes { get; set; }
    }

    public class RequestHeaders
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public static class GatewayUtils
    {

        #region XML
        internal static string formatExpiryDate(int month, int year)
        {
            if (month < 10)
                return "0" + month + "" + year;
            return month + "" + year;
        }

        internal static string formatExpiryDateYY(int month, int year)
        {
            string yearS = year.ToString().Substring(2);

            if (month < 10)
                return "0" + month + "" + yearS;
            return month + "" + yearS;
        }

        internal static string formatExpiryDateYYmm(int month, int year)
        {
            string yearS = year.ToString().Substring(2);

            if (month < 10)
                return yearS + "" + "0" + month;
            return yearS + "" + month;
        }

        internal static XmlDocument PostXMLTransaction(string v_strURL, String v_objXMLDoc)
        {
            //Declare XMLResponse document
            XmlDocument XMLResponse = null;

            //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebRequest objHttpWebRequest;

            //Declare an HTTP-specific implementation of the WebResponse class
            HttpWebResponse objHttpWebResponse = null;

            //Declare a generic view of a sequence of bytes
            Stream objRequestStream = null;
            Stream objResponseStream = null;

            //Declare XMLReader
            XmlTextReader objXMLReader;

            //Creates an HttpWebRequest for the specified URL.
            objHttpWebRequest = (HttpWebRequest)WebRequest.Create(v_strURL);

            try
            {
                //---------- Start HttpRequest 

                //Set HttpWebRequest properties
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(v_objXMLDoc);
                objHttpWebRequest.Method = "POST";
                objHttpWebRequest.ContentLength = bytes.Length;
                objHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                //Get Stream object 
                objRequestStream = objHttpWebRequest.GetRequestStream();

                //Writes a sequence of bytes to the current stream 
                objRequestStream.Write(bytes, 0, bytes.Length);

                //Close stream
                objRequestStream.Close();

                //---------- End HttpRequest

                //Sends the HttpWebRequest, and waits for a response.
                objHttpWebResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();

                //---------- Start HttpResponse
                if (objHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //Get response stream 
                    objResponseStream = objHttpWebResponse.GetResponseStream();

                    //Load response stream into XMLReader
                    objXMLReader = new XmlTextReader(objResponseStream);

                    //Declare XMLDocument
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(objXMLReader);

                    //Set XMLResponse object returned from XMLReader
                    XMLResponse = xmldoc;

                    //Close XMLReader
                    objXMLReader.Close();
                }

                //Close HttpWebResponse
                objHttpWebResponse.Close();
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
            finally
            {
                //Close connections
                objRequestStream.Close();
                objResponseStream.Close();
                objHttpWebResponse.Close();

                //Release objects
                objXMLReader = null;
                objRequestStream = null;
                objResponseStream = null;
                objHttpWebResponse = null;
                objHttpWebRequest = null;
            }

            //Return
            return XMLResponse;
        }

        internal static string PostMultiPartXMLTransaction(string v_strURL, Dictionary<string, string> qs_params, XmlDocument[] XmlFiles, CookieContainer cookies = null)
        {
            /* Reference http://www.asp.net/web-api/overview/advanced/sending-html-form-data,-part-2 */
            try
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());//needs to be any value that doesnt appear in data, used as a delimiter
                byte[] formDataBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + formDataBoundary + "\r\n");

                var purchase_request = (HttpWebRequest)System.Net.WebRequest.Create(v_strURL);

                if(cookies!=null && cookies.Count>0)
                {
                    purchase_request.CookieContainer = cookies; //used to keep session persistent
                }
                
                purchase_request.Method = "POST";
                purchase_request.ContentType = "multipart/form-data; boundary=" + formDataBoundary;
                purchase_request.KeepAlive = true;
                System.IO.Stream requestStream = purchase_request.GetRequestStream();

                foreach (string item in qs_params.Keys)
                {
                    requestStream.Write(formDataBoundaryBytes, 0, formDataBoundaryBytes.Length);
                    string formitem = string.Format(formdataTemplate, item, qs_params[item]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }
                
                foreach(var file in XmlFiles)
                {
                    requestStream.Write(formDataBoundaryBytes, 0, formDataBoundaryBytes.Length);
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, "xmlfile", "xmlfile.xml", "application/xml");
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                    requestStream.Write(headerbytes, 0, headerbytes.Length);

                    XmlWriter writer = XmlWriter.Create(requestStream, new XmlWriterSettings { Encoding = new System.Text.UTF8Encoding(false)  });
                    file.Save(writer);
                    writer.Flush();
                    writer.Close();
                }                

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + formDataBoundary + "--\r\n");

                requestStream.Write(trailer, 0, trailer.Length);
                requestStream.Close();

                System.Net.WebResponse response = purchase_request.GetResponse();
                requestStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(requestStream);
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

        internal static string PostMultiPartTransaction(string v_strURL, Dictionary<string, string> qs_params, GatewayUtils_file[] files, CookieContainer cookies = null)
        {
            /* Reference http://www.asp.net/web-api/overview/advanced/sending-html-form-data,-part-2 */
            try
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());//needs to be any value that doesnt appear in data, used as a delimiter
                byte[] formDataBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + formDataBoundary + "\r\n");

                var purchase_request = (HttpWebRequest)System.Net.WebRequest.Create(v_strURL);

                if (cookies != null && cookies.Count > 0)
                {
                    purchase_request.CookieContainer = cookies; //used to keep session persistent
                }

                purchase_request.Method = "POST";
                purchase_request.ContentType = "multipart/form-data; boundary=" + formDataBoundary;
                purchase_request.KeepAlive = true;
                System.IO.Stream requestStream = purchase_request.GetRequestStream();

                foreach (string item in qs_params.Keys)
                {
                    requestStream.Write(formDataBoundaryBytes, 0, formDataBoundaryBytes.Length);
                    string formitem = string.Format(formdataTemplate, item, qs_params[item]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }

                foreach (var file in files)
                {
                    requestStream.Write(formDataBoundaryBytes, 0, formDataBoundaryBytes.Length);
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, file.FileName, file.FileName+file.FileExtension, file.MIMEType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                    requestStream.Write(headerbytes, 0, headerbytes.Length);
                    requestStream.Write(file.bytes, 0, headerbytes.Length);
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + formDataBoundary + "--\r\n");

                requestStream.Write(trailer, 0, trailer.Length);
                requestStream.Close();

                System.Net.WebResponse response = purchase_request.GetResponse();
                requestStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(requestStream);
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

        #endregion

        #region JSON

        internal static T doWebRequest<T>(string endpoint, string url, List<RequestHeaders> RequestHeaders, string APIMethodVerb, String JsonObject)
        {
            Stream requestStream = null;
            Stream respStr = null;
            HttpWebResponse resp = null;

            try
            {
                //create request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint + url);

                //create headers
                foreach(var header in RequestHeaders)
                {
                    request.Headers.Add(header.Name, header.Value); //add header for basic authentication
                }
                

                request.Method = APIMethodVerb; //HTTP Verb
                request.ContentType = "application/json";

                //write post data
                if (!String.IsNullOrEmpty(JsonObject)) //If there is nothing to send then we don't need to write to request stream
                {
                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(JsonObject); //byte array of data that we are sending 

                    requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }

                //make request and get response from service
                resp = (HttpWebResponse)request.GetResponse();

                if (resp.ContentLength > 0) //if the response has content
                {
                    //get data from response stream (Should be of type JSON)
                    respStr = resp.GetResponseStream();

                    //parse response from JSON into Object

                    var sr = new StreamReader(respStr);
                    T cust = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());


                    respStr.Close();
                    return cust;
                }
                else
                {
                    if (resp.StatusCode == HttpStatusCode.NoContent || resp.StatusCode == HttpStatusCode.OK) //
                    {
                        object x = true;
                        return (T)(x);
                    }
                    else
                    {
                        object x = false;
                        return (T)(x);
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static string createJsonObject<T>(T obj)
        {
            //Converts object to json
            string json = JsonConvert.SerializeObject(obj);

            return json;
        }

        #endregion

        #region MD5 Hash
        public static string ConvertStringToMD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        
        #endregion
    }
}
