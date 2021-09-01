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
    public class PlanService : IPlanService
    {
        private readonly Factory _factory;

        public PlanService(Factory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<IReadOnlyList<Plan>> ListAsync() //int page_size = 20, int page = 1, bool total_required = true)
        {
            var client = await _factory.CreateAsync();

            var result = await client.HttpClient.GetAsync($"{client.Url}v1/billing/plans");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to get the plan list.");

            var responseModel = await client.ProcessResponse<JArray>(result, "plans");
            return responseModel.Data.ToObject<IReadOnlyList<Plan>>();
        }

        public async Task<Plan> CreateAsync(Plan model)
        {
            var client = await _factory.CreateAsync();
            
            var data = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await client.HttpClient.PostAsync($"{client.Url}v1/billing/plans", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to create the plan.");

            var responseModel = await client.ProcessResponse<JObject>(result);
            return responseModel.Data.ToObject<Plan>();
        }

        public async Task<bool> UpdateAsync(string id, RequestPatch model)
        {
            var client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await client.HttpClient.PatchAsync($"{client.Url}v1/billing/plans/{id}", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to update the plan.");

            return true;
        }

        public Task<Plan> ReviseAsync(string id, Plan model)
        {
            throw new NotImplementedException();
        }

        public async Task<Plan> DetailsAsync(string id)
        {
            var client = await _factory.CreateAsync();

            var result = await client.HttpClient.GetAsync($"{client.Url}v1/billing/plans/{id}");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to open the plan.");

            var responseModel = await client.ProcessResponse<JObject>(result);
            return responseModel.Data.ToObject<Plan>();
        }

        public async Task<bool> ActivateAsync(string id, string reason)
        {
            var client = await _factory.CreateAsync();

            var result = await client.HttpClient.GetAsync($"{client.Url}v1/billing/plans/{id}/activate");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to activate the plan.");

            return true;
        }

        public async Task<bool> DeactivateAsync(string id)
        {
            var client = await _factory.CreateAsync();

            var result = await client.HttpClient.GetAsync($"{client.Url}v1/billing/plans/{id}/deactivate");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to deactivate the plan.");

            return true;
        }
        
        public async Task<bool> UpdatePricingAsync(string id, PricingScheme model)
        {
            var client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            
            var result = await client.HttpClient.PostAsync($"{client.Url}v1/billing/plans/{id}/update-pricing-schemes", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to update pricing schemes to the plan.");

            return true;
        }
    }
}