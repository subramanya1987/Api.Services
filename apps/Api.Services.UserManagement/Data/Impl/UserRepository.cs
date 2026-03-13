using Api.Services.DataAccess.Entities.UserManagement;
using Api.Services.Infra.Cache;
using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Helper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.Services.UserManagement.Data.Impl
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManagementContext _dbContext;
        private readonly ICacheProvider _redisCache;
        private readonly IConfiguration _config;
        public UserRepository(UserManagementContext dbContext, ICacheProvider redisCache, IConfiguration config)
        {
            _dbContext = dbContext;
            _redisCache = redisCache;
            _config = config;
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var responseObject = new List<UserResponse>();

            //Define cache name
            string cacheName = CacheHelper.GetCacheName();

            var cacheData = await _redisCache.GetAsync<List<UserResponse>>(cacheName);

            if (cacheData != null && cacheData.Count > 0)
            {
                responseObject = cacheData;
                return responseObject;
            }
            var dbResponse = await _dbContext.TblUsers.Where(x => x.IsActive == true)
               .ToListAsync();

            if (dbResponse != null)
            {
                responseObject = JsonConvert.DeserializeObject<List<UserResponse>>(JsonConvert.SerializeObject(dbResponse));
            }
            //Set cache
            await _redisCache.SetAsync(cacheName, responseObject, TimeSpan.FromMinutes(int.Parse(_config["REDIS_CACHE_MINUTES"] ?? "600")));

#pragma warning disable CS8603 // Possible null reference return.
            return responseObject;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
