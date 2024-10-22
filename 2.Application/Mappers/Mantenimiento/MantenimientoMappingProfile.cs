using _2.Application.DTOs.Mantenimiento;
using _4.Domain.Entities.Mantenimiento;
using AutoMapper;

namespace _2.Application.Mappers.Mantenimiento
{
    public class MantenimientoMappingProfile : Profile
    {
        public MantenimientoMappingProfile()
        {
            CreateMap<MantenimientoModel, MantenimientoResponseDTO>()
                .ReverseMap();
        }
    }
}
