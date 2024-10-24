namespace _4.Domain.Entities.Usuario
{
    [Serializable]
    public class RolModel
    {
        public Guid UUIDRol { get; set; }
        public string? Descripcion { get; set; }
        public bool? Estatus { get; set; }
        //public string Nombre { get; set; }
        public string? IdUsuarioLogeado { get; set; }
    }

    public class ProcesoModel
    {
        public string? UID { get; set; }
        public string? Nombre { get; set; }
        public int IdProceso { get; set; }
        public string? Clave { get; set; }
        public string? SubTipo { get; set; }
    }

    [Serializable]
    public class ProcesoRolModel : ProcesoModel
    {
        public List<RolModel> Roles { get; set; }
    }

    public class ModuloModel
    {
        public string? UID { get; set; }
        public string? Nombre { get; set; }
        public string? Ruta { get; set; }
        public int Padre { get; set; }
        public int idModulo { get; set; }
        public int Orden { get; set; }
        public List<ModuloModel>? SubModuloModel { get; set; }
    }

    [Serializable]
    public class MenuModel
    {
        public string? UID { get; set; }
        public long IdRol { get; set; }
        public string? Rol { get; set; }
        public string? Proceso { get; set; }
        public string? Clave { get; set; }
        public List<ModuloModel>? Modulos { get; set; }
    }

    public class UsuarioRolesModel
    {
        public string? uid { get; set; }
        public string? rol { get; set; }
    }
}
