using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Models.Api
{
    public class ErrorDetail
    {
        public string? InnerMessage { get; set; }
        public string? SystemMessage { get; set; }
        public string? UserMessage { get; set; }    
        public List<FieldValidationError>? ValidationErrors { get; set; }
        public APIErrorDetail? APIErrorDetail { get; set; }
    }
}
