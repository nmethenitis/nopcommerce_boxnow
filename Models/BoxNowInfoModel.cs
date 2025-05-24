using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public record BoxNowInfoModel : BaseNopModel {
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string PartnerID { get; set; }
}
