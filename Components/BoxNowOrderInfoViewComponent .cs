using Microsoft.AspNetCore.Mvc;
using Nop.Data;
using Nop.Plugin.Shipping.BoxNow.Domain;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Services.Common;
using Nop.Services.Orders;

namespace Nop.Plugin.Shipping.BoxNow.Components;
public class BoxNowOrderInfoViewComponent : ViewComponent {
    private readonly IOrderService _orderService;
    private readonly IGenericAttributeService _genericAttributeService;
    private readonly IRepository<BoxNowRecord> _boxNowRecordRepository;

    public BoxNowOrderInfoViewComponent(IOrderService orderService, IGenericAttributeService genericAttributeService, IRepository<BoxNowRecord> boxNowRecordRepository) {
        _orderService = orderService;
        _genericAttributeService = genericAttributeService;
        _boxNowRecordRepository = boxNowRecordRepository;
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

            var boxNowRecord = await _boxNowRecordRepository.Table.Where(x => x.OrderId == order.Id).FirstOrDefaultAsync();

            var model = new BoxNowOrderInfoModel {
                OrderId = orderId,
                LockerId = lockerId,
                Address = address,
                ZipCode = zip,
                ActionUrl = Url.Action("SendToBoxNow", "BoxNow"),
                ParcelActionUrl = Url.Action("GetParcelVoucher", "BoxNow"),
                BoxNowRecord = boxNowRecord == null ? null : boxNowRecord
            };

            return View("~/Plugins/Shipping.BoxNow/Views/BoxNowOrderInfo.cshtml", model);
        }
        return Content("");
    }
}
