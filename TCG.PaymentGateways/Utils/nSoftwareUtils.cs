using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsoftware.InPay;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Utils
{
    internal class nSoftwareUtils
    {
        internal static void fill_AuthSaleRequest(ref Icharge icharge, Sale_Details details, bool AmtInCents)
        {
            //icharge.Config("CurrencyCode=" + details.CurrencyCode);
            icharge.InvoiceNumber = details.ExtRef;

            icharge.Card.Number = details.CardNumber;
            icharge.Card.ExpMonth = details.CardExpiryMonth;
            icharge.Card.ExpYear = details.CardExpiryYear;
            if (!string.IsNullOrEmpty(details.CardCCV))
                icharge.Card.CVVData = details.CardCCV;

            icharge.Customer.FirstName = details.CustomerFirstName;
            icharge.Customer.LastName = details.CustomerLastName;
            icharge.Customer.FullName = details.CustomerFirstName + " " + details.CustomerLastName;
            icharge.Customer.Address = details.CustomerAddress;
            icharge.Customer.City = details.CustomerCity;
            icharge.Customer.State = details.CustomerState;
            icharge.Customer.Zip = details.CustomerZip;
            icharge.Customer.Country = details.CustomerCountry;
            icharge.Customer.Phone = details.CustomerPhone;
            icharge.Customer.Email = details.CustomerEmail;
            icharge.Customer.Id = details.CustomerIdentifier;

            icharge.TransactionAmount = AmtInCents ? (details.Amount * 100).ToString() : details.Amount.ToString("F2");
        }

        internal static Transaction_Result parse_Response(ref Icharge icharge)
        {
            var result = new Transaction_Result
            {
                isApproved = icharge.Response.Approved,
                ApprovalCode = icharge.Response.Code,

                ResultCode = icharge.Response.Code,
                ResultText = icharge.Response.Text,
                TransactionIndex = icharge.Response.TransactionId,
                ProcessorCode = icharge.Response.ProcessorCode,

                FullRequest = icharge.Config("RawRequest"),
                FullResponse = icharge.Config("RawResponse"),

                hasServerError = false,
                ErrorCode = icharge.Response.ErrorCode,
                ErrorText = icharge.Response.ErrorText
            };
            return result;
        }
    }
}
