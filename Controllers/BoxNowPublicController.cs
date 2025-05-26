using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;
using Nop.Core.Domain.Orders;
using Nop.Services.Attributes;
using Nop.Services.Common;

namespace Nop.Plugin.Shipping.BoxNow.Controllers {
    public class BoxNowPublicController : BasePluginController {

        protected readonly IWorkContext _workContext;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IGenericAttributeService _genericAttributeService;

        public BoxNowPublicController(IWorkContext workContext, IShoppingCartService shoppingCartService, IGenericAttributeService genericAttributeService) {
            _workContext = workContext;
            _shoppingCartService = shoppingCartService;
            _genericAttributeService = genericAttributeService;
        }

        public IActionResult ShowMap() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetSelectedLocker([FromBody]BoxNowSelectedItem selectedItem) {
            var customer = await _workContext.GetCurrentCustomerAsync();
            await _genericAttributeService.SaveAttributeAsync(customer, BoxNowDefaults.BoxNowOrderLockerID, selectedItem.LockerID);
            await _genericAttributeService.SaveAttributeAsync(customer, BoxNowDefaults.BoxNowOrderAddress, selectedItem.AddressLine1);
            await _genericAttributeService.SaveAttributeAsync(customer, BoxNowDefaults.BoxNowOrderPostalCode, selectedItem.PostalCode);
            return NoContent();
        }
    }
}
