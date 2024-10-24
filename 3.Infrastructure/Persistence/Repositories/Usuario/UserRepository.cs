using _3.Infrastructure.Persistence.Interfaces.Usuario;
using _3.Infrastructure.Security;
using _4.Domain.Entities.Core;
using _4.Domain.Entities.Usuario;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using BC = BCrypt.Net.BCrypt;

namespace _3.Infrastructure.Persistence.Repositories.Usuario
{
    public class UserRepository : IUserRepository
    {
        public async Task<ResultSet<UsuarioModel>> RegisterUser(UsuarioModel usuarioModel)
        {
            var response = new ResultSet<UsuarioModel>();
            var responseResult = new Result<UsuarioModel>();

            DynamicParameters parameters = new();
            string notificacion = "";
            int resultado;

            SqlClient? data = null;
            data = new SqlClient(ClsLogic._passPhrase, ClsLogic._dataBase);
            using (IDbConnection cnn = new SqlConnection(SqlClient._conexion?.ConnectionString))
            {
                try
                {
                    if(string.IsNullOrEmpty(usuarioModel.Correo))
                        throw new ArgumentException("Correo no puede ser nulo o vacio");
                    if(string.IsNullOrEmpty(usuarioModel.Nombre))
                        throw new ArgumentException("Nombre no puede ser nulo o vacio");
                    if(string.IsNullOrEmpty(usuarioModel.PrimerApellido))
                        throw new ArgumentException("PrimerApellido no puede ser nulo o vacio");
                    if(string.IsNullOrEmpty(usuarioModel.Cuenta))
                        throw new ArgumentException("Cuenta no puede ser nulo o vacio");
                    if(string.IsNullOrEmpty(usuarioModel.Contra))
                        throw new ArgumentException("Contraseña no puede ser nulo o vacio");
                    if (string.IsNullOrEmpty(usuarioModel.IdUsuarioLogeado))
                        throw new ArgumentException("IdUsuarioLogeado no puede ser nulo o vacio");
                    
                    parameters.Add("@correo", usuarioModel.Correo);
                    parameters.Add("@nombre", usuarioModel.Nombre);
                    parameters.Add("@primerApellido", usuarioModel.PrimerApellido);
                    parameters.Add("@segundoApellido", usuarioModel.SegundoApellido);
                    parameters.Add("@cuenta", usuarioModel.Cuenta);
                    //Encriptar contraseña
                    string contraseñaEncriptada = BC.HashPassword(usuarioModel.Contra);
                    parameters.Add("@contra", contraseñaEncriptada);
                    parameters.Add("@uIdUsuarioLogueado", usuarioModel.IdUsuarioLogeado);

                    cnn.Open();

                    var query = cnn.QueryMultipleAsync("[dbo].[sp_Usuario_Register]", 
                                parameters, 
                                commandType: CommandType.StoredProcedure);

                    notificacion = await query.Result.ReadSingleOrDefaultAsync<string>() ?? "Error";
                    resultado = await query.Result.ReadSingleOrDefaultAsync<int>();

                    if (resultado == 0)
                    {
                        response = responseResult.Created(true);
                    }
                    else
                    {
                        response = responseResult.Error(notificacion);
                    }
                }
                catch (ArgumentException aex)
                {
                    response = responseResult.Error(aex.Message);
                }
                catch (Exception ex)
                {
                    response = responseResult.Error(ex.Message);
                }
            }
            return response;
        }
    }
}
