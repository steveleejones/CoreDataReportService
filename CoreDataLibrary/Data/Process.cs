using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Data
{
    public class Process
    {
        public static bool PropertySupplierMapping()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    RemoveTemporaryPropertySupplierMappingTable();
                    SqlCommand command = new SqlCommand("dbo.uspPropertySupplierMapping", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 72000;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->PropertySupplierMapping", e);
                    return false;
                }
            }
            System.Threading.Thread.Sleep(10000);
            return true;
        }

        public static bool RunStoredProcedure(string connectionString, string storedProcedure, ReportLogger reportLogger)
        {
            int logIdstepId = reportLogger.AddStep();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(storedProcedure, conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 72000;
                    conn.Open();
                    command.ExecuteNonQuery();
                    reportLogger.EndStep(logIdstepId);
                }
                catch (Exception e)
                {
                    reportLogger.EndStep(logIdstepId, e);
                    return false;
                }
            }
            return true;
        }
        public static bool ExportPropertySupplierMapping()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand command = new SqlCommand("dbo.ExportPropertySupplierMapping", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 72000;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->PropertySupplierMapping", e);
                    return false;
                }
            }
            return true;
        }

        public static void RemoveTemporaryPropertySupplierMappingTable()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand command = new SqlCommand("DROP TABLE PropertySupplierMappingTemp", conn);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 72000;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->RemoveTemporaryPropertySupplierMappingTable", e);
                }
            }
        }

        public static void ClearExportItemQue()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand command = new SqlCommand("DELETE FROM ExportItemQue", conn);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 72000;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->ClearExportItemQue", e);
                }
            }
        }

        public static void ClearWorkItemQue()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnNonCoreData))
            {
                try
                {
                    SqlCommand command = new SqlCommand("DELETE FROM WorkItemQue", conn);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 72000;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->ClearExportItemQue", e);
                }
            }
        }

        public static void CancelLog(int id)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @" UPDATE dbo.Logs SET LogItemStatus=@status WHERE LogId=@id";

                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@status", "Cancelled");

                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandTimeout = 200;
                    conn.Open();

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->CancelLog", e);
                }
            }
        }

        public static void MarkLogItemAsRemoved(int id)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @" UPDATE dbo.Logs SET LogItemRemoved=@removed WHERE LogId=@id";

                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@removed", true);

                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandTimeout = 200;
                    conn.Open();

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->CancelLog", e);
                }
            }
        }

        public static bool TableExists(SqlConnection connection, string tableName)
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                var sql = string.Format(
                        "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{0}'",
                         tableName);
                command.CommandText = sql;
                command.Connection.Open();

                var count = Convert.ToInt32(command.ExecuteScalar());
                return (count > 0);
            }
        }

        public static void SaveWorkItemLastRunDateTime(string name, DateTime dateTime)
        {
            if (WorkItemExists(name))
                UpdateWorkItemLastRunDateTime(name, dateTime);
            else
                AddWorkItemLastRunDateTime(name, dateTime);
        }

        private static void AddWorkItemLastRunDateTime(string name, DateTime lastRunTime)
        {
            try
            {
                var insertSql = "INSERT INTO CoreDataWorkItems (Name, LastRunTime) Values (@name, @lastRunTime)";
                using (var connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    var command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@lastRunTime", lastRunTime);

                    connection.Open();
                    command.CommandTimeout = 10;

                    var resultValue = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "Process->AddWorkItemLastRunDateTime", exception);
            }
        }

        private static void UpdateWorkItemLastRunDateTime(string name, DateTime dateTime)
        {
            try
            {
                using (var conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    var sql = @" UPDATE CoreDataWorkItems SET LastRunTime=@lastRunTime WHERE Name=@name";

                    sqlCommand.Parameters.AddWithValue("@lastRunTime", dateTime);
                    sqlCommand.Parameters.AddWithValue("@name", name);

                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandTimeout = 200;
                    conn.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "Process->UpdateWorkItemLastRunDateTime", e);
            }
        }

        private static bool WorkItemExists(string name)
        {
            try
            {
                using (var conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    var comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    var sql =
                        "SELECT LastRunTime FROM CoreDataWorkItems WHERE Name=@name";

                    comm.Parameters.Add("@name", SqlDbType.NVarChar);
                    comm.Parameters["@name"].Value = name;
                    comm.CommandText = sql;
                    conn.Open();
                    var dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                            return true;
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "Process.WorkItemExists", e);
            }
            return false;
        }
    }
}