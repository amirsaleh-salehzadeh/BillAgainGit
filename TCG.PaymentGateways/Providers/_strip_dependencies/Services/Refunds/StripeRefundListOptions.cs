using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeRefundListOptions : StripeListOptions
    {
        [JsonProperty("charge")]
        public string ChargeId { get; set; }
    }
}