using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.Shipping.BoxNow;
public class BoxNowComputationMethod : BasePlugin, IShippingRateComputationMethod {

    #region Fields

    protected readonly BoxNowSettings _boxNowSettings;
    protected readonly ILocalizationService _localizationService;
    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly ISettingService _settingService;
    protected readonly IShippingService _shippingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IWebHelper _webHelper;

    #endregion

    #region Ctor

    public BoxNowComputationMethod(BoxNowSettings boxNowSettings, ILocalizationService localizationService, IShoppingCartService shoppingCartService, ISettingService settingService, IShippingService shippingService, IStoreContext storeContext, IWebHelper webHelper) {
        _boxNowSettings = boxNowSettings;
        _localizationService = localizationService;
        _shoppingCartService = shoppingCartService;
        _settingService = settingService;
        _shippingService = shippingService;
        _storeContext = storeContext;
        _webHelper = webHelper;
    }

    #endregion

    public override async Task InstallAsync() {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string> {
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.ClientID"] = "Client ID",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.ClientSecret"] = "Client Secret",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.PartnerID"] = "Partner ID",
        });
        await base.InstallAsync();
    }

    public override async Task UninstallAsync() {
        await _localizationService.DeleteLocaleResourcesAsync($"Plugins.{BoxNowDefaults.PluginName}");
        await base.UninstallAsync();
    }

    public override string GetConfigurationPageUrl() {
        return $"{_webHelper.GetStoreLocation()}Admin/BoxNow/Configure";
    }

    public Task<decimal?> GetFixedRateAsync(GetShippingOptionRequest getShippingOptionRequest) {
        throw new NotImplementedException();
    }

    public Task<IShipmentTracker> GetShipmentTrackerAsync() {
        throw new NotImplementedException();
    }

    public Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest) {
        throw new NotImplementedException();
    }
}
