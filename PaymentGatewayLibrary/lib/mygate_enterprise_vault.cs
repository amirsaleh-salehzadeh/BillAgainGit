using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace TCG.PaymentGatewayLibrary.MyGateEnterpriseVault
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "tokenizationService.cfcSoapBinding", Namespace = "tokenizationService")]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(Map))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(QueryBean))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(CFCInvocationException))]
    public partial class tokenizationService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback fUpdateTokenCCOperationCompleted;

        private System.Threading.SendOrPostCallback fGetHashedCardDetailsOperationCompleted;

        private System.Threading.SendOrPostCallback fCreateTokenCCOperationCompleted;

        private System.Threading.SendOrPostCallback fGetMaskedCardDetailsOperationCompleted;

        private System.Threading.SendOrPostCallback fValidateExpiryDateOperationCompleted;

        private System.Threading.SendOrPostCallback fPayNowOperationCompleted;

        private System.Threading.SendOrPostCallback fDeregisterTokenCCOperationCompleted;

        private System.Threading.SendOrPostCallback fGetTokenOperationCompleted;

        /// <remarks/>
        public tokenizationService()
        {
            this.Url = "https://www.mygate.co.za/enterprise/services/tokenizationService.cfc";
        }

        /// <remarks/>
        public event fUpdateTokenCCCompletedEventHandler fUpdateTokenCCCompleted;

        /// <remarks/>
        public event fGetHashedCardDetailsCompletedEventHandler fGetHashedCardDetailsCompleted;

        /// <remarks/>
        public event fCreateTokenCCCompletedEventHandler fCreateTokenCCCompleted;

        /// <remarks/>
        public event fGetMaskedCardDetailsCompletedEventHandler fGetMaskedCardDetailsCompleted;

        /// <remarks/>
        public event fValidateExpiryDateCompletedEventHandler fValidateExpiryDateCompleted;

        /// <remarks/>
        public event fPayNowCompletedEventHandler fPayNowCompleted;

        /// <remarks/>
        public event fDeregisterTokenCCCompletedEventHandler fDeregisterTokenCCCompleted;

        /// <remarks/>
        public event fGetTokenCompletedEventHandler fGetTokenCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fUpdateTokenCCReturn")]
        public object[] fUpdateTokenCC(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear)
        {
            object[] results = this.Invoke("fUpdateTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken,
                    CardHolder,
                    CardNumber,
                    ExpiryMonth,
                    ExpiryYear});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfUpdateTokenCC(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fUpdateTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken,
                    CardHolder,
                    CardNumber,
                    ExpiryMonth,
                    ExpiryYear}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfUpdateTokenCC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fUpdateTokenCCAsync(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear)
        {
            this.fUpdateTokenCCAsync(MerchantUID, ApplicationUID, ClientToken, CardHolder, CardNumber, ExpiryMonth, ExpiryYear, null);
        }

        /// <remarks/>
        public void fUpdateTokenCCAsync(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear, object userState)
        {
            if ((this.fUpdateTokenCCOperationCompleted == null))
            {
                this.fUpdateTokenCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfUpdateTokenCCOperationCompleted);
            }
            this.InvokeAsync("fUpdateTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken,
                    CardHolder,
                    CardNumber,
                    ExpiryMonth,
                    ExpiryYear}, this.fUpdateTokenCCOperationCompleted, userState);
        }

        private void OnfUpdateTokenCCOperationCompleted(object arg)
        {
            if ((this.fUpdateTokenCCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fUpdateTokenCCCompleted(this, new fUpdateTokenCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fGetHashedCardDetailsReturn")]
        public object[] fGetHashedCardDetails(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            object[] results = this.Invoke("fGetHashedCardDetails", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfGetHashedCardDetails(string MerchantUID, string ApplicationUID, string ClientToken, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fGetHashedCardDetails", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfGetHashedCardDetails(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fGetHashedCardDetailsAsync(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            this.fGetHashedCardDetailsAsync(MerchantUID, ApplicationUID, ClientToken, null);
        }

        /// <remarks/>
        public void fGetHashedCardDetailsAsync(string MerchantUID, string ApplicationUID, string ClientToken, object userState)
        {
            if ((this.fGetHashedCardDetailsOperationCompleted == null))
            {
                this.fGetHashedCardDetailsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfGetHashedCardDetailsOperationCompleted);
            }
            this.InvokeAsync("fGetHashedCardDetails", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, this.fGetHashedCardDetailsOperationCompleted, userState);
        }

        private void OnfGetHashedCardDetailsOperationCompleted(object arg)
        {
            if ((this.fGetHashedCardDetailsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fGetHashedCardDetailsCompleted(this, new fGetHashedCardDetailsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fCreateTokenCCReturn")]
        public object[] fCreateTokenCC(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear)
        {
            object[] results = this.Invoke("fCreateTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken,
                    CardHolder,
                    CardNumber,
                    ExpiryMonth,
                    ExpiryYear});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfCreateTokenCC(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fCreateTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken,
                    CardHolder,
                    CardNumber,
                    ExpiryMonth,
                    ExpiryYear}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfCreateTokenCC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fCreateTokenCCAsync(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear)
        {
            this.fCreateTokenCCAsync(MerchantUID, ApplicationUID, ClientToken, CardHolder, CardNumber, ExpiryMonth, ExpiryYear, null);
        }

        /// <remarks/>
        public void fCreateTokenCCAsync(string MerchantUID, string ApplicationUID, string ClientToken, string CardHolder, string CardNumber, string ExpiryMonth, string ExpiryYear, object userState)
        {
            if ((this.fCreateTokenCCOperationCompleted == null))
            {
                this.fCreateTokenCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfCreateTokenCCOperationCompleted);
            }
            this.InvokeAsync("fCreateTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken,
                    CardHolder,
                    CardNumber,
                    ExpiryMonth,
                    ExpiryYear}, this.fCreateTokenCCOperationCompleted, userState);
        }

        private void OnfCreateTokenCCOperationCompleted(object arg)
        {
            if ((this.fCreateTokenCCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fCreateTokenCCCompleted(this, new fCreateTokenCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fGetMaskedCardDetailsReturn")]
        public object[] fGetMaskedCardDetails(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            object[] results = this.Invoke("fGetMaskedCardDetails", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfGetMaskedCardDetails(string MerchantUID, string ApplicationUID, string ClientToken, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fGetMaskedCardDetails", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfGetMaskedCardDetails(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fGetMaskedCardDetailsAsync(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            this.fGetMaskedCardDetailsAsync(MerchantUID, ApplicationUID, ClientToken, null);
        }

        /// <remarks/>
        public void fGetMaskedCardDetailsAsync(string MerchantUID, string ApplicationUID, string ClientToken, object userState)
        {
            if ((this.fGetMaskedCardDetailsOperationCompleted == null))
            {
                this.fGetMaskedCardDetailsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfGetMaskedCardDetailsOperationCompleted);
            }
            this.InvokeAsync("fGetMaskedCardDetails", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, this.fGetMaskedCardDetailsOperationCompleted, userState);
        }

        private void OnfGetMaskedCardDetailsOperationCompleted(object arg)
        {
            if ((this.fGetMaskedCardDetailsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fGetMaskedCardDetailsCompleted(this, new fGetMaskedCardDetailsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fValidateExpiryDateReturn")]
        public object[] fValidateExpiryDate(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            object[] results = this.Invoke("fValidateExpiryDate", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfValidateExpiryDate(string MerchantUID, string ApplicationUID, string ClientToken, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fValidateExpiryDate", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfValidateExpiryDate(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fValidateExpiryDateAsync(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            this.fValidateExpiryDateAsync(MerchantUID, ApplicationUID, ClientToken, null);
        }

        /// <remarks/>
        public void fValidateExpiryDateAsync(string MerchantUID, string ApplicationUID, string ClientToken, object userState)
        {
            if ((this.fValidateExpiryDateOperationCompleted == null))
            {
                this.fValidateExpiryDateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfValidateExpiryDateOperationCompleted);
            }
            this.InvokeAsync("fValidateExpiryDate", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, this.fValidateExpiryDateOperationCompleted, userState);
        }

        private void OnfValidateExpiryDateOperationCompleted(object arg)
        {
            if ((this.fValidateExpiryDateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fValidateExpiryDateCompleted(this, new fValidateExpiryDateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fPayNowReturn")]
        public object[] fPayNow(string MerchantUID, string ApplicationUID, string TransactionIndex, string ClientToken, string CVV, string Amount, string Mode, string MerchantReference, string Budget, string BudgetPeriod, string UCI, string IPAddress, string ShippingCountryCode)
        {
            object[] results = this.Invoke("fPayNow", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    TransactionIndex,
                    ClientToken,
                    CVV,
                    Amount,
                    Mode,
                    MerchantReference,
                    Budget,
                    BudgetPeriod,
                    UCI,
                    IPAddress,
                    ShippingCountryCode});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfPayNow(string MerchantUID, string ApplicationUID, string TransactionIndex, string ClientToken, string CVV, string Amount, string Mode, string MerchantReference, string Budget, string BudgetPeriod, string UCI, string IPAddress, string ShippingCountryCode, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fPayNow", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    TransactionIndex,
                    ClientToken,
                    CVV,
                    Amount,
                    Mode,
                    MerchantReference,
                    Budget,
                    BudgetPeriod,
                    UCI,
                    IPAddress,
                    ShippingCountryCode}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfPayNow(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fPayNowAsync(string MerchantUID, string ApplicationUID, string TransactionIndex, string ClientToken, string CVV, string Amount, string Mode, string MerchantReference, string Budget, string BudgetPeriod, string UCI, string IPAddress, string ShippingCountryCode)
        {
            this.fPayNowAsync(MerchantUID, ApplicationUID, TransactionIndex, ClientToken, CVV, Amount, Mode, MerchantReference, Budget, BudgetPeriod, UCI, IPAddress, ShippingCountryCode, null);
        }

        /// <remarks/>
        public void fPayNowAsync(string MerchantUID, string ApplicationUID, string TransactionIndex, string ClientToken, string CVV, string Amount, string Mode, string MerchantReference, string Budget, string BudgetPeriod, string UCI, string IPAddress, string ShippingCountryCode, object userState)
        {
            if ((this.fPayNowOperationCompleted == null))
            {
                this.fPayNowOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfPayNowOperationCompleted);
            }
            this.InvokeAsync("fPayNow", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    TransactionIndex,
                    ClientToken,
                    CVV,
                    Amount,
                    Mode,
                    MerchantReference,
                    Budget,
                    BudgetPeriod,
                    UCI,
                    IPAddress,
                    ShippingCountryCode}, this.fPayNowOperationCompleted, userState);
        }

        private void OnfPayNowOperationCompleted(object arg)
        {
            if ((this.fPayNowCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fPayNowCompleted(this, new fPayNowCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fDeregisterTokenCCReturn")]
        public object[] fDeregisterTokenCC(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            object[] results = this.Invoke("fDeregisterTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfDeregisterTokenCC(string MerchantUID, string ApplicationUID, string ClientToken, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fDeregisterTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfDeregisterTokenCC(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fDeregisterTokenCCAsync(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            this.fDeregisterTokenCCAsync(MerchantUID, ApplicationUID, ClientToken, null);
        }

        /// <remarks/>
        public void fDeregisterTokenCCAsync(string MerchantUID, string ApplicationUID, string ClientToken, object userState)
        {
            if ((this.fDeregisterTokenCCOperationCompleted == null))
            {
                this.fDeregisterTokenCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfDeregisterTokenCCOperationCompleted);
            }
            this.InvokeAsync("fDeregisterTokenCC", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, this.fDeregisterTokenCCOperationCompleted, userState);
        }

        private void OnfDeregisterTokenCCOperationCompleted(object arg)
        {
            if ((this.fDeregisterTokenCCCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fDeregisterTokenCCCompleted(this, new fDeregisterTokenCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "tokenizationService", ResponseNamespace = "tokenizationService")]
        [return: System.Xml.Serialization.SoapElementAttribute("fGetTokenReturn")]
        public object[] fGetToken(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            object[] results = this.Invoke("fGetToken", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken});
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginfGetToken(string MerchantUID, string ApplicationUID, string ClientToken, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("fGetToken", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, callback, asyncState);
        }

        /// <remarks/>
        public object[] EndfGetToken(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((object[])(results[0]));
        }

        /// <remarks/>
        public void fGetTokenAsync(string MerchantUID, string ApplicationUID, string ClientToken)
        {
            this.fGetTokenAsync(MerchantUID, ApplicationUID, ClientToken, null);
        }

        /// <remarks/>
        public void fGetTokenAsync(string MerchantUID, string ApplicationUID, string ClientToken, object userState)
        {
            if ((this.fGetTokenOperationCompleted == null))
            {
                this.fGetTokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfGetTokenOperationCompleted);
            }
            this.InvokeAsync("fGetToken", new object[] {
                    MerchantUID,
                    ApplicationUID,
                    ClientToken}, this.fGetTokenOperationCompleted, userState);
        }

        private void OnfGetTokenOperationCompleted(object arg)
        {
            if ((this.fGetTokenCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fGetTokenCompleted(this, new fGetTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void fUpdateTokenCCCompletedEventHandler(object sender, fUpdateTokenCCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fUpdateTokenCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fUpdateTokenCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void fGetHashedCardDetailsCompletedEventHandler(object sender, fGetHashedCardDetailsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fGetHashedCardDetailsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fGetHashedCardDetailsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void fCreateTokenCCCompletedEventHandler(object sender, fCreateTokenCCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fCreateTokenCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fCreateTokenCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void fGetMaskedCardDetailsCompletedEventHandler(object sender, fGetMaskedCardDetailsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fGetMaskedCardDetailsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fGetMaskedCardDetailsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void fValidateExpiryDateCompletedEventHandler(object sender, fValidateExpiryDateCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fValidateExpiryDateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fValidateExpiryDateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void fPayNowCompletedEventHandler(object sender, fPayNowCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fPayNowCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fPayNowCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void fDeregisterTokenCCCompletedEventHandler(object sender, fDeregisterTokenCCCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fDeregisterTokenCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fDeregisterTokenCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
    public delegate void fGetTokenCompletedEventHandler(object sender, fGetTokenCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fGetTokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal fGetTokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
