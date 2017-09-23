using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeCustomAccountKeys : StripeEntity
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("publishable")]
        public string Publishable { get; set; }
    }
}
