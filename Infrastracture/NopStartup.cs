using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Shipping.BoxNow.Services.Implementations;
using Nop.Plugin.Shipping.BoxNow.Services.Interfaces;

namespace Nop.Plugin.Payments.VivaPayments.Infrastracture;
internal class NopStartup : INopStartup {
    public int Order => 1;

    public void Configure(IApplicationBuilder application) {
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<IBoxNowService, BoxNowService>();
    }
}
