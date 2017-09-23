using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways
{
    public interface IPaymentStrategy : IGatewayStrategy
    {
        PaymentOptions paymentOptions { get; }

        StorePaymentMethod_Result StorePaymentMethod(StorePaymentMethod_Details details);
        RevokePaymentMethod_Result RevokePaymentMethod(RevokePaymentMethod_Details details);

        Lookup3dSecure_Result Lookup3dSecure(Lookup3dSecure_Details details);
        Authenticate3dSecure_Result Authenticate3dSecure(Authenticate3dSecure_Details details);

        /// <summary>
        /// Address Verification System
        /// This method can be used if you wish to perform fraud (AVS and CVV) checks on a card, but don't actually wish to charge the customer.
        /// This is useful for pre-ordering an item that has not yet been released or is currently back- ordered.
        /// The card information is validated by the merchant, and when the item is later in stock and ships to the customer, an Sale transaction can be performed.
        /// </summary>
        Transaction_Result Verify(Sale_Details details);

        /// <summary>
        /// This method sends an authorization-only request to the specified Gateway.
        /// This transaction is not added to the current open batch, and must be completed later with the Capture method (you may use the Sale method if you wish to authorize and capture in one step).
        /// </summary>
        Transaction_Result Auth(Sale_Details details);

        /// <summary>
        /// This method captures a transaction that has been previously authorized with the AuthOnly method.
        /// The TransactionId parameter indicates to the Gateway which transaction is to be captured, and should contain the TransactionId from the original transaction.
        /// The CaptureAmount parameter is the value to be captured from the customer's credit card, and can be different from the authorized amount.
        /// </summary>
        Transaction_Result Capture(AuthCapture_Details details);

        /// <summary>
        /// Sends a basic sale transaction to the Gateway.
        /// This transaction decrements the cardholder's open-to-buy funds for the TransactionAmount, and the transaction is automatically added to the current open batch.
        /// </summary>
        Transaction_Result Sale(Sale_Details details);

        /// <summary>
        /// This method refunds a transaction that has already been captured, or settled.
        /// If the transaction is still outstanding use the VoidTransaction method instead.
        /// The TransactionId parameter indicates to the Gateway which transaction is to be refunded, and should contain the TransactionId from the original transaction.
        /// The RefundAmount parameter is the value to be refunded back to the customer, and can be all or part of the original TransactionAmount
        /// </summary>
        Transaction_Result Refund(Refund_Details details);

        /// <summary>
        /// This method credits a customer's card specified via Card.
        /// This type of transaction is NOT based on previous transaction. Some gateways refer to these as "Open" or "Blind" Credits.
        /// TransactionAmount is used to specify the amount you wish to return to the customer's card.
        /// </summary>
        Transaction_Result Credit(Credit_Details details);

        /// <summary>
        /// This method voids a transaction that has been previously authorized, but which has not yet gone to settlement, or been "captured".
        /// The TransactionId parameter indicates to the Gateway which transaction is to be voided, and should contain the TransactionId from the original transaction.
        /// To cancel a transaction which has already been captured, use the Credit method.
        /// </summary>
        Transaction_Result Void(Void_Details details);
    }
}
