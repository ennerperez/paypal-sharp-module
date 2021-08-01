using PayPal.Interfaces;

namespace PayPal
{
    public class Wrapper
    {
        private readonly IProductsService _productsService;
        private readonly IPlansService _plansService;
        private readonly ISubscriptionsService _subscriptionsService;

        public Wrapper(IProductsService productsService, IPlansService plansService, ISubscriptionsService subscriptionsService)
        {
            _productsService = productsService;
            _plansService = plansService;
            _subscriptionsService = subscriptionsService;
        }

        public IProductsService Products => _productsService;
        public IPlansService Plans => _plansService;
        public ISubscriptionsService Subscriptions => _subscriptionsService;
    }
}