using System.Data;
using System.Data.SqlClient;

namespace _3.Infrastructure.Security
{
    public class SqlClient
    {
        public static SqlConnection? _conexion;
        private SqlTransaction? _transaccion;
        private string _passPhrase;

        public SqlClient(string PassPhrase)
        {
            _passPhrase = PassPhrase;
            ConnectionOpen();
        }

        public SqlClient(string PassPhrase, string Database)
        {
            _passPhrase = PassPhrase;
            ConnectionOpen(Database);
        }

        public SqlClient(string PassPhrase, string Database, string User, string Password)
        {
            _passPhrase = PassPhrase;
            ConnectionOpen(Database, Crypt.Desencripta(User, Crypt.Algoritmo.Aes, _passPhrase), Crypt.Desencripta(Password, Crypt.Algoritmo.Aes, _passPhrase));
        }

        public void Dispose()
        {
            ConnectionClose();
        }

        public void ConnectionOpen()
        {
            try
            {
                if (ConnectionExists())
                    ConnectionClose();

                _conexion = new SqlConnection(ConnectionString());
                _conexion.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ConnectionOpen(string Database)
        {
            try
            {
                if (ConnectionExists())
                    ConnectionClose();

                _conexion = new SqlConnection(ConnectionString(Database));
                _conexion.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ConnectionOpen(string Database, string User, string Password)
        {
            try
            {
                if (ConnectionExists())
                    ConnectionClose();

                _conexion = new SqlConnection(ConnectionString(Database, User, Password));
                _conexion.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ConnectionClose()
        {
            try
            {
                if (TransactionExists())
                {
                    _transaccion.Rollback();
                    _transaccion.Dispose();
                }

                if (ConnectionExists())
                {
                    _conexion.Close();
                    _conexion.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _transaccion = null;
                _conexion = null;
                GC.Collect();
            }
        }

        private bool ConnectionExists()
        {
            if (_conexion != null)
                if (_conexion.State != ConnectionState.Closed)
                    return true;
            return false;
        }

        private bool TransactionExists()
        {
            if (_transaccion != null)
                if (_transaccion.Connection != null)
                    return true;
            return false;
        }

        private string ConnectionString()
        {
            try
            {
                DataSet dataSet = new DataSet("Querys");
                dataSet.ReadXml(Directory.GetCurrentDirectory() + @"/data.xml");

                DataTable dataTable = dataSet.Tables["dataBase"];

                dataTable.DefaultView.RowFilter = "name='Default'";
                if (dataTable.DefaultView.Count == 0)
                    throw new Exception("No existe la configuración de la base de datos default.");
                DataRow dataRow = dataTable.DefaultView[0].Row;

                if (Crypt.Desencripta(dataRow["providerName"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) != "System.Data.SqlClient")
                    throw new Exception("El proveedor de la base de datos debe ser System.Data.SqlClient");

                string conexion = "Data Source=" + Crypt.Desencripta(dataRow["server"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Initial Catalog=" + Crypt.Desencripta(dataRow["db"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Persist Security Info=True;User ID=" + Crypt.Desencripta(dataRow["user"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Application Name=" + Crypt.Desencripta(dataRow["application"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    "; Password=" + Crypt.Desencripta(dataRow["pass"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) + ";";

                return conexion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ConnectionString(string Database)
        {
            try
            {
                DataSet dataSet = new("Querys");
                dataSet.ReadXml(Directory.GetCurrentDirectory() + @"/data.xml");

                DataTable dataTable = dataSet.Tables["dataBase"];

                dataTable.DefaultView.RowFilter = "name='" + Database + "'";
                if (dataTable.DefaultView.Count == 0)
                    throw new Exception("No existe la configuración de la base de datos " + Database + ".");
                DataRow dataRow = dataTable.DefaultView[0].Row;

                if (Crypt.Desencripta(dataRow["providerName"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) != "System.Data.SqlClient")
                    throw new Exception("El proveedor de la base de datos debe ser System.Data.SqlClient");

                string conexion = "Data Source=" + Crypt.Desencripta(dataRow["server"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Initial Catalog=" + Crypt.Desencripta(dataRow["db"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Persist Security Info=True;User ID=" + Crypt.Desencripta(dataRow["user"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Application Name=" + Crypt.Desencripta(dataRow["application"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    "; Password=" + Crypt.Desencripta(dataRow["pass"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) + ";";

                return conexion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ConnectionString(string Database, string UserName, string Password)
        {
            try
            {
                DataSet dataSet = new("Querys");
                dataSet.ReadXml(Directory.GetCurrentDirectory() + @"/data.xml");

                DataTable dataTable = dataSet.Tables["dataBase"];

                dataTable.DefaultView.RowFilter = "name='" + Database + "'";
                if (dataTable.DefaultView.Count == 0)
                    throw new Exception("No existe la configuración de la base de datos " + Database + ".");
                DataRow dataRow = dataTable.DefaultView[0].Row;

                if (Crypt.Desencripta(dataRow["providerName"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) != "System.Data.SqlClient")
                    throw new Exception("El proveedor de la base de datos debe ser System.Data.SqlClient");

                string conexion = "Data Source=" + Crypt.Desencripta(dataRow["server"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Initial Catalog=" + Crypt.Desencripta(dataRow["db"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    ";Persist Security Info=True;User ID=" + UserName +
                    ";Application Name=" + Crypt.Desencripta(dataRow["application"].ToString(), Crypt.Algoritmo.Aes, _passPhrase) +
                    "; Password=" + Password + ";";

                return conexion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void TransactionBegin()
        {
            if (!ConnectionExists())
                throw new Exception("No existe conexión con la base de datos");
            if (TransactionExists())
                throw new Exception("Ya existe una transacción activa");

            _transaccion = _conexion.BeginTransaction();
        }

        public void TransactionCommit()
        {
            if (!ConnectionExists())
                throw new Exception("No existe conexión con la base de datos");
            if (TransactionExists())
            {
                _transaccion.Commit();
                _transaccion.Dispose();
            }


            _transaccion = null;
            GC.Collect();
        }

        public void TransactionRollback()
        {
            if (!ConnectionExists())
                throw new Exception("No existe conexión con la base de datos");
            if (TransactionExists())
            {
                _transaccion.Rollback();
                _transaccion.Dispose();
            }

            _transaccion = null;
            GC.Collect();
        }

        public object ExecuteScalar(string CommandText)
        {
            SqlCommand commando;

            if (!ConnectionExists())
                throw new Exception("Debe antes iniciar una conexión con el origen de datos");

            try
            {
                if (TransactionExists())
                    commando = new SqlCommand(CommandText, _conexion, _transaccion);
                else
                    commando = new SqlCommand(CommandText, _conexion);

                commando.CommandTimeout = 0;
                return commando.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                commando = null;
                GC.Collect();
            }
        }

        public object ExecuteScalar(string Sentencia, string Parametros)
        {
            SqlCommand comando;
            SqlConnection conexion = null;
            Boolean existeConexion;
            object retorno = null;

            try
            {
                string sql = Xml.Sentencia(Sentencia, Parametros, _passPhrase);
                existeConexion = ConnectionExists();

                if (existeConexion)
                {
                    if (TransactionExists())
                        comando = new SqlCommand(sql, _conexion, _transaccion);
                    else
                        comando = new SqlCommand(sql, _conexion);
                }
                else
                {
                    conexion = new SqlConnection(ConnectionString());
                    conexion.Open();
                    comando = new SqlCommand(sql, conexion);
                }

                comando.CommandTimeout = 0;
                retorno = comando.ExecuteScalar();

                if (!existeConexion)
                    conexion.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                comando = null;
                conexion = null;
                GC.Collect();
            }

            return retorno;
        }

        public int ExecuteNonQuery(string Sentencia, string Parametros, byte[] Array)
        {
            SqlCommand comando;
            SqlConnection conexion = null;
            Boolean existeConexion;
            int retorno;

            try
            {
                existeConexion = ConnectionExists();
                string sql = Xml.Sentencia(Sentencia, Parametros, _passPhrase);

                if (existeConexion)
                {
                    if (TransactionExists())
                        comando = new SqlCommand(sql, _conexion, _transaccion);
                    else
                        comando = new SqlCommand(sql, _conexion);
                }
                else
                {
                    conexion = new SqlConnection(ConnectionString());
                    conexion.Open();
                    comando = new SqlCommand(sql, conexion);
                }

                SqlParameter spArray = comando.Parameters.Add("@Array", SqlDbType.VarBinary);
                spArray.Value = Array;

                comando.CommandTimeout = 0;
                retorno = comando.ExecuteNonQuery();

                if (!existeConexion)
                    conexion.Close();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                comando = null;
                conexion = null;
                GC.Collect();
            }

            return retorno;
        }

        public int ExecuteNonQuery(string Sentencia, string Parametros)
        {
            SqlCommand comando;
            SqlConnection conexion = null;
            Boolean existeConexion;
            int retorno;

            try
            {
                existeConexion = ConnectionExists();
                string sql = Xml.Sentencia(Sentencia, Parametros, _passPhrase);

                if (existeConexion)
                {
                    if (TransactionExists())
                        comando = new SqlCommand(sql, _conexion, _transaccion);
                    else
                        comando = new SqlCommand(sql, _conexion);
                }
                else
                {
                    conexion = new SqlConnection(ConnectionString());
                    conexion.Open();
                    comando = new SqlCommand(sql, conexion);
                }

                comando.CommandTimeout = 0;
                retorno = comando.ExecuteNonQuery();

                if (!existeConexion)
                    conexion.Close();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                comando = null;
                conexion = null;
                GC.Collect();
            }

            return retorno;
        }

        public DataTable GetDataTable(string Sentencia, string Parametros)
        {
            SqlDataAdapter dataAdapter;
            SqlConnection conexion = null;
            DataSet dataSet = new DataSet("Query");
            Boolean existeConexion;

            try
            {
                string sql = Xml.Sentencia(Sentencia, Parametros, _passPhrase);
                existeConexion = ConnectionExists();

                if (existeConexion)
                {
                    dataAdapter = new SqlDataAdapter(sql, _conexion);
                    if (TransactionExists())
                        dataAdapter.SelectCommand.Transaction = _transaccion;
                }
                else
                {
                    conexion = new SqlConnection(ConnectionString());
                    conexion.Open();

                    dataAdapter = new SqlDataAdapter(sql, conexion);
                }

                dataAdapter.SelectCommand.CommandTimeout = 0;
                dataAdapter.Fill(dataSet);

                if (!existeConexion)
                    conexion.Close();

                return dataSet.Tables[0];

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dataAdapter = null;
                conexion = null;
                dataSet = null;
                GC.Collect();
            }
        }

        public DataTable GetDataTable(string Sentencia)
        {
            SqlDataAdapter dataAdapter;
            SqlConnection conexion = null;
            DataSet dataSet = new DataSet("Query");
            Boolean existeConexion;

            try
            {
                string sSql = Xml.Sentencia(Sentencia, "", _passPhrase);
                existeConexion = ConnectionExists();

                if (existeConexion)
                {
                    dataAdapter = new SqlDataAdapter(sSql, _conexion);
                    if (TransactionExists())
                        dataAdapter.SelectCommand.Transaction = _transaccion;
                }
                else
                {
                    conexion = new SqlConnection(ConnectionString());
                    conexion.Open();

                    dataAdapter = new SqlDataAdapter(sSql, conexion);
                }

                dataAdapter.SelectCommand.CommandTimeout = 0;
                dataAdapter.Fill(dataSet);

                if (!existeConexion)
                    conexion.Close();

                return dataSet.Tables[0];

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dataAdapter = null;
                conexion = null;
                dataSet = null;
                GC.Collect();
            }
        }

        public void BulkCopy(string TableName, DataTable Table)
        {
            SqlBulkCopy bulkCopy;
            Boolean existeConexion;
            SqlConnection conexion = null;

            try
            {
                existeConexion = ConnectionExists();

                if (existeConexion)
                {
                    if (TransactionExists())
                        bulkCopy = new SqlBulkCopy(_conexion, SqlBulkCopyOptions.TableLock, _transaccion);
                    else
                        bulkCopy = new SqlBulkCopy(_conexion);
                }
                else
                {
                    conexion = new SqlConnection(ConnectionString());
                    conexion.Open();
                    bulkCopy = new SqlBulkCopy(conexion);
                }

                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.BatchSize = 100;
                bulkCopy.DestinationTableName = TableName;
                bulkCopy.WriteToServer(Table);

                if (!existeConexion)
                    conexion.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conexion = null;
                bulkCopy = null;
                GC.Collect();
            }
        }
    }
}
