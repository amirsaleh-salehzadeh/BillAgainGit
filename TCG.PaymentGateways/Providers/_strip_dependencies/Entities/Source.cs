using Newtonsoft.Json;
using TCG.PaymentGateways.Providers.Stripe.Infrastructure;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public enum SourceType
    {
        Card,
        BankAccount,
        Deleted
    }

    [JsonConverter(typeof(SourceConverter))]
    public class Source : StripeEntityWithId
    {
        public SourceType Type { get; set; }

        public StripeDeleted Deleted { get; set; }
        public StripeCard Card { get; set; }
        public StripeBankAccount BankAccount { get; set; }
    }
}
