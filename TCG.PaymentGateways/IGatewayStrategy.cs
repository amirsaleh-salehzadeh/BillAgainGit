using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes;

namespace TCG.PaymentGateways
{
    /// <summary>
    /// Gets a value indicating the Payment Gateway Name
    /// Gets a value indicating the Payment Gateway Display Name
    /// Gets a value indicating the Payment Gateway's web address
    /// Gets a value indicating the Description
    /// Indicates if Gateway is still Active Online and Valid
    /// Country 2 Letter ISO
    /// Currency 3 Letter ISO
    /// </summary>
    public interface IGatewayStrategy
    {
        ProviderType GatewayType { get; }
        GatewayOptions gatewayOptions { get; }

        void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues);
        void LoginTest();

        void runTests();
    }
}
