using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using abbuffet.Backend.Services;
using abbuffet.Backend.Services.Implementations;
using abbuffet.Backend;
using Microsoft.Extensions.Configuration;
using WebAppServiceLibrary.Providers;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace abbuffet.Backend
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ITablesService, TablesService>();
            builder.Services.AddTransient<IOrdersService, OrdersService>();
        }
    }
}