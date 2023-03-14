using RafaStore.Shared;
using RafaStore.Shared.Model;

namespace RafaStore.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserModel user, string password);
        Task<UserModel> UserExists(string email);
        Task<ServiceResponse<string>> Login(string email, string password);
    }
}
