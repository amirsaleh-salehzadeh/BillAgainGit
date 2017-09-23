using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeTransferReversal : StripeEntityWithId
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        #region Expandable Balance Transaction
        public string BalanceTransactionId { get; set; }

        [JsonIgnore]
        public StripeBalanceTransaction BalanceTransaction { get; set; }

        [JsonProperty("balance_transaction")]
        internal object InternalBalanceTransaction
        {
            set
            {
                Infrastructure.StringOrObject<StripeBalanceTransaction>.Map(value, s => BalanceTransactionId = s, o => BalanceTransaction = o);
            }
        }
        #endregion

        [JsonProperty("created")]
        [JsonConverter(typeof(Infrastructure.StripeDateTimeConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        #region Expandable Transfer
        public string TransferId { get; set; }

        [JsonIgnore]
        public StripeTransfer Transfer { get; set; }

        [JsonProperty("transfer")]
        internal object InternalTransfer
        {
            set
            {
                Infrastructure.StringOrObject<StripeTransfer>.Map(value, s => TransferId = s, o => Transfer = o);
            }
        }
        #endregion
    }
}