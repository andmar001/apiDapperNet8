namespace _4.Domain.Entities.Usuario
{
    [Serializable]
    public class UsuarioModel
    {
        public int? IdUsuario { get; set; }
        public Guid UUIDUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Correo { get; set; }
        public string? Cuenta { get; set; }
        public string? Contra { get; set; }
        public bool? Estatus { get; set; }
        public List<RolModel>? ListaRoles { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
        public string? reTkn { get; set; }
        public bool? Oauth { get; set; }
        public string? IdUsuarioLogeado { get; set; }
    }

    public class PeticionUsuarioRPModel
    {
        public string? procesorol { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string? filtro { get; set; }
    }

    public class UsuariosRolProcesoModel
    {
        public string uid { get; set; }
        public string usuario { get; set; }
        public string rol { get; set; }
        public string procesoclave { get; set; }
        public string procesonombre { get; set; }
    }
}
