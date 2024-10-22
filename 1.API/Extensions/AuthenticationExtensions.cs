using _3.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace _1.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                FuncionesEncriptacionLogic fn = new();
                /// token
                string llave = "";
                string audience = "";
                string issu = "";
                HttpStatusCode statusCode;
                // Obtener datos para generar token
                SqlClient Data = null;
                Data = new SqlClient(ClsLogic._passPhrase, ClsLogic._dataBase);
                using (SqlConnection cnn = new(SqlClient._conexion.ConnectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = new("sp_Datos_JWT_token", cnn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cnn.Open();
                            using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if (dr.Read())
                                {
                                    llave = dr.GetString(0);
                                    audience = dr.GetString(1);
                                    issu = dr.GetString(2);
                                }
                                dr.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        statusCode = HttpStatusCode.InternalServerError;
                    }
                    finally
                    {
                        cnn.Close();
                    }
                }
                // Asignar valores desencriptados y de forma que se puedan utilizar
                var secretKey = FuncionesEncriptacionLogic.DecryptValue(llave);
                var audienceToken = FuncionesEncriptacionLogic.DecryptValue(audience);
                var issuerToken = FuncionesEncriptacionLogic.DecryptValue(issu);
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
                // Parametros para la validacion de los token
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuerToken,
                    ValidAudience = audienceToken,
                    IssuerSigningKey = securityKey,
                    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters TokenValidationParameters) =>
                    {
                        if (expires != null)
                        {
                            long exp = (long)((JwtSecurityToken)securityToken).Payload["exp"];
                            DateTime TiempoActual = DateTime.UtcNow;
                            long unixTime = ((DateTimeOffset)TiempoActual).ToUnixTimeSeconds();
                            if (unixTime < exp)
                                return true;
                            else
                                return false;
                        }
                        return false;
                    },
                };
            });

            return services;
        }
    }
}
