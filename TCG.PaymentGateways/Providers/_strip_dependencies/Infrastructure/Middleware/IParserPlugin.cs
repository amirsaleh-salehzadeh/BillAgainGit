using System.Reflection;
using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe.Infrastructure.Middleware
{
    public interface IParserPlugin
    {
         bool Parse(ref string requestString, JsonPropertyAttribute attribute, PropertyInfo property, object propertyValue, object propertyParent);
    }
}
