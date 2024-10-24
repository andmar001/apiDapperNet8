using _3.Infrastructure.Helpers;
using _3.Infrastructure.Persistence.Interfaces.Auth;
using _3.Infrastructure.Persistence.Interfaces.Token;
using _3.Infrastructure.Security;
using _4.Domain.Entities.Core;
using _4.Domain.Entities.Token;
using _4.Domain.Entities.Usuario;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace _3.Infrastructure.Persistence.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ITokenRepository _tokenRepository;
        public AuthRepository(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public ResultSet<UsuarioModel> LoginUsuario(LoginAuthModel loginAuthModel)
        {
            FuncionesEncriptacionLogic fn = new();
            Result<UsuarioModel> result = new();
            ResultSet<UsuarioModel> resultSet = new();
            DynamicParameters parameters = new();
            parameters.Add("@correoElectronico", loginAuthModel.Correo);

            SqlClient? data = null;
            data = new SqlClient(ClsLogic._passPhrase, ClsLogic._dataBase);
            using (IDbConnection cnn = new SqlConnection(SqlClient._conexion?.ConnectionString))
            {
                try
                {
                    var usuarioDictionary = new Dictionary<Guid, UsuarioModel>();
                    UsuarioModel? usuarioValue =
                    cnn.Query<UsuarioModel, RolModel, UsuarioModel>(
                        "sp_Usuario_GetCuenta",
                       (usuario, rol) =>
                       {
                           var rolDictionary = new Dictionary<Guid, RolModel>();
                           if (!usuarioDictionary.TryGetValue(usuario.UUIDUsuario, out UsuarioModel? usuarioEntry))
                           {
                               usuarioEntry = usuario;
                               usuarioEntry.ListaRoles = new List<RolModel>();
                               usuarioDictionary.Add(usuarioEntry.UUIDUsuario, usuarioEntry);
                           }
                           if (rol != null)
                           {
                               if (!rolDictionary.TryGetValue(rol.UUIDRol, out RolModel? rolEntry))
                               {
                                   usuarioEntry.ListaRoles.Add(rol);
                                   rolEntry = rol;
                                   rolDictionary.Add(rolEntry.UUIDRol, rolEntry);
                               }
                           }
                           return usuarioEntry;
                       },
                       parameters,
                       splitOn: "UUIDRol",
                       commandType: System.Data.CommandType.StoredProcedure
                       ).Distinct().FirstOrDefault();

                    if (usuarioValue != null)
                    {
                        usuarioValue.Token = _tokenRepository.GenerateTokenJwt(usuarioValue.Nombre, usuarioValue.Correo);
                        resultSet = result.OKObject(usuarioValue);
                    }
                    else
                    {
                        resultSet.Estatus = "FAILED";
                        resultSet.CodigoEstatus = 400;
                        resultSet = result.Error("El usuario proporcionado no se encuentra registrado en el sistema.");
                    }
                }
                catch (Exception ex)
                {
                    //Implement WatchDog in this layer
                    resultSet.Estatus = "FAILED";
                    resultSet.CodigoEstatus = 400;
                    resultSet.Notificaciones = "Ha ocurrido un error al validar el usuario. Contacte a soporte técnico.";
                }
                finally
                {
                    cnn.Close();
                }
            }
            return resultSet;
        }

        public ResultSet<UsuarioModel> LoginUsuarioCredenciales(UsuarioCredencialesModel usuarioCredenciales)
        {
            Result<UsuarioModel> result = new();
            ResultSet<UsuarioModel> resultSet = new();
            DynamicParameters parameters = new();

            string phrase = Funciones.GetPhrase(usuarioCredenciales.reTkn!);

            parameters.Add("@usuario", FuncionesEncriptacionLogic.DecryptValueFront(usuarioCredenciales.Usuario!, phrase));
            parameters.Add("@contra", FuncionesEncriptacionLogic.DecryptValueFront(usuarioCredenciales.Contra!, phrase));
 
            SqlClient? data = null;
            data = new SqlClient(ClsLogic._passPhrase, ClsLogic._dataBase);
            using (IDbConnection cnn = new SqlConnection(SqlClient._conexion?.ConnectionString))
            {
                try
                {
                    var querys = cnn.QueryMultiple(
                        "sp_Usuario_Credenciales", parameters,
                        commandType: System.Data.CommandType.StoredProcedure
                    );
                    if (querys != null)
                    {
                        string errorMessage = querys.Read<string>().First();
                        int resultado = querys.Read<int>().First();

                        if (resultado == 0)
                        {
                            UsuarioModel? usuario = querys.Read<UsuarioModel>().FirstOrDefault();
                            List<RolModel> roles = querys.Read<RolModel>().ToList();
                            usuario.ListaRoles = roles;
                            usuario.Token = _tokenRepository.GenerateTokenJwt(usuario.Nombre, "");
                            resultSet = result.OKObject(usuario);
                        }
                        else
                        {
                            resultSet.Estatus = "FAILED";
                            resultSet.CodigoEstatus = 400;
                            resultSet = result.Error(errorMessage);
                        }
                    }
                    else
                    {
                        resultSet.Estatus = "FAILED";
                        resultSet.CodigoEstatus = 400;
                        resultSet = result.Error("El usuario proporcionado no se encuentra registrado en el sistema.");
                    }
                }
                catch (Exception ex)
                {
                    // ------------------------------check if we save watchdog------------------------------------------------
                    resultSet.Estatus = "FAILED";
                    resultSet.CodigoEstatus = 400;
                    resultSet.Notificaciones = "Ha ocurrido un error al validar el usuario. Contacte a soporte técnico.";
                }
                finally
                {
                    cnn.Close();
                }
            }
            return resultSet;
        }
    }
}