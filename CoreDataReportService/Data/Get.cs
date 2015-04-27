using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreDataReportService.Helpers;

namespace CoreDataReportService.Data
{
    public class Get
    {
        //public static ExportItem GetExportItem(string name)
        //{
        //    ExportItem exportItem = null;   
        //    using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
        //    {
        //        SqlCommand comm = new SqlCommand();
        //        comm.CommandType = CommandType.Text;
        //        comm.Connection = conn;
        //        string sql = "SELECT ExportItemId, ExportItemName, ExportItemData, FTPId FROM ExportItems WHERE ExportItemName=@exportItemName";

        //        comm.Parameters.Add("@exportItemName", SqlDbType.VarChar);
        //        comm.Parameters["@exportItemName"].Value = name;
        //        comm.CommandText = sql;
        //        conn.Open();
        //        SqlDataReader dataReader = comm.ExecuteReader();

        //        if (dataReader.HasRows)
        //        {
        //            while (dataReader.Read())
        //            {
        //                exportItem = new ExportItem(Convert.ToInt32(dataReader["ExportItemId"].ToString()), dataReader["ExportItemName"].ToString(), dataReader["ExportItemData"].ToString(), Convert.ToInt32(dataReader["FTPId"].ToString()));
        //            }
        //        }
        //    }
        //    return exportItem;
        //}

        //public static ExportItem GetExportItem(int exportItemId)
        //{
        //    ExportItem exportedItem = new ExportItem();
        //    using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
        //    {
        //        SqlCommand comm = new SqlCommand();
        //        comm.CommandType = CommandType.Text;
        //        comm.Connection = conn;
        //        string sql = "SELECT ExportItemId, ExportItemName, ExportItem FROM ExportItems WHERE ExportItemId=@exportItemId";

        //        comm.Parameters.Add("@exportItemId", SqlDbType.Int);
        //        comm.Parameters["@exportItemId"].Value = exportItemId;
        //        comm.CommandText = sql;
        //        conn.Open();
        //        SqlDataReader dataReader = comm.ExecuteReader();

        //        if (dataReader.HasRows)
        //        {
        //            while (dataReader.Read())
        //            {
        //                exportedItem.ExportItemId = dataReader["ExportItemId"] == DBNull.Value
        //                    ? 0
        //                    : Convert.ToInt32(dataReader["ExportItemId"]);
        //                exportedItem.ExportItemName = dataReader["ExportItemName"].ToString();
        //                exportedItem.ExportItemData = dataReader["ExportItem"].ToString();

        //            }
        //        }
        //    }
        //    return exportedItem;
        //}

        //public static List<ExportItem> GetDailyRunItems()
        //{
        //    List<ExportItem> exportItems = new List<ExportItem>();
        //    using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
        //    {
        //        SqlCommand comm = new SqlCommand();
        //        comm.CommandType = CommandType.Text;
        //        comm.Connection = conn;
        //        string sql = "SELECT ExportItemId, ExportItemName, ExportItemData, FTPId FROM ExportItems";

        //        comm.CommandText = sql;
        //        conn.Open();
        //        SqlDataReader dataReader = comm.ExecuteReader();

        //        if (dataReader.HasRows)
        //        {
        //            while (dataReader.Read())
        //            {
        //                exportItems.Add(new ExportItem(Convert.ToInt32(dataReader["ExportItemId"].ToString()), dataReader["ExportItemName"].ToString(), dataReader["ExportItemData"].ToString(), Convert.ToInt32(dataReader["FTPId"].ToString())));
        //            }
        //        }
        //    }
        //    return exportItems;
        //}
        //public static FtpItem GetFtpItem(int id)
        //{
        //    FtpItem ftpItem = new FtpItem();
        //    using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
        //    {
        //        SqlCommand comm = new SqlCommand();
        //        comm.CommandType = CommandType.Text;
        //        comm.Connection = conn;
        //        string sql = "SELECT FTPAddress, FTPUserName, FTPPassword FROM FTPSites WHERE FTPId=@id";

        //        comm.Parameters.Add("@id", SqlDbType.Int);
        //        comm.Parameters["@id"].Value = id;
        //        comm.CommandText = sql;
        //        conn.Open();
        //        SqlDataReader dataReader = comm.ExecuteReader();

        //        if (dataReader.HasRows)
        //        {
        //            while (dataReader.Read())
        //            {
        //                ftpItem.FtpAddress = dataReader["FTPAddress"].ToString();
        //                ftpItem.FtpUsername = dataReader["FTPUserName"].ToString();
        //                ftpItem.FtpPassword = dataReader["FTPPassword"].ToString();
        //            }
        //        }
        //    }
        //    return ftpItem;
        //}
    }
}
