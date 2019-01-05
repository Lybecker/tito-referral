using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Configuration;
using WebAPI.Model;

namespace WebAPI.Services
{
    // Using the HttpClientFactory https://www.talkingdotnet.com/3-ways-to-use-httpclientfactory-in-asp-net-core-2-1/
    public class TitoClient : ITitoClient
    {
        private readonly TitoConfiguration _config;
        private readonly ILogger<TitoClient> _logger;

        public string AccountName { get; set; }
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public TitoClient(HttpClient httpClient, TitoConfiguration config, ILogger<TitoClient> logger)
        {
            _config = config;
            _logger = logger;

            httpClient.BaseAddress = new Uri("https://api.tito.io/v3/");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Token", "token=" + _config.ApiToken);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client = httpClient;


            AccountName = _config.AccountName;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }

        public async Task<string> GetEventsAsync()
        {
            return await _client.GetStringAsync($"{AccountName}/events");
        }

        public async Task<Discount_Code> GetDiscountCodeAsync(string eventName, int id)
        {
            var jsonResponse = await _client.GetStringAsync($"{AccountName}/{eventName}/discount_codes/{id}");

            var root = JsonConvert.DeserializeObject<RootDiscount_Code>(jsonResponse, _jsonSerializerSettings);

            return root.discount_code;
        }

        public async Task<Discount_Code> CreateDiscountCodeAsync(string eventName, Discount_Code discount)
        {
            var json = JsonConvert.SerializeObject(discount, _jsonSerializerSettings);

            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var response = await _client.PostAsync($"{AccountName}/{eventName}/discount_codes/",
                    content);

                if (!response.IsSuccessStatusCode)
                    return null;

                string jsonResponse = await response.Content.ReadAsStringAsync();
                response.Dispose();

                var root = JsonConvert.DeserializeObject<RootDiscount_Code>(jsonResponse, _jsonSerializerSettings);
                return root.discount_code;
            }
        }
    }
}