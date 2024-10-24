namespace _4.Domain.Entities.Mantenimiento
{
    public class MantenimientoModel
    {
        public int IdMantenimiento { get; set; }
        public string? Tipo { get; set; } // puede ser Aviso, Mantenimiento o Vasio
        public string? Descripcion { get; set; } // Mensaje que mostrara el front en el dialogo
        public DateTime FechaInicio { get; set; } // Apartir de que fecha se muestra el dialogo
    }
}
