
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Shipping.BoxNow.Components {
    public class BoxNowConfigure : NopViewComponent {
        private readonly BoxNowSettings _settingService;

        public BoxNowConfigure(BoxNowSettings settingService) {
            _settingService = settingService;
        }

        /*public IViewComponentResult Invoke() {
            var model = new BoxNowSettings();
            model.PublicToken = _settingService.GetSettingByKey<string>("Shipping.BoxNow.PublicToken");
            model.PrivateToken = _settingService.GetSettingByKey<string>("Shipping.BoxNow.PrivateToken");

            return View("~/Plugins/Shipping.BoxNow/Views/Configure.cshtml", model);
        }*/
    }
}
