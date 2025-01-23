using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RentalsProAPIV8.Client.API.Interface;
using RentalsProAPIV8.Core.Extensions;

namespace RentalsProAPIV8.Client.API
{
    public class ApiManager : IApiManager
    {
        public async Task<ApiResponse<T>> ExecuteRequestAsync<T>(HttpMethod method, string uri, object? payload = null, bool isRetry = false, int apiTimeout = ApiConstants.DefaultTimeout, CancellationToken cancellationToken = default)
        {
            return await HandleResponseAsync(() => SendRequestAsync<T>(method, uri, payload, apiTimeout, cancellationToken), isRetry);
        }

        private async Task<ApiResponse<T>> SendRequestAsync<T>(HttpMethod method, string uri, object? payload, int apiTimeout, CancellationToken cancellationToken)
        {
            using var request = CreateRequestMessage(method, uri, payload);
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(apiTimeout));

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new ApiResponse<T>(response.StatusCode, DeserializeContent<T>(content), "Request successful.");
                    }
                    return new ApiResponse<T>(response.StatusCode, default, $"Request failed with status code {response.StatusCode}: {content}");
                }
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"The request to {uri} timed out.", ex);
            }
        }

        private async Task<ApiResponse<T>> HandleResponseAsync<T>(Func<Task<ApiResponse<T>>> requestFunc, bool isRetry)
        {
            try
            {
                return await requestFunc();
            }
            catch (HttpRequestException ex) when (!isRetry)
            {
                return await HandleResponseAsync(requestFunc, true); // Retry once
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        private static HttpRequestMessage CreateRequestMessage(HttpMethod method, string uri, object? payload)
        {
            var request = new HttpRequestMessage(method, uri);

            if (payload != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch))
            {
                string json = JsonConvert.SerializeObject(payload);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return request;
        }

        private static T? DeserializeContent<T>(string content)
        {
            if (content.IsEmpty())
                return default;
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
