using Nop.Plugin.Shipping.BoxNow.Domain;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public class BoxNowOrderInfoModel {
    public int OrderId { get; set; }
    public string LockerId { get; set; }
    public string Address { get; set; }
    public string ZipCode { get; set; }
    public string ActionUrl { get; set; }
    public string ParcelActionUrl { get; set; }
    public BoxNowRecord BoxNowRecord { get; set; }
}
