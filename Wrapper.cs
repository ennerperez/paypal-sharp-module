using PayPal.Interfaces;

namespace PayPal
{
    public class Wrapper
    {
        private readonly IProductService _productService;
        private readonly IPlanService _planService;
        private readonly ISubscriptionService _subscriptionService;

        public Wrapper(IProductService productService, IPlanService planService, ISubscriptionService subscriptionService)
        {
            _productService = productService;
            _planService = planService;
            _subscriptionService = subscriptionService;
        }

        public IProductService Product => _productService;
        public IPlanService Plan => _planService;
        public ISubscriptionService Subscription => _subscriptionService;
    }
}