using Api.Services.Infra.Exception;
using System.Net;

// Base class for HTTP exceptions in the API.
public class APIHttpException : APIException
{
    public virtual HttpStatusCode StatusCode { get; protected set; } 
    
    public string ThirdpartyErrorCode { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public APIHttpException(string? message = null, Exception? innerException = null) : base(message ?? string.Empty, innerException) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public APIHttpException(string message, HttpStatusCode statusCode, Exception? innerException = null): base(message ?? string.Empty, innerException)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        StatusCode = statusCode;
    }
}

