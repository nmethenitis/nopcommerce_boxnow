using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public class BoxNowSelectedItem {
    public string LockerID {  get; set; }
    public string AddressLine1 { get; set; }
    public string PostalCode { get; set; }
}
