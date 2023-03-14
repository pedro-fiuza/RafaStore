using RafaStore.Shared;
using System.Net.Http.Json;

namespace RafaStore.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient http;

        public AuthService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterViewModel user)
        {
            var result = await http.PostAsJsonAsync("api/auth/register", user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<ServiceResponse<string>> Login(LoginViewModel login)
        {
            var result = await http.PostAsJsonAsync("api/auth/login", login);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}
