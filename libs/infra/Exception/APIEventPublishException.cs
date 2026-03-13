using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Exception
{
    public class APIEventPublishException:APIException
    {
        public APIEventPublishException(string message) : base(message) { }
            
        public APIEventPublishException(string message, System.Exception? innerException=null) : base(message, innerException) { }

    }
}
