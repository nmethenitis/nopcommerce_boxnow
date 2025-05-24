using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public record ConfigurationModel : BaseNopModel {
    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.BoxNow.Fields.Code")]
    public string Code { get; set; }
    public bool Code_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.BoxNow.Fields.DisplayName")]
    public string DisplayName { get; set; }
    public bool DisplayName_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.BoxNow.Fields.Description")]
    public string Description { get; set; }
    public bool Description_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.BoxNow.Fields.ClientID")]
    public string ClientID { get; set; }
    public bool ClientID_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.BoxNow.Fields.ClientSecret")]
    public string ClientSecret { get; set; }
    public bool ClientSecret_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.BoxNow.Fields.PartnerID")]
    public string PartnerID { get; set; }
    public bool PartnerID_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.BoxNow.Fields.Rate")]
    public decimal FixedRate { get; set; }
    public bool FixedRate_OverrideForStore { get; set; }
}
