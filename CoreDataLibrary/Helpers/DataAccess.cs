using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Helpers
{
    public class DataAccess
    {
        public static void BulkInsert(int timeout, string tablename, DataTable table, SqlConnection conn)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.BulkCopyTimeout = timeout; // in seconds
                    bulkCopy.DestinationTableName = tablename;
                    conn.Open();
                    bulkCopy.WriteToServer(table);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static void BulkInsert(int timeout, string tablename, DataTable table)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_Scon))
                {
                    bulkCopy.BulkCopyTimeout = timeout; // in seconds
                    bulkCopy.DestinationTableName = tablename;
                    _Scon.Open();
                    bulkCopy.WriteToServer(table);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static SqlConnection GetConnection()
        {
            try
            {
                //return new SqlConnection(CoreDataLibrary.Data.DataConnection.SqlConnNonCoreData);
                return new SqlConnection(CoreDataLibrary.Data.DataConnection.MssqldevConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ExecuteSelectSQL(string strSQLin)
        {
            return GetDataSet(strSQLin);
        }

        public static DataSet GetDataSet(string strSQLin)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                DataSet dsresult = new DataSet();
                _Scon.Open();
                SqlDataAdapter _Sadp = new SqlDataAdapter(strSQLin, _Scon);
                _Sadp.Fill(dsresult);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static DataSet GetDataSet(ref SqlCommand cmd)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                _Scon.Open();
                cmd.Connection = _Scon;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet Dtab = new DataSet();
                adp.Fill(Dtab);
                return Dtab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static DataSet GetDataSet(List<SqlCommand> cmds)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                DataSet Dset = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter();
                _Scon.Open();
                foreach (SqlCommand cmd in cmds)
                {
                    cmd.Connection = _Scon;
                    adp.SelectCommand = cmd;
                    DataTable Dtab = new DataTable();
                    adp.Fill(Dtab);
                    Dset.Tables.Add(Dtab);
                }
                return Dset;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static DataTable GetDataTable(string strSqLin)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                DataTable dsresult = new DataTable();
                _Scon.Open();
                SqlDataAdapter _Sadp = new SqlDataAdapter(strSqLin, _Scon);
                _Sadp.Fill(dsresult);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static DataTable GetDataTable(string strSqLin, SqlConnection conn)
        {
            try
            {
                DataTable dsresult = new DataTable();
                conn.Open();
                SqlDataAdapter _Sadp = new SqlDataAdapter(strSqLin, conn);
                _Sadp.Fill(dsresult);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(ref SqlCommand cmd)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                _Scon.Open();
                cmd.Connection = _Scon;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable Dtab = new DataTable();
                adp.Fill(Dtab);
                return Dtab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cmdType"></param>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(ref SqlCommand cmd, CommandType cmdType, string CommandText)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                if (cmdType == CommandType.StoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else if (cmdType == CommandType.Text)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (cmdType == CommandType.TableDirect)
                {
                    cmd.CommandType = CommandType.TableDirect;
                }
                cmd.CommandText = CommandText;
                _Scon.Open();
                cmd.Connection = _Scon;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable Dtab = new DataTable();
                adp.Fill(Dtab);
                return Dtab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        /// <summary>
        /// Excecute the SQL Command
        /// </summary>
        /// <param name="cmd">Command Object Reference</param>
        /// <param name="cmdType">SQL Command Type</param>
        /// <param name="CommandText">SQL Query</param>
        public static void ExecuteCommand(ref SqlCommand cmd, CommandType cmdType, string CommandText)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                if (cmdType == CommandType.StoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else if (cmdType == CommandType.Text)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (cmdType == CommandType.TableDirect)
                {
                    cmd.CommandType = CommandType.TableDirect;
                }
                cmd.CommandText = CommandText;
                _Scon.Open();
                cmd.Connection = _Scon;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static void ExecuteCommand(ref SqlCommand cmd)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                _Scon.Open();
                cmd.Connection = _Scon;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static void ExecuteTransaction(SqlCommand[] CMD)
        {
            SqlConnection _Scon = GetConnection();
            if (CMD.Length > 0)
            {
                _Scon.Open();
                SqlTransaction Xtran = _Scon.BeginTransaction();
                try
                {
                    foreach (SqlCommand CM in CMD)
                    {
                        CM.Connection = _Scon;
                        CM.Transaction = Xtran;
                        CM.ExecuteNonQuery();
                    }
                    Xtran.Commit();
                }
                catch (Exception ex)
                {
                    Xtran.Rollback();
                    throw ex;
                }
                finally
                {
                    if (_Scon.State == ConnectionState.Open)
                    {
                        _Scon.Close();
                    }
                }
            }
        }

        public static string ExecuteScalar(string strSqLin)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                SqlCommand CMD = new SqlCommand();
                CMD.CommandText = strSqLin;
                _Scon.Open();
                CMD.Connection = _Scon;
                object Result = CMD.ExecuteScalar();
                return Result == null ? null : Result.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static string ExecuteScalar(string strSqLin, SqlConnection conn)
        {
            try
            {
                SqlCommand CMD = new SqlCommand();
                CMD.CommandText = strSqLin;
                conn.Open();
                CMD.Connection = conn;
                object Result = CMD.ExecuteScalar();
                return Result == null ? null : Result.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static string ExecuteScalar(ref SqlCommand cmd)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                _Scon.Open();
                cmd.Connection = _Scon;
                object test = cmd.ExecuteScalar();
                if (test != null)
                {
                    return Convert.ToString(test);
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static string ExecuteNonQuery(ref SqlCommand cmd, CommandType cmdType, string CommandText)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                _Scon.Open();
                cmd.Connection = _Scon;

                GetCommandType(cmd, cmdType);
                cmd.CommandText = CommandText;

                object test = cmd.ExecuteNonQuery();
                if (test != null)
                {
                    return Convert.ToString(test);
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        private static void GetCommandType(SqlCommand cmd, CommandType cmdType)
        {
            if (cmdType == CommandType.StoredProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else if (cmdType == CommandType.Text)
            {
                cmd.CommandType = CommandType.Text;
            }
            else if (cmdType == CommandType.TableDirect)
            {
                cmd.CommandType = CommandType.TableDirect;
            }
        }

        public static string ExecuteNonQuery(ref SqlCommand cmd, CommandType cmdType, string CommandText, SqlConnection _Scon)
        {
            try
            {
                _Scon.Open();
                cmd.Connection = _Scon;

                if (cmdType == CommandType.StoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else if (cmdType == CommandType.Text)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (cmdType == CommandType.TableDirect)
                {
                    cmd.CommandType = CommandType.TableDirect;
                }
                cmd.CommandText = CommandText;

                object test = cmd.ExecuteNonQuery();
                if (test != null)
                {
                    return Convert.ToString(test);
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static DataTable ExecuteReader(ref SqlCommand cmd, CommandType cmdType, string CommandText)
        {
            DataTable dt = new DataTable();
            SqlConnection _Scon = GetConnection();
            try
            {
                _Scon.Open();
                cmd.Connection = _Scon;

                if (cmdType == CommandType.StoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else if (cmdType == CommandType.Text)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (cmdType == CommandType.TableDirect)
                {
                    cmd.CommandType = CommandType.TableDirect;
                }
                cmd.CommandText = CommandText;

                SqlDataReader test = cmd.ExecuteReader();
                if (test != null)
                {
                    dt.Load(test);
                    //dt = test.GetSchemaTable();
                    //while (test.Read())
                    //{
                    //    object [] CurrentRow = new object[test.FieldCount];
                    //    int rows = test.GetValues(CurrentRow);                        
                    //    dt.Rows.Add(CurrentRow);
                    //}

                    return dt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static void ExecuteSQL(string strSQLin)
        {
            using (SqlConnection _Scon = GetConnection())
            {
                _Scon.Open();
                SqlCommand CMD = new SqlCommand(strSQLin, _Scon);
                CMD.ExecuteNonQuery();
            }
        }

        public static void ExecuteParamCollection(string prcName, ref SqlParameterCollection _SparaCollec)
        {
            SqlConnection _Scon = GetConnection();
            _Scon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = prcName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter _sparm in _SparaCollec)
                {
                    cmd.Parameters.Add(_sparm);
                }
                cmd.Connection = _Scon;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }

        public static DataSet GetDataSet(ref SqlCommand cmd, CommandType cmdType, string CommandText)
        {
            SqlConnection _Scon = GetConnection();
            try
            {
                if (cmdType == CommandType.StoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else if (cmdType == CommandType.Text)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (cmdType == CommandType.TableDirect)
                {
                    cmd.CommandType = CommandType.TableDirect;
                }
                cmd.CommandText = CommandText;
                _Scon.Open();
                cmd.Connection = _Scon;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet Dtab = new DataSet();
                adp.Fill(Dtab);
                return Dtab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_Scon.State == ConnectionState.Open)
                {
                    _Scon.Close();
                }
            }
        }
    }
}
