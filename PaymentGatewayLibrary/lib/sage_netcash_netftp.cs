using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace TCG.PaymentGatewayLibrary.SagePayNetCashNetFTP
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ServiceSoap", Namespace = "http://www.netcash.co.za/netserv/ncUpload")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback Request_UploadReportOperationCompleted;

        private System.Threading.SendOrPostCallback Request_StatementByTransactionTypeOperationCompleted;

        private System.Threading.SendOrPostCallback UploadBatchFileOperationCompleted;

        private System.Threading.SendOrPostCallback Request_MerchantStatementOperationCompleted;

        private System.Threading.SendOrPostCallback Request_Debit_Batch_UnauthorizedOperationCompleted;

        private System.Threading.SendOrPostCallback Request_Credit_Batch_UnauthorizedOperationCompleted;

        private System.Threading.SendOrPostCallback UploadBatchFile_CompactOperationCompleted;

        private System.Threading.SendOrPostCallback Request_ActionDateOperationCompleted;

        /// <remarks/>
        public Service()
        {
            this.Url = "https://www.sagepay.co.za/ws/wsUpload/v2.0/Service.asmx";
        }

        /// <remarks/>
        public event Request_UploadReportCompletedEventHandler Request_UploadReportCompleted;

        /// <remarks/>
        public event Request_StatementByTransactionTypeCompletedEventHandler Request_StatementByTransactionTypeCompleted;

        /// <remarks/>
        public event UploadBatchFileCompletedEventHandler UploadBatchFileCompleted;

        /// <remarks/>
        public event Request_MerchantStatementCompletedEventHandler Request_MerchantStatementCompleted;

        /// <remarks/>
        public event Request_Debit_Batch_UnauthorizedCompletedEventHandler Request_Debit_Batch_UnauthorizedCompleted;

        /// <remarks/>
        public event Request_Credit_Batch_UnauthorizedCompletedEventHandler Request_Credit_Batch_UnauthorizedCompleted;

        /// <remarks/>
        public event UploadBatchFile_CompactCompletedEventHandler UploadBatchFile_CompactCompleted;

        /// <remarks/>
        public event Request_ActionDateCompletedEventHandler Request_ActionDateCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/Request_UploadReport", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Request_UploadReport(string Username, string Password, string PIN)
        {
            object[] results = this.Invoke("Request_UploadReport", new object[] {
                    Username,
                    Password,
                    PIN});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRequest_UploadReport(string Username, string Password, string PIN, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Request_UploadReport", new object[] {
                    Username,
                    Password,
                    PIN}, callback, asyncState);
        }

        /// <remarks/>
        public string EndRequest_UploadReport(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void Request_UploadReportAsync(string Username, string Password, string PIN)
        {
            this.Request_UploadReportAsync(Username, Password, PIN, null);
        }

        /// <remarks/>
        public void Request_UploadReportAsync(string Username, string Password, string PIN, object userState)
        {
            if ((this.Request_UploadReportOperationCompleted == null))
            {
                this.Request_UploadReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequest_UploadReportOperationCompleted);
            }
            this.InvokeAsync("Request_UploadReport", new object[] {
                    Username,
                    Password,
                    PIN}, this.Request_UploadReportOperationCompleted, userState);
        }

        private void OnRequest_UploadReportOperationCompleted(object arg)
        {
            if ((this.Request_UploadReportCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Request_UploadReportCompleted(this, new Request_UploadReportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/Request_StatementByTransactionType", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Request_StatementByTransactionType(string Username, string Password, string PIN, string TransactionType)
        {
            object[] results = this.Invoke("Request_StatementByTransactionType", new object[] {
                    Username,
                    Password,
                    PIN,
                    TransactionType});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRequest_StatementByTransactionType(string Username, string Password, string PIN, string TransactionType, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Request_StatementByTransactionType", new object[] {
                    Username,
                    Password,
                    PIN,
                    TransactionType}, callback, asyncState);
        }

        /// <remarks/>
        public string EndRequest_StatementByTransactionType(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void Request_StatementByTransactionTypeAsync(string Username, string Password, string PIN, string TransactionType)
        {
            this.Request_StatementByTransactionTypeAsync(Username, Password, PIN, TransactionType, null);
        }

        /// <remarks/>
        public void Request_StatementByTransactionTypeAsync(string Username, string Password, string PIN, string TransactionType, object userState)
        {
            if ((this.Request_StatementByTransactionTypeOperationCompleted == null))
            {
                this.Request_StatementByTransactionTypeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequest_StatementByTransactionTypeOperationCompleted);
            }
            this.InvokeAsync("Request_StatementByTransactionType", new object[] {
                    Username,
                    Password,
                    PIN,
                    TransactionType}, this.Request_StatementByTransactionTypeOperationCompleted, userState);
        }

        private void OnRequest_StatementByTransactionTypeOperationCompleted(object arg)
        {
            if ((this.Request_StatementByTransactionTypeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Request_StatementByTransactionTypeCompleted(this, new Request_StatementByTransactionTypeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/UploadBatchFile", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadBatchFile(string Username, string Password, string PIN, string FileContents)
        {
            object[] results = this.Invoke("UploadBatchFile", new object[] {
                    Username,
                    Password,
                    PIN,
                    FileContents});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginUploadBatchFile(string Username, string Password, string PIN, string FileContents, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("UploadBatchFile", new object[] {
                    Username,
                    Password,
                    PIN,
                    FileContents}, callback, asyncState);
        }

        /// <remarks/>
        public string EndUploadBatchFile(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void UploadBatchFileAsync(string Username, string Password, string PIN, string FileContents)
        {
            this.UploadBatchFileAsync(Username, Password, PIN, FileContents, null);
        }

        /// <remarks/>
        public void UploadBatchFileAsync(string Username, string Password, string PIN, string FileContents, object userState)
        {
            if ((this.UploadBatchFileOperationCompleted == null))
            {
                this.UploadBatchFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadBatchFileOperationCompleted);
            }
            this.InvokeAsync("UploadBatchFile", new object[] {
                    Username,
                    Password,
                    PIN,
                    FileContents}, this.UploadBatchFileOperationCompleted, userState);
        }

        private void OnUploadBatchFileOperationCompleted(object arg)
        {
            if ((this.UploadBatchFileCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadBatchFileCompleted(this, new UploadBatchFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/Request_MerchantStatement", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Request_MerchantStatement(string Username, string Password, string PIN, System.DateTime tDate)
        {
            object[] results = this.Invoke("Request_MerchantStatement", new object[] {
                    Username,
                    Password,
                    PIN,
                    tDate});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRequest_MerchantStatement(string Username, string Password, string PIN, System.DateTime tDate, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Request_MerchantStatement", new object[] {
                    Username,
                    Password,
                    PIN,
                    tDate}, callback, asyncState);
        }

        /// <remarks/>
        public string EndRequest_MerchantStatement(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void Request_MerchantStatementAsync(string Username, string Password, string PIN, System.DateTime tDate)
        {
            this.Request_MerchantStatementAsync(Username, Password, PIN, tDate, null);
        }

        /// <remarks/>
        public void Request_MerchantStatementAsync(string Username, string Password, string PIN, System.DateTime tDate, object userState)
        {
            if ((this.Request_MerchantStatementOperationCompleted == null))
            {
                this.Request_MerchantStatementOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequest_MerchantStatementOperationCompleted);
            }
            this.InvokeAsync("Request_MerchantStatement", new object[] {
                    Username,
                    Password,
                    PIN,
                    tDate}, this.Request_MerchantStatementOperationCompleted, userState);
        }

        private void OnRequest_MerchantStatementOperationCompleted(object arg)
        {
            if ((this.Request_MerchantStatementCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Request_MerchantStatementCompleted(this, new Request_MerchantStatementCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/Request_Debit_Batch_Unauthorized", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Request_Debit_Batch_Unauthorized(string Username, string Password, string PIN)
        {
            object[] results = this.Invoke("Request_Debit_Batch_Unauthorized", new object[] {
                    Username,
                    Password,
                    PIN});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRequest_Debit_Batch_Unauthorized(string Username, string Password, string PIN, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Request_Debit_Batch_Unauthorized", new object[] {
                    Username,
                    Password,
                    PIN}, callback, asyncState);
        }

        /// <remarks/>
        public string EndRequest_Debit_Batch_Unauthorized(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void Request_Debit_Batch_UnauthorizedAsync(string Username, string Password, string PIN)
        {
            this.Request_Debit_Batch_UnauthorizedAsync(Username, Password, PIN, null);
        }

        /// <remarks/>
        public void Request_Debit_Batch_UnauthorizedAsync(string Username, string Password, string PIN, object userState)
        {
            if ((this.Request_Debit_Batch_UnauthorizedOperationCompleted == null))
            {
                this.Request_Debit_Batch_UnauthorizedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequest_Debit_Batch_UnauthorizedOperationCompleted);
            }
            this.InvokeAsync("Request_Debit_Batch_Unauthorized", new object[] {
                    Username,
                    Password,
                    PIN}, this.Request_Debit_Batch_UnauthorizedOperationCompleted, userState);
        }

        private void OnRequest_Debit_Batch_UnauthorizedOperationCompleted(object arg)
        {
            if ((this.Request_Debit_Batch_UnauthorizedCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Request_Debit_Batch_UnauthorizedCompleted(this, new Request_Debit_Batch_UnauthorizedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/Request_Credit_Batch_Unauthorized", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Request_Credit_Batch_Unauthorized(string Username, string Password, string PIN)
        {
            object[] results = this.Invoke("Request_Credit_Batch_Unauthorized", new object[] {
                    Username,
                    Password,
                    PIN});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRequest_Credit_Batch_Unauthorized(string Username, string Password, string PIN, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Request_Credit_Batch_Unauthorized", new object[] {
                    Username,
                    Password,
                    PIN}, callback, asyncState);
        }

        /// <remarks/>
        public string EndRequest_Credit_Batch_Unauthorized(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void Request_Credit_Batch_UnauthorizedAsync(string Username, string Password, string PIN)
        {
            this.Request_Credit_Batch_UnauthorizedAsync(Username, Password, PIN, null);
        }

        /// <remarks/>
        public void Request_Credit_Batch_UnauthorizedAsync(string Username, string Password, string PIN, object userState)
        {
            if ((this.Request_Credit_Batch_UnauthorizedOperationCompleted == null))
            {
                this.Request_Credit_Batch_UnauthorizedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequest_Credit_Batch_UnauthorizedOperationCompleted);
            }
            this.InvokeAsync("Request_Credit_Batch_Unauthorized", new object[] {
                    Username,
                    Password,
                    PIN}, this.Request_Credit_Batch_UnauthorizedOperationCompleted, userState);
        }

        private void OnRequest_Credit_Batch_UnauthorizedOperationCompleted(object arg)
        {
            if ((this.Request_Credit_Batch_UnauthorizedCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Request_Credit_Batch_UnauthorizedCompleted(this, new Request_Credit_Batch_UnauthorizedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/UploadBatchFile_Compact", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadBatchFile_Compact(string Username, string Password, string PIN, string FileContents)
        {
            object[] results = this.Invoke("UploadBatchFile_Compact", new object[] {
                    Username,
                    Password,
                    PIN,
                    FileContents});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginUploadBatchFile_Compact(string Username, string Password, string PIN, string FileContents, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("UploadBatchFile_Compact", new object[] {
                    Username,
                    Password,
                    PIN,
                    FileContents}, callback, asyncState);
        }

        /// <remarks/>
        public string EndUploadBatchFile_Compact(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void UploadBatchFile_CompactAsync(string Username, string Password, string PIN, string FileContents)
        {
            this.UploadBatchFile_CompactAsync(Username, Password, PIN, FileContents, null);
        }

        /// <remarks/>
        public void UploadBatchFile_CompactAsync(string Username, string Password, string PIN, string FileContents, object userState)
        {
            if ((this.UploadBatchFile_CompactOperationCompleted == null))
            {
                this.UploadBatchFile_CompactOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadBatchFile_CompactOperationCompleted);
            }
            this.InvokeAsync("UploadBatchFile_Compact", new object[] {
                    Username,
                    Password,
                    PIN,
                    FileContents}, this.UploadBatchFile_CompactOperationCompleted, userState);
        }

        private void OnUploadBatchFile_CompactOperationCompleted(object arg)
        {
            if ((this.UploadBatchFile_CompactCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadBatchFile_CompactCompleted(this, new UploadBatchFile_CompactCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.netcash.co.za/netserv/ncUpload/Request_ActionDate", RequestNamespace = "http://www.netcash.co.za/netserv/ncUpload", ResponseNamespace = "http://www.netcash.co.za/netserv/ncUpload", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Request_ActionDate(string Username, string Password, string PIN, System.DateTime tDate, string BatchInstruction, string ForwardActionDate)
        {
            object[] results = this.Invoke("Request_ActionDate", new object[] {
                    Username,
                    Password,
                    PIN,
                    tDate,
                    BatchInstruction,
                    ForwardActionDate});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRequest_ActionDate(string Username, string Password, string PIN, System.DateTime tDate, string BatchInstruction, string ForwardActionDate, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Request_ActionDate", new object[] {
                    Username,
                    Password,
                    PIN,
                    tDate,
                    BatchInstruction,
                    ForwardActionDate}, callback, asyncState);
        }

        /// <remarks/>
        public string EndRequest_ActionDate(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void Request_ActionDateAsync(string Username, string Password, string PIN, System.DateTime tDate, string BatchInstruction, string ForwardActionDate)
        {
            this.Request_ActionDateAsync(Username, Password, PIN, tDate, BatchInstruction, ForwardActionDate, null);
        }

        /// <remarks/>
        public void Request_ActionDateAsync(string Username, string Password, string PIN, System.DateTime tDate, string BatchInstruction, string ForwardActionDate, object userState)
        {
            if ((this.Request_ActionDateOperationCompleted == null))
            {
                this.Request_ActionDateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequest_ActionDateOperationCompleted);
            }
            this.InvokeAsync("Request_ActionDate", new object[] {
                    Username,
                    Password,
                    PIN,
                    tDate,
                    BatchInstruction,
                    ForwardActionDate}, this.Request_ActionDateOperationCompleted, userState);
        }

        private void OnRequest_ActionDateOperationCompleted(object arg)
        {
            if ((this.Request_ActionDateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Request_ActionDateCompleted(this, new Request_ActionDateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void Request_UploadReportCompletedEventHandler(object sender, Request_UploadReportCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Request_UploadReportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Request_UploadReportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void Request_StatementByTransactionTypeCompletedEventHandler(object sender, Request_StatementByTransactionTypeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Request_StatementByTransactionTypeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Request_StatementByTransactionTypeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void UploadBatchFileCompletedEventHandler(object sender, UploadBatchFileCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadBatchFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal UploadBatchFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void Request_MerchantStatementCompletedEventHandler(object sender, Request_MerchantStatementCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Request_MerchantStatementCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Request_MerchantStatementCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void Request_Debit_Batch_UnauthorizedCompletedEventHandler(object sender, Request_Debit_Batch_UnauthorizedCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Request_Debit_Batch_UnauthorizedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Request_Debit_Batch_UnauthorizedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void Request_Credit_Batch_UnauthorizedCompletedEventHandler(object sender, Request_Credit_Batch_UnauthorizedCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Request_Credit_Batch_UnauthorizedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Request_Credit_Batch_UnauthorizedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void UploadBatchFile_CompactCompletedEventHandler(object sender, UploadBatchFile_CompactCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadBatchFile_CompactCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal UploadBatchFile_CompactCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void Request_ActionDateCompletedEventHandler(object sender, Request_ActionDateCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Request_ActionDateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Request_ActionDateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
