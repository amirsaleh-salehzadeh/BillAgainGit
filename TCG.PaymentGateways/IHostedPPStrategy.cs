using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.HPP;

namespace TCG.PaymentGateways
{
    public interface IHostedPPStrategy : IGatewayStrategy
    {
        HPP_PostInfo SalePost(HPP_Details details); // gets information for the transaction to be performed
        HPP_Result SaleResult(NameValueCollection data); // processes result that comes from hosted payment page
        Transaction_Result_HPP SaleVerify(HPP_Result details); // returns confirmation of receipt data
    }
}
