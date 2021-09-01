using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Interfaces;
using PayPal.Models;
using PayPal.Shared;

namespace PayPal.Services
{
    public class ProductService : IProductService
    {
        private readonly Factory _factory;

        public ProductService(Factory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<IReadOnlyList<Product>> ListAsync() //int page_size = 20, int page = 1, bool total_required = true)
        {
            var client = await _factory.CreateAsync();

            var result = await client.HttpClient.GetAsync($"{client.Url}v1/catalogs/products");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to get the product list.");

            var responseModel = await client.ProcessResponse<JArray>(result, "products");
            return responseModel.Data.ToObject<IReadOnlyList<Product>>();
        }

        public async Task<Product> CreateAsync(Product model)
        {
            var client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await client.HttpClient.PostAsync($"{client.Url}v1/catalogs/products", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to create the product.");

            var responseModel = await client.ProcessResponse<JObject>(result);
            return responseModel.Data.ToObject<Product>();
        }

        public async Task<bool> UpdateAsync(string id, RequestPatch model)
        {
            var client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await client.HttpClient.PatchAsync($"{client.Url}v1/catalogs/products/{id}", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to update the product.");

            return true;
        }

        public Task<Product> ReviseAsync(string id, Product model)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> DetailsAsync(string id)
        {
            var client = await _factory.CreateAsync();

            var result = await client.HttpClient.GetAsync($"{client.Url}v1/catalogs/products/{id}");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to open the product.");

            var responseModel = await client.ProcessResponse<JObject>(result);
            return responseModel.Data.ToObject<Product>();
        }

        public Task<bool> ActivateAsync(string id, string reason)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivateAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}