using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Batch;

namespace TCG.PaymentGateways
{
    public interface IBatchStrategy : IGatewayStrategy
    {
        BatchOptions batchOptions { get; }
        
        /// <summary>
        /// Stores the payment method to vault.
        /// </summary>
        StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details);

        /// <summary>
        /// Removes the payment method from vault.
        /// </summary>
        RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details);

        /// <summary>
        /// Verifies bank account details
        /// </summary>
        Batch_Verify_Result Verify(Batch_Verify_Details details);

        /// <summary>
        /// Builds the sale batch file
        /// </summary>
        Batch_Sale_Build_Result Sale_Build(Batch_Sale_Build_Details details);
        /// <summary>
        /// Submits the sale batch file, depending on gateway it either does a complete submission, or just does a validation together with Sale_Release to complete transaction
        /// </summary>
        Batch_Sale_Submit_Result Sale_Submit(Batch_Sale_Submit_Details details);
        /// <summary>
        /// Releases / confirms the sale batch file that was submitted if Sale_Build and Sale_Submit successful and only if gateway requires this phase
        /// </summary>
        Batch_Sale_Release_Result Sale_Release(Batch_Sale_Release_Details details);
        /// <summary>
        /// Gets a report on failed transactions and reconciliates that with database (database assumes success until recon says otherwise), can not always be done immediately
        /// </summary>
        Batch_Sale_Recon_Result Sale_Recon(Batch_Sale_Recon_Details details);
    }
}
