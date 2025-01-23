using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RentalsProAPIV8.Client.DataTransferObjects;

namespace RentalsProAPIV8.Client.API
{
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString)]
    [JsonSerializable(typeof(AddressDTO))]
    [JsonSerializable(typeof(PropertyDTO))]
    [JsonSerializable(typeof(CompanyDTO))]
    [JsonSerializable(typeof(LeaseDTO))]
    [JsonSerializable(typeof(LenderDTO))]
    [JsonSerializable(typeof(StatusDTO))]
    [JsonSerializable(typeof(TypeDTO))]
    [JsonSerializable(typeof(UnitDTO))]
    [JsonSerializable(typeof(UserDTO))]
    internal partial class RentalsProJsonSerializerContext : JsonSerializerContext
    {
    }
}
