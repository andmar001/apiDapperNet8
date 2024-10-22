using System.Data;

namespace _3.Infrastructure.Security
{
    internal class Xml
    {
        internal static string Sentencia(string Id, string Parametros, string passPhrase)
        {
            try
            {
                string archivo = DeterminarArchivo(Id);
                DataSet dataSet = new("Querys");

                dataSet.ReadXml(archivo);

                DataTable dataTable = dataSet.Tables["Query"];

                dataTable.DefaultView.RowFilter = "ID = '" + Id + "'";
                if (dataTable.DefaultView.Count == 0)
                    throw new Exception("El Id: " + Id + " no existe en: " + archivo);
                DataRow drXml = dataTable.DefaultView[0].Row;
                string sSql = Crypt.Desencripta(drXml["Query_Text"].ToString(), Crypt.Algoritmo.Aes, passPhrase);

                string sCaracter = "A";
                foreach (string Par in Parametros.Split('|'))
                {
                    if (sSql.Contains("?" + sCaracter + "#")) //Encriptar Valor
                        sSql = sSql.Replace("?" + sCaracter + "#", Crypt.Encripta(Par, Crypt.Algoritmo.Aes, passPhrase));
                    else
                        sSql = sSql.Replace("?" + sCaracter, Par);

                    sCaracter = Parametro(sCaracter);
                }

                return sSql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string DeterminarArchivo(string Id)
        {
            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"geg.pei.data." + Id.Split('_')[0] + ".xml";
        }

        private static string Parametro(string Caracter)
        {
            string parametro = "";
            switch (Caracter.Length)
            {
                case 1:
                    if (!Caracter.Equals("Z"))
                        parametro = Convert.ToString(Convert.ToChar(Convert.ToInt32(Convert.ToChar(Caracter)) + 1));
                    else
                        parametro = "1A";
                    break;
                case 2:
                    if (Caracter.Substring(Caracter.Length - 1, 1) == "Z")
                        Caracter = Convert.ToString(Convert.ToInt32(Caracter.Substring(0, 1)) + 1) + "A";
                    else
                        parametro = Convert.ToString(Convert.ToChar(Convert.ToInt32(Convert.ToChar(Caracter.Substring(1, 1))) + 1));
                    break;
            }
            return parametro;
        }
    }
}
