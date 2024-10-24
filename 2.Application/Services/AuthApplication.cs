using _2.Application.DTOs.Auth;
using _2.Application.Interfaces;
using _3.Infrastructure.Helpers;
using _3.Infrastructure.Persistence.Interfaces.Auth;
using _3.Infrastructure.Persistence.Interfaces.JWT;
using _4.Domain.Entities.Core;
using _4.Domain.Entities.Token;
using _4.Domain.Entities.Usuario;
using AutoMapper;
using WatchDog;

namespace _2.Application.Services
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IAuthRepository _authRepository;
        private readonly IDatosJWTRepository _datosJWTRepository;
        private readonly IMapper _mapper;
        public AuthApplication(IAuthRepository authRepository, IDatosJWTRepository datosJWTRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _datosJWTRepository = datosJWTRepository;
            _mapper = mapper;
        }
        public async Task<ResultSet<UsuarioDTO>> LoginUsuario(LoginOauthDto loginOauthDto)
        {
            var response = new ResultSet<UsuarioDTO>();

            try
            {
                var payload = await _datosJWTRepository.VerifyGoogleToken(loginOauthDto.Token);

                if (payload != null)
                {
                    long ExpiraToken = payload.ExpirationTimeSeconds!.Value;
                    DateTime tiempoActual = DateTime.UtcNow;
                    long unixTime = ((DateTimeOffset)tiempoActual).ToUnixTimeSeconds();
                    if ((payload.Email != null) && (unixTime < ExpiraToken))
                    {
                        loginOauthDto.Correo = payload.Email;

                        var email = _mapper.Map<LoginAuthModel>(loginOauthDto);

                        var login = _authRepository.LoginUsuario(_mapper.Map<LoginAuthModel>(email));

                        response.ObjData = _mapper.Map<UsuarioDTO>(login.ObjData);
                        response.Estatus = login.Estatus;
                        response.CodigoEstatus = login.CodigoEstatus;
                        response.Notificaciones = login.Notificaciones;
                    }
                    else
                    {
                        response.Estatus = "FAILED";
                        response.CodigoEstatus = 400;
                        response.Notificaciones = "Token invalido";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Estatus = "FAILED";
                response.CodigoEstatus = 400;
                response.Notificaciones = "Ha ocurrido un error al obtener al loguearte con credenciales. Contacte a soporte técnico.";
                response.ErrorMessage = $"Error: {ex.Message}";
                WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<ResultSet<UsuarioDTO>> LoginUsuarioCredenciales(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            var response = new ResultSet<UsuarioDTO>();

            try
            {
                if (await _datosJWTRepository.IsCaptchaValid(Funciones.GetReToken(usuarioCredencialesDTO.reTkn!)))
                {
                    var loginMapper = _mapper.Map<UsuarioCredencialesModel>(usuarioCredencialesDTO);

                    var login = _authRepository.LoginUsuarioCredenciales(loginMapper);

                    if (login is not null)
                    {
                        response.ObjData = _mapper.Map<UsuarioDTO>(login.ObjData);
                        response.Estatus = login.Estatus;
                        response.CodigoEstatus = login.CodigoEstatus;
                        response.Notificaciones = login.Notificaciones;
                    }
                    else
                    {
                        response.Estatus = "FAILED";
                        response.CodigoEstatus = 500;
                        response.Notificaciones = "Error al consultar usuario mediante credenciales";
                    }
                }
                else
                {
                    response.Estatus = "FAILED";
                    response.CodigoEstatus = 400;
                    response.Notificaciones = "Recaptcha invalido";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error: {ex.Message}";
                response.Estatus = "FAILED";
                response.CodigoEstatus = 400;
                response.Notificaciones = "Ha ocurrido un error al obtener al loguearte con credenciales. Contacte a soporte técnico.";
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
    }
}
