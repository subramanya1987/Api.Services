using System.Net;

namespace Api.Services.Infra.Exception
{
    public class APIBadRequestException:APIHttpException
    {
        public APIBadRequestException():this(string.Empty) { }
        public APIBadRequestException(string message): base(message)
            
        {
                StatusCode = HttpStatusCode.BadRequest;
        }
    }
}