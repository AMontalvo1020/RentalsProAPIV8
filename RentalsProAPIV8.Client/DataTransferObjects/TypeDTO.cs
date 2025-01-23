using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record TypeDTO
    {
        public int? ID { get; set; }
        public string? Name { get; set; }

        public TypeDTO(int? id, string? name)
        {
            ID = id;
            Name = name;
        }
    }
}
