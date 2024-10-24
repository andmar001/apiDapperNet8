using _2.Application.DTOs.User;
using _4.Domain.Entities.Usuario;
using AutoMapper;

namespace _2.Application.Mappers.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UsuarioModel, UsuarioRegisterDto>()
                .ReverseMap();
        }
    }
}
