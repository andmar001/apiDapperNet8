using Microsoft.Extensions.Configuration;

namespace _3.Infrastructure.Security
{
    public class ConnectionStringCustom
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            try
            {
                string source = FuncionesEncriptacionLogic.DecryptValue(configuration["ConnectionStrings:Source"]!);
                string database = FuncionesEncriptacionLogic.DecryptValue(configuration["ConnectionStrings:Database"]!);
                string user = FuncionesEncriptacionLogic.DecryptValue(configuration["ConnectionStrings:User"]!);
                string password = FuncionesEncriptacionLogic.DecryptValue(configuration["ConnectionStrings:Password"]!);
                string trustServerCert = FuncionesEncriptacionLogic.DecryptValue(configuration["ConnectionStrings:TrustServerCertificate"]!);

                string conexion = $"Data Source={source};" +
                  $"Database={database};" +
                  $"User Id={user};" +
                  $"Password={password};" +
                  $"TrustServerCertificate={trustServerCert};";

                return conexion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la cadena de conexión", ex);
            }
        }
    }
}
