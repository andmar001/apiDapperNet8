using _2.Application.Common.Bases;
using _2.Application.DTOs.Mantenimiento;

namespace _2.Application.Interfaces
{
    public interface IMantenimientoService
    {
        public Task<ResultSet<MantenimientoResponseDTO>> GetEstatusMantenimiento();
    }
}
