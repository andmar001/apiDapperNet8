using _4.Domain.Entities.Core;
using _4.Domain.Entities.Token;

namespace _3.Infrastructure.Persistence.Interfaces.JWT
{
    public interface IDatosJWTRepository
    {
        public ResultSet<TokenDatosModel> ObtenerDatosJWT();
    }
}
