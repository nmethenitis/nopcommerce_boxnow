using Nop.Core;

namespace Nop.Plugin.Shipping.BoxNow;
public class BoxNowDefaults {
    public static string PluginName = "Shipping.BoxNow";
    public static string UserAgent => $"nopCommerce-{NopVersion.FULL_VERSION}";
    public static (string Staging, string Production) ApiUrl => ("https://api-stage.boxnow.gr", "https://api-production.boxnow.gr");
    public static string AuthPath = "/api/v1/auth-sessions";
    public static string DeliveryRequestPath = "/api/v1/delivery-requests";
    public static string ParcelRequestPath = "/api/v1/parcels";
    public const string FIXED_RATE_SETTINGS_KEY = "ShippingRateComputationMethod.BoxNow.Rate.ShippingMethodId{0}";
    public const string TRANSIT_DAYS_SETTINGS_KEY = "ShippingRateComputationMethod.BoxNow.TransitDays.ShippingMethodId{0}";
    public static string BoxNowSelectedItem = "Plugin.Shipping.BoxNow.SelectedItem";
    public static string BoxNowCallVouvher = "Plugin.Shipping.BoxNow.Voucher.Call";
    public static string BoxNowGetParcelVouvher = "Plugin.Shipping.BoxNow.Get.Parcel.Voucher";
    public static string BoxNowOrderLockerID = "Plugin.Shipping.BoxNow.LockerID";
    public static string BoxNowOrderAddress = "Plugin.Shipping.BoxNow.Address";
    public static string BoxNowOrderPostalCode = "Plugin.Shipping.BoxNow.PostalCode";
}
