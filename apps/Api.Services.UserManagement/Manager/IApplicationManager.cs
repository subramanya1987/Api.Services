using Api.Services.Models.UserManagement;
using System;

namespace Api.Services.UserManagement.Manager
{
    public interface IApplicationManager
    {
        Task<List<ApplicationResponse>> GetAllApplications();

        Task<ApplicationResponse> GetApplicationById(string applicationId);

        Task<bool> RemoveRedisKey(string redisKey);
    }
}
