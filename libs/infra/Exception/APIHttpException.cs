using Api.Services.Infra.Exception;
using System.Net;

// Base class for HTTP exceptions in the API.
public class APIHttpException : APIException
{
    public virtual HttpStatusCode StatusCode { get; protected set; } 
    
    public string ThirdpartyErrorCode { get; set; }

    public APIHttpException(string? message = null, Exception? innerException = null) : base(message ?? string.Empty, innerException) { }
    
    public APIHttpException(string message, HttpStatusCode statusCode, Exception? innerException = null): base(message ?? string.Empty, innerException) 
    {
        StatusCode = statusCode;
    }
}

