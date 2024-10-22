using _3.Infrastructure.Persistence.Interfaces.Mantenimiento;
using _3.Infrastructure.Security;
using _4.Domain.Entities.Core;
using _4.Domain.Entities.Mantenimiento;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace _3.Infrastructure.Persistence.Repositories.Mantenimiento
{
    public class MantenimientoRepository : IMantenimientoRepository
    {
        public async Task<ResultSet<MantenimientoModel>> GetEstatusMantenimiento()
        {
            var response = new ResultSet<MantenimientoModel>();

            SqlClient? data = null;
            data = new SqlClient(ClsLogic._passPhrase, ClsLogic._dataBase);
            using (IDbConnection cnn = new SqlConnection(SqlClient._conexion?.ConnectionString))
            {
                try
                {
                    var query = cnn.QueryMultipleAsync("sp_Mantenimiento", commandType: CommandType.StoredProcedure);

                    MantenimientoModel mantenimiento = await query.Result.ReadSingleOrDefaultAsync<MantenimientoModel>() ?? new MantenimientoModel();

                    response.ObjData = mantenimiento;
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
