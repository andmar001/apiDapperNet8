using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace _3.Infrastructure.Security
{
    public class FuncionesEncriptacionLogic
    {
        public static string EncryptValue(string value)
        {
            return Crypt.Encripta(value, Crypt.Algoritmo.Aes, ClsLogic._passPhrase);
        }

        public static string DecryptValue(string value)
        {
            return Crypt.Desencripta(value, Crypt.Algoritmo.Aes, ClsLogic._passPhrase);
        }

        public static string EncryptValueFront(string value, string phrase)
        {
            return Crypt.EncriptaFrontEnd(value, Crypt.Algoritmo.Aes, phrase);
        }

        public static string DecryptValueFront(string value, string phrase)
        {
            return Crypt.DesencriptaFrontEnd(value, Crypt.Algoritmo.Aes, phrase);
        }

        public static string GetValueConf(string tipo, string nombre, string key)
        {
            string value;

            try
            {
                // Crea un dataser vacio
                DataSet dataSet = new DataSet();

                // Busca y carga el archivo xml de configuracion. se debe llamar data.xml y existir en bin
                if (ClsLogic._servicioAmbiente == "DES")
                {
                    dataSet.ReadXml(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"/data.xml");
                }
                else
                {
                    dataSet.ReadXml(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"/dataProd.xml");
                }

                // Filtra tipo de nodo
                DataTable dataTable = dataSet.Tables[tipo];

                // filtra por nombre en el archivo el nodo debe tener la propiedad name
                dataTable.DefaultView.RowFilter = "name='" + nombre + "'";

                // Si no hay coincidencia detona error
                if (dataTable.DefaultView.Count == 0)
                {
                    throw new Exception("No existe la configuración para la configuración de tipo: " + tipo + " con el nombre: " + nombre);
                }

                // Recupera los nodos contenidos en la coincidencia encontrada
                DataRow dataRow = dataTable.DefaultView[0].Row;

                // Recupera valor buscado
                value = Crypt.Desencripta(dataRow[key].ToString(), Crypt.Algoritmo.Aes, ClsLogic._passPhrase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return value;
        }

        public static string GetMD5(string str)
        {
            ASCIIEncoding encoding = new();
            StringBuilder sb = new();
            byte[]? stream = MD5.HashData(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) { sb.AppendFormat("{0:x2}", stream[i]); }
            return sb.ToString();
        }
    }
}
