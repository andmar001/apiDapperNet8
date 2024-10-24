using _2.Application.DTOs.User;
using _2.Application.Interfaces;
using _3.Infrastructure.Persistence.Interfaces.Usuario;
using _4.Domain.Entities.Core;
using _4.Domain.Entities.Usuario;
using AutoMapper;
using WatchDog;

namespace _2.Application.Services
{
    public class UserApplication : IUserApplication
    {
        private  readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserApplication(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ResultSet<UsuarioRegisterDto>> RegisterUser(UsuarioRegisterDto usuarioRegisterDto)
        {
            var response = new ResultSet<UsuarioRegisterDto>();

            try
            {
                var paylaod = _mapper.Map<UsuarioModel>(usuarioRegisterDto);

                var user = await _userRepository.RegisterUser(paylaod);

                if (user is not null)
                {
                    response.Estatus = user.Estatus;
                    response.CodigoEstatus = user.CodigoEstatus;
                    response.Notificaciones = user.Notificaciones;
                }
                else
                {
                    response.Estatus = "FAILED";
                    response.CodigoEstatus = 400;
                    response.Notificaciones = "Registro incorrecto";
                }
            }
            catch (ArgumentException aex)
            {
                response.Estatus = "FAILED";
                response.CodigoEstatus = 400;
                response.Notificaciones = aex.Message;
            }
            catch (Exception ex)
            {
                response.Estatus = "FAILED";
                response.CodigoEstatus = 400;
                response.Notificaciones = "Ha ocurrido un error al registrar el usuario. Contacte a soporte técnico.";
                response.ErrorMessage = $"Error: {ex.Message}";
                WatchLogger.Log(ex.Message);
            }
            return response;
        }
    }
}
