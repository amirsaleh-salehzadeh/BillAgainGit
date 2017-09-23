using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PaymentGateways.Classes.Extras
{
    public class ExternalUser
    {
        public string ExternalIdentifier { get; set; }
        public string Ref { get; set; }
        public string CompName { get; set; }
        public string CustName { get; set; }
        public string CustLastName { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string CountryCodeTwoLetter { get; set; }
        public string TimeZoneOffset { get; set; }
        public string Currency { get; set; }
        public MerchantConfigValue[] MerchantConfigValues { get; set; }
        public List<ExternalUser_Contact> Contacts { get; set; }
    }

    public class ExternalUser_Contact
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
