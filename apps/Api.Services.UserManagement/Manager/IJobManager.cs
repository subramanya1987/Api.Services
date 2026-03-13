using Api.Services.Models.UserManagement;

namespace Api.Services.UserManagement.Manager
{
    public interface IJobManager
    {
        /// <summary>
        /// Get all jobs.
        /// </summary>
        /// <returns>List of job responses.</returns>
        Task<List<UserResponse>> GetUserList();
        
    }
}
