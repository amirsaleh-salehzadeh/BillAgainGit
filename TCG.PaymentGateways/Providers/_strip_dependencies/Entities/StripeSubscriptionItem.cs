﻿using System;
using Newtonsoft.Json;
using TCG.PaymentGateways.Providers.Stripe.Infrastructure;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeSubscriptionItem : StripeEntityWithId
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(StripeDateTimeConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("plan")]
        public StripePlan Plan { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}