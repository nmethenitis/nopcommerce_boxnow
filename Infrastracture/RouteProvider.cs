﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Shipping.BoxNow.Infrastracture;
public class RouteProvider : IRouteProvider {
    public int Priority => 100;

    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder) {
        endpointRouteBuilder.MapControllerRoute(
           name: BoxNowDefaults.BoxNowSelectedItem,
           pattern: "box-now/selected-item",
           defaults: new { controller = "BoxNowPublic", action = "SetSelectedLocker" });
    }
}
