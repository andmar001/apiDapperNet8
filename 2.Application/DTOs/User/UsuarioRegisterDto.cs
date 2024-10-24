using _4.Domain.Entities.Usuario;

namespace _2.Application.DTOs.User
{
    public class UsuarioRegisterDto
    {
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Correo { get; set; }
        public string? Cuenta { get; set; }
        public string? Contra { get; set; }
        public List<RolModel>? ListaRoles { get; set; }
        public bool? IsOauth { get; set; }
        public string? IdUsuarioLogeado { get; set; }
    }
}
