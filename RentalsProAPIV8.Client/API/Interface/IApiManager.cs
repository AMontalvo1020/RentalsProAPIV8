using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.API.Interface
{
    public interface IApiManager
    {
        Task<ApiResponse<T>> ExecuteRequestAsync<T>(HttpMethod method, string uri, object? payload = null, bool isRetry = false, int apiTimeout = ApiConstants.DefaultTimeout, CancellationToken cancellationToken = default);
    }
}
