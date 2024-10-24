using _2.Application.DTOs.User;
using _2.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication _userApplication;
        public UserController(IUserApplication userApplication)
        {
            _userApplication = userApplication;                    
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UsuarioRegisterDto usuarioRegisterDto)
        {
            var response = await _userApplication.RegisterUser(usuarioRegisterDto);
            return StatusCode(response.CodigoEstatus, response);
        }
    }
}
