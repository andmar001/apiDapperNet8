using _4.Domain.Entities.Core;
using _4.Domain.Entities.Token;
using Google.Apis.Auth;

namespace _3.Infrastructure.Persistence.Interfaces.JWT
{
    public interface IDatosJWTRepository
    {
        public ResultSet<TokenDatosModel> ObtenerDatosJWT();
        public Task<bool> IsCaptchaValid(string token);
        public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token);
    }
}
