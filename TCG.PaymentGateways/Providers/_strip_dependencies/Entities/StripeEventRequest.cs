using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
	public class StripeEventRequest : StripeEntityWithId
	{
		[JsonProperty("idempotency_key")]
		public string IdempotencyKey { get; set; }
	}
}