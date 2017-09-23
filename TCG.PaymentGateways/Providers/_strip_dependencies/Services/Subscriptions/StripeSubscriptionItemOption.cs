using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe.Services
{
    public class StripeSubscriptionItemOption : INestedOptions
    {
        [JsonProperty("plan")]
        public string PlanId { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }
    }
}