using _2.Application.DTOs.Auth;
using _2.Application.Interfaces;
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
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UsuarioDTO usuarioDTO, LoginOauthDto loginOauthDto)
        {
            var response = await _authApplication.LoginUsuario(usuarioDTO,loginOauthDto);
            return StatusCode(response.CodigoEstatus, response);
        }
        [HttpPost]
        [Route("credenciales")]
        public async Task<IActionResult> LoginUsuarioCredenciales(UsuarioCredencialesDTO usuarioCredenciales)
        {
            var response = await _authApplication.LoginUsuarioCredenciales(usuarioCredenciales);
            return StatusCode(response.CodigoEstatus, response);
        }
    }
}
