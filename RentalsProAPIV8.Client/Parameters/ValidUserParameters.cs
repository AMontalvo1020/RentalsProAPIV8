using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.Parameters
{
    [Serializable]
    public class ValidUserParameters
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
