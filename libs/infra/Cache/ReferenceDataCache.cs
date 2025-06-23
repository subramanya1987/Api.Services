using Api.Services.DataAccess.Entities.UserManagement;
using Api.Services.Models.UserManagement;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Cache
{
    public class ReferenceDataCache : IReferenceDataCache
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly int _cacheMinutes;
        private readonly IServiceProvider _serviceProvider;

        public ReferenceDataCache(ICacheProvider cacheProvider, IConfiguration config, ILogger<ReferenceDataCache> logger, IServiceProvider serviceProvider)
        {
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheMinutes = int.TryParse(_config["REDIS_CACHE_MINUTES"], out int minutes) ? minutes : 60;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IEnumerable<ApplicationResponse> Applications => GetTableUserManagementAsync<ApplicationResponse>().Result;

        private async Task<IEnumerable<TEntity>> GetTableUserManagementAsync<TEntity>() where TEntity : class
        {
            var cacheManager = new CacheManager(_cacheProvider, _logger);
            var cacheValues = await cacheManager.GetCacheAsync<IEnumerable<TEntity>>(typeof(TEntity).Name, nameof(ReferenceDataCache));

            if(cacheValues==null)
            {
                using (var scope = _serviceProvider.CreateAsyncScope())
                using (var dbContext = scope.ServiceProvider.GetRequiredService<UserManagementContext>())
                {
                    cacheValues= dbContext.Set<TEntity>().ToArray();
                    await cacheManager.SetCacheAsync(typeof(TEntity).Name, nameof(ReferenceDataCache), cacheValues, _cacheMinutes);                    
                }
            }
            return cacheValues;
        }
    }
}
