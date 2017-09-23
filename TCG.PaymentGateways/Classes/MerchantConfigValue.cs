using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes
{
    public class MerchantConfigValue
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public MerchantConfigValue() { }
        public MerchantConfigValue(string _key, string _value)
        {
            Key = _key;
            Value = _value;
        }
    }
}
