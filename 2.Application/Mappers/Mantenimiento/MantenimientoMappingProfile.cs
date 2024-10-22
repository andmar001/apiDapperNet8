using _2.Application.DTOs.Mantenimiento;
using _4.Domain.Entities.Core;
using _4.Domain.Entities.Mantenimiento;
using AutoMapper;

namespace _2.Application.Mappers.Mantenimiento
{
    public class MantenimientoMappingProfile : Profile
    {
        public MantenimientoMappingProfile()
        {
            CreateMap<ResultSet<MantenimientoModel>,ResultSet<MantenimientoResponseDTO>>()
                .ReverseMap();

            // Crear mapeo de MantenimientoModel a MantenimientoResponseDTO
            CreateMap<MantenimientoModel, MantenimientoResponseDTO>();

            // Si lo necesitas, puedes definir el mapeo inverso también
            CreateMap<MantenimientoResponseDTO, MantenimientoModel>();
        }
    }
}
