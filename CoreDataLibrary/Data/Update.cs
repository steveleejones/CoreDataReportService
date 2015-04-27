using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Data
{
    public class Update
    {
        public static void UpdateExportItem(ExportItem exportItem)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @" UPDATE dbo.ExportItems SET ExportItemName=@exportItemName, ExportItemData=@exportItemData, FTPId=@ftpId, RunTime=@runTime, ExportEnabled=@exportEnabled, ExportType=@exportType WHERE ExportItemId=@exportItemId";

                    sqlCommand.Parameters.AddWithValue("@exportItemName", exportItem.ExportItemName);
                    sqlCommand.Parameters.AddWithValue("@exportItemData", exportItem.SelectStatementBuilder.SerializeToXml());
                    sqlCommand.Parameters.AddWithValue("@ftpId", exportItem.ExportItemFtpId);
                    sqlCommand.Parameters.AddWithValue("@runTime", exportItem.ExportItemRunTime);
                    sqlCommand.Parameters.AddWithValue("@exportItemId", exportItem.ExportItemId);
                    sqlCommand.Parameters.AddWithValue("@exportEnabled", exportItem.ExportEnabled);
                    sqlCommand.Parameters.AddWithValue("@exportType", exportItem.ExportType);

                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandTimeout = 200;
                    conn.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Update->UpdateExportItem", e);
            }
        }

        public static void UpdateFtp(string name, string address, string username, string password, int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @" UPDATE dbo.FTPSites SET FTPName=@name, FTPAddress=@address, FTPUserName=@username, FTPPassword=@password WHERE FTPId=@id";

                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@address", address);
                    sqlCommand.Parameters.AddWithValue("@username", username);
                    sqlCommand.Parameters.AddWithValue("@password", password);

                    sqlCommand.CommandText = sql;
                    sqlCommand.CommandTimeout = 200;
                    conn.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Update->UpdateExportItem", e);
            }
        }

        public static bool UpdateReportLastRunDateTime(string reportName, DateTime dateTime)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "UPDATE ReportRuns SET LastRunTime=@lastRunTime WHERE ReportName=@reportName";

                    comm.Parameters.AddWithValue("@lastRunTime", dateTime);
                    comm.Parameters.AddWithValue("@reportName", reportName);

                    comm.CommandText = sql;
                    conn.Open();

                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
