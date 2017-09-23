using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeDeleted : StripeEntityWithId
    {
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}