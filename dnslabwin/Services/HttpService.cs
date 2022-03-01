using dnslabwin.Utilities;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dnslabwin.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions defaultJsonSerializationOption
            = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        //private const string BaseAddress = "http://192.168.1.7";
        private const string BaseAddress = "http://api.dnslab.ir";
        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            string token = SettingsUtility.Get(SettingKeys.Token);
            if (!String.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
        public async Task<HttpResponseWraper<object>> Post<T>(string url, T data)
        {
            url = BaseAddress + url;
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            return new HttpResponseWraper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWraper<TResponse>> Get<TResponse>(string url)
        {
            url = BaseAddress + url;

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializationOption);
                return new HttpResponseWraper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWraper<TResponse>(default, false, response);
            }
        }

        public async Task<HttpResponseWraper<TResponse>> Put<T, TResponse>(string url, T data)
        {
            url = BaseAddress + url;

            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializationOption);
                return new HttpResponseWraper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWraper<TResponse>(default, false, response);
            }
        }

        public async Task<HttpResponseWraper<object>> Put<T>(string url, T data)
        {
            url = BaseAddress + url;

            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, stringContent);

            return new HttpResponseWraper<object>(null, response.IsSuccessStatusCode, response);

        }


        public async Task<HttpResponseWraper<TResponse>> Delete<TResponse>(string url)
        {
            url = BaseAddress + url;

            var response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializationOption);
                return new HttpResponseWraper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWraper<TResponse>(default, false, response);
            }
        }

        public async Task<HttpResponseWraper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            url = BaseAddress + url;

            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializationOption);
                return new HttpResponseWraper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWraper<TResponse>(default, false, response);
            }
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }
    }
}
