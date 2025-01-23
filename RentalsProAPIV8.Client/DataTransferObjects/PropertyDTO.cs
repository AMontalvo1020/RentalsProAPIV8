using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record PropertyDTO
    {
        public int? ID { get; set; }
        public int? OwnerID { get; set; }
        public int? CompanyID { get; set; }
        public AddressDTO? Address { get; set; }
        public decimal? Bedrooms { get; set; }
        public decimal? Bathrooms { get; set; }
        public int? Size { get; set; }
        public int TypeID { get; set; }
        public int StatusID { get; set; }
        public int PaymentStatusID { get; set; }
        public string? Amenities { get; set; }
        public decimal? PurchasePrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal? RentAmount { get; set; }
        public bool HasUnits => Units?.Any() == true;
        public UserDTO? Owner { get; set; }
        public CompanyDTO? Company { get; set; }
        public TypeDTO? Type { get; set; }
        public StatusDTO? Status { get; set; }
        public StatusDTO? PaymentStatus { get; set; }
        public LeaseDTO? Lease { get; set; }
        public List<UnitDTO>? Units { get; set; }
    }
}
