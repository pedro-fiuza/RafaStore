using RafaStore.Shared;

namespace RafaStore.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterViewModel user);
        Task<ServiceResponse<string>> Login(LoginViewModel login);
    }
}
