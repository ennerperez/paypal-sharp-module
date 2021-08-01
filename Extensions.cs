using System;
using Microsoft.Extensions.DependencyInjection;
using PayPal.Interfaces;
using PayPal.Models;
using PayPal.Services;

namespace PayPal
{
    public static class Extensions
    {
        public static IServiceCollection AddPayPal(this IServiceCollection services, Action<Options> configureOptions)
        {
            services.AddHttpClient<PayPalService>("PayPalService");
            services.AddSingleton<Factory>().Configure(configureOptions);
            
            services.AddSingleton<IPayPalService<Product>, ProductsService>();
            services.AddSingleton<IPayPalService<Plan>, PlansService>();
            services.AddSingleton<IPayPalService<Subscription>, SubscriptionsesService>();
            
            services.AddSingleton<IProductsService, ProductsService>();
            services.AddSingleton<IPlansService, PlansService>();
            services.AddSingleton<ISubscriptionsService, SubscriptionsesService>();
            
            services.AddSingleton<Wrapper>();
            
            return services;
        }
    }
}