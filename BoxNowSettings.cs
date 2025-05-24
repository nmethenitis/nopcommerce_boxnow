using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.BoxNow;
public class BoxNowSettings : ISettings {
    public string Code { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string ClientID { get; set; }
    public string ClientSecret { get; set; }
    public string PartnerID { get; set; }
    public decimal FixedRate { get; set; }
    public bool IsStaging { get; set; }
}
