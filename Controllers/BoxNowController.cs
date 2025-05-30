﻿using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.BoxNow.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class BoxNowController : BasePluginController {
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;

    #endregion
    #region Ctor

    public BoxNowController(ILocalizationService localizationService, INotificationService notificationService, IPermissionService permissionService, ISettingService settingService, IStoreContext storeContext) {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
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
            FixedRate = boxNowSettings.FixedRate
        };
        if (storeScope > 0) {
            model.Code_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.Code, storeScope);
            model.DisplayName_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.DisplayName, storeScope);
            model.Description_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.Description, storeScope);
            model.ClientID_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.ClientID, storeScope);
            model.ClientSecret_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.ClientSecret, storeScope);
            model.PartnerID_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.PartnerID, storeScope);
            model.FixedRate_OverrideForStore = _settingService.SettingExists(boxNowSettings, x => x.FixedRate, storeScope);
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
        //now clear settings cache
        _settingService.ClearCache();
        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
        return await Configure();
    }
    #endregion
}