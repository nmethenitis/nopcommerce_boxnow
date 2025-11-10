using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Orders;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Shipping.BoxNow.Components;

[ViewComponent(Name = "BoxNowMap")]
public class BoxNowViewComponent : NopViewComponent {
    protected readonly BoxNowSettings _boxNowSettings;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IWorkContext _workContext;
    private readonly IAddressService _addressService;
    private readonly ICheckoutModelFactory _checkoutModelFactory;
    private readonly ICountryService _countryService;

    public BoxNowViewComponent(ICheckoutModelFactory checkoutModelFactory, BoxNowSettings boxNowSettings, IShoppingCartService shoppingCartService, IWorkContext workContext, IAddressService addressService, ICountryService countryService) {
        _checkoutModelFactory = checkoutModelFactory;
        _boxNowSettings = boxNowSettings;
        _workContext = workContext;
        _shoppingCartService = shoppingCartService;
        _addressService = addressService;
        _countryService = countryService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData) {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var shoppingCart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart);
        var shippingAddress = await _addressService.GetAddressByIdAsync((int)(customer.ShippingAddressId ?? customer.BillingAddressId));
        var country = await _countryService.GetCountryByIdAsync(shippingAddress?.CountryId ?? 0);
        var checkoutModel = await _checkoutModelFactory.PrepareShippingMethodModelAsync(shoppingCart, shippingAddress);
        var model = new BoxNowInfoModel() {
            PartnerID = _boxNowSettings.PartnerID,
            CountryCode = country?.TwoLetterIsoCode ?? "GR",
        };
        return View("~/Plugins/Shipping.BoxNow/Views/ShowMap.cshtml", model);
    }
}
