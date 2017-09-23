using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace TCG.PaymentGatewayLibrary.MyGateCollectionsVault
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "pinManagement.cfcSoapBinding", Namespace = "PinManagement")]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(Map))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(QueryBean))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(CFCInvocationException))]
    public partial class PinManagement : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback fUpdatePinBAOperationCompleted;

        private System.Threading.SendOrPostCallback fUpdatePinCCOperationCompleted;

        private System.Threading.SendOrPostCallback fDeletePinBAOperationCompleted;

        private System.Threading.SendOrPostCallback fDeletePinCCOperationCompleted;

        private System.Threading.SendOrPostCallback fLoadPinBAOperationCompleted;

        private System.Threading.SendOrPostCallback fLoadPinCCOperationCompleted;

        private System.Threading.SendOrPostCallback fGetExpiredPinCCOperationCompleted;

        /// <remarks/>
        public PinManagement()
        {
            this.Url = "https://www.mygate.co.za/collections/1x0x0/pinManagement.cfc";
        }

        /// <remarks/>
        public event fUpdatePinBACompletedEventHandler fUpdatePinBACompleted;

        /// <remarks/>
        public event fUpdatePinCCCompletedEventHandler fUpdatePinCCCompleted;

        /// <remarks/>
        public event fDeletePinBACompletedEventHandler fDeletePinBACompleted;

        /// <remarks/>
        public event fDeletePinCCCompletedEventHandler fDeletePinCCCompleted;

        /// <remarks/>
        public event fLoadPinBACompletedEventHandler fLoadPinBACompleted;

        /// <remarks/>
        public event fLoadPinCCCompletedEventHandler fLoadPinCCCompleted;

        /// <remarks/>
        public event fGetExpiredPinCCCompletedEventHandler fGetExpiredPinCCCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "PinManagement", ResponseNamespace = "PinManagement")]
        [return: System.Xml.Serialization.SoapElementAttribute("fUpdatePinBAReturn")]
        public object[] fUpdatePinBA(string ClientID, string ApplicationID, string TransactionIndex, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI)
        {
            object[] results = this.Invoke("fUpdatePinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    AccountNumber,
                    AccountHolder,
                    BranchCode,
                    AccountType,
                    ClientPin,
                    ClientUCI});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfUpdatePinBA(string ClientID, string ApplicationID, string TransactionIndex, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fUpdatePinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    AccountNumber,
                    AccountHolder,
                    BranchCode,
                    AccountType,
                    ClientPin,
                    ClientUCI}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfUpdatePinBA(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fUpdatePinBAAsync(string ClientID, string ApplicationID, string TransactionIndex, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI)
        {
            this.fUpdatePinBAAsync(ClientID, ApplicationID, TransactionIndex, AccountNumber, AccountHolder, BranchCode, AccountType, ClientPin, ClientUCI, null);
        }

        /// <remarks/>
        public void fUpdatePinBAAsync(string ClientID, string ApplicationID, string TransactionIndex, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI, object userState)
        {
            if ((this.fUpdatePinBAOperationCompleted == null))
            {
                this.fUpdatePinBAOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfUpdatePinBAOperationCompleted);
            }
            this.InvokeAsync("fUpdatePinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    AccountNumber,
                    AccountHolder,
                    BranchCode,
                    AccountType,
                    ClientPin,
                    ClientUCI}, this.fUpdatePinBAOperationCompleted, userState);
        }

        private void OnfUpdatePinBAOperationCompleted(object arg)
        {
            if ((this.fUpdatePinBACompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fUpdatePinBACompleted(this, new fUpdatePinBACompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "PinManagement", ResponseNamespace = "PinManagement")]
        [return: System.Xml.Serialization.SoapElementAttribute("fUpdatePinCCReturn")]
        public object[] fUpdatePinCC(string ClientID, string ApplicationID, string TransactionIndex, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI)
        {
            object[] results = this.Invoke("fUpdatePinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    CardNumber,
                    CardHolder,
                    ExpiryMonth,
                    ExpiryYear,
                    CardType,
                    ClientPin,
                    ClientUCI});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfUpdatePinCC(string ClientID, string ApplicationID, string TransactionIndex, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fUpdatePinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    CardNumber,
                    CardHolder,
                    ExpiryMonth,
                    ExpiryYear,
                    CardType,
                    ClientPin,
                    ClientUCI}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfUpdatePinCC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fUpdatePinCCAsync(string ClientID, string ApplicationID, string TransactionIndex, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI)
        {
            this.fUpdatePinCCAsync(ClientID, ApplicationID, TransactionIndex, CardNumber, CardHolder, ExpiryMonth, ExpiryYear, CardType, ClientPin, ClientUCI, null);
        }

        /// <remarks/>
        public void fUpdatePinCCAsync(string ClientID, string ApplicationID, string TransactionIndex, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI, object userState)
        {
            if ((this.fUpdatePinCCOperationCompleted == null))
            {
                this.fUpdatePinCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfUpdatePinCCOperationCompleted);
            }
            this.InvokeAsync("fUpdatePinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    CardNumber,
                    CardHolder,
                    ExpiryMonth,
                    ExpiryYear,
                    CardType,
                    ClientPin,
                    ClientUCI}, this.fUpdatePinCCOperationCompleted, userState);
        }

        private void OnfUpdatePinCCOperationCompleted(object arg)
        {
            if ((this.fUpdatePinCCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fUpdatePinCCCompleted(this, new fUpdatePinCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "PinManagement", ResponseNamespace = "PinManagement")]
        [return: System.Xml.Serialization.SoapElementAttribute("fDeletePinBAReturn")]
        public object[] fDeletePinBA(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI)
        {
            object[] results = this.Invoke("fDeletePinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    ClientUCI});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfDeletePinBA(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fDeletePinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    ClientUCI}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfDeletePinBA(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fDeletePinBAAsync(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI)
        {
            this.fDeletePinBAAsync(ClientID, ApplicationID, TransactionIndex, ClientUCI, null);
        }

        /// <remarks/>
        public void fDeletePinBAAsync(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI, object userState)
        {
            if ((this.fDeletePinBAOperationCompleted == null))
            {
                this.fDeletePinBAOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfDeletePinBAOperationCompleted);
            }
            this.InvokeAsync("fDeletePinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    ClientUCI}, this.fDeletePinBAOperationCompleted, userState);
        }

        private void OnfDeletePinBAOperationCompleted(object arg)
        {
            if ((this.fDeletePinBACompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fDeletePinBACompleted(this, new fDeletePinBACompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "PinManagement", ResponseNamespace = "PinManagement")]
        [return: System.Xml.Serialization.SoapElementAttribute("fDeletePinCCReturn")]
        public object[] fDeletePinCC(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI)
        {
            object[] results = this.Invoke("fDeletePinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    ClientUCI});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfDeletePinCC(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fDeletePinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    ClientUCI}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfDeletePinCC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fDeletePinCCAsync(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI)
        {
            this.fDeletePinCCAsync(ClientID, ApplicationID, TransactionIndex, ClientUCI, null);
        }

        /// <remarks/>
        public void fDeletePinCCAsync(string ClientID, string ApplicationID, string TransactionIndex, string ClientUCI, object userState)
        {
            if ((this.fDeletePinCCOperationCompleted == null))
            {
                this.fDeletePinCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfDeletePinCCOperationCompleted);
            }
            this.InvokeAsync("fDeletePinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    TransactionIndex,
                    ClientUCI}, this.fDeletePinCCOperationCompleted, userState);
        }

        private void OnfDeletePinCCOperationCompleted(object arg)
        {
            if ((this.fDeletePinCCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fDeletePinCCCompleted(this, new fDeletePinCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "PinManagement", ResponseNamespace = "PinManagement")]
        [return: System.Xml.Serialization.SoapElementAttribute("fLoadPinBAReturn")]
        public object[] fLoadPinBA(string ClientID, string ApplicationID, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI)
        {
            object[] results = this.Invoke("fLoadPinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    AccountNumber,
                    AccountHolder,
                    BranchCode,
                    AccountType,
                    ClientPin,
                    ClientUCI});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfLoadPinBA(string ClientID, string ApplicationID, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fLoadPinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    AccountNumber,
                    AccountHolder,
                    BranchCode,
                    AccountType,
                    ClientPin,
                    ClientUCI}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfLoadPinBA(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fLoadPinBAAsync(string ClientID, string ApplicationID, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI)
        {
            this.fLoadPinBAAsync(ClientID, ApplicationID, AccountNumber, AccountHolder, BranchCode, AccountType, ClientPin, ClientUCI, null);
        }

        /// <remarks/>
        public void fLoadPinBAAsync(string ClientID, string ApplicationID, string AccountNumber, string AccountHolder, string BranchCode, string AccountType, string ClientPin, string ClientUCI, object userState)
        {
            if ((this.fLoadPinBAOperationCompleted == null))
            {
                this.fLoadPinBAOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfLoadPinBAOperationCompleted);
            }
            this.InvokeAsync("fLoadPinBA", new object[] {
                    ClientID,
                    ApplicationID,
                    AccountNumber,
                    AccountHolder,
                    BranchCode,
                    AccountType,
                    ClientPin,
                    ClientUCI}, this.fLoadPinBAOperationCompleted, userState);
        }

        private void OnfLoadPinBAOperationCompleted(object arg)
        {
            if ((this.fLoadPinBACompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fLoadPinBACompleted(this, new fLoadPinBACompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "PinManagement", ResponseNamespace = "PinManagement")]
        [return: System.Xml.Serialization.SoapElementAttribute("fLoadPinCCReturn")]
        public object[] fLoadPinCC(string ClientID, string ApplicationID, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI)
        {
            object[] results = this.Invoke("fLoadPinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    CardNumber,
                    CardHolder,
                    ExpiryMonth,
                    ExpiryYear,
                    CardType,
                    ClientPin,
                    ClientUCI});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfLoadPinCC(string ClientID, string ApplicationID, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fLoadPinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    CardNumber,
                    CardHolder,
                    ExpiryMonth,
                    ExpiryYear,
                    CardType,
                    ClientPin,
                    ClientUCI}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfLoadPinCC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fLoadPinCCAsync(string ClientID, string ApplicationID, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI)
        {
            this.fLoadPinCCAsync(ClientID, ApplicationID, CardNumber, CardHolder, ExpiryMonth, ExpiryYear, CardType, ClientPin, ClientUCI, null);
        }

        /// <remarks/>
        public void fLoadPinCCAsync(string ClientID, string ApplicationID, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI, object userState)
        {
            if ((this.fLoadPinCCOperationCompleted == null))
            {
                this.fLoadPinCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfLoadPinCCOperationCompleted);
            }
            this.InvokeAsync("fLoadPinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    CardNumber,
                    CardHolder,
                    ExpiryMonth,
                    ExpiryYear,
                    CardType,
                    ClientPin,
                    ClientUCI}, this.fLoadPinCCOperationCompleted, userState);
        }

        private void OnfLoadPinCCOperationCompleted(object arg)
        {
            if ((this.fLoadPinCCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fLoadPinCCCompleted(this, new fLoadPinCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "PinManagement", ResponseNamespace = "PinManagement")]
        [return: System.Xml.Serialization.SoapElementAttribute("fGetExpiredPinCCReturn")]
        public object[] fGetExpiredPinCC(string ClientID, string ApplicationID, string ExpiryMonth, string ExpiryYear)
        {
            object[] results = this.Invoke("fGetExpiredPinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    ExpiryMonth,
                    ExpiryYear});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfGetExpiredPinCC(string ClientID, string ApplicationID, string ExpiryMonth, string ExpiryYear, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fGetExpiredPinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    ExpiryMonth,
                    ExpiryYear}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfGetExpiredPinCC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fGetExpiredPinCCAsync(string ClientID, string ApplicationID, string ExpiryMonth, string ExpiryYear)
        {
            this.fGetExpiredPinCCAsync(ClientID, ApplicationID, ExpiryMonth, ExpiryYear, null);
        }

        /// <remarks/>
        public void fGetExpiredPinCCAsync(string ClientID, string ApplicationID, string ExpiryMonth, string ExpiryYear, object userState)
        {
            if ((this.fGetExpiredPinCCOperationCompleted == null))
            {
                this.fGetExpiredPinCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfGetExpiredPinCCOperationCompleted);
            }
            this.InvokeAsync("fGetExpiredPinCC", new object[] {
                    ClientID,
                    ApplicationID,
                    ExpiryMonth,
                    ExpiryYear}, this.fGetExpiredPinCCOperationCompleted, userState);
        }

        private void OnfGetExpiredPinCCOperationCompleted(object arg)
        {
            if ((this.fGetExpiredPinCCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fGetExpiredPinCCCompleted(this, new fGetExpiredPinCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://xml.apache.org/xml-soap")]
    public partial class Map
    {

        private mapItem[] itemField;

        /// <remarks/>
        public mapItem[] item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://xml.apache.org/xml-soap")]
    public partial class mapItem
    {

        private object keyField;

        private object valueField;

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public object key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public object value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://rpc.xml.coldfusion")]
    public partial class QueryBean
    {

        private string[] columnListField;

        private object[] dataField;

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string[] columnList
        {
            get
            {
                return this.columnListField;
            }
            set
            {
                this.columnListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public object[] data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://rpc.xml.coldfusion")]
    public partial class CFCInvocationException
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fUpdatePinBACompletedEventHandler(object sender, fUpdatePinBACompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fUpdatePinBACompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fUpdatePinBACompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fUpdatePinCCCompletedEventHandler(object sender, fUpdatePinCCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fUpdatePinCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fUpdatePinCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fDeletePinBACompletedEventHandler(object sender, fDeletePinBACompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fDeletePinBACompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fDeletePinBACompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fDeletePinCCCompletedEventHandler(object sender, fDeletePinCCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fDeletePinCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fDeletePinCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fLoadPinBACompletedEventHandler(object sender, fLoadPinBACompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fLoadPinBACompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fLoadPinBACompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fLoadPinCCCompletedEventHandler(object sender, fLoadPinCCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fLoadPinCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fLoadPinCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void fGetExpiredPinCCCompletedEventHandler(object sender, fGetExpiredPinCCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fGetExpiredPinCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fGetExpiredPinCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }

}
