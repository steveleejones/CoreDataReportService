using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataReportService.Helpers;

namespace CoreDataReportService.Data
{
    public class Insert
    {
        //public static bool AddExportItem(string exportItemName, string exportItem, int ftpId)
        //{
        //    bool success = false;
        //    try
        //    {
        //        string insertSql = "insert into ExportItems (ExportItemName, ExportItem, FTPId) Values (@ExportItemName, @ExportItem, @FTPId)";
        //        using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
        //        {
        //            SqlCommand command = new SqlCommand(insertSql);
        //            command.Connection = connection;

        //            command.Parameters.AddWithValue("@ExportItemName", exportItemName);
        //            command.Parameters.AddWithValue("@ExportItem", exportItem);
        //            command.Parameters.AddWithValue("@FTPId", ftpId);

        //            connection.Open();
        //            command.CommandTimeout = 10;

        //            var resultValue = command.ExecuteReader();
        //            connection.Close();
        //        }
        //        success = true;
        //    }
        //    catch(Exception exception)
        //    {
                
        //    }
        //    return success;
        //}
    }
}
