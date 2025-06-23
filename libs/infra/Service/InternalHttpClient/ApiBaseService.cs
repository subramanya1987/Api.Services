using Api.Services.Models.Api;
using Microsoft.Extensions.Logging;

namespace Api.Services.Infra.Service.InternalHttpClient
{
    public abstract class ApiBaseService : IApiBaseService
    {
        public abstract string BaseUrl { get; }

        public abstract string ControllerPrefix { get; }

        public abstract ILogger Logger { get; }

        public Task<Response<T>> DeleteAsync<T>(string endpoint)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> DeleteAsync<T>(string endpoint, object requestObject)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> GetAsync<T>(string endpoint)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> GetAsync<T>(string endpoint, IReadOnlyDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> PatchAsync<T>(string endpoint, object requestObject)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> PostAsync<T>(string endpoint, object requestObject)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> PostAsync<T>(string endpoint, HttpContent httpContent)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> PutAsync<T>(string endpoint, object requestObject)
        {
            throw new NotImplementedException();
        }

        public Task<Response<T>> PutAsync<T>(string endpoint, HttpContent httpContent)
        {
            throw new NotImplementedException();
        }
    }
}
