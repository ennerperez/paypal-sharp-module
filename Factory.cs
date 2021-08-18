using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PayPal.Services;

namespace PayPal
{
    public class Factory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Options _options;

        public Factory(IServiceProvider serviceProvider, IOptionsMonitor<Options> optionsMonitor)
        {
            _serviceProvider = serviceProvider;
            _options = optionsMonitor.CurrentValue;
        }

        public async Task<PayPalService> CreateAsync()
        {
            var _client = _serviceProvider.GetRequiredService<PayPalService>();
            _client.Configure(_options); 
            await _client.GetTokenAsync();
            return _client;
        }
    }
}