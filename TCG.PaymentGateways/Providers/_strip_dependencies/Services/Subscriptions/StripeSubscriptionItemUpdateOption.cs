using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe.Services
{
    public class StripeSubscriptionItemUpdateOption : INestedOptions
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("plan")]
        public string PlanId { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }
    }
}