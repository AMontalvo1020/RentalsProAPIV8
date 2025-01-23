using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.Constants
{
    public class Enums
    {
        public enum HttpMethods
        {
            Get = 1,
            Post = 2,
            Delete = 3,
            Patch = 4,
            Put = 5,
        }

        public enum PropertyStatus : int
        {
            [Description("Rented")]
            Rented = 1,
            [Description("Vacant")]
            Vacant = 2,
            [Description("Eviction")]
            Eviction = 3,
            [Description("Inactive")]
            Inactive = 4,
            [Description("Move Out")]
            MoveOut = 5
        }

        public enum PropertyDocumentAccess
        {
            [Description("Admin")]
            Admin = 1,
            [Description("Company")]
            Company = 2,
            [Description("Tenant")]
            Tenant = 3
        }

        public enum NoticeTypes
        {
            [Description("Late Notice")]
            LateNotice = 1,
            [Description("10 Day Notice")]
            TenDayNotice = 2,
            [Description("Notice To Quit")]
            NoticeToQuit = 3,
            [Description("Notice Of Late Rent And Demand For Payment")]
            NoticeOfLateRentAndDemandForPayment = 3,
        }

        public enum PaymentStatus
        {
            [Description("Paid")]
            Paid = 1,
            [Description("Unpaid")]
            Unpaid = 2,
        }

        public enum DocumentTypes
        {
            [Description("PDF")]
            PDF = 1,
            [Description("Excel")]
            Excel = 2,
        }

        public enum UnitStatus
        {
            [Description("Rented")]
            Rented = 1,
            [Description("Vacant")]
            Vacant = 2,
            [Description("Eviction")]
            Eviction = 3,
            [Description("Inactive")]
            Inactive = 4,
            [Description("Move Out")]
            MoveOut = 5
        }

        public enum ServiceTypes
        {
            [Description("Electric")]
            Electric = 1,
            [Description("Water")]
            Water = 2
        }

        public enum UserRole
        {
            [Description("Admin")]
            Admin = 1,
            [Description("Property Owner")]
            Owner = 2,
            [Description("Property Manager")]
            PropertyManager = 3,
            [Description("Leasing Agent")]
            LeasingAgent = 4,
            [Description("Maintenance Staff")]
            MaintenanceStaff = 5,
            [Description("Tenant")]
            Tenant = 6,
            [Description("Guest")]
            Guest = 7
        }

        public enum PropertyType
        {
            [Description("Residential")]
            Residential = 1,
            [Description("Commercial")]
            Commercial = 2
        }

        public enum ApiVersions
        {
            v1,
        }

        public enum ApiEndpoint
        {
            Companies,
            Properties,
            PropertyTypes,
            PropertyCategories,
            PaymentStatuses,
            Statuses,
            Leases,
            Images,
            Users,
            Units
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            return fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any()
                ? attributes.First().Description
                : value.ToString();
        }
    }
}
