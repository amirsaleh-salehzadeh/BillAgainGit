using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TCG.PaymentGateways.Classes.Extras
{
    [XmlType("billagainusers")]
    public class DevInfos : List<DevInfo> { }

    [XmlType("billagainuser")]
    public class DevInfo
    {
        public string dev_devtoken { get; set; }
        public string dev_id { get; set; }
    }
}
