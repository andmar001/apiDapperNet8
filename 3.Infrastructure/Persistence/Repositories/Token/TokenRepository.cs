using _3.Infrastructure.Persistence.Interfaces.JWT;
using _3.Infrastructure.Persistence.Interfaces.Token;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _3.Infrastructure.Persistence.Repositories.Token
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IDatosJWTRepository _datosJWTRepository;
        public TokenRepository(IDatosJWTRepository datosJWTRepository)
        {
            _datosJWTRepository = datosJWTRepository;
        }
        public string GenerateTokenJwt(string username, string cuenta)
        {
            // Obtener datos para el token
            var objDatos = _datosJWTRepository.ObtenerDatosJWT();

            // Crear token si existen datos
            if (objDatos != null && objDatos.Estatus == "OK" && objDatos.ObjData != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(objDatos.ObjData.Keyy);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("nombre",username),
                    new Claim("cuenta",cuenta)
                    }),
                    Audience = objDatos.ObjData.Audience,
                    Issuer = objDatos.ObjData.Issuer,
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(objDatos.ObjData.Expira),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            else
            {
                throw new Exception("Error consultar datos de para obtener JWT");
            }
        }
    }
}
