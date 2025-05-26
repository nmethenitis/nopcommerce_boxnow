using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Services.Common;
using Nop.Services.Orders;

namespace Nop.Plugin.Shipping.BoxNow.Components;
public class BoxNowOrderInfoViewComponent : ViewComponent {
    private readonly IOrderService _orderService;
    private readonly IGenericAttributeService _genericAttributeService;

    public BoxNowOrderInfoViewComponent(
        IOrderService orderService,
        IGenericAttributeService genericAttributeService) {
        _orderService = orderService;
        _genericAttributeService = genericAttributeService;
    }

    public async Task<IViewComponentResult> InvokeAsync() {
        var routeData = ViewComponentContext?.ViewContext?.RouteData;
        if (routeData?.Values.TryGetValue("id", out var idObj) == true && int.TryParse(idObj?.ToString(), out var orderId)) {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) {
                return Content("");
            }

            var lockerId = await _genericAttributeService.GetAttributeAsync<string>(order, BoxNowDefaults.BoxNowOrderLockerID);
            var address = await _genericAttributeService.GetAttributeAsync<string>(order, BoxNowDefaults.BoxNowOrderAddress);
            var zip = await _genericAttributeService.GetAttributeAsync<string>(order, BoxNowDefaults.BoxNowOrderPostalCode);

            var model = new BoxNowOrderInfoModel {
                LockerId = lockerId,
                Address = address,
                ZipCode = zip
            };

            return View("~/Plugins/Shipping.BoxNow/Views/BoxNowOrderInfo.cshtml", model);
        }
        return Content("");
    }
}
