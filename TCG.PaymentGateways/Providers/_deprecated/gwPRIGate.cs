using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Providers
{
    /// <summary>
    /// DEAD
    /// </summary>
    public class gwPRIGate : IPaymentStrategy
    {
        #region Properties
        public ProviderType GatewayType { get { return ProviderType.gwPRIGate; } }
        public GatewayOptions gatewayOptions
        {
            get
            {
                return new GatewayOptions
                    (
                        DisplayName: "PRI Gate",
                        WebUrl: "http://www.transfirst.com",
                        Description: "TransFirst Transaction Central Classic (formerly PRIGate)",
                        isActive: false,
                        isLive: false,
                        MerchantConfigValues: new string[] { },
                        Currencies: new string[] { },
                        Countries: new string[] { },
                        CardTypes: new CardTypeEnum[] { }
                    );
            }
        }
        public PaymentOptions paymentOptions
        {
            get
            {
                return new PaymentOptions
                    (
                        requires_CVV: false,
                        PaymentTokenize: false,
                        PaymentTokenize_requires_CVV: false,
                        PaymentTokenize_external: false,
                        ThreeDSecure: false,
                        Verify: false,
                        Auth: false,
                        AuthCapture: false,
                        AuthCapturePartial: false,
                        Sale: false,
                        Refund: false,
                        RefundPartial: false,
                        Credit: false,
                        Void: false
                    );
            }
        }
        #endregion

        #region Methods
        public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
        {
            throw new NotImplementedException();
        }

        public void LoginTest()
        {
            throw new NotImplementedException();
        }

        public StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details)
        {
            throw new NotImplementedException();
        }

        public Lookup3dSecure_Result Lookup3dSecure(Lookup3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Authenticate3dSecure_Result Authenticate3dSecure(Authenticate3dSecure_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Verify(Sale_Details details)
        {
            throw new NotSupportedException();
        }

        public Transaction_Result Auth(Sale_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Capture(AuthCapture_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Sale(Sale_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Refund(Refund_Details details)
        {
            throw new NotImplementedException();
        }

        public Transaction_Result Credit(Credit_Details details)
        {
            throw new NotSupportedException();
        }

        public Transaction_Result Void(Void_Details details)
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
