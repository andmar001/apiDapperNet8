using _4.Domain.Entities.Core;
using _4.Domain.Entities.Mantenimiento;

namespace _3.Infrastructure.Persistence.Interfaces.Mantenimiento
{
    public interface IMantenimientoRepository
    {
        public Task<ResultSet<MantenimientoModel>> GetEstatusMantenimiento();
    }
}
