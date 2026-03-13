using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Events
{
    /// <summary>
    /// PublishResults is a class that represents the results of a publish operation in an eventing system.
    /// </summary>
    public class PublishResults
    {
        public string? theTopic { get; set; }
        public string? theDisposition { get; set; }
        public long messageId { get; set; }
        public DateTime? utcDateTime { get; set; }
    }
}
