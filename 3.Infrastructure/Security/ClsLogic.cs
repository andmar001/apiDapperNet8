using System.Runtime.InteropServices;

namespace _3.Infrastructure.Security
{
    public class ClsLogic
    {
        public static string _passPhrase = Environment.GetEnvironmentVariable("PlantillaNet8Phrase")!; // Debe de ser de 32 caracteres
        internal static string _servicioAmbiente = "DES";
        public static string _dataBase = "PlantillaNet8";
        // la primer ruta debe ser con formato para Windows y las segunda en formato de Linux
        public static string _rutaServerFiletmp = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"C:\plantilla\temp" : "/plantilla/api";
    }
}
