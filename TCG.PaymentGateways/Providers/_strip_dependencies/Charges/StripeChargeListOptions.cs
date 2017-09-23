using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeChargeListOptions : Stripe.StripeListOptions
    {
        [JsonProperty("created")]
        public StripeDateFilter Created { get; set; }

        [JsonProperty("customer")]
        public string CustomerId { get; set; }

        [JsonProperty("include_total_count")]
        public bool IncludeTotalCount { get; set; }
    }
}
