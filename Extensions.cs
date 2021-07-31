using System;
using Microsoft.Extensions.DependencyInjection;
using Abstractions.Services;
using Infrastructure.Models;
using Infrastructure.Services;

namespace Infrastructure
{
	public static class Extensions
	{

		public static IServiceCollection AddPayPalServices(this IServiceCollection services, Action<GatewayOptions> configureOptions)
		{
			services.AddSingleton<IPaymentGatewayService, PaymentGatewayService>().Configure(configureOptions);

			return services;
		}
	}
}