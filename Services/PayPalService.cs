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
    public class PayPalService
    {
        private readonly ILogger _logger;

        private readonly HttpClient _httpClient;
        //private readonly Utf8JsonSerializer _jsonSerializer;

        private DateTime _expiresIn;
        private string _authToken;

        internal DateTime ExpiresIn => _expiresIn;
        internal string AuthToken => _authToken;
        internal string Url => _options.Url;
        internal HttpClient HttpClient => _httpClient;

        public PayPalService(HttpClient httpClient, ILoggerFactory loggerFactory) //, Utf8JsonSerializer jsonSerializer
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            //_jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _logger = loggerFactory.CreateLogger(GetType());
        }

        private Options _options;

        public void Configure(Options options)
        {
            _options = options;
        }

        public async Task GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_authToken) && DateTime.Now < _expiresIn) return;

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

            var contentKeys = new Dictionary<string, string>();
            contentKeys.Add("grant_type", "client_credentials");

            using (var content = new FormUrlEncodedContent(contentKeys))
            {
                var bytes = Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.ClientSecret}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

                var result = await _httpClient.PostAsync($"{_options.Url}v1/oauth2/token", content);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        var jobject = JObject.Parse(data);
                        _authToken = jobject.GetValue("access_token")?.ToString();
#if DEBUG
                        _logger.LogInformation($"AuthToken: {_authToken}");
#endif

                        if (!string.IsNullOrWhiteSpace(_authToken))
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

        internal async Task<Entity<T>> ProcessResponse<T>(HttpResponseMessage response, string subnode = "")
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
                    return new Entity<T> {Error = new InvalidOperationException("API call did not completed successfully or response parse error occurred", exception)};
                }

                if (null == errorError || string.IsNullOrWhiteSpace(errorError.Title)) return new Entity<T> {Error = new InvalidOperationException("API call did not completed successfully or response parse error occurred")};

                return new Entity<T> {Error = new InvalidOperationException(errorError.Description)};
            }

            if (typeof(T) == typeof(bool)) return new Entity<T> {Data = (T) (object) response.IsSuccessStatusCode};

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
                    if (innerNodeContent != null && innerNodeContent.ContainsKey(subnode) && innerNodeContent[subnode] != null)
                    {
                        var data = innerNodeContent[subnode].ToObject<T>();
                        return new Entity<T> {Data = data};
                    }
                }

                return new Entity<T> {Data = JsonConvert.DeserializeObject<T>(rawResponseContent)};
            }
            catch (Exception exception)
            {
                return new Entity<T> {Error = new InvalidOperationException("API call did not completed successfully or response parse error occurred", exception)};
            }
        }
    }
}