using System;
using System.Threading.Tasks;

namespace PayPal.Interfaces
{
	public interface IGatewayService
	{
		Task GetTokenAsync();

		[Obsolete]
		Task<T> CreateProductAsync<T>(T input);

		Task<T> CreatePlanAsync<T>(T input);

		Task<bool> ActivatePlanAsync(string id);
		Task<bool> DeactivatePlanAsync(string id);

		Task<T> CreateSubscriptionAsync<T>(T input);
	}
}