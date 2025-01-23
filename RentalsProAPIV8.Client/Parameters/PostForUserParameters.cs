using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.Parameters
{
    [Serializable]
    public class PostForUserParameters
    {
        public int? UserID { get; set; }
        public string? Username { get; set; }
        public int? Role { get; set; }
        public int? CompanyID { get; set; }
        public int? LeaseID { get; set; }
    }
}
