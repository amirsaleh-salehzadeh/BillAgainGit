using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Batch
{
    public class Batch_InvalidActionDates_Validator
    {
        internal DateTimeOffset? DailyCutoff;
        internal string[] InvalidActionDays;
        internal List<DateTime> InvalidActionDates;

        internal Batch_InvalidActionDates_Validator(DateTimeOffset? _DailyCutoff, string[] _InvalidActionDays, List<DateTime> _InvalidActionDates)
        {
            DailyCutoff = _DailyCutoff;
            InvalidActionDays = _InvalidActionDays;
            InvalidActionDates = _InvalidActionDates;
        }

        public Batch_InvalidActionDate_Result Validate_Date(DateTimeOffset ActionDate)
        {

            if(!DailyCutoff.HasValue)
            {
                return new Batch_InvalidActionDate_Result
                {
                    isValid = true
                };
            }

            //standardise them to UTC
            DateTimeOffset dailyCutoffGWUTC = DailyCutoff.Value;
            var ActionDateGWUTC = ActionDate.ToOffset(DailyCutoff.Value.Offset);

            var ActionDateDay = ActionDateGWUTC.DayOfWeek.ToString();
            if (InvalidActionDays.Contains(ActionDateDay))
            {
                return new Batch_InvalidActionDate_Result
                {
                    isValid = false,
                    ErrorText = "Action date cannot be on a " + ActionDateDay
                };
            }

            if (InvalidActionDates.Select(r=>r.Date).Contains(ActionDateGWUTC.Date))
            {
                return new Batch_InvalidActionDate_Result
                {
                    isValid = false,
                    ErrorText = "Invalid action date as specified by gateway"
                };
            }

            //compare UTC values
            if (dailyCutoffGWUTC.Date == ActionDateGWUTC.Date)//if same day
            {
                if (ActionDateGWUTC.TimeOfDay > dailyCutoffGWUTC.TimeOfDay)//if after the cutoff
                {
                    return new Batch_InvalidActionDate_Result
                    {
                        isValid = false,
                        ErrorText = "Action date cannot be on same day if after cutoff time. The cutoff time is " + dailyCutoffGWUTC.ToString("HH:mm:ss") + dailyCutoffGWUTC.Offset.Hours + " UTC"
                    };
                }
            }

            return new Batch_InvalidActionDate_Result
            {
                isValid = true
            };
        }

        public Batch_InvalidActionDates_Result Validate_Date(DateTime StartDate, DateTime EndDate)
        {
            if (EndDate < StartDate)
                return null;

            throw new NotImplementedException();
        }
    }

    public class Batch_InvalidActionDate_Result
    {
        public bool isValid;
        public string ErrorText;
    }

    public class Batch_InvalidActionDates_Result
    {
        public List<DateTime> InvalidDates;
        public TimeSpan DailyCutoff;
    }
}
