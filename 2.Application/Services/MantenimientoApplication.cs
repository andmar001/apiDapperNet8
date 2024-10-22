using _2.Application.DTOs.Mantenimiento;
using _2.Application.Interfaces;
using _3.Infrastructure.Persistence.Interfaces.Mantenimiento;
using _4.Domain.Entities.Core;
using AutoMapper;
using WatchDog;

namespace _2.Application.Services
{
    public class MantenimientoApplication : IMantenimientoApplication
    {
        private readonly IMantenimientoRepository _repository;
        private readonly IMapper _mapper;
        public MantenimientoApplication(IMantenimientoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResultSet<MantenimientoResponseDTO>> GetEstatusMantenimiento()
        {
            var response = new ResultSet<MantenimientoResponseDTO>();

            try
            {
                var mantenimiento = _repository.GetEstatusMantenimiento();

                if (mantenimiento is not null)
                {
                    response.CodigoEstatus = 200;
                    response.ObjData = _mapper.Map<MantenimientoResponseDTO>(mantenimiento);
                    response.Notificaciones = "Registro encontrado";
                }
                else
                {
                    response.CodigoEstatus = 202;
                    response.Notificaciones = "Sin registros";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error: {ex.Message}";
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
    }
}
