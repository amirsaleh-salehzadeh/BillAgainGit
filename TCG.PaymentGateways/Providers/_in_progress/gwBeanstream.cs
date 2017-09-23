using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsoftware.InPay;
using TCG.PaymentGateways.Classes;
using TCG.PaymentGateways.Classes.Payments;

namespace TCG.PaymentGateways.Providers
{
    /// <summary>
    /// nsoftware notes:
    /// MerchantLogin and MerchantPassword are required properties.
    /// The TransactionAmount can represented as either cents without a decimal point or dollars and cents with a decimal point. For example, a value of "100" would equate to "$1.00" while a value of "100.00" would equate to "$100.00" for these gateways.
    /// The "CurrencyCode" configuration setting is not applicable.
    /// This gateway supports sending ThreeDSecure (3DS) verification data by setting the following configs: CAVV, XID, ECI.
    /// TestMode is not supported and when set to "True" an exception will be thrown by the component.
    /// You must set a "username" and "password" in the merchant interface, and add them to the component via the AddSpecialField method if you wish to use any transaction other than Sale.
    /// </summary>
    //public class gwBeanstream : IPaymentStrategy
    //{
    //    #region Properties
    //    public ProviderType GatewayType { get { return ProviderType.gwBeanstream; } }
    //    public string GatewayDisplayName { get { return "Beanstream"; } }
    //    public string GatewayWebUrl { get { return "http://www.beanstream.com/"; } }

    //    public string GatewayDescription
    //    {
    //        get { return "Payment processing in 140 currencies. A Digital River Company."; }
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
    //                "XXX","XXX",
    //            };
    //        }
    //    }

    //    public string[] supportedCountries
    //    {
    //        get
    //        {
    //            return new string[] { 
    //                "XXX", "XXX",                      // USA, Canada, Aus, Europe - need full list
    //            };
    //        }
    //    }

    //    public string[] requiredMerchantConfigValues    // none required
    //    {
    //        get { return new string[] { "MerchantLogin", "MerchantPassword", "PrivateKey" }; }
    //    }

    //    public bool requires_CVV
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public bool supports_3dSecure { get { return false; } }
    //    public bool supports_AVSOnly { get { return false; } }
    //    public bool supports_AuthOnly { get { return true; } }
    //    public bool supports_AuthCapture { get { return true; } }
    //    public bool supports_AuthCapturePartial { get { return true; } }
    //    public bool supports_Sale { get { return true; } }
    //    public bool supports_Refund { get { return true; } }
    //    public bool supports_RefundPartial { get { return true; } }
    //    public bool supports_Credit { get { return false; } }
    //    public bool supports_Void { get { return true; } }

    //    public bool ready_3dSecure { get { return false; } }
    //    public bool ready_AVSOnly { get { return false; } }
    //    public bool ready_AuthOnly { get { return false; } }
    //    public bool ready_AuthCapture { get { return false; } }
    //    public bool ready_Sale { get { return false; } }
    //    public bool ready_Refund { get { return false; } }
    //    public bool ready_Credit { get { return false; } }
    //    public bool ready_Void { get { return false; } }
    //    #endregion

    //    #region Fields
    //    private string login;
    //    private string password;
    //    private bool istestmode;
    //    private string privatekey;
    //    #endregion

    //    #region Helpers
    //    private Icharge newClient()
    //    {
    //        var icharge = new Icharge();
    //        icharge.Gateway = IchargeGateways.gwBeanstream;
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
    //        privatekey = MerchantConfigValues.Where(r => r.Key.Equals("PrivateKey")).FirstOrDefault().Value;
    //        istestmode = isTestMode;
    //    }

    //    public void LoginTest()
    //    {
    //        MerchantConfigValue[] config = new MerchantConfigValue[]
    //        {
    //            //This corresponds to the x_login field and is your API Login ID
    //            new MerchantConfigValue
    //            {
    //                Key="MerchantLogin",
    //                Value="ffsxw99q6z4bcg87"
    //            },
    //            //This correspond to the x_tran_key field and is your Transaction Key value
    //            new MerchantConfigValue
    //            {
    //                Key="MerchantPassword",
    //                Value="xjrf3g4ndfrrn6q2"
    //            },
    //            new MerchantConfigValue 
    //            { 
    //                Key = "PrivateKey", 
    //                Value = "74063683b81c1c3654ed1cf80ce0eafc" 
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
    //        throw new NotSupportedException();
    //    }

    //    public Transaction_Result Auth(Sale_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Transaction_Result Capture(AuthCapture_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Transaction_Result Sale(Sale_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Transaction_Result Refund(Refund_Details details)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Transaction_Result Credit(Credit_Details details)
    //    {
    //        throw new NotSupportedException();
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
