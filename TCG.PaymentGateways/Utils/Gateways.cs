using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PaymentGateways.Classes;

namespace TCG.PaymentGateways.Utils
{
    public class Gateways
    {
        public static List<IGatewayStrategy> getAllGateways()
        {
            var result = new List<IGatewayStrategy>();
            var values = Enum.GetValues(typeof(ProviderType));

            //foreach (var item in values)
            //{
            //    var icharge = new nsoftware.InPay.Icharge();

            //    var ns_Type = (nsoftware.InPay.IchargeGateways)Enum.Parse(typeof(nsoftware.InPay.IchargeGateways), item.ToString());

            //    icharge.Gateway = ns_Type;
            //    var gwURL = icharge.GatewayURL;
            //}

            foreach (var item in values)
            {
                // get class if exists
                Type t = Type.GetType("TCG.PaymentGateways.Providers." + item.ToString());
                if (t != null)
                {
                    Object[] args = { };
                    Object o = Activator.CreateInstance(t, args);
                    result.Add((IGatewayStrategy)o);
                }
            }

            return result;
        }

        public static List<IGatewayStrategy> getAllActiveGateways(bool? isLive = null)
        {
            var gws = getAllGateways();
            var result = new List<IGatewayStrategy>();
            foreach (var gw in gws)
            {
                if (!gw.gatewayOptions.isActive)
                    continue;

                if (isLive.HasValue)
                    if (isLive.Value && !gw.gatewayOptions.isLive)
                        continue;
                    else if (!isLive.Value && gw.gatewayOptions.isLive)
                        continue;

                result.Add(gw);
            }
            return result;
        }

        public static List<IGatewayStrategy> getAllActiveBatchGateways(bool? isLive = null)
        {
            var gws = getAllGateways();
            var result = new List<IGatewayStrategy>();
            foreach (var gw in gws)
            {
                if (!gw.gatewayOptions.isActive)
                    continue;

                if (isLive.HasValue)
                    if (isLive.Value && !gw.gatewayOptions.isLive)
                        continue;
                    else if (!isLive.Value && gw.gatewayOptions.isLive)
                        continue;

                if(gw is IBatchStrategy)
                {
                    result.Add(gw);
                }
                
            }
            return result;
        }

        public static IGatewayStrategy getGateway(string gatewayType)
        {
            Type t = Type.GetType("TCG.PaymentGateways.Providers." + gatewayType);
            if (t != null)
            {
                Object[] args = { };
                Object o = Activator.CreateInstance(t, args);
                return (IGatewayStrategy)o;
            }
            return null;
        }

        public static IGatewayStrategy getGateway(ProviderType gateway)
        {
            Type t = Type.GetType("TCG.PaymentGateways.Providers." + gateway.ToString());
            if (t != null)
            {
                Object[] args = { };
                Object o = Activator.CreateInstance(t, args);
                return (IGatewayStrategy)o;
            }
            return null;
        }

        public static string GatewayTypeString(ProviderType GatewayType)
        {
            return Enum.GetName(typeof(TCG.PaymentGateways.Classes.ProviderType), GatewayType);
        }

        public static bool? gatewayIsBatch(string gwCode)
        {
            var gw = getGateway(gwCode);

            if (gw == null)
            {
                return null;
            }

            if (gw is IBatchStrategy)
                return true;

            return false;
        }

        public static bool? gatewayIsHPP(string gwCode)
        {
            var gw = getGateway(gwCode);

            if (gw == null)
            {
                return null;
            }

            if (gw is IHostedPPStrategy)
                return true;

            return false;
        }

        public static string gatewayIsTypeOf(string gwCode)
        {
            var gw = getGateway(gwCode);

            if(gw == null)
            {
                return null;
            }

            if (gw is IHostedPPStrategy)
                return "HPP";

            if (gw is IBatchStrategy)
                return "Batch";

            if (gw is IPaymentStrategy)
                return "Card";

            return null;
        }
    }
}
