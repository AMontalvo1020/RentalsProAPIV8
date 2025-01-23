using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.Parameters
{
    [Serializable]
    public class PostForLeaseParameters
    {
        public int? LeaseID { get; set; }
        public int? PropertyID { get; set; }
        public int? UnitID { get; set; }
        public bool? Active { get; set; } = true;
    }
}
