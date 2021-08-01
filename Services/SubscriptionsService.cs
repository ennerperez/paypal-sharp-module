using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Interfaces;
using PayPal.Models;
using PayPal.Shared;

namespace PayPal.Services
{
    public class SubscriptionsesService : ISubscriptionsService
    {
        private readonly Factory _factory;

        public SubscriptionsesService(Factory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(Factory));
        }

        public Task<IReadOnlyList<Subscription>> ListAsync() //int page_size = 20, int page = 1, bool total_required = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Subscription> CreateAsync(Subscription model)
        {
            var _client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await _client.HttpClient.PostAsync($"{_client.Url}v1/billing/subscriptions", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to create the subscription.");

            var responseModel = await _client.ProcessResponse<JObject>(result);
            return responseModel.Data.ToObject<Subscription>();
        }

        public async Task<bool> UpdateAsync(string id, RequestPatch model)
        {
            var _client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await _client.HttpClient.PatchAsync($"{_client.Url}v1/billing/subscriptions/{id}", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to update the subscription.");

            return true;
        }

        public async Task<Subscription> DetailsAsync(string id)
        {
            var _client = await _factory.CreateAsync();

            var result = await _client.HttpClient.GetAsync($"{_client.Url}v1/billing/subscriptions/{id}");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to open the subscription.");

            var responseModel = await _client.ProcessResponse<JObject>(result);
            return responseModel.Data.ToObject<Subscription>();
        }

        public async Task<bool> ActivateAsync(string id)
        {
            var _client = await _factory.CreateAsync();

            var result = await _client.HttpClient.GetAsync($"{_client.Url}v1/billing/subscriptions/{id}/activate");
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to activate the subscription.");

            return true;
        }

        public Task<bool> DeactivateAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CancelAsync(string id, string reason)
        {
            var _client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(new {reason = reason}, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await _client.HttpClient.PostAsync($"{_client.Url}v1/billing/subscriptions/{id}/cancel", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to cancel the subscription.");

            return true;
        }

        public async Task<bool> SuspendAsync(string id, string reason)
        {
            var _client = await _factory.CreateAsync();

            var data = JsonConvert.SerializeObject(new {reason = reason}, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await _client.HttpClient.PostAsync($"{_client.Url}v1/billing/subscriptions/{id}/suspend", content);
            if (!result.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Unable to suspend the subscription.");

            return true;
        }
    }
}