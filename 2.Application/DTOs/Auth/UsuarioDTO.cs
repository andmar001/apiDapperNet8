using _4.Domain.Entities.Usuario;

namespace _2.Application.DTOs.Auth
{
    [Serializable]
    public class UsuarioDTO
    {
        public Guid UUIDUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contra { get; set; }
        public bool? Estatus { get; set; }
        public List<RolModel> ListaRoles { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public string reTkn { get; set; }
        public bool? Oauth { get; set; }
        public string IdUsuarioLogeado { get; set; }
    }
}
