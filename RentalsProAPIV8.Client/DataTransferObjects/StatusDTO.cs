using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record StatusDTO
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? BackgroundColor { get; set; } 

        public StatusDTO(int? Id, string? name, string? backgroundColor)
        {
            ID = Id;
            Name = name;
            BackgroundColor = backgroundColor;
        }
    }
}
