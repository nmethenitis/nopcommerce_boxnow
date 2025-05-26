using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public class BoxNowOrderInfoModel {
    public string LockerId { get; set; }
    public string Address { get; set; }
    public string ZipCode { get; set; }
}
