using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes
{
    public class GatewayOptions
    {
        public readonly string DisplayName;
        public readonly string WebUrl;
        public readonly string Description;
        public readonly bool isActive;              // DEAD
        public readonly bool isLive;                // IMPLEMENTED

        public readonly string[] MerchantConfigValues;

        public readonly string[] Currencies;
        public readonly string[] Countries;
        public readonly CardTypeEnum[] CardTypes;

        public GatewayOptions(string DisplayName, string WebUrl, string Description, bool isActive, bool isLive,
            string[] MerchantConfigValues,
            string[] Currencies, string[] Countries, CardTypeEnum[] CardTypes)
        {
            this.DisplayName = DisplayName;
            this.WebUrl = WebUrl;
            this.Description = Description;
            this.isActive = isActive;
            this.isLive = isLive;

            this.MerchantConfigValues = MerchantConfigValues;

            this.Currencies = Currencies;
            this.Countries = Countries;
            this.CardTypes = CardTypes;
        }
    }
}
