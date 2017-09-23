using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Services;
using System.Collections.Specialized;
using System.Net;

namespace TCG.PaymentGatewayLibrary.PayGatePayBatch
{
    public class PayBatchHelper
    {

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Web.Services.WebServiceBindingAttribute(Name = "PaybatchBinding", Namespace = "urn:paygate.paybatch")]
        public partial class paybatch : System.Web.Services.Protocols.SoapHttpClientProtocol
        {

            private System.Threading.SendOrPostCallback AuthOperationCompleted;

            private System.Threading.SendOrPostCallback ConfirmOperationCompleted;

            private System.Threading.SendOrPostCallback QueryOperationCompleted;

            /// <remarks/>
            public paybatch()
            {
                this.Url = "https://secure.paygate.co.za/paybatch/process.trans";
            }

            public NameValueCollection _customHeaders = new NameValueCollection();

            protected override System.Net.WebRequest GetWebRequest(System.Uri Uri)
            {
                HttpWebRequest req = (HttpWebRequest)base.GetWebRequest(Uri);
                for (int i = 0; i <= _customHeaders.Count - 1; i++)
                {
                    req.Headers.Add(_customHeaders.Keys[i], _customHeaders.GetValues(i).GetValue(0).ToString());
                }
                return req;
            }

            /// <remarks/>
            public event AuthCompletedEventHandler AuthCompleted;

            /// <remarks/>
            public event ConfirmCompletedEventHandler ConfirmCompleted;

            /// <remarks/>
            public event QueryCompletedEventHandler QueryCompleted;

            /// <remarks/>
            [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:paygate.paybatch#Auth", RequestNamespace = "urn:paygate.paybatch", ResponseNamespace = "urn:paygate.paybatch")]
            [return: System.Xml.Serialization.SoapElementAttribute("Return")]
            public BatchReturn Auth(string BatchReference, string NotificationUrl, BatchData BatchData)
            {
                object[] results = this.Invoke("Auth", new object[] {
                    BatchReference,
                    NotificationUrl,
                    BatchData});
                return ((BatchReturn)(results[0]));
            }

            /// <remarks/>
            public System.IAsyncResult BeginAuth(string BatchReference, string NotificationUrl, BatchData BatchData, System.AsyncCallback callback, object asyncState)
            {
                return this.BeginInvoke("Auth", new object[] {
                    BatchReference,
                    NotificationUrl,
                    BatchData}, callback, asyncState);
            }

            /// <remarks/>
            public BatchReturn EndAuth(System.IAsyncResult asyncResult)
            {
                object[] results = this.EndInvoke(asyncResult);
                return ((BatchReturn)(results[0]));
            }

            /// <remarks/>
            public void AuthAsync(string BatchReference, string NotificationUrl, BatchData BatchData)
            {
                this.AuthAsync(BatchReference, NotificationUrl, BatchData, null);
            }

            /// <remarks/>
            public void AuthAsync(string BatchReference, string NotificationUrl, BatchData BatchData, object userState)
            {
                if ((this.AuthOperationCompleted == null))
                {
                    this.AuthOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAuthOperationCompleted);
                }
                this.InvokeAsync("Auth", new object[] {
                    BatchReference,
                    NotificationUrl,
                    BatchData}, this.AuthOperationCompleted, userState);
            }

            private void OnAuthOperationCompleted(object arg)
            {
                if ((this.AuthCompleted != null))
                {
                    System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                    this.AuthCompleted(this, new AuthCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
                }
            }

            /// <remarks/>
            [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:paygate.paybatch#Confirm", RequestNamespace = "urn:paygate.paybatch", ResponseNamespace = "urn:paygate.paybatch")]
            [return: System.Xml.Serialization.SoapElementAttribute("Return")]
            public BatchReturn Confirm(string UploadID)
            {
                object[] results = this.Invoke("Confirm", new object[] {
                    UploadID});
                return ((BatchReturn)(results[0]));
            }

            /// <remarks/>
            public System.IAsyncResult BeginConfirm(string UploadID, System.AsyncCallback callback, object asyncState)
            {
                return this.BeginInvoke("Confirm", new object[] {
                    UploadID}, callback, asyncState);
            }

            /// <remarks/>
            public BatchReturn EndConfirm(System.IAsyncResult asyncResult)
            {
                object[] results = this.EndInvoke(asyncResult);
                return ((BatchReturn)(results[0]));
            }

            /// <remarks/>
            public void ConfirmAsync(string UploadID)
            {
                this.ConfirmAsync(UploadID, null);
            }

            /// <remarks/>
            public void ConfirmAsync(string UploadID, object userState)
            {
                if ((this.ConfirmOperationCompleted == null))
                {
                    this.ConfirmOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConfirmOperationCompleted);
                }
                this.InvokeAsync("Confirm", new object[] {
                    UploadID}, this.ConfirmOperationCompleted, userState);
            }

            private void OnConfirmOperationCompleted(object arg)
            {
                if ((this.ConfirmCompleted != null))
                {
                    System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                    this.ConfirmCompleted(this, new ConfirmCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
                }
            }

            /// <remarks/>
            [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:paygate.paybatch#Query", RequestNamespace = "urn:paygate.paybatch", ResponseNamespace = "urn:paygate.paybatch")]
            [return: System.Xml.Serialization.SoapElementAttribute("Return")]
            public QueryReturn Query(string UploadID)
            {
                object[] results = this.Invoke("Query", new object[] {
                    UploadID});
                return ((QueryReturn)(results[0]));
            }

            /// <remarks/>
            public System.IAsyncResult BeginQuery(string UploadID, System.AsyncCallback callback, object asyncState)
            {
                return this.BeginInvoke("Query", new object[] {
                    UploadID}, callback, asyncState);
            }

            /// <remarks/>
            public QueryReturn EndQuery(System.IAsyncResult asyncResult)
            {
                object[] results = this.EndInvoke(asyncResult);
                return ((QueryReturn)(results[0]));
            }

            /// <remarks/>
            public void QueryAsync(string UploadID)
            {
                this.QueryAsync(UploadID, null);
            }

            /// <remarks/>
            public void QueryAsync(string UploadID, object userState)
            {
                if ((this.QueryOperationCompleted == null))
                {
                    this.QueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnQueryOperationCompleted);
                }
                this.InvokeAsync("Query", new object[] {
                    UploadID}, this.QueryOperationCompleted, userState);
            }

            private void OnQueryOperationCompleted(object arg)
            {
                if ((this.QueryCompleted != null))
                {
                    System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                    this.QueryCompleted(this, new QueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
                }
            }

            /// <remarks/>
            public new void CancelAsync(object userState)
            {
                base.CancelAsync(userState);
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.SoapTypeAttribute(Namespace = "urn:paygate.paybatch")]
        public partial class BatchData
        {

            private string[] batchLineField;

            /// <remarks/>
            public string[] BatchLine
            {
                get
                {
                    return this.batchLineField;
                }
                set
                {
                    this.batchLineField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.SoapTypeAttribute(Namespace = "urn:paygate.paybatch")]
        public partial class QueryReturn
        {

            private string referenceField;

            private string dateUploadedField;

            private string dateCompletedField;

            private int successField;

            private int failField;

            private int unprocessedField;

            private string[] transResultField;

            /// <remarks/>
            public string Reference
            {
                get
                {
                    return this.referenceField;
                }
                set
                {
                    this.referenceField = value;
                }
            }

            /// <remarks/>
            public string DateUploaded
            {
                get
                {
                    return this.dateUploadedField;
                }
                set
                {
                    this.dateUploadedField = value;
                }
            }

            /// <remarks/>
            public string DateCompleted
            {
                get
                {
                    return this.dateCompletedField;
                }
                set
                {
                    this.dateCompletedField = value;
                }
            }

            /// <remarks/>
            public int Success
            {
                get
                {
                    return this.successField;
                }
                set
                {
                    this.successField = value;
                }
            }

            /// <remarks/>
            public int Fail
            {
                get
                {
                    return this.failField;
                }
                set
                {
                    this.failField = value;
                }
            }

            /// <remarks/>
            public int Unprocessed
            {
                get
                {
                    return this.unprocessedField;
                }
                set
                {
                    this.unprocessedField = value;
                }
            }

            /// <remarks/>
            public string[] TransResult
            {
                get
                {
                    return this.transResultField;
                }
                set
                {
                    this.transResultField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.SoapTypeAttribute(Namespace = "urn:paygate.paybatch")]
        public partial class InvalidData
        {

            private int lineField;

            private string reasonField;

            /// <remarks/>
            public int Line
            {
                get
                {
                    return this.lineField;
                }
                set
                {
                    this.lineField = value;
                }
            }

            /// <remarks/>
            public string Reason
            {
                get
                {
                    return this.reasonField;
                }
                set
                {
                    this.reasonField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.SoapTypeAttribute(Namespace = "urn:paygate.paybatch")]
        public partial class BatchReturn
        {

            private int totalField;

            private int validField;

            private int invalidField;

            private InvalidData[] invalidReasonField;

            private string uploadIDField;

            /// <remarks/>
            public int Total
            {
                get
                {
                    return this.totalField;
                }
                set
                {
                    this.totalField = value;
                }
            }

            /// <remarks/>
            public int Valid
            {
                get
                {
                    return this.validField;
                }
                set
                {
                    this.validField = value;
                }
            }

            /// <remarks/>
            public int Invalid
            {
                get
                {
                    return this.invalidField;
                }
                set
                {
                    this.invalidField = value;
                }
            }

            /// <remarks/>
            public InvalidData[] InvalidReason
            {
                get
                {
                    return this.invalidReasonField;
                }
                set
                {
                    this.invalidReasonField = value;
                }
            }

            /// <remarks/>
            public string UploadID
            {
                get
                {
                    return this.uploadIDField;
                }
                set
                {
                    this.uploadIDField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        public delegate void AuthCompletedEventHandler(object sender, AuthCompletedEventArgs e);

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        public partial class AuthCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
        {

            private object[] results;

            internal AuthCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
            {
                this.results = results;
            }

            /// <remarks/>
            public BatchReturn Result
            {
                get
                {
                    this.RaiseExceptionIfNecessary();
                    return ((BatchReturn)(this.results[0]));
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        public delegate void ConfirmCompletedEventHandler(object sender, ConfirmCompletedEventArgs e);

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        public partial class ConfirmCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
        {

            private object[] results;

            internal ConfirmCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
            {
                this.results = results;
            }

            /// <remarks/>
            public BatchReturn Result
            {
                get
                {
                    this.RaiseExceptionIfNecessary();
                    return ((BatchReturn)(this.results[0]));
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        public delegate void QueryCompletedEventHandler(object sender, QueryCompletedEventArgs e);

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        public partial class QueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
        {

            private object[] results;

            internal QueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
            {
                this.results = results;
            }

            /// <remarks/>
            public QueryReturn Result
            {
                get
                {
                    this.RaiseExceptionIfNecessary();
                    return ((QueryReturn)(this.results[0]));
                }
            }
        }
    }
}
