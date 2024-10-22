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
                    response.Estatus = "OK";
                    response.CodigoEstatus = 200;
                    response.ObjData = _mapper.Map<MantenimientoResponseDTO>(mantenimiento.Result.ObjData);
                    response.Notificaciones = "En mantenimiento";
                }
                else
                {
                    response.Estatus = "OK";
                    response.CodigoEstatus = 202;
                    response.Notificaciones = "Sin registros";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error: {ex.Message}";
                response.Estatus = "FAILED";
                response.CodigoEstatus = 400;
                response.Notificaciones = "Ha ocurrido un error al obtener el estatus del mantenimiento. Contacte a soporte técnico.";
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
    }
}
