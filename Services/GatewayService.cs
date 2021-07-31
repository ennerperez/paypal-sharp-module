using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PayPal.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Interfaces;

namespace PayPal.Services
{
	public class GatewayService : IGatewayService
	{
		public static string AuthToken;
		private readonly ILogger _logger;
		private DateTime _expiresIn;

		private readonly GatewayOptions _options;

		public GatewayService(ILoggerFactory loggerFactory, IOptionsMonitor<GatewayOptions> optionsMonitor)
		{
			_logger = loggerFactory.CreateLogger(GetType());
			_options = optionsMonitor.CurrentValue;
		}

		public async Task GetTokenAsync()
		{
			if (!string.IsNullOrEmpty(AuthToken) && DateTime.Now < _expiresIn) return;

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

				var contentKeys = new Dictionary<string, string>();
				contentKeys.Add("grant_type", "client_credentials");

				using (var content = new FormUrlEncodedContent(contentKeys))
				{
					var bytes = Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.ClientSecret}");
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

					var result = await httpClient.PostAsync($"{_options.Url}v1/oauth2/token", content);
					if (result.IsSuccessStatusCode)
					{
						var data = await result.Content.ReadAsStringAsync();
						if (!string.IsNullOrWhiteSpace(data))
						{
							var jobject = JObject.Parse(data);
							AuthToken = jobject.GetValue("access_token")?.ToString();
#if DEBUG
							_logger.LogInformation($"AuthToken: {AuthToken}");
#endif

							if (!string.IsNullOrWhiteSpace(AuthToken))
							{
								var expiresIn = int.Parse(jobject.GetValue("expires_in")?.ToString() ?? "3600");
								_expiresIn = DateTime.Now.AddMinutes(expiresIn);
							}
						}
					}
					else
					{
						throw new UnauthorizedAccessException("Unable to get token.");
					}
				}
			}
		}

		[Obsolete]
		public async Task<T> CreateProductAsync<T>(T input)
		{
			await GetTokenAsync();

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);

				var data = JsonConvert.SerializeObject(input, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc });
				var content = new StringContent(data, Encoding.UTF8, "application/json");

				var result = await httpClient.PostAsync($"{_options.Url}v1/catalogs/products", content);
				if (result.IsSuccessStatusCode)
				{
					var responseModel = await ProcessResponse<JObject>(result);
					return responseModel.Data.ToObject<T>();
				}
				else
				{
					throw new UnauthorizedAccessException("Unable to create the product.");
				}
			}
		}

		public async Task<T> CreatePlanAsync<T>(T input)
		{
			await GetTokenAsync();

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);

				var data = JsonConvert.SerializeObject(input, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
				var content = new StringContent(data, Encoding.UTF8, "application/json");

				var result = await httpClient.PostAsync($"{_options.Url}v1/billing/plans", content);
				if (result.IsSuccessStatusCode)
				{
					var responseModel = await ProcessResponse<JObject>(result);
					return responseModel.Data.ToObject<T>();
				}
				else
				{
					throw new UnauthorizedAccessException("Unable to create the plan.");
				}
			}
		}
		
		public async Task<bool> ActivatePlanAsync(string id)
		{
			await GetTokenAsync();

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);

				var result = await httpClient.PostAsync($"{_options.Url}/v1/billing/plans/{id}/activate",null);
				if (result.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					throw new UnauthorizedAccessException("Unable to deactivate the plan.");
				}
			}
		}

		public async Task<bool> DeactivatePlanAsync(string id)
		{
			await GetTokenAsync();

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);

				var result = await httpClient.PostAsync($"{_options.Url}/v1/billing/plans/{id}/deactivate",null);
				if (result.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					throw new UnauthorizedAccessException("Unable to deactivate the plan.");
				}
			}
		}

		public async Task<T> CreateSubscriptionAsync<T>(T input)
		{
			await GetTokenAsync();

			var id = input.GetType().GetProperty("Id")?.GetValue(input).ToString(); // Reflexion

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
				
				var data = JsonConvert.SerializeObject(input, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc});
				var content = new StringContent(data, Encoding.UTF8, "application/json");

				var result = await httpClient.PostAsync($"{_options.Url}v1/billing/subscriptions", content);
				if (result.IsSuccessStatusCode)
				{
					var responseModel = await ProcessResponse<JObject>(result);
					return responseModel.Data.ToObject<T>();
				}
				else
				{
					throw new UnauthorizedAccessException("Unable to create the subscription.");
				}
			}
		}
		
		
		public async Task<ProcessEntity<T>> ProcessResponse<T>(HttpResponseMessage response, string subnode = "")
		{
			if (null == response) throw new ArgumentNullException("response");

			if (!response.IsSuccessStatusCode)
			{
				Error errorError;
				try
				{
					var rawErrorResponse = await response.Content.ReadAsStringAsync();

					if (_options.Debug)
					{
						if (!string.IsNullOrWhiteSpace(rawErrorResponse))
						{
							try
							{
								var path = "responses";
#if DEBUG
								path = Path.Combine("bin", "Debug", "net5.0", "responses");
#endif
								if (!Directory.Exists(path)) Directory.CreateDirectory(path);
								var file = Path.Combine(path, DateTime.Now.Ticks.ToString() + ".json");
								File.WriteAllText(file, rawErrorResponse);
							}
							catch (Exception e)
							{
								Console.WriteLine(e);
							}
						}
					}

					if (string.IsNullOrWhiteSpace(rawErrorResponse)) throw new InvalidDataException();

					errorError = JsonConvert.DeserializeObject<Error>(rawErrorResponse);
				}
				catch (Exception exception)
				{
					return new ProcessEntity<T> {Error = new InvalidOperationException("API call did not completed successfully or response parse error occurred", exception)};
				}

				if (null == errorError || string.IsNullOrWhiteSpace(errorError.Title)) return new ProcessEntity<T> {Error = new InvalidOperationException("API call did not completed successfully or response parse error occurred")};

				return new ProcessEntity<T> {Error = new InvalidOperationException(errorError.Description)};
			}

			if (typeof(T) == typeof(bool)) return new ProcessEntity<T> {Data = (T) (object) response.IsSuccessStatusCode};

			try
			{
				var rawResponseContent = await response.Content.ReadAsStringAsync();

				if (_options.Debug)
				{
					if (!string.IsNullOrWhiteSpace(rawResponseContent))
					{
						try
						{
							var path = "responses";
#if DEBUG
							path = Path.Combine("bin", "Debug", "net5.0", "responses");
#endif
							if (!Directory.Exists(path)) Directory.CreateDirectory(path);
							var file = Path.Combine(path, DateTime.Now.Ticks.ToString() + ".json");
							File.WriteAllText(file, rawResponseContent);
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
						}
					}
				}

				if (string.IsNullOrWhiteSpace(rawResponseContent)) throw new InvalidDataException();

				if (!string.IsNullOrWhiteSpace(subnode))
				{
					var innerNodeContent = JsonConvert.DeserializeObject<JObject>(rawResponseContent);
					if (innerNodeContent.ContainsKey(subnode) && innerNodeContent[subnode] != null)
					{
						var data = innerNodeContent[subnode].ToObject<T>();
						return new ProcessEntity<T> {Data = data};
					}
				}

				return new ProcessEntity<T> {Data = JsonConvert.DeserializeObject<T>(rawResponseContent)};
			}
			catch (Exception exception)
			{
				return new ProcessEntity<T> {Error = new InvalidOperationException("API call did not completed successfully or response parse error occurred", exception)};
			}
		}
	}
}