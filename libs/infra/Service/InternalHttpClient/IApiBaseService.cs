using Api.Services.Models.Api;

namespace Api.Services.Infra.Service.InternalHttpClient
{
    public interface IApiBaseService
    {
        string BaseUrl { get; }
        string ControllerPrefix { get; }

        /// <summary>
        /// Performs a HTTP DELETE request to the specified endpoint and returns the response messagee.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task<Response<T>> DeleteAsync<T>(string endpoint);

        Task<Response<T>> DeleteAsync<T>(string endpoint, object requestObject);
        Task<Response<T>> GetAsync<T>(string endpoint);
        Task<Response<T>> GetAsync<T>(string endpoint, IReadOnlyDictionary<string,string> parameters);
        Task<Response<T>> PostAsync<T>(string endpoint, object requestObject);
        Task<Response<T>> PostAsync<T>(string endpoint, HttpContent httpContent);
        Task<Response<T>> PutAsync<T>(string endpoint, object requestObject);
        Task<Response<T>> PutAsync<T>(string endpoint, HttpContent httpContent);        
        Task<Response<T>> PatchAsync<T>(string endpoint, object requestObject);
    }
}
