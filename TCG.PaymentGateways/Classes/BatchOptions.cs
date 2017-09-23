using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes.Batch;

namespace TCG.PaymentGateways.Classes
{
    public class BatchOptions
    {
        public readonly bool Bank_Batching;
        public readonly bool CreditCard_Batching;

        public readonly string Bank_Batching_Type; //ach sepa or debit_order

        public readonly bool BankPaymentTokenize;
        public readonly bool CCPaymentTokenize;

        public readonly bool CCThreeDSecure;

        public readonly bool require_BankPaymentTokenize;
        public readonly bool require_CCPaymentTokenize;

        /// <summary>
        /// do we have to get notifications from gateway (i.e. pull); if false then notifications will be pushed to a certain url on our side by gateway
        /// </summary>
        public readonly bool is_NotifyPull;
        /// <summary>
        /// is the release already done by code or does it require human intervention i.e. out of system control; maybe send notification at this stage
        /// </summary>
        public readonly bool is_AutoRelease;
        /// <summary>
        /// can the reconciliation be done by code or does it need to be done manually by a human, meaning that payments will be assumed successful until user sets as unsuccessful
        /// </summary>
        public readonly bool is_AutoRecon;

        public readonly bool Verify;
        //public readonly bool Auth;
        //public readonly bool AuthCapture;
        //public readonly bool AuthCapturePartial;
        public readonly bool Sale;
        public readonly bool Refund;
        //public readonly bool RefundPartial;
        //public readonly bool Credit;
        //public readonly bool Void;        

        public readonly DateTimeOffset? DailyCutoff;
        public readonly string[] InvalidSubmitDays;

        /// <summary>
        /// Gets or sets a value indicating whether gateway uses its own identifier for identifying batch.
        /// </summary>
        /// <value>
        /// <c>true</c> if [uses own identifier]; otherwise, <c>false</c>.
        public readonly bool UsesExternalIdentifier;

        public BatchOptions(bool Bank_Batching, bool CreditCard_Batching, string Bank_Batching_Type,
            bool BankPaymentTokenize, bool CCPaymentTokenize, bool CCThreeDSecure,
            bool require_BankPaymentTokenize, bool require_CCPaymentTokenize,
            bool is_NotifyPull, bool is_AutoRelease, bool is_AutoRecon,
            bool Verify, bool Sale, bool Refund, bool UsesExternalIdentifier, DateTimeOffset? DailyCutoff = null, string[] InvalidSubmitDays = null)
        {
            this.Bank_Batching = Bank_Batching;
            this.CreditCard_Batching = CreditCard_Batching;

            this.BankPaymentTokenize = BankPaymentTokenize;
            this.CCPaymentTokenize = CCPaymentTokenize;

            this.require_BankPaymentTokenize = require_BankPaymentTokenize;
            this.require_CCPaymentTokenize = require_CCPaymentTokenize;

            this.CCThreeDSecure = CCThreeDSecure;

            this.is_NotifyPull = is_NotifyPull;
            this.is_AutoRelease = is_AutoRelease;
            this.is_AutoRecon = is_AutoRecon;

            this.Verify = Verify;
            //this.Auth = Auth;
            //this.AuthCapture = AuthCapture;
            //this.AuthCapturePartial = AuthCapturePartial;
            this.Sale = Sale;
            this.Refund = Refund;
            //this.RefundPartial = RefundPartial;
            //this.Credit = Credit;
            //this.Void = Void;

            this.Bank_Batching_Type = Bank_Batching_Type;
            this.UsesExternalIdentifier = UsesExternalIdentifier;
            this.DailyCutoff = DailyCutoff;
            this.InvalidSubmitDays = InvalidSubmitDays;
        }

        /// <summary>
        /// Returns Calculator used to compute cutofftimes and invalid submit dates
        /// </summary>
        public Batch_InvalidActionDates_Validator ActionDate_Validator(List<DateTime> InvalidSubmitDates)
        {
            if (InvalidSubmitDates == null)
                InvalidSubmitDates = new List<DateTime>();

            return new Batch_InvalidActionDates_Validator(DailyCutoff, InvalidSubmitDays, InvalidSubmitDates);
        }
    }
}
