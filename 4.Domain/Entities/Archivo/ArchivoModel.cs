namespace _4.Domain.Entities.Archivo
{
    public class ArchivoModel
    {
        public Guid? UUIDUsuario { get; set; }
        public Guid? UUIDUsuarioRol { get; set; }
        public Guid? TipoArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public string Carpeta { get; set; }
        public List<ArchivoModel> Archivos { get; set; }
        public string Ruta { get; set; }
        public int? Resultado { get; set; }
        public string ErrorMessage { get; set; }
    }
}
