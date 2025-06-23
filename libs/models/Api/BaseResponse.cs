using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Models.Api
{
    public class BaseResponse
    {
        /// <summary>
        /// Generic properties object
        /// </summary>
        public Dictionary<string, string> properties { get; set; } = new Dictionary<string, string>();
    }
}
