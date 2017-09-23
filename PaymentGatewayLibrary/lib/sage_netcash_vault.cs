using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace TCG.PaymentGatewayLibrary.SagePayNetCashVault
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "cceServiceSoap", Namespace = "localhost")]
    public partial class cceService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback PutCardOperationCompleted;

        /// <remarks/>
        public cceService()
        {
            this.Url = "https://cde.sagepay.co.za/Service/cceService.asmx";
        }

        /// <remarks/>
        public event PutCardCompletedEventHandler PutCardCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("localhost/PutCard", RequestNamespace = "localhost", ResponseNamespace = "localhost", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PutCard(string CredentialToken, string ccName, string ccNumber, string ccExpiryMonth, string ccExpiryYear, string Caller)
        {
            object[] results = this.Invoke("PutCard", new object[] {
                    CredentialToken,
                    ccName,
                    ccNumber,
                    ccExpiryMonth,
                    ccExpiryYear,
                    Caller});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginPutCard(string CredentialToken, string ccName, string ccNumber, string ccExpiryMonth, string ccExpiryYear, string Caller, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("PutCard", new object[] {
                    CredentialToken,
                    ccName,
                    ccNumber,
                    ccExpiryMonth,
                    ccExpiryYear,
                    Caller}, callback, asyncState);
        }

        /// <remarks/>
        public string EndPutCard(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void PutCardAsync(string CredentialToken, string ccName, string ccNumber, string ccExpiryMonth, string ccExpiryYear, string Caller)
        {
            this.PutCardAsync(CredentialToken, ccName, ccNumber, ccExpiryMonth, ccExpiryYear, Caller, null);
        }

        /// <remarks/>
        public void PutCardAsync(string CredentialToken, string ccName, string ccNumber, string ccExpiryMonth, string ccExpiryYear, string Caller, object userState)
        {
            if ((this.PutCardOperationCompleted == null))
            {
                this.PutCardOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPutCardOperationCompleted);
            }
            this.InvokeAsync("PutCard", new object[] {
                    CredentialToken,
                    ccName,
                    ccNumber,
                    ccExpiryMonth,
                    ccExpiryYear,
                    Caller}, this.PutCardOperationCompleted, userState);
        }

        private void OnPutCardOperationCompleted(object arg)
        {
            if ((this.PutCardCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PutCardCompleted(this, new PutCardCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void PutCardCompletedEventHandler(object sender, PutCardCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PutCardCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal PutCardCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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
