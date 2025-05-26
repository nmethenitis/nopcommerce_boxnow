using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;

namespace Nop.Plugin.Shipping.BoxNow.Services;
public class EventConsumer : IConsumer<OrderPlacedEvent> {

    private readonly IGenericAttributeService _genericAttributeService;
    private readonly ICustomerService _customerService;

    public EventConsumer(IGenericAttributeService genericAttributeService, ICustomerService customerService) {
        _genericAttributeService = genericAttributeService;
        _customerService = customerService;
    }

    public async Task HandleEventAsync(OrderPlacedEvent eventMessage) {
        var order = eventMessage.Order;
        var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);
        if (customer == null) {
            return;
        }        
        var lockerId = await _genericAttributeService.GetAttributeAsync<string>(customer, BoxNowDefaults.BoxNowOrderLockerID);
        var address = await _genericAttributeService.GetAttributeAsync<string>(customer, BoxNowDefaults.BoxNowOrderAddress);
        var zip = await _genericAttributeService.GetAttributeAsync<string>(customer, BoxNowDefaults.BoxNowOrderPostalCode);

        // Save values on the order
        await _genericAttributeService.SaveAttributeAsync(order, BoxNowDefaults.BoxNowOrderLockerID, lockerId);
        await _genericAttributeService.SaveAttributeAsync(order, BoxNowDefaults.BoxNowOrderAddress, address);
        await _genericAttributeService.SaveAttributeAsync(order, BoxNowDefaults.BoxNowOrderPostalCode, zip);
    }
}
