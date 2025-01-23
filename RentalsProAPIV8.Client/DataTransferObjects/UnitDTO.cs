using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record UnitDTO
    {
        public int? UnitID { get; set; }
        public int PropertyID { get; set; }
        public int StatusID { get; set; }
        public int PaymentStatusID { get; set; }
        public string UnitNumber { get; set; }
        public int? Floor { get; set; }
        public decimal? Bedrooms { get; set; }
        public decimal? Bathrooms { get; set; }
        public int SquareFeet { get; set; }
        public string LeaseTerms { get; set; }
        public bool? Utilities { get; set; }
        public bool? Furnished { get; set; }
        public bool? ParkingSpace { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; } = true;
        public StatusDTO? Status { get; set; }
        public StatusDTO? PaymentStatus { get; set; }
    }
}
