namespace Nop.Plugin.Shipping.BoxNow.Models;
public class BoxNowParcelRequest {
    public string ParcelId { get; set; }
    public string Type { get; set; } = "pdf";
}
