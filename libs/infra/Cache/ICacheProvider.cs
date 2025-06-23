using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Cache
{
    public interface ICacheProvider
    {
        Task SetAsync<T>(string key, T value);
        Task SetAsync<T>(string key, T value,TimeSpan timeout);
        Task<T> GetAsync<T>(string key);
        Task<List<string>> GetListAsync<T>(string key);
        Task<bool> RemoveAsync(string key);
    }
}
