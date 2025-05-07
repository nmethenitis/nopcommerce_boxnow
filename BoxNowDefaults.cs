using Nop.Core;

namespace Nop.Plugin.Shipping.BoxNow;
public class BoxNowDefaults {
    public static string PluginName = "Shipping.BoxNow";
    public static string UserAgent => $"nopCommerce-{NopVersion.FULL_VERSION}";
    public static (string Staging, string Production) ApiUrl => ("https://api-stage.boxnow.gr", "https://api-production.boxnow.gr");
    public static string AuthPath = "/api/v1/auth-sessions";
    public static string DeliveryRequestPath = "/api/v1/delivery-requests";
}
