using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsoftware.InPay;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;
using TCG.PaymentGateways.Utils;

namespace TCG.PaymentGateways.Providers
{
    /// <summary>
    /// nsoftware notes:
    /// MerchantLogin and MerchantPassword are required properties.
    /// The TransactionAmount is required to be represented as cents with a decimal point. For example, "1.00".
    /// The "CurrencyCode" configuration setting is not applicable.
    /// This gateway supports sending ThreeDSecure (3DS) verification data by setting the following configs: CAVV, ECI.
    /// TestMode is supported and when set to "True" test transactions can be sent using a live account that will not be captured and settled.
    /// This gateway has a unique security feature. To use it, you must add the secret hash value provided by the Authorize.Net merchant web interface to the "HashSecret" configuration setting (via the Config method). Both of these values are provided by the Authorize.Net merchant web interface, which is used to set up your account. For example; Config("HashSecret=myhashvalue"). If no hash secret is supplied in the config method, the hash value returned by the server will NOT be checked.
    /// </summary>
    //public class gwAuthorizeNet : IPaymentStrategy
    //{
    //    #region Properties
    //    public ProviderType GatewayType { get { return ProviderType.gwAuthorizeNet; } }
    //    public string GatewayDisplayName { get { return "Authorize.net"; } }
    //    public string GatewayWebUrl { get { return "http://www.authorize.net/"; } }

    //    public string GatewayDescription
    //    {
    //        get { return "Authorize.Net makes accepting credit card payments and e-checks safe, easy and affordable."; }
    //    }

    //    public bool isActive { get { return false; } }
    //    public bool isSupported { get { return true; } }
    //    public bool isComplete { get { return false; } }
    //    public bool isTested { get { return false; } }

    //    public string[] supportedCurrencies
    //    {
    //        get
    //        {
    //            return new string[] { 
    //                "XXX","XXX","XXX",
    //            };
    //        }
    //    }

    //    public string[] supportedCountries
    //    {
    //        get
    //        {
    //            return new string[] { 
    //                "XXX", "XXX",
    //            };
    //        }
    //    }

    //    public string[] requiredMerchantConfigValues    // none required
    //    {
    //        get { return new string[] { "MerchantLogin", "MerchantPassword" }; }
    //    }

    //    public bool requires_CVV
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public bool supports_3dSecure { get { return false; } }
    //    public bool supports_AVSOnly { get { return false; } }
    //    public bool supports_AuthOnly { get { return true; } }
    //    public bool supports_AuthCapture { get { return true; } }
    //    public bool supports_AuthCapturePartial { get { return false; } }
    //    public bool supports_Sale { get { return true; } }
    //    public bool supports_Refund { get { return true; } }
    //    public bool supports_RefundPartial { get { return false; } }
    //    public bool supports_Credit { get { return true; } }
    //    public bool supports_Void { get { return true; } }

    //    public bool ready_3dSecure { get { return false; } }
    //    public bool ready_AVSOnly { get { return false; } }
    //    public bool ready_AuthOnly { get { return true; } }
    //    public bool ready_AuthCapture { get { return true; } }
    //    public bool ready_Sale { get { return true; } }
    //    public bool ready_Refund { get { return false; } }
    //    public bool ready_Credit { get { return false; } }
    //    public bool ready_Void { get { return false; } }
    //    #endregion

    //    #region Fields
    //    private string login;
    //    private string password;
    //    private bool istestmode;
    //    #endregion

    //    #region Helpers
    //    private Icharge newClient()
    //    {
    //        var icharge = new Icharge();
    //        icharge.Gateway = IchargeGateways.gwAuthorizeNet;
    //        if (istestmode)
    //        {
    //            icharge.TestMode = true;
    //            icharge.GatewayURL = "https://test.authorize.net/gateway/transact.dll";
    //        }
    //        icharge.MerchantLogin = login;
    //        icharge.MerchantPassword = password;
    //        return icharge;
    //    }
    //    #endregion

    //    #region Methods
    //    public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
    //    {
    //        login = MerchantConfigValues.Where(r => r.Key.Equals("MerchantLogin")).FirstOrDefault().Value;
    //        password = MerchantConfigValues.Where(r => r.Key.Equals("MerchantPassword")).FirstOrDefault().Value;

    //        istestmode = isTestMode;
    //    }

    //    public void LoginTest()
    //    {

    //        MerchantConfigValue[] config = new MerchantConfigValue[]
    //        {
    //            new MerchantConfigValue
    //            {
    //                Key="MerchantLogin",
    //                Value="663b8cQRf"
    //            },
    //            new MerchantConfigValue
    //            {
    //                Key="MerchantPassword",
    //                Value="9C6C59t2cke75DVV"
    //            }
    //        };

    //        Login(true, config);
    //    }

    //    public Lookup3dSecure_Result Lookup3dSecure(Lookup3dSecure_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Authenticate3dSecure_Result Authenticate3dSecure(Authenticate3dSecure_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Transaction_Result Verify(Sale_Details details)
    //    {
    //        // not supported
    //        throw new NotSupportedException();
    //    }

    //    public Transaction_Result Auth(Sale_Details details)
    //    {
    //        try
    //        {
    //            var icharge = newClient();
    //            nSoftwareUtils.fill_AuthSaleRequest(ref icharge, details, false);
    //            icharge.AuthOnly();
    //            var result = nSoftwareUtils.parse_Response(ref icharge);
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            return new Transaction_Result
    //            {
    //                isApproved = false,
    //                hasServerError = true,
    //                ErrorText = ex.Message
    //            };
    //        }
    //    }

    //    public Transaction_Result Capture(AuthCapture_Details details)
    //    {
    //        try
    //        {
    //            var icharge = newClient();
    //            icharge.Capture(details.TransactionIndex, details.Amount.Value.ToString("F2"));
    //            var result = nSoftwareUtils.parse_Response(ref icharge);
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            return new Transaction_Result
    //            {
    //                isApproved = false,
    //                hasServerError = true,
    //                ErrorText = ex.Message
    //            };
    //        }
    //    }

    //    public Transaction_Result Sale(Sale_Details details)
    //    {
    //        // tested
    //        try
    //        {
    //            var icharge = newClient();
    //            // set currency code
    //            nSoftwareUtils.fill_AuthSaleRequest(ref icharge, details, false);
    //            icharge.Sale();
    //            var result = nSoftwareUtils.parse_Response(ref icharge);
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            return new Transaction_Result
    //            {
    //                isApproved = false,
    //                hasServerError = true,
    //                ErrorText = ex.Message
    //            };
    //        }
    //    }

    //    public Transaction_Result Refund(Refund_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Transaction_Result Credit(Credit_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Transaction_Result Void(Void_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    #endregion


    //    public List<TestResults> doTests()
    //    {
    //        throw new NotImplementedException();
    //    }





        
    //}
}
