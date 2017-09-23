﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.0.30319.17929.
// 

namespace TCG.PaymentGatewayLibrary.MyGateDebitOrder
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "MyGate_DebitOrder_WebService.cfcSoapBinding", Namespace = "http://functions.debitorders.includes.console")]
    public partial class MyGate_DebitOrder_WebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback releaseRefundFileOperationCompleted;

        private System.Threading.SendOrPostCallback uploadRefundFileOperationCompleted;

        private System.Threading.SendOrPostCallback downloadRDFilesOperationCompleted;

        private System.Threading.SendOrPostCallback downloadResponseFiles_NaedoOperationCompleted;

        private System.Threading.SendOrPostCallback uploadDebitFileOperationCompleted;

        private System.Threading.SendOrPostCallback downloadResponseFiles_CCOperationCompleted;

        private System.Threading.SendOrPostCallback releaseDebitFileOperationCompleted;

        private System.Threading.SendOrPostCallback fRetrieveBatchStatusOperationCompleted;

        /// <remarks/>
        public MyGate_DebitOrder_WebServiceService()
        {
            this.Url = "https://console.mygateglobal.com/includes/debitorders/functions/MyGate_DebitOrder" +
                "_WebService.cfc";
        }

        /// <remarks/>
        public event releaseRefundFileCompletedEventHandler releaseRefundFileCompleted;

        /// <remarks/>
        public event uploadRefundFileCompletedEventHandler uploadRefundFileCompleted;

        /// <remarks/>
        public event downloadRDFilesCompletedEventHandler downloadRDFilesCompleted;

        /// <remarks/>
        public event downloadResponseFiles_NaedoCompletedEventHandler downloadResponseFiles_NaedoCompleted;

        /// <remarks/>
        public event uploadDebitFileCompletedEventHandler uploadDebitFileCompleted;

        /// <remarks/>
        public event downloadResponseFiles_CCCompletedEventHandler downloadResponseFiles_CCCompleted;

        /// <remarks/>
        public event releaseDebitFileCompletedEventHandler releaseDebitFileCompleted;

        /// <remarks/>
        public event fRetrieveBatchStatusCompletedEventHandler fRetrieveBatchStatusCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("releaseRefundFileReturn")]
        public string releaseRefundFile(string refundXML)
        {
            object[] results = this.Invoke("releaseRefundFile", new object[] {
                    refundXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginreleaseRefundFile(string refundXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("releaseRefundFile", new object[] {
                    refundXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EndreleaseRefundFile(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void releaseRefundFileAsync(string refundXML)
        {
            this.releaseRefundFileAsync(refundXML, null);
        }

        /// <remarks/>
        public void releaseRefundFileAsync(string refundXML, object userState)
        {
            if ((this.releaseRefundFileOperationCompleted == null))
            {
                this.releaseRefundFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnreleaseRefundFileOperationCompleted);
            }
            this.InvokeAsync("releaseRefundFile", new object[] {
                    refundXML}, this.releaseRefundFileOperationCompleted, userState);
        }

        private void OnreleaseRefundFileOperationCompleted(object arg)
        {
            if ((this.releaseRefundFileCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.releaseRefundFileCompleted(this, new releaseRefundFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("uploadRefundFileReturn")]
        public string uploadRefundFile(string debitOrderXML)
        {
            object[] results = this.Invoke("uploadRefundFile", new object[] {
                    debitOrderXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginuploadRefundFile(string debitOrderXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("uploadRefundFile", new object[] {
                    debitOrderXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EnduploadRefundFile(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void uploadRefundFileAsync(string debitOrderXML)
        {
            this.uploadRefundFileAsync(debitOrderXML, null);
        }

        /// <remarks/>
        public void uploadRefundFileAsync(string debitOrderXML, object userState)
        {
            if ((this.uploadRefundFileOperationCompleted == null))
            {
                this.uploadRefundFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnuploadRefundFileOperationCompleted);
            }
            this.InvokeAsync("uploadRefundFile", new object[] {
                    debitOrderXML}, this.uploadRefundFileOperationCompleted, userState);
        }

        private void OnuploadRefundFileOperationCompleted(object arg)
        {
            if ((this.uploadRefundFileCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.uploadRefundFileCompleted(this, new uploadRefundFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("downloadRDFilesReturn")]
        public string downloadRDFiles(string inputXML)
        {
            object[] results = this.Invoke("downloadRDFiles", new object[] {
                    inputXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindownloadRDFiles(string inputXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("downloadRDFiles", new object[] {
                    inputXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EnddownloadRDFiles(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void downloadRDFilesAsync(string inputXML)
        {
            this.downloadRDFilesAsync(inputXML, null);
        }

        /// <remarks/>
        public void downloadRDFilesAsync(string inputXML, object userState)
        {
            if ((this.downloadRDFilesOperationCompleted == null))
            {
                this.downloadRDFilesOperationCompleted = new System.Threading.SendOrPostCallback(this.OndownloadRDFilesOperationCompleted);
            }
            this.InvokeAsync("downloadRDFiles", new object[] {
                    inputXML}, this.downloadRDFilesOperationCompleted, userState);
        }

        private void OndownloadRDFilesOperationCompleted(object arg)
        {
            if ((this.downloadRDFilesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.downloadRDFilesCompleted(this, new downloadRDFilesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("downloadResponseFiles_NaedoReturn")]
        public string downloadResponseFiles_Naedo(string inputXML)
        {
            object[] results = this.Invoke("downloadResponseFiles_Naedo", new object[] {
                    inputXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindownloadResponseFiles_Naedo(string inputXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("downloadResponseFiles_Naedo", new object[] {
                    inputXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EnddownloadResponseFiles_Naedo(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void downloadResponseFiles_NaedoAsync(string inputXML)
        {
            this.downloadResponseFiles_NaedoAsync(inputXML, null);
        }

        /// <remarks/>
        public void downloadResponseFiles_NaedoAsync(string inputXML, object userState)
        {
            if ((this.downloadResponseFiles_NaedoOperationCompleted == null))
            {
                this.downloadResponseFiles_NaedoOperationCompleted = new System.Threading.SendOrPostCallback(this.OndownloadResponseFiles_NaedoOperationCompleted);
            }
            this.InvokeAsync("downloadResponseFiles_Naedo", new object[] {
                    inputXML}, this.downloadResponseFiles_NaedoOperationCompleted, userState);
        }

        private void OndownloadResponseFiles_NaedoOperationCompleted(object arg)
        {
            if ((this.downloadResponseFiles_NaedoCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.downloadResponseFiles_NaedoCompleted(this, new downloadResponseFiles_NaedoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("uploadDebitFileReturn")]
        public string uploadDebitFile(string debitOrderXML)
        {
            object[] results = this.Invoke("uploadDebitFile", new object[] {
                    debitOrderXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginuploadDebitFile(string debitOrderXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("uploadDebitFile", new object[] {
                    debitOrderXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EnduploadDebitFile(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void uploadDebitFileAsync(string debitOrderXML)
        {
            this.uploadDebitFileAsync(debitOrderXML, null);
        }

        /// <remarks/>
        public void uploadDebitFileAsync(string debitOrderXML, object userState)
        {
            if ((this.uploadDebitFileOperationCompleted == null))
            {
                this.uploadDebitFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnuploadDebitFileOperationCompleted);
            }
            this.InvokeAsync("uploadDebitFile", new object[] {
                    debitOrderXML}, this.uploadDebitFileOperationCompleted, userState);
        }

        private void OnuploadDebitFileOperationCompleted(object arg)
        {
            if ((this.uploadDebitFileCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.uploadDebitFileCompleted(this, new uploadDebitFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("downloadResponseFiles_CCReturn")]
        public string downloadResponseFiles_CC(string inputXML)
        {
            object[] results = this.Invoke("downloadResponseFiles_CC", new object[] {
                    inputXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindownloadResponseFiles_CC(string inputXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("downloadResponseFiles_CC", new object[] {
                    inputXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EnddownloadResponseFiles_CC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void downloadResponseFiles_CCAsync(string inputXML)
        {
            this.downloadResponseFiles_CCAsync(inputXML, null);
        }

        /// <remarks/>
        public void downloadResponseFiles_CCAsync(string inputXML, object userState)
        {
            if ((this.downloadResponseFiles_CCOperationCompleted == null))
            {
                this.downloadResponseFiles_CCOperationCompleted = new System.Threading.SendOrPostCallback(this.OndownloadResponseFiles_CCOperationCompleted);
            }
            this.InvokeAsync("downloadResponseFiles_CC", new object[] {
                    inputXML}, this.downloadResponseFiles_CCOperationCompleted, userState);
        }

        private void OndownloadResponseFiles_CCOperationCompleted(object arg)
        {
            if ((this.downloadResponseFiles_CCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.downloadResponseFiles_CCCompleted(this, new downloadResponseFiles_CCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("releaseDebitFileReturn")]
        public string releaseDebitFile(string releaseXML)
        {
            object[] results = this.Invoke("releaseDebitFile", new object[] {
                    releaseXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginreleaseDebitFile(string releaseXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("releaseDebitFile", new object[] {
                    releaseXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EndreleaseDebitFile(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void releaseDebitFileAsync(string releaseXML)
        {
            this.releaseDebitFileAsync(releaseXML, null);
        }

        /// <remarks/>
        public void releaseDebitFileAsync(string releaseXML, object userState)
        {
            if ((this.releaseDebitFileOperationCompleted == null))
            {
                this.releaseDebitFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnreleaseDebitFileOperationCompleted);
            }
            this.InvokeAsync("releaseDebitFile", new object[] {
                    releaseXML}, this.releaseDebitFileOperationCompleted, userState);
        }

        private void OnreleaseDebitFileOperationCompleted(object arg)
        {
            if ((this.releaseDebitFileCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.releaseDebitFileCompleted(this, new releaseDebitFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://functions.debitorders.includes.console", ResponseNamespace = "http://functions.debitorders.includes.console")]
        [return: System.Xml.Serialization.SoapElementAttribute("fRetrieveBatchStatusReturn")]
        public string fRetrieveBatchStatus(string input)
        {
            object[] results = this.Invoke("fRetrieveBatchStatus", new object[] {
                    input});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfRetrieveBatchStatus(string input, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fRetrieveBatchStatus", new object[] {
                    input}, callback, asyncState);
        }

        /// <remarks/>
        public string EndfRetrieveBatchStatus(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void fRetrieveBatchStatusAsync(string input)
        {
            this.fRetrieveBatchStatusAsync(input, null);
        }

        /// <remarks/>
        public void fRetrieveBatchStatusAsync(string input, object userState)
        {
            if ((this.fRetrieveBatchStatusOperationCompleted == null))
            {
                this.fRetrieveBatchStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfRetrieveBatchStatusOperationCompleted);
            }
            this.InvokeAsync("fRetrieveBatchStatus", new object[] {
                    input}, this.fRetrieveBatchStatusOperationCompleted, userState);
        }

        private void OnfRetrieveBatchStatusOperationCompleted(object arg)
        {
            if ((this.fRetrieveBatchStatusCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fRetrieveBatchStatusCompleted(this, new fRetrieveBatchStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void releaseRefundFileCompletedEventHandler(object sender, releaseRefundFileCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class releaseRefundFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal releaseRefundFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void uploadRefundFileCompletedEventHandler(object sender, uploadRefundFileCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class uploadRefundFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal uploadRefundFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void downloadRDFilesCompletedEventHandler(object sender, downloadRDFilesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class downloadRDFilesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal downloadRDFilesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void downloadResponseFiles_NaedoCompletedEventHandler(object sender, downloadResponseFiles_NaedoCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class downloadResponseFiles_NaedoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal downloadResponseFiles_NaedoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void uploadDebitFileCompletedEventHandler(object sender, uploadDebitFileCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class uploadDebitFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal uploadDebitFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void downloadResponseFiles_CCCompletedEventHandler(object sender, downloadResponseFiles_CCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class downloadResponseFiles_CCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal downloadResponseFiles_CCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void releaseDebitFileCompletedEventHandler(object sender, releaseDebitFileCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class releaseDebitFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal releaseDebitFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fRetrieveBatchStatusCompletedEventHandler(object sender, fRetrieveBatchStatusCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fRetrieveBatchStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fRetrieveBatchStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

