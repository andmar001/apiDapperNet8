using _2.Application.DTOs.Mantenimiento;
using _4.Domain.Entities.Core;

namespace _2.Application.Interfaces
{
    public interface IMantenimientoApplication
    {
        public Task<ResultSet<MantenimientoResponseDTO>> GetEstatusMantenimiento();
    }
}
