using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TCG.PaymentGateways.Classes.Extras
{
    [XmlType("billagainusers")]
    public class ThreePeaksUsers : List<ThreePeaksUser>{ }
    
    [XmlType("billagainuser")]
    public class ThreePeaksUser
    {
        public string bau_id { get; set; }
        public string cpr_ref { get; set; }
        public string cpr_name { get; set; }
        public string cpr_tel { get; set; }
        public string cpr_fax { get; set; }
        public string cpr_website { get; set; }
        public string cpr_address_1 { get; set; }
        public string cpr_address_2 { get; set; }
        public string cpr_address_3 { get; set; }
        public string cpr_address_zip { get; set; }
        public string con_name { get; set; }
        public string con_surname { get; set; }
        public string con_email { get; set; }
        public string bau_country { get; set; }
        public string bau_timezone { get; set; }
        public string bau_currency { get; set; }
        public string bau_received { get; set; }
        public string dev_id { get; set; }
        public string dev_devtoken { get; set; }
    }
}
