using Api.Services.Models.Api;
using System.Net;
using System.Collections.Generic;


namespace Api.Services.Infra.Exception
{
    public class APIValidationException : APIHttpException
    {
        public IEnumerable<FieldValidationError> ValidationErrors { get; private set; }

        public APIValidationException():this(string.Empty) { }

        public APIValidationException(string message) : this(message, Array.Empty<FieldValidationError>()) { }

        public APIValidationException(string message, IEnumerable<FieldValidationError> validationErrors)
            : this(message, HttpStatusCode.BadRequest, validationErrors) { }

        public APIValidationException(string message, HttpStatusCode httpStatusCode, IEnumerable<FieldValidationError> validationErrors)
            : base(message, HttpStatusCode.BadRequest)        
        {
            StatusCode = httpStatusCode;
            ValidationErrors = validationErrors ?? Array.Empty<FieldValidationError>();
        }
    }
}
