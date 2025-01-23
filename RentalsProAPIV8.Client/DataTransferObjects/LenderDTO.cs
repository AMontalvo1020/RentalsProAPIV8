using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record LenderDTO
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public AddressDTO? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
