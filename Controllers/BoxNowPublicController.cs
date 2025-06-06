﻿using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Services.Common;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Shipping.BoxNow.Controllers {
    public class BoxNowPublicController : BasePluginController {

        protected readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;

        public BoxNowPublicController(IWorkContext workContext, IShoppingCartService shoppingCartService, IGenericAttributeService genericAttributeService) {
            _workContext = workContext;
            _genericAttributeService = genericAttributeService;
        }

        public IActionResult ShowMap() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetSelectedLocker([FromBody] BoxNowSelectedItem selectedItem) {
            var customer = await _workContext.GetCurrentCustomerAsync();
            await _genericAttributeService.SaveAttributeAsync(customer, BoxNowDefaults.BoxNowOrderLockerID, selectedItem.LockerID);
            await _genericAttributeService.SaveAttributeAsync(customer, BoxNowDefaults.BoxNowOrderAddress, selectedItem.AddressLine1);
            await _genericAttributeService.SaveAttributeAsync(customer, BoxNowDefaults.BoxNowOrderPostalCode, selectedItem.PostalCode);
            return NoContent();
        }
    }
}
