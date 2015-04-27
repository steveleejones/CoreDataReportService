using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataReportService.Helpers;

namespace CoreDataReportService.Data
{
    public class Update
    {
        //public static void UpdateExportItem(ExportItem exportItem)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
        //        {
        //            SqlCommand sqlCommand = new SqlCommand();
        //            sqlCommand.CommandType = CommandType.Text;
        //            sqlCommand.Connection = conn;

        //            string sql = @" UPDATE dbo.ExportItems SET ExportItemName=@exportItemName, ExportItemData=@exportItemData, FTPId=@ftpId WHERE ExportItemId=@exportItemId";

        //            sqlCommand.Parameters.AddWithValue("@exportItemName", exportItem.ExportItemName);
        //            sqlCommand.Parameters.AddWithValue("@exportItemData", exportItem.SelectStatementBuilder.SerializeToXml());
        //            sqlCommand.Parameters.AddWithValue("@ftpId", exportItem.ExportItemFtpId);
        //            sqlCommand.Parameters.AddWithValue("@exportItemId", exportItem.ExportItemId);

        //            sqlCommand.CommandText = sql;
        //            sqlCommand.CommandTimeout = 200;
        //            conn.Open();

        //            sqlCommand.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}
    }
}
