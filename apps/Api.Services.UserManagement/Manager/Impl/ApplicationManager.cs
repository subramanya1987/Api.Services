using Api.Services.Infra.Cache;
using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Data;

namespace Api.Services.UserManagement.Manager.Impl
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICacheProvider _cacheProvider;
        public ApplicationManager(IApplicationRepository applicationRepository, ICacheProvider cacheProvider)
        {
            _applicationRepository = applicationRepository;
            _cacheProvider = cacheProvider;
        }
        public async Task<List<ApplicationResponse>> GetAllApplications()
        {
            return await _applicationRepository.GetAllApplications();
        }

        public async Task<ApplicationResponse> GetApplicationById(string applicationId)
        {
            return await _applicationRepository.GetApplicationById(applicationId);
        }

        public async Task<bool> RemoveRedisKey(string redisKey)
        {
            // Implementation for removing a Redis key can be added here if needed.
            // This is a placeholder method as per the interface contract.
             return await _cacheProvider.RemoveAsync(redisKey);
        }
    }
}
