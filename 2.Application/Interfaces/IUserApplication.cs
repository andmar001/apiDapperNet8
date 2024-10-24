using _2.Application.DTOs.User;
using _4.Domain.Entities.Core;

namespace _2.Application.Interfaces
{
    public interface IUserApplication
    {
        public Task<ResultSet<UsuarioRegisterDto>> RegisterUser(UsuarioRegisterDto usuarioModel);
    }
}
