using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Services.Attributes;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Shipping.BoxNow;
public class BoxNowComputationMethod : BasePlugin, IShippingRateComputationMethod, IWidgetPlugin {

    #region Fields

    protected readonly BoxNowSettings _boxNowSettings;
    protected readonly ILocalizationService _localizationService;
    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly ISettingService _settingService;
    protected readonly IShippingService _shippingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IWebHelper _webHelper;

    public bool HideInWidgetList => false;

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
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.Code"] = "Code",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.DisplayName"] = "Display Name",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.Description"] = "Description",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.ClientID"] = "Client ID",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.ClientSecret"] = "Client Secret",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.PartnerID"] = "Partner ID",
            [$"Plugins.{BoxNowDefaults.PluginName}.Fields.FixedRate"] = "Fixed Rate",
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

    public async Task<decimal?> GetFixedRateAsync(GetShippingOptionRequest getShippingOptionRequest) {
        ArgumentNullException.ThrowIfNull(getShippingOptionRequest);
        return _boxNowSettings.FixedRate;
    }

    public Task<IShipmentTracker> GetShipmentTrackerAsync() {
        return Task.FromResult<IShipmentTracker>(null);
    }

    public Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest) {
        ArgumentNullException.ThrowIfNull(getShippingOptionRequest);
        var response = new GetShippingOptionResponse();
        if (getShippingOptionRequest.Items == null || !getShippingOptionRequest.Items.Any()) {
            response.AddError("No shipment items");
            return Task.FromResult(response);
        }

        response.ShippingOptions = new List<ShippingOption>() {
            new ShippingOption() {
                Name = _boxNowSettings.DisplayName,
                Description = _boxNowSettings.Description,
                Rate = _boxNowSettings.FixedRate,
                TransitDays = 2
            }
        };            
        return Task.FromResult(response);
    }

    public async Task<IList<string>> GetWidgetZonesAsync() {
        return new List<string> { 
            PublicWidgetZones.CheckoutShippingMethodBottom
        };
    }

    public Type GetWidgetViewComponent(string widgetZone) {
        return typeof(Components.BoxNowViewComponent);
    }
}
