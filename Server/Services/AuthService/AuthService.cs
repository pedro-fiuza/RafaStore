using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RafaStore.Server.Data;
using RafaStore.Shared;
using RafaStore.Shared.Model;
using SecureIdentity.Password;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RafaStore.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            
            var user = await UserExists(email);

            if(user is null)
            {
                response.Success = false;
                response.Message = "Usuario nao encontrado.";
            }
            else if(!PasswordHasher.Verify(user.PasswordHash, password))
            {
                response.Success = false;
                response.Message = "Senha incorreta.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserModel user, string password)
        {
            try
            {
                var userVerify = await UserExists(user.Email);

                if (userVerify is not null)
                    return new ServiceResponse<int>
                    {
                        Success = false,
                        Message = "O usuario ja existe"
                    };

                user.PasswordHash = PasswordHasher.Hash(password);

                context.User.Add(user);
                await context.SaveChangesAsync();

                return new ServiceResponse<int> { Data = (int)user.Id, Message = "Cadastro feito com sucesso!" };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<UserModel> UserExists(string email)
        {
            return await context.User.FirstOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));
        }

        private string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(8),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
