using Api.Services.Models.UserManagement;

namespace Api.Services.UserManagement.Data
{
    public interface IUserRepository
    {
        Task<List<UserResponse>> GetAllUsers();
    }
}
