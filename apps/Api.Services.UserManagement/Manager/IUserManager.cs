using Api.Services.Models.UserManagement;

namespace Api.Services.UserManagement.Manager
{
    public interface IUserManager
    {
        Task<List<UserResponse>> GetAllUsers();
    }
}
