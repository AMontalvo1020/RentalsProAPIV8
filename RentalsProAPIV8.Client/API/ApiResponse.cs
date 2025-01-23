using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.API
{
    public record ApiResponse<T>(HttpStatusCode StatusCode, T Content, string? Message = null);
}
