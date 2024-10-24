using _4.Domain.Entities.Core;
using _4.Domain.Entities.Token;
using _4.Domain.Entities.Usuario;

namespace _3.Infrastructure.Persistence.Interfaces.Auth
{
    public interface IAuthRepository
    {
        public ResultSet<UsuarioModel> LoginUsuario(LoginAuthModel loginAuthModel);
        public ResultSet<UsuarioModel> LoginUsuarioCredenciales(UsuarioCredencialesModel usuarioCredenciales);
    }
}