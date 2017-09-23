using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public abstract class StripeEntityWithId : StripeEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}