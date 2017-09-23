using System.Threading;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class StripeChargeService : StripeService
    {
        public StripeChargeService(string apiKey = null) : base(apiKey) { }

        public bool ExpandApplicationFee { get; set; }
        public bool ExpandBalanceTransaction { get; set; }
        public bool ExpandCustomer { get; set; }
        public bool ExpandDestination { get; set; }
        public bool ExpandInvoice { get; set; }
        public bool ExpandReview { get; set; }
        public bool ExpandTransfer { get; set; }
        public bool ExpandOnBehalfOf { get; set; }
        public bool ExpandSourceTransfer { get; set; }
        public bool ExpandDispute { get; set; }
        public bool ExpandOutcome { get; set; }


        //Sync
        public virtual StripeCharge Create(StripeChargeCreateOptions createOptions, StripeRequestOptions requestOptions = null)
        {
            return Mapper<StripeCharge>.MapFromJson(
                Requestor.PostString(Infrastructure.ParameterBuilder.ApplyAllParameters(this, createOptions, Urls.Charges, false),
                SetupRequestOptions(requestOptions))
            );
        }

        public virtual StripeCharge Update(string chargeId, StripeChargeUpdateOptions updateOptions, StripeRequestOptions requestOptions = null)
        {
            return Mapper<StripeCharge>.MapFromJson(
                Requestor.PostString(Infrastructure.ParameterBuilder.ApplyAllParameters(this, updateOptions, $"{Urls.Charges}/{chargeId}", false),
                SetupRequestOptions(requestOptions))
            );
        }

        public virtual StripeCharge Get(string chargeId, StripeRequestOptions requestOptions = null)
        {
            return Mapper<StripeCharge>.MapFromJson(
                Requestor.GetString(Infrastructure.ParameterBuilder.ApplyAllParameters(this, null, $"{Urls.Charges}/{chargeId}", false),
                SetupRequestOptions(requestOptions))
            );
        }

        public virtual StripeList<StripeCharge> List(StripeChargeListOptions listOptions = null, StripeRequestOptions requestOptions = null)
        {
            return Mapper<StripeList<StripeCharge>>.MapFromJson(
                Requestor.GetString(Infrastructure.ParameterBuilder.ApplyAllParameters(this, listOptions, Urls.Charges, true),
                SetupRequestOptions(requestOptions))
            );
        }

        public virtual StripeCharge Capture(string chargeId, int? captureAmount = null, int? applicationFee = null, StripeRequestOptions requestOptions = null)
        {
            var url = Infrastructure.ParameterBuilder.ApplyAllParameters(this, null, $"{Urls.Charges}/{chargeId}/capture", false);

            if (captureAmount.HasValue)
                url = Infrastructure.ParameterBuilder.ApplyParameterToUrl(url, "amount", captureAmount.Value.ToString());
            if (applicationFee.HasValue)
                url = Infrastructure.ParameterBuilder.ApplyParameterToUrl(url, "application_fee", applicationFee.Value.ToString());

            return Mapper<StripeCharge>.MapFromJson(
                Requestor.PostString(url,
                SetupRequestOptions(requestOptions))
            );
        }



        //Async
        public virtual async Task<StripeCharge> CreateAsync(StripeChargeCreateOptions createOptions, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Mapper<StripeCharge>.MapFromJson(
                await Requestor.PostStringAsync(Infrastructure.ParameterBuilder.ApplyAllParameters(this, createOptions, Urls.Charges, false),
                SetupRequestOptions(requestOptions),
                cancellationToken)
            );
        }

        public virtual async Task<StripeCharge> UpdateAsync(string chargeId, StripeChargeUpdateOptions updateOptions, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Mapper<StripeCharge>.MapFromJson(
                await Requestor.PostStringAsync(Infrastructure.ParameterBuilder.ApplyAllParameters(this, updateOptions, $"{Urls.Charges}/{chargeId}", false),
                SetupRequestOptions(requestOptions),
                cancellationToken)
            );
        }

        public virtual async Task<StripeCharge> GetAsync(string chargeId, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Mapper<StripeCharge>.MapFromJson(
                await Requestor.GetStringAsync(Infrastructure.ParameterBuilder.ApplyAllParameters(this, null, $"{Urls.Charges}/{chargeId}", false),
                SetupRequestOptions(requestOptions),
                cancellationToken)
            );
        }

        public virtual async Task<StripeList<StripeCharge>> ListAsync(StripeChargeListOptions listOptions = null, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Mapper<StripeList<StripeCharge>>.MapFromJson(
                await Requestor.GetStringAsync(Infrastructure.ParameterBuilder.ApplyAllParameters(this, listOptions, Urls.Charges, true),
                    SetupRequestOptions(requestOptions),
                    cancellationToken)
            );
        }

        public virtual async Task<StripeCharge> CaptureAsync(string chargeId, int? captureAmount = null, int? applicationFee = null, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = Infrastructure.ParameterBuilder.ApplyAllParameters(this, null, $"{Urls.Charges}/{chargeId}/capture", false);

            if (captureAmount.HasValue)
                url = Infrastructure.ParameterBuilder.ApplyParameterToUrl(url, "amount", captureAmount.Value.ToString());
            if (applicationFee.HasValue)
                url = Infrastructure.ParameterBuilder.ApplyParameterToUrl(url, "application_fee", applicationFee.Value.ToString());

            return Mapper<StripeCharge>.MapFromJson(
                await Requestor.PostStringAsync(url,
                SetupRequestOptions(requestOptions),
                cancellationToken)
            );
        }
    }
}
