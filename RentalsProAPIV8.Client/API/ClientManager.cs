using System.Net;
using RentalsProAPIV8.Client.API.Interface;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using static RentalsProAPIV8.Client.API.ApiEndpoints;

namespace RentalsProAPIV8.Client.API
{
    public class ClientManager
    {
        static string _apiUrl = string.Empty;
        static IApiManager _apiClient;

        public ClientManager(string apiUrl, IApiManager apiClient)
        {
            _apiUrl = apiUrl;
            _apiClient = apiClient;
        }

        // Property-related methods
        public static async Task<ApiResponse<PropertyDTO>> GetPropertyAsync(int propertyID)
        {
            return await ExecuteReadResponseAsync<PropertyDTO>(Paths.GetPropertyUri(_apiUrl, propertyID));
        }

        public static async Task<ApiResponse<List<PropertyDTO>>> PostForPropertiesAsync(PostForPropertiesParameters parameters)
        {
            return await ExecutePostResponseAsync<List<PropertyDTO>>(Paths.PostForPropertiesUri(_apiUrl), parameters);
        }

        public static async Task<ApiResponse<string>> PatchPropertyStatusAsync(int propertyID, int statusID)
        {
            return await ExecutePatchResponseAsync<string>(Paths.PatchPropertyStatusUri(_apiUrl, propertyID, statusID));
        }

        public static async Task<ApiResponse<string>> PatchPropertyPaymentStatusAsync(int propertyID, int statusID)
        {
            return await ExecutePatchResponseAsync<string>(Paths.PatchPropertyPaymentStatusUri(_apiUrl, propertyID, statusID));
        }

        public static async Task<ApiResponse<string>> UpdatePropertyAsync(PropertyDTO parameters)
        {
            return await ExecutePutResponseAsync<string>(Paths.UpdatePropertyUri(_apiUrl), parameters);
        }

        // Status-related methods
        public static async Task<ApiResponse<List<StatusDTO>>> GetStatusesAsync()
        {
            return await ExecuteReadResponseAsync<List<StatusDTO>>(Paths.GetStatusesUri(_apiUrl));
        }

        // Payment Status-related methods
        public static async Task<ApiResponse<List<StatusDTO>>> GetPaymentStatusesAsync()
        {
            return await ExecuteReadResponseAsync<List<StatusDTO>>(Paths.GetPaymentStatusesUri(_apiUrl));
        }

        // Lease-related methods
        public static async Task<ApiResponse<LeaseDTO>> PostForLeaseAsync(PostForLeaseParameters parameters)
        {
            return await ExecutePostResponseAsync<LeaseDTO>(Paths.PostForLeaseUri(_apiUrl), parameters);
        }

        public static async Task<ApiResponse<string>> UpdateLeaseAsync(LeaseDTO leaseDTO)
        {
            return await ExecutePutResponseAsync<string>(Paths.UpdateLeaseUri(_apiUrl), leaseDTO);
        }

        //public static async Task<ApiResponse<HttpStatusCode>> UpdateLeaseAsync(LeaseDTO leaseDTO)
        //{
        //    return await ExecutePutResponseAsync<HttpStatusCode>(Paths.UpdateLeaseUri(_apiUrl), leaseDTO);
        //}

        public static async Task<ApiResponse<string>> PostLeaseAsync(LeaseDTO parameters)
        {
            return await ExecutePostResponseAsync<string>(Paths.PostLeaseUri(_apiUrl), parameters);
        }

        //public static async Task<ApiResponse<bool>> PostLeaseAsync(LeaseDTO parameters)
        //{
        //    return await ExecutePostResponseAsync<bool>(Paths.PostLeaseUri(_apiUrl), parameters);
        //}

        // Type-related methods
        public static async Task<ApiResponse<List<TypeDTO>>> GetPropertyTypesAsync()
        {
            return await ExecuteReadResponseAsync<List<TypeDTO>>(Paths.GetPropertyTypesUri(_apiUrl));
        }

        // Unit-related methods
        public static async Task<ApiResponse<UnitDTO>> GetUnitAsync(int UnitID)
        {
            return await ExecuteReadResponseAsync<UnitDTO>(Paths.GetUnitUri(_apiUrl, UnitID));
        }

        public static async Task<ApiResponse<List<UnitDTO>>> GetUnitsAsync(int PropertyID, bool Active)
        {
            return await ExecuteReadResponseAsync<List<UnitDTO>>(Paths.GetUnitsUri(_apiUrl, PropertyID, Active));
        }

        public static async Task<ApiResponse<bool>> PatchUnitStatusAsync(int unitID, int statusID)
        {
            return await ExecutePatchResponseAsync<bool>(Paths.PatchUnitStatusUri(_apiUrl, unitID, statusID));
        }

        public static async Task<ApiResponse<bool>> PatchUnitPaymentStatusAsync(int unitID, int statusID)
        {
            return await ExecutePatchResponseAsync<bool>(Paths.PatchUnitPaymentStatusUri(_apiUrl, unitID, statusID));
        }

        public static async Task<ApiResponse<bool>> PostUnitAsync(UnitDTO unit)
        {
            return await ExecutePostResponseAsync<bool>(Paths.PostUnitUri(_apiUrl), unit);
        }

        public static async Task<ApiResponse<bool>> PostUnitsAsync(List<UnitDTO> units)
        {
            return await ExecutePostResponseAsync<bool>(Paths.PostUnitsUri(_apiUrl), units);
        }

        // User-related methods
        public static async Task<ApiResponse<UserDTO>> GetUserAsync(int UserID)
        {
            return await ExecuteReadResponseAsync<UserDTO>(Paths.GetUserUri(_apiUrl, UserID));
        }

        public static async Task<ApiResponse<bool>> PutUserAsync(UserDTO userDTO)
        {
            return await ExecutePutResponseAsync<bool>(Paths.UpdateUserUri(_apiUrl), userDTO);
        }

        public static async Task<ApiResponse<UserDTO>> PostForValidUserAsync(ValidUserParameters parameters)
        {
            return await ExecutePostResponseAsync<UserDTO>(Paths.PostForValidUserUri(_apiUrl), parameters);
        }

        public static async Task<ApiResponse<List<UserDTO>>> PostForUsersAsync(PostForUserParameters parameters)
        {
            return await ExecutePostResponseAsync<List<UserDTO>>(Paths.PostForUsersUri(_apiUrl), parameters);
        }

        // Wrapper methods for specific HTTP verbs
        private static async Task<ApiResponse<T>> ExecuteReadResponseAsync<T>(string uri)
        {
            return await _apiClient.ExecuteRequestAsync<T>(HttpMethod.Get, uri);
        }

        private static async Task<ApiResponse<T>> ExecutePostResponseAsync<T>(string uri, object? data = null)
        {
            return await _apiClient.ExecuteRequestAsync<T>(HttpMethod.Post, uri, data);
        }

        private static async Task<ApiResponse<T>> ExecutePatchResponseAsync<T>(string uri, object? data = null)
        {
            return await _apiClient.ExecuteRequestAsync<T>(HttpMethod.Patch, uri, data);
        }

        private static async Task<ApiResponse<T>> ExecutePutResponseAsync<T>(string uri, object? data = null)
        {
            return await _apiClient.ExecuteRequestAsync<T>(HttpMethod.Put, uri, data);
        }
    }
}
