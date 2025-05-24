using Microsoft.AspNetCore.Mvc;
using Nop.Data;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Shipping.BoxNow.Controllers {
    public class BoxNowPublicController : BasePluginController {

        public BoxNowPublicController() {
        }

        public IActionResult ShowMap() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetSelectedLocker(int lockerId) {
            // Logic to call delivery-request API
            return Json(new { success = true });
        }
    }
}
