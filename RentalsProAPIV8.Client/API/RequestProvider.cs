using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using RentalsProAPIV8.Client.API.Interface;
using System.Text.Json;
using RentalsProAPIV8.Core.Extensions;

namespace RentalsProAPIV8.Client.API
{
    public class RequestProvider(HttpMessageHandler _messageHandler) : IRequestProvider
    {
        private readonly Lazy<HttpClient> _httpClient = new(() =>
        {
            var httpClient = _messageHandler is not null ? new HttpClient(_messageHandler) : new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        },
        LazyThreadSafetyMode.ExecutionAndPublication);

        public async Task<TResult?> GetAsync<TResult>(string uri, string token = "")
        {
            var httpClient = GetOrCreateHttpClient(token);
            using var response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var result = await ReadFromJsonAsync<TResult>(response.Content);

            return result;
        }

        public async Task<TResult?> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "")
        {
            var httpClient = GetOrCreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var requestContent = SerializeToJson(data);
            using HttpResponseMessage response = await httpClient.PostAsync(uri, requestContent);

            await HandleResponse(response);
            var result = await ReadFromJsonAsync<TResult>(response.Content);

            return result;
        }

        public async Task<bool> PostAsync<TRequest>(string uri, TRequest data, string token = "", string header = "")
        {
            var httpClient = GetOrCreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var requestContent = SerializeToJson(data);
            using var response = await httpClient.PostAsync(uri, requestContent);

            await HandleResponse(response);

            return response.IsSuccessStatusCode;
        }

        public async Task<TResult?> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret)

        {
            var httpClient = GetOrCreateHttpClient(string.Empty);

            if (!clientId.IsEmpty() && !clientSecret.IsEmpty())
            {
                AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
            }

            using var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            using var response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            var result = await ReadFromJsonAsync<TResult>(response.Content);

            return result;
        }

        public async Task<TResult?> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            var httpClient = GetOrCreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var requestContent = SerializeToJson(data);
            using HttpResponseMessage response = await httpClient.PutAsync(uri, requestContent);

            await HandleResponse(response);
            var result = await ReadFromJsonAsync<TResult>(response.Content);

            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            var httpClient = GetOrCreateHttpClient(token);
            await httpClient.DeleteAsync(uri);
        }

        private HttpClient GetOrCreateHttpClient(string token = "")
        {
            var httpClient = _httpClient.Value;

            httpClient.DefaultRequestHeaders.Authorization = !token.IsEmpty() ? new AuthenticationHeaderValue("Bearer", token) : null;

            return httpClient;
        }

        private static void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(parameter))
            {
                return;
            }

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private static void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
        {
            if (httpClient == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                return;
            }

            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        private static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
            }
        }

        private static async Task<T?> ReadFromJsonAsync<T>(HttpContent content)
        {
            using var contentStream = await content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync(contentStream, typeof(T), RentalsProJsonSerializerContext.Default);
            return (T?)data;
        }

        private static JsonContent SerializeToJson<T>(T data)
        {
            var typeInfo = RentalsProJsonSerializerContext.Default.GetTypeInfo(typeof(T)) ?? throw new InvalidOperationException($"Missing type info for {typeof(T)}");
            return JsonContent.Create(data, typeInfo);
        }
    }
}
