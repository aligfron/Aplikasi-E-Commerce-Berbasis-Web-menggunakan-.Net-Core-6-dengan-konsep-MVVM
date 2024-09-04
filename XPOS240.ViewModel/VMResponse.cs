using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XPOS240.ViewModel
{
    public class VMResponse<T>
    {
        
        public string? message { get; set; }
        public HttpStatusCode statusCode {  get; set; }
        public T? data { get; set; }

        public VMResponse() { 
            statusCode = HttpStatusCode.InternalServerError;
            message = string.Empty;
            data = default(T);
        }
    }
}
