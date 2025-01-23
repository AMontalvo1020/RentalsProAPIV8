using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record AddressDTO
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress => $"{Address} {City}, {State} {ZipCode}";

        public AddressDTO(string address, string cityy, string state, string zip)
        {
            Address = address;
            City = cityy;
            State = state;
            ZipCode = zip;
        }
    }
}
