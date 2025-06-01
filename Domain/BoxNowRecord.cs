using Nop.Core;

namespace Nop.Plugin.Shipping.BoxNow.Domain;
public class BoxNowRecord : BaseEntity {
    public int OrderId { get; set; }
    public int LockerId { get; set; }
    public string ParcelId { get; set; }
}
