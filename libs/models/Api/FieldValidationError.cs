using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Models.Api
{
    public class ErrorSeverity
    {
        public const string Infromation = "info";
        public const string Warning = "warning";
        public const string Error = "error";
    }

    public class FieldValidationError
    {
        public string Field { get; }
        public string Reason { get;}
        public string? Code { get;  } 
        public string  Severity { get; set; }
        public FieldValidationError(string field, string reason, string ? code = "", string severity = ErrorSeverity.Warning)
        {
            Field = field;
            Reason = reason;
            Code = code;
            Severity = severity;
        }
    }
}
