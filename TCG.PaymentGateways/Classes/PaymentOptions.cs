using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Classes
{
    public class PaymentOptions
    {
        public readonly bool requires_CVV;

        public readonly bool PaymentTokenize;
        public readonly bool PaymentTokenize_requires_CVV;
        public readonly bool PaymentTokenize_external;

        public readonly bool ThreeDSecure;
        public readonly bool Verify;
        public readonly bool Auth;
        public readonly bool AuthCapture;
        public readonly bool AuthCapturePartial;
        public readonly bool Sale;
        public readonly bool Refund;
        public readonly bool RefundPartial;
        public readonly bool Credit;
        public readonly bool Void;

        public readonly TestCard[] TestCards;

        public PaymentOptions(bool requires_CVV,
            bool PaymentTokenize, bool PaymentTokenize_requires_CVV,
            bool PaymentTokenize_external,
            bool ThreeDSecure, bool Verify,
            bool Auth, bool AuthCapture, bool AuthCapturePartial,
            bool Sale, bool Refund, bool RefundPartial, bool Credit, bool Void, TestCard[] TestCards = null)
        {
            this.requires_CVV = requires_CVV;

            this.PaymentTokenize = PaymentTokenize;
            this.PaymentTokenize_requires_CVV = PaymentTokenize_requires_CVV;
            this.PaymentTokenize_external = PaymentTokenize_external;

            this.ThreeDSecure = ThreeDSecure;
            this.Verify = Verify;
            this.Auth = Auth;
            this.AuthCapture = AuthCapture;
            this.AuthCapturePartial = AuthCapturePartial;
            this.Sale = Sale;
            this.Refund = Refund;
            this.RefundPartial = RefundPartial;
            this.Credit = Credit;
            this.Void = Void;

            this.TestCards = TestCards;
        }
    }
}
