using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Service.InternalHttpClient
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }
    public class HttpClientFactory: IHttpClientFactory
    {
        public HttpClient CreateClient() => new();        
    }
}
