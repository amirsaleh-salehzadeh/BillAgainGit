using System;
using Newtonsoft.Json;
using TCG.PaymentGateways.Providers.Stripe.Infrastructure;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeTermsOfServiceAcceptance : StripeEntity
    {
        [JsonProperty("date")]
        [JsonConverter(typeof(StripeDateTimeConverter))]
        public DateTime? Date { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }
    }
}
