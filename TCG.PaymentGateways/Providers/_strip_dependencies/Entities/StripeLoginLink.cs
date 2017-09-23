using System;
using Newtonsoft.Json;
using TCG.PaymentGateways.Providers.Stripe.Infrastructure;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeLoginLink : StripeEntity
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(StripeDateTimeConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
