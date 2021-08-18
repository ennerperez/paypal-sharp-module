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
            
            services.AddSingleton<IPayPalService<Product>, ProductService>();
            services.AddSingleton<IPayPalService<Plan>, PlanService>();
            services.AddSingleton<IPayPalService<Subscription>, SubscriptionService>();
            
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IPlanService, PlanService>();
            services.AddSingleton<ISubscriptionService, SubscriptionService>();
            
            services.AddSingleton<Wrapper>();
            
            return services;
        }
    }
}