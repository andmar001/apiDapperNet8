using _2.Application.DTOs.Auth;
using _4.Domain.Entities.Core;

namespace _2.Application.Interfaces
{
    public interface IAuthApplication
    {
        public Task<ResultSet<UsuarioDTO>> LoginUsuario(UsuarioDTO usuarioDTO, LoginOauthDto loginOauthDto);
        public Task<ResultSet<UsuarioCredencialesDTO>> LoginUsuarioCredenciales(UsuarioCredencialesDTO usuarioCredencialesDTO);
    }
}
