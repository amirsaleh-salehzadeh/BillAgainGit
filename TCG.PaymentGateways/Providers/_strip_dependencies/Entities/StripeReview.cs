using System;
using Newtonsoft.Json;
using TCG.PaymentGateways.Providers.Stripe.Infrastructure;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeReview : StripeEntityWithId
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        #region Expandable Charge
        public string ChargeId { get; set; }

        [JsonIgnore]
        public StripeCharge Charge { get; set; }

        [JsonProperty("charge")]
        internal object InternalCharge
        {
            set
            {
                StringOrObject<StripeCharge>.Map(value, s => ChargeId = s, o => Charge = o);
            }
        }
        #endregion

        [JsonProperty("created")]
        [JsonConverter(typeof(StripeDateTimeConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("livemode")]
        public bool LiveMode { get; set; }

        [JsonProperty("open")]
        public bool Open { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}