using _3.Infrastructure.Persistence.Interfaces.JWT;
using _3.Infrastructure.Security;
using _4.Domain.Entities.Core;
using _4.Domain.Entities.Token;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace _3.Infrastructure.Persistence.Repositories.JWT
{
    public class DatosJWTRepository : IDatosJWTRepository
    {
        public ResultSet<TokenDatosModel> ObtenerDatosJWT()
        {
            ResultSet<TokenDatosModel> objResult = new();
            TokenDatosModel tokenDatos = new();

            SqlClient? Data = null;
            Data = new SqlClient(ClsLogic._passPhrase, ClsLogic._dataBase);
            using (IDbConnection cnn = new SqlConnection(SqlClient._conexion.ConnectionString))
            {
                try
                {
                    using (var multi = cnn.QueryMultiple("sp_Datos_JWT_token", commandType: CommandType.StoredProcedure))
                    {
                        var obj = multi.Read<TokenDatosModel>().FirstOrDefault();

                        tokenDatos.Secret = obj.Secret != "" || obj.Secret != null ? FuncionesEncriptacionLogic.DecryptValue(obj.Secret) : "";
                        tokenDatos.Keyy = obj.Keyy != "" ? FuncionesEncriptacionLogic.DecryptValue(obj.Keyy) : "";
                        tokenDatos.Audience = obj.Audience != "" ? FuncionesEncriptacionLogic.DecryptValue(obj.Audience) : "";
                        tokenDatos.Issuer = obj.Issuer != "" ? FuncionesEncriptacionLogic.DecryptValue(obj.Issuer) : "";
                        tokenDatos.Url = obj.Url != "" || obj.Secret != null ? FuncionesEncriptacionLogic.DecryptValue(obj.Url) : "";
                        tokenDatos.Ren = obj.Ren;
                        tokenDatos.Expira = obj.Expira;

                        objResult.Estatus = "OK";
                        objResult.CodigoEstatus = 200;
                        objResult.ObjData = tokenDatos;
                    }
                }
                catch (Exception ex)
                {
                    objResult.Estatus = "FAILED";
                    objResult.CodigoEstatus = 400;
                    objResult.Notificaciones = "Error consultar datos de para obtener JWT " + ex.Message;
                }
                finally
                {
                    cnn.Close();
                }
                return objResult;
            }
        }
    }
}
