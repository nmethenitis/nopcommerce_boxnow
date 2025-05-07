using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.BoxNow;
public class BoxNowSettings : ISettings {
    public string ClientID { get; set; }
    public string ClientSecret { get; set; }
    public string PartnerID { get; set; }
    public bool IsStaging { get; set; }
}
