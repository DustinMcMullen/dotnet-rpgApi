using dotnet_rpgApi.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public IAuthRepository _AuthRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _AuthRepo = authRepo;

        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _AuthRepo.Register(
                new User {Username = request.Username}, request.Password
            );
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<bool>>> Login(UserLoginDto request)
        {
            var response = await _AuthRepo.Login(request.Username, request.Password);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}