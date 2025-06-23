using Api.Services.Models.UserManagement;

namespace Api.Services.UserManagement.Data
{
    public interface IApplicationRepository
    {
        Task<List<ApplicationResponse>> GetAllApplications();

        Task<ApplicationResponse> GetApplicationById(string applicationId);
    }
}
