using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using CoreDataLibrary.Helpers;
using CoreDataLibrary.Models;
using CoreDataLibrary.Objects;

namespace CoreDataLibrary.Data
{
    public class Get
    {
        public static DateTime GetWorkItemLastRunDate(string workItemName)
        {
            DateTime lastRunDateTime = new DateTime();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql =
                        "SELECT LastRunTime FROM CoreDataWorkItems WHERE Name=@workItemName";

                    comm.Parameters.Add("@workItemName", SqlDbType.NVarChar);
                    comm.Parameters["@workItemName"].Value = workItemName;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string dt = dataReader["LastRunTime"].ToString();
                            lastRunDateTime = DateTime.Parse(dt);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "GetWorkItemLastRunDate", e);
            }
            return lastRunDateTime;
        }

        public static ExportItem GetExportItem(string name)
        {
            ExportItem exportItem = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql =
                        "SELECT ExportItemId, ExportItemName, ExportItemData, FTPId, RunTime, ExportEnabled, ExportType FROM ExportItems WHERE ExportItemName=@exportItemName";

                    comm.Parameters.Add("@exportItemName", SqlDbType.VarChar);
                    comm.Parameters["@exportItemName"].Value = name;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            exportItem = new ExportItem(
                                Convert.ToInt32(dataReader["ExportItemId"].ToString()),
                                dataReader["ExportItemName"].ToString(),
                                dataReader["ExportItemData"].ToString(),
                                Convert.ToInt32(dataReader["FTPId"].ToString()),
                                Convert.ToInt32(dataReader["RunTime"].ToString()),
                                Convert.ToInt32(dataReader["ExportEnabled"].ToString()),
                                dataReader["ExportType"].ToString()
                                );
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "Insert->AddExportItem", e);
            }
            return exportItem;
        }

        public static ExportItem GetExportItem(int exportItemId)
        {
            ExportItem exportedItem = new ExportItem();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql =
                        "SELECT ExportItemId, ExportItemName, ExportItem, ExportEnabled FROM ExportItems WHERE ExportItemId=@exportItemId";

                    comm.Parameters.Add("@exportItemId", SqlDbType.Int);
                    comm.Parameters["@exportItemId"].Value = exportItemId;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            exportedItem.ExportItemId = dataReader["ExportItemId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ExportItemId"]);
                            exportedItem.ExportItemName = dataReader["ExportItemName"].ToString();
                            exportedItem.ExportItemData = dataReader["ExportItem"].ToString();
                            int i = dataReader["ExportEnabled"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ExportItemId"]);

                            exportedItem.ExportEnabled = i;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "Insert->AddExportItem", e);
            }
            return exportedItem;
        }

        public static ExportItem GetExportItemByName(string exportItemName)
        {
            ExportItem exportedItem = new ExportItem();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql =
                        "SELECT ExportItemId, ExportItemName, ExportItemData, ExportEnabled, ExportType FROM ExportItems WHERE ExportItemName=@exportItemName";

                    comm.Parameters.Add("@exportItemName", SqlDbType.VarChar);
                    comm.Parameters["@exportItemName"].Value = exportItemName;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            exportedItem.ExportItemId = dataReader["ExportItemId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ExportItemId"]);
                            exportedItem.ExportItemName = dataReader["ExportItemName"].ToString();
                            exportedItem.ExportItemData = dataReader["ExportItemData"].ToString();
                            int i = dataReader["ExportEnabled"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ExportItemId"]);

                            exportedItem.ExportEnabled = i;
                            exportedItem.ExportType = dataReader["ExportType"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "Insert->AddExportItem", e);
            }
            return exportedItem;
        }

        public static string GetLanguageEncoding(string id)
        {
            int languageId = Convert.ToInt32(id);

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT Encoding FROM Language WHERE LanguageId=" + languageId;

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        string encoding = dataReader["Encoding"].ToString();
                        return encoding;
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Get->GetLanguageEncoding", e);
            }
            return "windows-1250";
        }

        public static List<ExportItem> GetRunItems(int hour)
        {
            List<ExportItem> exportItems = new List<ExportItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT ExportItemId, ExportItemName, ExportItemData, FTPId, RunTime, ExportEnabled, ExportType FROM ExportItems WHERE RunTime=@RunTime AND ExportEnabled=1";

                    comm.Parameters.Add("@RunTime", SqlDbType.Int);
                    comm.Parameters["@RunTime"].Value = hour;

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            exportItems.Add(new ExportItem(Convert.ToInt32(dataReader["ExportItemId"].ToString()),
                                dataReader["ExportItemName"].ToString(),
                                dataReader["ExportItemData"].ToString(),
                                Convert.ToInt32(dataReader["FTPId"].ToString()),
                                Convert.ToInt32(dataReader["RunTime"].ToString()),
                                Convert.ToInt32(dataReader["ExportEnabled"].ToString()),
                                dataReader["ExportType"].ToString() 
                                ));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return exportItems;
        }

        public static List<ExportItem> GetAllRunItems()
        {
            List<ExportItem> exportItems = new List<ExportItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT ExportItemId, ExportItemName, ExportItemData, FTPId, RunTime, ExportEnabled, ExportType FROM ExportItems WHERE ExportEnabled=1";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            exportItems.Add(new ExportItem(Convert.ToInt32(dataReader["ExportItemId"].ToString()),
                                dataReader["ExportItemName"].ToString(),
                                dataReader["ExportItemData"].ToString(),
                                Convert.ToInt32(dataReader["FTPId"].ToString()),
                                Convert.ToInt32(dataReader["RunTime"].ToString()),
                                Convert.ToInt32(dataReader["ExportEnabled"].ToString()),
                                dataReader["ExportType"].ToString() 
                                ));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return exportItems;
        }

        public static List<ExportItem> GetDailyRunItems()
        {
            List<ExportItem> exportItems = new List<ExportItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT ExportItemId, ExportItemName, ExportItemData, FTPId, RunTime, ExportEnabled, ExportType FROM ExportItems";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            exportItems.Add(new ExportItem(Convert.ToInt32(dataReader["ExportItemId"].ToString()),
                                dataReader["ExportItemName"].ToString(),
                                dataReader["ExportItemData"].ToString(),
                                Convert.ToInt32(dataReader["FTPId"].ToString()),
                                Convert.ToInt32(dataReader["RunTime"].ToString()),
                                Convert.ToInt32(dataReader["ExportEnabled"].ToString()),
                                dataReader["ExportType"].ToString() 
                                ));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return exportItems;
        }

        public static FtpItem GetFtpItem(int id)
        {
            FtpItem ftpItem = new FtpItem();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT FTPName, FTPAddress, FTPUserName, FTPPassword FROM FTPSites WHERE FTPId=@id";

                    comm.Parameters.Add("@id", SqlDbType.Int);
                    comm.Parameters["@id"].Value = id;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ftpItem.FtpName = dataReader["FtpName"].ToString();
                            ftpItem.FtpAddress = dataReader["FTPAddress"].ToString();
                            ftpItem.FtpUsername = dataReader["FTPUserName"].ToString();
                            ftpItem.FtpPassword = dataReader["FTPPassword"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return ftpItem;
        }

        public static int GetFtpItemId(string name)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT FTPId FROM FTPSites WHERE FTPName=@name";

                    comm.Parameters.Add("@name", SqlDbType.VarChar);
                    comm.Parameters["@name"].Value = name;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            return dataReader["FTPId"] == DBNull.Value
                               ? 0
                               : Convert.ToInt32(dataReader["FTPId"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return 0;
        }

        public static List<ListItem> GetCountryItems()
        {
            List<ListItem> countryList = new List<ListItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT DISTINCT Country, CountryId FROM [CoreData].[dbo].[Properties] ORDER BY Country ASC";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            countryList.Add(new ListItem(dataReader["Country"].ToString(), dataReader["CountryId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return countryList;
        }

        public static List<ListItem> GetFtpListItems()
        {
            List<ListItem> ftpList = new List<ListItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT FTPId, FTPName FROM [CoreData].[dbo].[FTPSites]";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ftpList.Add(new ListItem(dataReader["FTPName"].ToString(), dataReader["FTPId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return ftpList;
        }

        public static List<ListItem> GetLanguages()
        {
            List<ListItem> languageNameList = new List<ListItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT TOP 13 LanguageName, LanguageId FROM Language";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            languageNameList.Add(new ListItem(dataReader["LanguageName"].ToString(), dataReader["LanguageId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return languageNameList;
        }

        public static List<ListItem> GetRegionItems(int countryId)
        {
            List<ListItem> regionList = new List<ListItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT DISTINCT Region, RegionId FROM [CoreData].[dbo].[Properties] WHERE CountryId=@countryId ORDER BY Region ASC";

                    comm.Parameters.Add("@countryId", SqlDbType.Int);
                    comm.Parameters["@countryId"].Value = countryId;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            regionList.Add(new ListItem(dataReader["Region"].ToString(), dataReader["RegionId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return regionList;
        }

        public static List<ListItem> GetResortItems(int regionId)
        {
            List<ListItem> resortList = new List<ListItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT DISTINCT Resort, ResortId FROM [CoreData].[dbo].[Properties] WHERE RegionId=@regionId ORDER BY Resort ASC";

                    comm.Parameters.Add("@regionId", SqlDbType.Int);
                    comm.Parameters["@regionId"].Value = regionId;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            resortList.Add(new ListItem(dataReader["Resort"].ToString(), dataReader["ResortId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return resortList;
        }

        public static List<ListItem> GetExportItems()
        {
            List<ListItem> exportedItemsList = new List<ListItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT ExportItemId, ExportItemName FROM ExportItems ORDER BY ExportItemName ASC";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            exportedItemsList.Add(new ListItem(dataReader["ExportItemName"].ToString(), dataReader["ExportItemId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return exportedItemsList;
        }

        public static List<ServiceConfiguration> GetServiceConfigurationItems(string serviceName)
        {
            List<ServiceConfiguration> serviceConfigurationItems = new List<ServiceConfiguration>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT ServiceConfigurationName, ServiceConfigurationItem, ServiceConfigurationValue FROM ServiceConfiguration WHERE ServiceConfigurationName=@ServiceName";

                    comm.Parameters.Add("@ServiceName", SqlDbType.VarChar);
                    comm.Parameters["@ServiceName"].Value = serviceName;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            serviceConfigurationItems.Add(new ServiceConfiguration(dataReader["ServiceConfigurationName"].ToString(), dataReader["ServiceConfigurationItem"].ToString(), dataReader["ServiceConfigurationValue"].ToString()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return serviceConfigurationItems;
        }

        public static List<LogEntry> GetSuccesfulActiveLogItems()
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            List<LogEntry> logEntries = new List<LogEntry>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus FROM Logs WHERE LogItemStatus='Active' ORDER BY StartTimeStamp DESC";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();

                            logEntries.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntries;
        }

        public static List<LogEntry> GetUnSuccesfulActiveLogItems()
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            List<LogEntry> logEntries = new List<LogEntry>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus FROM Logs ORDER BY StartTimeStamp DESC";// " WHERE LogItemStatus='Active'";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();

                            if (logEntry.LogItemMessage.Contains("Error"))
                                logEntries.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntries;
        }

        public static List<LogEntry> GetInActiveLogItems()
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            List<LogEntry> logEntries = new List<LogEntry>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus FROM Logs WHERE LogItemStatus='InActive' ORDER BY StartTimeStamp DESC";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();

                            logEntries.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntries;
        }

        public static List<LogEntry> GetErrorLogItems()
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            List<LogEntry> logEntries = new List<LogEntry>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus FROM Logs WHERE LogItemMessage LIKE 'ERROR%' ORDER BY StartTimeStamp DESC";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();

                            logEntries.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntries;
        }

        public static int GetDayLogItemCount()
        {
            int count = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT COUNT(*) FROM [CoreData_Test].[dbo].[Logs]";

                    comm.CommandText = sql;
                    conn.Open();
                    count = Convert.ToInt32(comm.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "GetDayLogItemCount", e);
            }
            return count;
        }

        public static List<LogEntry> GetDayLogItems(DateTime dateTime)
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            List<LogEntry> logEntries = new List<LogEntry>();
            DateTime start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            DateTime end = start.AddDays(1.0);

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus, LogItemRemoved FROM Logs WHERE StartTimeStamp BETWEEN @start AND @end AND (LogItemRemoved='False' OR LogItemRemoved IS NULL) ORDER BY LogItemStatus ASC, StartTimeStamp DESC";

                    comm.Parameters.AddWithValue("@start", start);
                    comm.Parameters.AddWithValue("@end", end);

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();

                            if (logEntry.LogItemStatus != "Message")
                                logEntries.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntries;
        }

        public static List<LogEntry> GetDayMessages(DateTime dateTime)
        {
            List<LogEntry> logEntries = new List<LogEntry>();
            DateTime start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            DateTime end = start.AddDays(1.0);

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus, LogItemRemoved FROM Logs WHERE StartTimeStamp BETWEEN @start AND @end AND (LogItemRemoved='False' OR LogItemRemoved IS NULL) ORDER BY LogItemStatus ASC, StartTimeStamp DESC";

                    comm.Parameters.AddWithValue("@start", start);
                    comm.Parameters.AddWithValue("@end", end);

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();

                            if (logEntry.LogItemStatus == "Message")
                                logEntries.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntries;
        }

        public static List<LogItemStep> GetActiveLogSteps()
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            List<LogItemStep> logSteps = new List<LogItemStep>();
            try
            {
                //using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT Id, LogItemId, StartTimeStamp, EndTimeStamp, Step, Status, Messages FROM LogSteps WHERE Status='Active' ORDER BY StartTimeStamp DESC";

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogItemStep logEntry = new LogItemStep();
                            logEntry.Id = (int)dataReader["Id"];
                            logEntry.LogItemId = (int)dataReader["LogItemId"];
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.Step = dataReader["Step"].ToString();
                            logEntry.Status = dataReader["Status"].ToString();
                            logEntry.Messages = dataReader["Messages"].ToString();

                            logSteps.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logSteps;
        }

        public static List<LogItemStep> GetLogSteps(int logItemId)
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            List<LogItemStep> logSteps = new List<LogItemStep>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT Id, LogItemId, StartTimeStamp, EndTimeStamp, Step, Status, Messages FROM LogSteps WHERE LogItemId=@logItemId ORDER BY StartTimeStamp DESC";

                    comm.Parameters.Add("@logItemId", SqlDbType.Int);
                    comm.Parameters["@logItemId"].Value = logItemId;

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogItemStep logEntry = new LogItemStep();
                            logEntry.Id = (int)dataReader["Id"];
                            logEntry.LogItemId = (int)dataReader["LogItemId"];
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.Step = dataReader["Step"].ToString();
                            logEntry.Status = dataReader["Status"].ToString();
                            logEntry.Messages = dataReader["Messages"].ToString();

                            logSteps.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logSteps;
        }

        public static LogItemStep GetLogStep(int Id)
        {
            const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
            LogItemStep logStep = new LogItemStep();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT Id, LogItemId, StartTimeStamp, EndTimeStamp, Step, Status, Messages FROM LogSteps WHERE Id=@id ORDER BY StartTimeStamp DESC";

                    comm.Parameters.Add("@id", SqlDbType.Int);
                    comm.Parameters["@id"].Value = Id;

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            logStep.Id = (int)dataReader["Id"];
                            logStep.LogItemId = (int)dataReader["LogItemId"];
                            logStep.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logStep.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logStep.Step = dataReader["Step"].ToString();
                            logStep.Status = dataReader["Status"].ToString();
                            logStep.Messages = dataReader["Messages"].ToString();

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logStep;
        }

        public static LogEntry GetLogEntry(int id)
        {
            LogEntry logEntry = new LogEntry();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                //using (SqlConnection conn = new SqlConnection(SqlConnCoreData_Live))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus FROM Logs WHERE LogId=@logId";

                    comm.Parameters.AddWithValue("@logId", id);

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntry;
        }

        public static List<LogEntry> GetLogs()
        {
            List<LogEntry> logEntries = new List<LogEntry>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LogId, LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus FROM Logs";


                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.LogId = (int)dataReader["LogId"];
                            logEntry.LogItemName = dataReader["LogItemName"].ToString();
                            logEntry.StartTimeStamp = dataReader["StartTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                            logEntry.EndTimeStamp = dataReader["EndTimeStamp"] == DBNull.Value
                                ? new DateTime(1, 1, 1, 1, 1, 1)
                                : DateTime.Parse(dataReader["EndTimeStamp"].ToString());
                            logEntry.LogItemMessage = dataReader["LogItemMessage"].ToString();
                            logEntry.LogItemStatus = dataReader["LogItemStatus"].ToString();
                            logEntries.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return logEntries;
        }

        public static List<ConfigOPFile> GetInternationalOfferLoaderFiles()
        {
            List<ConfigOPFile> files = new List<ConfigOPFile>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.InternationalOfferLoaderConnection))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql =
                        "SELECT FileCriteriaID, FileName, FileType, Site, PackageGroup, SellingCurrency ,MaxOffers ,StartDays ,EndDays ,MaxPrice ,MinPrice ,OutputName ,OutputPath ,Template ,Frequency ,Login ,CastlesOnly ,Include FROM ConfigOPFile WHERE Include=1";

                    //comm.Parameters.Add("@exportItemName", SqlDbType.VarChar);
                    //comm.Parameters["@exportItemName"].Value = name;
                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ConfigOPFile configOPFile = new ConfigOPFile();
                            configOPFile.FileCriteriaID = dataReader["FileCriteriaID"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["FileCriteriaID"]);
                            configOPFile.FileName = dataReader["FileName"].ToString();
                            configOPFile.FileType = dataReader["FileType"].ToString();
                            configOPFile.Site = dataReader["Site"].ToString();
                            configOPFile.PackageGroup = dataReader["PackageGroup"].ToString();
                            configOPFile.SellingCurrency = dataReader["SellingCurrency"].ToString();
                            configOPFile.MaxOffers = dataReader["MaxOffers"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["MaxOffers"]);
                            configOPFile.StartDays = dataReader["StartDays"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["StartDays"]);
                            configOPFile.EndDays = dataReader["EndDays"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["EndDays"]);
                            configOPFile.MaxPrice = dataReader["MaxPrice"] == DBNull.Value
                               ? 0
                               : Convert.ToDecimal(dataReader["MaxPrice"]);
                            configOPFile.MinPrice = dataReader["MinPrice"] == DBNull.Value
                               ? 0
                               : Convert.ToDecimal(dataReader["MinPrice"]);
                            configOPFile.OutputName = dataReader["OutputName"].ToString();
                            configOPFile.OutputPath = dataReader["OutputPath"].ToString();
                            configOPFile.Template = dataReader["Template"].ToString();
                            configOPFile.Frequency = dataReader["Frequency"].ToString();
                            configOPFile.Login = dataReader["Login"].ToString();
                            int i = dataReader["CastlesOnly"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["CastlesOnly"]);
                            configOPFile.CastlesOnly = i == 1;
                            i = dataReader["Include"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["Include"]);
                            configOPFile.Include = i == 1;

                            files.Add(configOPFile);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "NonCore  [SVRSQL4]: Error ",
                    "GetInternationalOfferLoaderFiles", e);
            }
            return files;
        }

        public static DateTime GetReportLastRunDateTime(string reportName)
        {
            DateTime lastRunTime = DateTime.MinValue;
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {

                    SqlCommand comm = new SqlCommand();
                    comm.CommandType = CommandType.Text;
                    comm.Connection = conn;
                    string sql = "SELECT LastRunTime FROM ReportRuns WHERE ReportName=@reportName";

                    comm.Parameters.AddWithValue("@reportName", reportName);

                    comm.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = comm.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            lastRunTime = dataReader["LastRunTime"] == DBNull.Value
                                ? DateTime.MinValue
                                : DateTime.Parse(dataReader["LastRunTime"].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", e);
            }
            return lastRunTime;
        }


        public static DateTime LogItemLastExecute(string logItemName)
        {
            DateTime lastRunTime = DateTime.MinValue;

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT TOP 1 StartTimeStamp FROM Logs WHERE LogItemName=@logItemName ORDER BY StartTimeStamp DESC";

                    sqlCommand.Parameters.AddWithValue("@logItemName", logItemName);

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            lastRunTime = dataReader["StartTimeStamp"] == DBNull.Value
                                ? DateTime.MinValue
                                : DateTime.Parse(dataReader["StartTimeStamp"].ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->CancelLog", e);
                }
            }
            return lastRunTime;
        }

        public static List<string> GetQueItems()
        {
            List<string> queItems = new List<string>();

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT QueItemName FROM ExportItemQue";

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            queItems.Add(dataReader["QueItemName"].ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetQueItems", e);
                }
            }
            CoreDataLibrary.Data.Process.ClearExportItemQue();
            return queItems;
        }

        public static List<string> GetWorkItemQueItems()
        {
            List<string> queItems = new List<string>();

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnNonCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT WorkItem FROM WorkItemQue";

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            queItems.Add(dataReader["WorkItem"].ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetQueItems", e);
                }
            }
            CoreDataLibrary.Data.Process.ClearWorkItemQue();
            return queItems;
        }

        public static List<string> GetAirports()
        {
            List<string> airports = new List<string>();

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT AirportName FROM Airports";

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            airports.Add(dataReader["AirportName"].ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetQueItems", e);
                }
            }
            return airports;
        }

        public static string GetAirportName(int id)
        {
            string airportName = string.Empty;

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT AirportName FROM Airports WHERE AirportId=@id";

                    sqlCommand.Parameters.AddWithValue("@id", id);

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            airportName = dataReader["AirportName"].ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetQueItems", e);
                }
            }
            return airportName;
        }

        public static string GetMealBasisName(int id)
        {
            string mealBasisName = string.Empty;

            using (SqlConnection conn = new SqlConnection(DataConnection.MssqldevConnection))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT MealBasis FROM [IVDB].[LCB].[dbo].[MealBasis] WHERE MealBasisID=@id";

                    sqlCommand.Parameters.AddWithValue("@id", id);

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            mealBasisName = dataReader["MealBasis"].ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetMealBasisName", e);
                }
            }
            return mealBasisName; 
        }

        public static string GetMealBasisCode(int id)
        {
            string mealBasisCode = string.Empty;

            using (SqlConnection conn = new SqlConnection(DataConnection.MssqldevConnection))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT MealBasisCode FROM [IVDB].[LCB].[dbo].[MealBasis] WHERE MealBasisID=@id";

                    sqlCommand.Parameters.AddWithValue("@id", id);

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            mealBasisCode = dataReader["MealBasis"].ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetMealBasisName", e);
                }
            }
            return mealBasisCode;
        }

        public static List<Property> GetProperties()
        {
            List<Property> Properties = new List<Property>();

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT PropertyReferenceId ,PropertyName ,Country ,Region ,Resort ,CountryId ,RegionId ,ResortId ,IncludesOwnStock, BestSeller ,Rating ,ContractCount ,PropertyTypeId FROM Properties";

                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            Property property = new Property();
                            property.PropertyRefrenceId = dataReader["PropertyReferenceId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["PropertyReferenceId"]);
                            property.PropertyName = dataReader["PropertyName"].ToString();
                            property.Country = dataReader["Country"].ToString();
                            property.Region = dataReader["Region"].ToString();
                            property.Resort = dataReader["Resort"].ToString();
                            property.CountryId = dataReader["CountryId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["CountryId"]);
                            property.RegionId = dataReader["RegionId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["RegionId"]);
                            property.ResortId = dataReader["ResortId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ResortId"]);
                            property.IncludesOwnStock = dataReader["IncludesOwnStock"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["IncludesOwnStock"]);
                            property.BestSeller = dataReader["BestSeller"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["BestSeller"]);
                            property.Rating = dataReader["Rating"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["Rating"]);
                            property.ContractCount = dataReader["ContractCount"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ContractCount"]);
                            property.PropertyTypeId = dataReader["PropertyTypeId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["PropertyTypeId"]);

                            Properties.Add(property);
                            if (Properties.Count >= 50)
                                return Properties;
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetQueItems", e);
                }
            }
            return Properties;
        }

        public static Property GetProperty(int id)
        {
            Property property = new Property();

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT PropertyReferenceId ,PropertyName ,Country ,Region ,Resort ,CountryId ,RegionId ,ResortId ,IncludesOwnStock, BestSeller ,Rating ,ContractCount ,PropertyTypeId FROM Properties WHERE PropertyReferenceId=@propertyId";

                    sqlCommand.Parameters.AddWithValue("@propertyId", id);

                    sqlCommand.CommandText = sql;

                    conn.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            
                            property.PropertyRefrenceId = dataReader["PropertyReferenceId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["PropertyReferenceId"]);
                            property.PropertyName = dataReader["PropertyName"].ToString();
                            property.Country = dataReader["Country"].ToString();
                            property.Region = dataReader["Region"].ToString();
                            property.Resort = dataReader["Resort"].ToString();
                            property.CountryId = dataReader["CountryId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["CountryId"]);
                            property.RegionId = dataReader["RegionId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["RegionId"]);
                            property.ResortId = dataReader["ResortId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ResortId"]);
                            property.IncludesOwnStock = dataReader["IncludesOwnStock"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["IncludesOwnStock"]);
                            property.BestSeller = dataReader["BestSeller"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["BestSeller"]);
                            property.Rating = dataReader["Rating"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["Rating"]);
                            property.ContractCount = dataReader["ContractCount"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["ContractCount"]);
                            property.PropertyTypeId = dataReader["PropertyTypeId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dataReader["PropertyTypeId"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Get->GetQueItems", e);
                }
            }
            return property;
        }

        public static bool PropertyDescriptionCountNotEqual()
        {
            int lastRunCount = GetLastRunPropertyDescriptionCount();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 60000;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT COUNT(prr.PropertyReferenceId) FROM  IVDB.lcb.dbo.PropertyReference prr ";
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    int res = (int)sqlCommand.ExecuteScalar();

                    if (res > lastRunCount)
                    {
                        UpdateLastRunPropertyDescriptionCount(res);
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                return false;
            }
            return false;
        }

        private static void UpdateLastRunPropertyDescriptionCount(int res)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandTimeout = 60000;
                sqlCommand.Connection = conn;

                string sql = @"UPDATE CoreData SET Value=@value WHERE Item='PropertyDescriptionCount'";
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@value", res.ToString());

                conn.Open();
                sqlCommand.CommandTimeout = 1000;
                sqlCommand.ExecuteScalar();
                conn.Close();
            }
        }

        private static int GetLastRunPropertyDescriptionCount()
        {
            int res = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 60000;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT Value FROM CoreData WHERE Item='PropertyDescriptionCount'";
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        res = Convert.ToInt32(reader["Value"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                return res;
            }
            return res;
        }

        public static bool IvdbCurrent()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 60000;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT TOP 1 [BookingDate] FROM [IVDB].[LCB].[dbo].[Booking] ORDER BY [BookingDate] DESC";
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime lastBookingDateTime = DateTime.Parse(reader["BookingDate"].ToString());
                            TimeSpan timeSpan = DateTime.Now - lastBookingDateTime;
                            return !(timeSpan.TotalHours > 24);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                return false;
            }
            return false;
        }

        public static DateTime ReadCoreDataWorkItemLastRunTimeFromDb(string name)
        {
            var lastRunDateTime = new DateTime();
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
                        {
                            var dt = dataReader["LastRunTime"].ToString();
                            lastRunDateTime = DateTime.Parse(dt);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "GetWorkItemLastRunDate", e);
            }
            return lastRunDateTime;
        }
    }
}
