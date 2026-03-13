using Api.Services.DataAccess.Entities.UserManagement;
using Api.Services.Infra.Cache;
using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Helper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.Services.UserManagement.Data.Impl
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly UserManagementContext _dbContext;
        private readonly ICacheProvider _redisCache;
        private readonly IConfiguration _config;
        public ApplicationRepository(UserManagementContext dbContext,ICacheProvider redisCache, IConfiguration config)
        {
            _dbContext = dbContext;
            _redisCache = redisCache;
            _config = config;
        }
        public async Task<List<ApplicationResponse>> GetAllApplications()
        {     
            var responseObject = new List<ApplicationResponse>();

            //Define cache name
            string cacheName = CacheHelper.GetCacheName();

            var cacheData= await _redisCache.GetAsync<List<ApplicationResponse>>(cacheName);

            if(cacheData != null && cacheData.Count > 0)
            {
                responseObject = cacheData; 
                return responseObject;
            }
            var dbResponse = await  _dbContext.TblApplications.Where(x => x.IsActive == true)
               .ToListAsync();

            if(dbResponse != null)
            {
                responseObject=JsonConvert.DeserializeObject<List<ApplicationResponse>>(JsonConvert.SerializeObject(dbResponse));               
            }
            //Set cache
            await _redisCache.SetAsync(cacheName,responseObject, TimeSpan.FromMinutes(int.Parse(_config["REDIS_CACHE_MINUTES"]??"600")));

#pragma warning disable CS8603 // Possible null reference return.
            return responseObject;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<ApplicationResponse> GetApplicationById(string applicationId)
        {
            var appId = new Guid(applicationId);
            var responseObject = new ApplicationResponse();
            var dbResponse = await _dbContext.TblApplications.Where(x => x.IsActive == true && x.Id== appId)
               .FirstOrDefaultAsync();

            if (dbResponse != null)
            {
                responseObject = JsonConvert.DeserializeObject<ApplicationResponse>(JsonConvert.SerializeObject(dbResponse));
            }
#pragma warning disable CS8603 // Possible null reference return.
            return responseObject;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
