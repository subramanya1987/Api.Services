using Microsoft.Extensions.Logging;

namespace Api.Services.Infra.Cache
{
    public class CacheManager
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ILogger _logger;

        public CacheManager(ICacheProvider cacheProvider, ILogger logger)
        {
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T?> GetCacheAsync<T>(string prefix, int root)
        {
            var rootAsString = ((int)root).ToString("000");
            return await GetCacheAsync<T>(prefix, rootAsString);
        }       

        public async Task<T?> GetCacheAsync<T>(string prefix, string root)
        {
            var key = $"api:{prefix}_{root}";
            T? result = default(T);
            try
            {
                result = await _cacheProvider!.GetAsync<T>(key);
                string resultNull = result != null ? "(not null)" : "(null)";
                _logger.LogDebug($"Retrieved cache for key: {key} with result: {resultNull} from cache.");
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($"CacheManager.GetCacheAsync: Caching disabled due to error.{ex.Message}");                
            }
            return result;
        }

        public async Task SetCacheAsync(string prefix, int root, object value, int minutes)
        {
            var rootAsString = ((int)root).ToString("000");
            await SetCacheAsync(prefix, rootAsString, value,minutes);
        }

        public async Task SetCacheAsync(string prefix, string root, object value,int minutes)
        {
            var key = $"api:{prefix}_{root}";
            try
            {
                await _cacheProvider!.SetAsync(key, value,TimeSpan.FromMinutes(minutes));
                _logger.LogDebug($"Set cache for key: {key} with value: {value}.");
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($"CacheManager.SetCacheAsync: Caching disabled due to error.{ex.Message}");
            }
        }

        public async Task RemoveCacheAsync(string prefix, string root)
        {
            var key = $"api:{prefix}_{root}";
            try
            {
                await _cacheProvider!.RemoveAsync(key);
                _logger.LogDebug($"Removed cache for key: {key}.");
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($"CacheManager.RemoveCacheAsync: Caching disabled due to error.{ex.Message}");
            }             
        }
    }
}
