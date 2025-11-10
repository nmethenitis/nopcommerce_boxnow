using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Directory;
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
    protected readonly ICountryService _countryService;

    public bool HideInWidgetList => false;

    #endregion

    #region Ctor

    public BoxNowComputationMethod(BoxNowSettings boxNowSettings, ILocalizationService localizationService, IShoppingCartService shoppingCartService, ISettingService settingService, IShippingService shippingService, IStoreContext storeContext, IWebHelper webHelper, ICountryService countryService) {
        _boxNowSettings = boxNowSettings;
        _localizationService = localizationService;
        _shoppingCartService = shoppingCartService;
        _settingService = settingService;
        _shippingService = shippingService;
        _storeContext = storeContext;
        _webHelper = webHelper;
        _countryService = countryService;
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
        var country = await _countryService.GetCountryByIdAsync(getShippingOptionRequest.ShippingAddress?.CountryId ?? 0);
        if(country == null) {
            return _boxNowSettings.FixedRate;
        }
        return country.TwoLetterIsoCode == "GR" ? _boxNowSettings.FixedRate : _boxNowSettings.FixedRateCyprus;
    }

    public Task<IShipmentTracker> GetShipmentTrackerAsync() {
        return Task.FromResult<IShipmentTracker>(null);
    }

    public async Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest) {
        ArgumentNullException.ThrowIfNull(getShippingOptionRequest);
        var response = new GetShippingOptionResponse();
        if (getShippingOptionRequest.Items == null || !getShippingOptionRequest.Items.Any()) {
            response.AddError("No shipment items");
            return response;
        }
        var country = await _countryService.GetCountryByIdAsync(getShippingOptionRequest.ShippingAddress?.CountryId ?? 0);
        if(country == null) {
            response.AddError("Shipping country is not set");
            return response;
        }

        response.ShippingOptions = new List<ShippingOption>() {
            new ShippingOption() {
                Name = _boxNowSettings.DisplayName,
                Description = _boxNowSettings.Description,
                Rate = country.TwoLetterIsoCode == "GR" ? _boxNowSettings.FixedRate : _boxNowSettings.FixedRateCyprus,
                TransitDays = 2
            }
        };
        return response;
    }

    public async Task<IList<string>> GetWidgetZonesAsync() {
        return new List<string> {
            PublicWidgetZones.CheckoutShippingMethodBottom,
            AdminWidgetZones.OrderDetailsBlock
        };
    }

    public Type GetWidgetViewComponent(string widgetZone) {
        if (widgetZone == AdminWidgetZones.OrderDetailsBlock)
            return typeof(Components.BoxNowOrderInfoViewComponent);

        return typeof(Components.BoxNowViewComponent);
    }
}
