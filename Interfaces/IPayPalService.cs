using System.Collections.Generic;
using System.Threading.Tasks;
using PayPal.Models;
using PayPal.Shared;

namespace PayPal.Interfaces
{

    public interface IPayPalService<T> where T : class
    {
        Task<IReadOnlyList<T>> ListAsync(); //int page_size = 20, int page = 1, bool total_required = true);

        Task<T> CreateAsync(T model);

        Task<bool> UpdateAsync(string id, RequestPatch model);
        
        Task<T> ReviseAsync(string id, T model);

        Task<T> DetailsAsync(string id);

        Task<bool> ActivateAsync(string id, string reason);

        Task<bool> DeactivateAsync(string id);
    }

    public interface IProductService : IPayPalService<Product>
    {
    }

    public interface IPlanService : IPayPalService<Plan>
    {
        Task<bool> UpdatePricingAsync(string id, PricingScheme model);
    }

    public interface ISubscriptionService : IPayPalService<Subscription>
    {
        Task<bool> SuspendAsync(string id, string reason);
        Task<bool> ActivateAsync(string id, string reason);
        
        Task<bool> CancelAsync(string id, string reason);
    }
}