using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes
{
    [Serializable()]
    public enum ProviderType
    {
        gwNoGateway = 0,

        // HOSTED PAYMENT PAGE
        gwPayFastHPP = 1,
        gwSagePayNetCashHPP = 2,
        gwMonsterPayHPP = 3, //DEPRECATED 
        gwAliPayHPP = 4, //CHINA
        gwTenPayHPP = 5, //CHINA
        gwPayPalHPP = 6,

        // CREDIT CARD
        gwMyGate = 300,
        gwPayGate = 301,
        gwPayU = 302,
        gwSetcom = 303, //DEPRECATED
        gwVCS = 304,
        gwSagePayNetCash = 305,
        gwCashFlows = 306,
        gwBrainTree = 307,
        gwPayFastCC = 308,
        gwPayPalCC = 309,
        gwStripe = 310,

        // BATCH
        gwMyGateDD = 90,                // - IMPLEMENTED
        gwPayGateDD = 93,               // - IMPLEMENTED
        gwSagePayNetCashDD = 96,         // - IMPLEMENTED   
        gwThreePeaksDD = 98,

        // PENDING IMLEMENTATION
        gwAuthorizeNet = 5001,
        gwEprocessing = 5002,
        gwIntellipay = 5003,
        gwITransact = 5004,
        gwNetBilling = 5005,
        gwPayFlowPro = 5006,
        gwUSAePay = 5007,
        gwPlugNPay = 5008,
        gwPlanetPayment = 5009,
        gwMPCS = 5010,
        gwRTWare = 5011,
        gwECX = 5012,
        gwBankOfAmerica = 5013,
        gwInnovative = 5014,
        gwMerchantAnywhere = 5015,
        gwSkipjack = 5016,
        gwIntuitPaymentSolutions = 5017,
        gw3DSI = 5018,
        gwTrustCommerce = 5019,
        gwPSIGate = 5020,
        gwPayFuse = 5021,
        gwPayFlowLink = 5022,
        gwOrbital = 5023,
        gwLinkPoint = 5024,
        gwMoneris = 5025,
        gwFastTransact = 5027,
        gwNetworkMerchants = 5028,
        gwOgone = 5029,
        gwMerchantPartners = 5031,
        gwFirstData = 5033,
        gwYourPay = 5034,
        gwACHPayments = 5035,
        gwPaymentsGateway = 5036,
        gwCyberSource = 5037,
        gwEway = 5038,
        gwGoEMerchant = 5039,
        gwTransFirst = 5040,
        gwChase = 5041,
        gwWorldPay = 5043,
        gwTransactionCentral = 5044,
        gwSterling = 5045,
        gwPayJunction = 5046,
        gwSECPay = 5047,
        gwPaymentExpress = 5048,
        gwMyVirtualMerchant = 5049,
        gwSagePayments = 5050,
        gwSecurePay = 5051,
        gwMonerisUSA = 5052,
        gwBeanstream = 5053,
        gwVerifi = 5054,
        gwSagePay = 5055,
        gwMerchantESolutions = 5056,
        gwPayLeap = 5057,
        gwPayPoint = 5058,
        gwWorldPayXML = 5059,
        gwProPay = 5060,
        gwQBMS = 5061,
        gwHeartland = 5062,
        gwLitle = 5063,
        gwJetPay = 5065,
        gwHSBC = 5066,
        gwBluePay = 5067,
        gwAdyen = 5068,
        gwBarclay = 5069,
        gwPayTrace = 5070,
        gwCyberbit = 5072,
        gwGoToBilling = 5073,
        gwTransNationalBankcard = 5074,
        gwNetbanx = 5075,
        gwDataCash = 5077,
        gwACHFederal = 5078,
        gwGlobalIris = 5079,
        gwFirstDataE4 = 5080,
        gwFirstAtlantic = 5081,
        gwBluefin = 5082,
        gwPayscape = 5083,
        gwWePayCC = 5084,

        // RESELLER
        gwNexCommerce = 7001,

        // FOREIGN
        gwYKC = 8001,                     // JAPAN
        gwMIT = 8002,                     // Foreign

        // DEAD
        gwUSight = 9001,
        gwPRIGate = 9002,
        gwCyberCash = 9003,
        gwPayDirect = 9004,
    }
}