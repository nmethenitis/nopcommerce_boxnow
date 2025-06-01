using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Data;
using Nop.Plugin.Shipping.BoxNow.Domain;
using Nop.Plugin.Shipping.BoxNow.Helpers;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Plugin.Shipping.BoxNow.Services.Interfaces;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.BoxNow.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class BoxNowController : BaseAdminController {
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IBoxNowService _boxNowService;
    protected readonly IOrderService _orderService;
    protected readonly ICustomerService _customerService;
    protected readonly BoxNowSettings _boxNowSettings;
    protected readonly ShippingSettings _shippingSettings;
    protected readonly IAddressService _addressService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ICountryService _countryService;
    protected readonly IRepository<BoxNowRecord> _boxNowRecordRepository;

    #endregion
    #region Ctor

    public BoxNowController(ILocalizationService localizationService, INotificationService notificationService, IPermissionService permissionService, ISettingService settingService, IStoreContext storeContext, IBoxNowService boxNowService, IOrderService orderService, ICustomerService customerService, BoxNowSettings boxNowSettings, ShippingSettings shippingSettings, IAddressService addressService, IGenericAttributeService genericAttributeService, ICountryService countryService, IRepository<BoxNowRecord> boxNowRecordRepository) {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
        _boxNowService = boxNowService;
        _orderService = orderService;
        _customerService = customerService;
        _boxNowSettings = boxNowSettings;
        _shippingSettings = shippingSettings;
        _addressService = addressService;
        _genericAttributeService = genericAttributeService;
        _countryService = countryService;
        _boxNowRecordRepository = boxNowRecordRepository;
    }

    #endregion
    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public async Task<IActionResult> Configure() {
        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var boxNowSettings = await _settingService.LoadSettingAsync<BoxNowSettings>(storeScope);

        var model = new ConfigurationModel {
            Code = boxNowSettings.Code,
            DisplayName = boxNowSettings.DisplayName,
            Description = boxNowSettings.Description,
            ClientID = boxNowSettings.ClientID,
            ClientSecret = boxNowSettings.ClientSecret,
            PartnerID = boxNowSettings.PartnerID,
            FixedRate = boxNowSettings.FixedRate,
            IsStaging = boxNowSettings.IsStaging
        };
        if (storeScope > 0) {
            model.Code_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.Code, storeScope);
            model.DisplayName_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.DisplayName, storeScope);
            model.Description_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.Description, storeScope);
            model.ClientID_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.ClientID, storeScope);
            model.ClientSecret_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.ClientSecret, storeScope);
            model.PartnerID_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.PartnerID, storeScope);
            model.FixedRate_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.FixedRate, storeScope);
            model.IsStaging_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.IsStaging, storeScope);
        }

        return View("~/Plugins/Shipping.BoxNow/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_PAYMENT_METHODS)]
    public async Task<IActionResult> Configure(ConfigurationModel model) {
        if (!ModelState.IsValid)
            return await Configure();

        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var boxNowSettings = _settingService.LoadSetting<BoxNowSettings>(storeScope);

        //save settings
        boxNowSettings.Code = model.Code;
        boxNowSettings.DisplayName = model.DisplayName;
        boxNowSettings.Description = model.Description;
        boxNowSettings.ClientID = model.ClientID;
        boxNowSettings.ClientSecret = model.ClientSecret;
        boxNowSettings.PartnerID = model.PartnerID;
        boxNowSettings.FixedRate = model.FixedRate;
        boxNowSettings.IsStaging = model.IsStaging;

        /* We do not clear cache after each setting update.
         * This behavior can increase performance because cached settings will not be cleared 
         * and loaded from database after each update */

        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.Code, model.Code_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.DisplayName, model.DisplayName_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.Description, model.Description_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.ClientID, model.ClientID_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.ClientSecret, model.ClientSecret_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.PartnerID, model.PartnerID_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.FixedRate, model.FixedRate_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(boxNowSettings, x => x.IsStaging, model.IsStaging_OverrideForStore, storeScope, true);
        //now clear settings cache
        _settingService.ClearCache();
        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
        return await Configure();
    }
    #endregion

    [HttpPost]
    [ValidateAntiForgeryToken]
    //[CheckPermission(StandardPermission.Orders.SHIPMENTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> SendToBoxNow([FromBody] BoxNowInfoRequestVoucherModel requestModel) {
        var order = await _orderService.GetOrderByIdAsync(requestModel.OrderId);
        if (order == null) {
            return RedirectToAction("List", "Order");
        }
        var originAddress = await _addressService.GetAddressByIdAsync(_shippingSettings.ShippingOriginAddressId);
        var destinationAddress = await _addressService.GetAddressByIdAsync((int)order.ShippingAddressId);
        var country = await _countryService.GetCountryByAddressAsync(destinationAddress);
        var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);
        var lockerId = await _genericAttributeService.GetAttributeAsync<string>(order, BoxNowDefaults.BoxNowOrderLockerID);
        var request = new BoxNowDeliveryRequest() {
            TypeOfService = "same-day",
            OrderNumber = order.Id.ToString() + "-1",
            InvoiceValue = order.OrderTotal.ToString(),
            PaymentMode = "prepaid",
            AmountToBeCollected = "0.0",
            AllowReturn = true,
            NotifyOnAccepted = originAddress.Email,
            NotifySMSOnAccepted = originAddress.PhoneNumber.StartsWith("+") ? originAddress.PhoneNumber : $"{CountryPhoneCodes.Codes.GetValueOrDefault(country.TwoLetterIsoCode)}{originAddress.PhoneNumber}",
            Origin = new LocationModel() {
                ContactNumber = originAddress.PhoneNumber.StartsWith("+") ? originAddress.PhoneNumber : $"{CountryPhoneCodes.Codes.GetValueOrDefault(country.TwoLetterIsoCode)}{originAddress.PhoneNumber}",
                ContactEmail = originAddress.Email,
                ContactName = $"{originAddress.LastName} {originAddress.FirstName}",
                Name = $"{originAddress.LastName} {originAddress.FirstName}",
                AddressLine1 = originAddress.Address1,
                PostalCode = originAddress.ZipPostalCode,
                Country = country.TwoLetterIsoCode,
                LocationId = "2"
            },
            Destination = new LocationModel() {
                ContactNumber = destinationAddress.PhoneNumber.StartsWith("+") ? destinationAddress.PhoneNumber : $"{CountryPhoneCodes.Codes.GetValueOrDefault(country.TwoLetterIsoCode)}{destinationAddress.PhoneNumber}",
                ContactEmail = destinationAddress.Email,
                ContactName = $"{destinationAddress.LastName} {destinationAddress.FirstName}",
                Name = $"{destinationAddress.LastName} {destinationAddress.FirstName}",
                AddressLine1 = destinationAddress.Address1,
                PostalCode = destinationAddress.ZipPostalCode,
                Country = country.TwoLetterIsoCode,
                LocationId = lockerId
            },
            Items = new List<ItemModel>() {
                new ItemModel() {
                    Id = order.Id.ToString(),
                    Name = order.Id.ToString(),
                    Value = "0.00",
                    Weight = 0,
                    CompartmentSize = 1
                }
            }
        };

        var result = await _boxNowService.DeliveryRequest(request);
        if (result != null) {
            foreach (var parcel in result.Parcels) {
                var boxNowRecordModel = new BoxNowRecord {
                    LockerId = int.Parse(lockerId),
                    OrderId = order.Id,
                    ParcelId = parcel.Id
                };
                await _boxNowRecordRepository.InsertAsync(boxNowRecordModel);
            }
        }

        return RedirectToAction("Edit", "Order", new { id = order.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    //[CheckPermission(StandardPermission.Orders.SHIPMENTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> GetParcelVoucher([FromBody] BoxNowInfoRequestParcelModel requestModel) {
        if (requestModel == null) {
            return BadRequest();
        }
        var request = new BoxNowParcelRequest() {
            ParcelId = requestModel.ParcelId
        };
        var result = await _boxNowService.ParcelRequest(request);
        return File(result, "application/pdf", $"BoxNowLabel_Order_{requestModel.ParcelId}.pdf");
    }
}