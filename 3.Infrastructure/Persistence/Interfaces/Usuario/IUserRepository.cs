using _4.Domain.Entities.Core;
using _4.Domain.Entities.Usuario;

namespace _3.Infrastructure.Persistence.Interfaces.Usuario
{
    public interface IUserRepository
    {
        public Task<ResultSet<UsuarioModel>> RegisterUser(UsuarioModel usuarioModel);
    }
}
