using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using static RentalsProAPIV8.Client.Constants.Enums;

namespace RentalsProAPIV8.Client.API
{
    public static class ApiEndpoints
    {
        private const string v1 = "v1";
        public enum Actions
        {
            GetProperty,
            PatchPropertyStatus,
            PatchPropertyPaymentStatus,
            UpdateProperty,
            PostForProperties,
            GetPropertyType,
            GetPropertyTypes,
            GetStatuses,
            GetPaymentStatuses,
            GetLease,
            UpdateLease,
            PostLease,
            PostForLease,
            GetUnit,
            GetUnits,
            PatchUnitStatus,
            PatchUnitPaymentStatus,
            PostUnit,
            PostUnits,
            GetUser,
            UpdateUser,
            PostForValidUser,
            PostForUsers
        }

        public static class Paths
        {
            public static string GetPropertyUri(string baseUrl, int PropertyID)
            {
                return BuildUri(baseUrl, ApiEndpoint.Properties, Actions.GetProperty, (nameof(PropertyID), PropertyID));
            }

            public static string PostForPropertiesUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Properties, Actions.PostForProperties);
            }

            public static string PatchPropertyStatusUri(string baseUrl, int PropertyID, int StatusID)
            {
                return BuildUri(baseUrl, ApiEndpoint.Properties, Actions.PatchPropertyStatus, (nameof(PropertyID), PropertyID), (nameof(StatusID), StatusID));
            }

            public static string PatchPropertyPaymentStatusUri(string baseUrl, int PropertyID, int StatusID)
            {
                return BuildUri(baseUrl, ApiEndpoint.Properties, Actions.PatchPropertyPaymentStatus, (nameof(PropertyID), PropertyID), (nameof(StatusID), StatusID));
            }

            public static string UpdatePropertyUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Properties, Actions.UpdateProperty);
            }

            public static string GetStatusesUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Statuses, Actions.GetStatuses);
            }

            public static string GetPaymentStatusesUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.PaymentStatuses, Actions.GetPaymentStatuses);
            }

            public static string GetPropertyTypesUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.PropertyTypes, Actions.GetPropertyTypes);
            }

            public static string UpdateLeaseUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Leases, Actions.UpdateLease);
            }

            public static string PostLeaseUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Leases, Actions.PostLease);
            }

            public static string PostForLeaseUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Leases, Actions.PostForLease);
            }

            public static string GetUnitUri(string baseUrl, int UnitID)
            {
                return BuildUri(baseUrl, ApiEndpoint.Units, Actions.GetUnit, (nameof(UnitID), UnitID));
            }

            public static string GetUnitsUri(string baseUrl, int PropertyID, bool Active)
            {
                return BuildUri(baseUrl, ApiEndpoint.Units, Actions.GetUnits, (nameof(PropertyID), PropertyID), (nameof(Active), Active));
            }

            public static string PatchUnitStatusUri(string baseUrl, int UnitID, int StatusID)
            {
                return BuildUri(baseUrl, ApiEndpoint.Units, Actions.PatchUnitStatus, (nameof(UnitID), UnitID), (nameof(StatusID), StatusID));
            }

            public static string PatchUnitPaymentStatusUri(string baseUrl, int UnitID, int StatusID)
            {
                return BuildUri(baseUrl, ApiEndpoint.Units, Actions.PatchUnitPaymentStatus, (nameof(UnitID), UnitID), (nameof(StatusID), StatusID));
            }

            public static string PostUnitUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Units, Actions.PostUnit);
            }

            public static string PostUnitsUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Units, Actions.PostUnits);
            }

            public static string GetUserUri(string baseUrl, int UserID)
            {
                return BuildUri(baseUrl, ApiEndpoint.Users, Actions.GetUser, (nameof(UserID), UserID));
            }

            public static string UpdateUserUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Users, Actions.UpdateUser);
            }

            public static string PostForValidUserUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Users, Actions.PostForValidUser);
            }

            public static string PostForUsersUri(string baseUrl)
            {
                return BuildUri(baseUrl, ApiEndpoint.Users, Actions.PostForUsers);
            }

            private static string BuildUri(string baseUrl, ApiEndpoint endpoint, Actions action, params (string Key, object? Value)[] queryParams)
            {
                var apiSegment = endpoint.ToString();
                var apiAction = action.ToString();

                var query = string.Join("&", queryParams.Where(q => q.Value != null).Select(q => $"{Uri.EscapeDataString(q.Key)}={Uri.EscapeDataString(q.Value.ToString()!)}"));

                return $"{baseUrl}/api/{v1}/{apiSegment}/{apiAction}{(string.IsNullOrEmpty(query) ? string.Empty : $"?{query}")}";
            }
        }
    }
}
