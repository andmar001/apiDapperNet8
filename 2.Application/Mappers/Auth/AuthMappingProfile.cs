using _2.Application.DTOs.Auth;
using _4.Domain.Entities.Token;
using _4.Domain.Entities.Usuario;
using AutoMapper;

namespace _2.Application.Mappers.Auth
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile() 
        {
            CreateMap<UsuarioModel, UsuarioDTO>()
                .ReverseMap();
            
            CreateMap<UsuarioModel, UsuarioCredencialesDTO>()
                .ReverseMap();

            CreateMap<UsuarioModel, LoginOauthDto>()
                .ReverseMap();

            CreateMap<LoginAuthModel, LoginOauthDto>()
                .ReverseMap();

            CreateMap<UsuarioCredencialesModel, UsuarioCredencialesDTO>()
                .ReverseMap();
        }
    }
}
