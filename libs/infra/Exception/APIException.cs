
namespace Api.Services.Infra.Exception;

using System;
public class APIException : Exception
{
    public APIException( string message, Exception? innerException = null):base(message, innerException) {}

    public override string ToString()
    {
            return $"{GetType().Name}: {Message}";
    }
}

