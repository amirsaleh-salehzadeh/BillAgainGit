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
    //public class gwAdyen : IPaymentStrategy
    //{
    //    private string login;
    //    private string password;
    //    private bool istestmode;
    //    private MerchantConfigValue terminalid;

    //    #region Properties
    //    public ProviderType GatewayType { get { return ProviderType.gwAdyen; } }
    //    public GatewayOptions gatewayOptions
    //    {
    //        get
    //        {
    //            return new GatewayOptions
    //                (
    //                    DisplayName: "Adyen",
    //                    WebUrl: "https://www.adyen.com/home/",
    //                    Description: "Adyen is a leading provider of omni-channel payment solutions with over 250 payment methods and 187 transaction currencies. By operating from a single, Internet-based platform we make it easy for businesses to accept payments worldwide.",
    //                    isActive: true,
    //                    MerchantConfigValues: new[] { "TerminalId", "MerchantLogin", "MerchantPassword" },
    //                    Currencies: new[] { "ZAR", "USD" },
    //                    Countries: new[] { "US", "ZA", },
    //                    CardTypes: new[] 
    //                    {
    //                        CardTypeEnum.VISA, 
    //                        CardTypeEnum.MASTERCARD, 
    //                        CardTypeEnum.AMERICAN_EXPRESS, 
    //                        CardTypeEnum.DINERS_CLUB 
    //                    }
    //                );
    //        }
    //    }
    //    public PaymentOptions paymentOptions
    //    {
    //        get
    //        {
    //            return new PaymentOptions
    //                (
    //                    requires_CVV: false,
    //                    ThreeDSecure: false,
    //                    Verify: true,
    //                    Auth: true,
    //                    AuthCapture: true,
    //                    AuthCapturePartial: false,
    //                    Sale: true,
    //                    Refund: true,
    //                    RefundPartial: false,
    //                    Credit: true,
    //                    Void: true
    //                );
    //        }
    //    }
    //    #endregion

    //    #region Methods
    //    public void Login(bool isTestMode, params MerchantConfigValue[] MerchantConfigValues)
    //    {
    //        terminalid = MerchantConfigValues.Where(r => r.Key.Equals("TerminalId")).FirstOrDefault();
    //        //MerchantConfigValues.Where(r => r.Key.Equals("TerminalId")).FirstOrDefault().Value;
    //        login = MerchantConfigValues.Where(r => r.Key.Equals("MerchantLogin")).FirstOrDefault().Value;
    //        password = MerchantConfigValues.Where(r => r.Key.Equals("MerchantPassword")).FirstOrDefault().Value;
    //    }

    //    public void LoginTest()
    //    {
    //        MerchantConfigValue[] config = new MerchantConfigValue[]
    //        {
    //            new MerchantConfigValue
    //            {
    //                Key="MerchantLogin",
    //                Value="ws@Company.TheCodeGroup"
    //            },
    //            new MerchantConfigValue
    //            {
    //                Key="MerchantPassword",
    //                Value="7wJD~5f*?L8gs~@m<4ZFshL7m"
    //            },
    //            new MerchantConfigValue 
    //            { 
    //                Key = "TerminalId", 
    //                Value = "TheCodeGroupZA" 
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
    //            nSoftwareUtils.fill_AuthSaleRequest(ref icharge, details, true);
    //            icharge.Config("CurrencyCode=" + details.CurrencyCode); //change to icharge.Config("CurrencyCode=" + details.CurrencyCodeNumeric) for gateways that want ISO Code; 

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
    //            nSoftwareUtils.fill_AuthSaleRequest(ref icharge, details, true);
    //            icharge.Config("CurrencyCode=" + details.CurrencyCode);
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

    //    public void runTests()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    #endregion

    //    #region Helpers
    //    private Icharge newClient()
    //    {
    //        var icharge = new Icharge();
    //        icharge.Gateway = IchargeGateways.gwAdyen;
    //        icharge.GatewayURL = "https://pal-test.adyen.com/pal/servlet/soap/Payment";
    //        icharge.MerchantLogin = login;
    //        icharge.MerchantPassword = password;
    //        icharge.Config(terminalid.Key + "=" + terminalid.Value);
    //        return icharge;
    //    }
    //    #endregion      
    //}
}
