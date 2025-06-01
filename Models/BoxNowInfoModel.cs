using Nop.Web.Framework.Models;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public record BoxNowInfoModel : BaseNopModel {
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string PartnerID { get; set; }
}
