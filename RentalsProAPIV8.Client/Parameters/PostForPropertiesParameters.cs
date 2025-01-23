using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.Parameters
{
    [Serializable]
    public class PostForPropertiesParameters
    {
        public string? Address { get; set; }
        public int? PaymentStatus { get; set; }
        public int? Category { get; set; }
        public List<int> Status { get; set; } = new List<int>();
        public List<int> Type { get; set; } = new List<int>();
        public int? UserID { get; set; }
        public int? CompanyID { get; set; }
        public bool? Active { get; set; } = true;
    }
}
