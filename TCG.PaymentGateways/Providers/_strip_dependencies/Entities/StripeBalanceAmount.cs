using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeBalanceAmount : StripeEntity
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
 }