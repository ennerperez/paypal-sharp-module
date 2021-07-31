using System;
using Microsoft.Extensions.DependencyInjection;
using PayPal.Interfaces;
using PayPal.Models;
using PayPal.Services;

namespace PayPal
{
    public static class Extensions
    {
        public static IServiceCollection AddPayPalServices(this IServiceCollection services, Action<GatewayOptions> configureOptions)
        {
            services.AddSingleton<IGatewayService, GatewayService>().Configure(configureOptions);
            return services;
        }
    }
}