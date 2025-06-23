using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Exception
{
    public class APINotFoundException : APIHttpException
    {
        public APINotFoundException() : this(string.Empty) { }
        public APINotFoundException(string message) : base(message)
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }       
    }
}
