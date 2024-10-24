using _2.Application.DTOs.Auth;
using _2.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;
        public AuthController(IAuthApplication authApplication)
        {
            _authApplication = authApplication;        
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginOauthDto loginOauthDto)
        {
            var response = await _authApplication.LoginUsuario(loginOauthDto);
            return StatusCode(response.CodigoEstatus, response);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("credenciales")]
        public async Task<IActionResult> LoginUsuarioCredenciales([FromBody] UsuarioCredencialesDTO usuarioCredenciales)
        {
            var response = await _authApplication.LoginUsuarioCredenciales(usuarioCredenciales);
            return StatusCode(response.CodigoEstatus, response);
        }
    }
}
