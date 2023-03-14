using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RafaStore.Shared;
using RafaStore.Shared.Model;

namespace RafaStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterViewModel user)
        {
            var response = await authService.Register(new UserModel
            {
                Email = user.Email,
            },
        user.Password);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginViewModel login)
        {
            var response = await authService.Login(login.Email, login.Password);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
