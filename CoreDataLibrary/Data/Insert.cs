using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Helpers;
using CoreDataLibrary.Objects;

namespace CoreDataLibrary.Data
{
    public class Insert
    {
        public static void AddCoreDataWorkItem(string coreDataWorkItemName, DateTime dateTime)
        {
            try
            {
                string insertSql = "INSERT INTO CoreDataWorkItems (Name, LastRunTime) Values (@CoreDataWorkItemName, @DateTime)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@CoreDataWorkItemName", coreDataWorkItemName);
                    command.Parameters.AddWithValue("@DateTime", dateTime);

                    connection.Open();
                    command.CommandTimeout = 10;

                    var resultValue = command.ExecuteReader();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddCoreDataWorkItem", exception);
            }
        }

        public static bool AddExportItem(string exportItemName, string exportItem, int ftpId, int runTime, int exportEnabled, string exportItemExportType)
        {
            bool success = false;
            try
            {
                string insertSql = "insert into ExportItems (ExportItemName, ExportItemData, FTPId, RunTime, ExportEnabled, ExportType) Values (@ExportItemName, @ExportItemData, @FTPId, @RunTime, @exportEnabled, @exportType)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@ExportItemName", exportItemName);
                    command.Parameters.AddWithValue("@ExportItemData", exportItem);
                    command.Parameters.AddWithValue("@FTPId", ftpId);
                    command.Parameters.AddWithValue("@RunTime", runTime);
                    command.Parameters.AddWithValue("@ExportEnabled", exportEnabled);
                    command.Parameters.AddWithValue("@ExportType", exportItemExportType);

                    connection.Open();
                    command.CommandTimeout = 10;

                    var resultValue = command.ExecuteReader();
                    connection.Close();
                }
                success = true;
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", exception);
            }
            return success;
        }

        public static bool AddFtpSite(string ftpName, string ftpAddress, string ftpUseerName, string ftpPassword)
        {
            bool success = false;
            try
            {
                string insertSql = "insert into FTPSites (FTPName, FTPAddress, FTPUserName, FTPPassword) Values (@FTPName, @FTPAddress, @FTPUserName, @FTPPassword)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@FTPName", ftpName);
                    command.Parameters.AddWithValue("@FTPAddress", ftpAddress);
                    command.Parameters.AddWithValue("@FTPUserName", ftpUseerName);
                    command.Parameters.AddWithValue("@FTPPassword", ftpPassword);

                    connection.Open();
                    command.CommandTimeout = 10;

                    var resultValue = command.ExecuteReader();
                    connection.Close();
                }
                success = true;
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddExportItem", exception);
            }
            return success;
        }

        public static int AddLogStart(string logItem, DateTime startTime, string message)
        {
            int idValue = 0;
            try
            {
                string insertSql = "INSERT INTO Logs (LogItemName, StartTimeStamp, LogItemMessage, LogItemStatus) OUTPUT INSERTED.LogId Values (@LogItemName, @StartTimeStamp, @LogItemMessage, @LogItemStatus)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@LogItemName", logItem);
                    command.Parameters.AddWithValue("@StartTimeStamp", startTime);
                    command.Parameters.AddWithValue("@LogItemMessage", message);
                    command.Parameters.AddWithValue("@LogItemStatus", "Active");

                    connection.Open();
                    command.CommandTimeout = 1000;

                    idValue = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
            return idValue;
        }

        public static int AddLogStep(int logItemId, DateTime startTime, string step)
        {
            int idValue = 0;
            try
            {
                string insertSql = "INSERT INTO LogSteps (LogItemId, StartTimeStamp, Step, Status) OUTPUT INSERTED.Id Values (@LogItemId, @StartTimeStamp, @Step, @Status)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@LogItemId", logItemId);
                    command.Parameters.AddWithValue("@StartTimeStamp", startTime);
                    command.Parameters.AddWithValue("@Step", step);
                    command.Parameters.AddWithValue("@Status", "Active");

                    connection.Open();
                    command.CommandTimeout = 1000;

                    idValue = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
            return idValue;
        }

        public static bool AddLogEnd(int logId, DateTime endTimeStampTime, string message)
        {
            bool success = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    string sql = @" UPDATE dbo.Logs SET EndTimeStamp=@endTimeStamp, LogItemMessage=@logItemMessage, LogItemStatus=@logItemStatus WHERE LogId=@logId";

                    SqlCommand command = new SqlCommand(sql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@endTimeStamp", endTimeStampTime);
                    command.Parameters.AddWithValue("@logItemMessage", message);
                    command.Parameters.AddWithValue("@logId", logId);
                    command.Parameters.AddWithValue("@logItemStatus", "InActive");

                    connection.Open();
                    command.CommandTimeout = 10;

                    var resultValue = command.ExecuteReader();
                    connection.Close();
                }
                success = true;
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogEnd", exception);
            }
            return success;
        }

        public static bool AddLogStepEnd(int stepId, DateTime endTimeStampTime)
        {
            return AddLogStepEnd(stepId, endTimeStampTime, "Completed");
        }

        public static bool AddLogStepEnd(int stepId, DateTime endTimeStampTime, string message)
        {
            bool success = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    string sql = @" UPDATE dbo.LogSteps SET EndTimeStamp=@endTimeStamp, Status=@status, Messages=@messages WHERE Id=@stepId";

                    SqlCommand command = new SqlCommand(sql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@endTimeStamp", endTimeStampTime);
                    command.Parameters.AddWithValue("@status", "InActive");
                    command.Parameters.AddWithValue("@messages", message);
                    command.Parameters.AddWithValue("@stepId", stepId);

                    connection.Open();
                    command.CommandTimeout = 10;

                    var resultValue = command.ExecuteReader();
                    connection.Close();
                }
                success = true;
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogEnd", exception);
            }
            return success;
        }

        public static void AddMessage(string name, string message)
        {
            try
            {
                string insertSql = "INSERT INTO Logs (LogItemName, StartTimeStamp, EndTimeStamp, LogItemMessage, LogItemStatus) OUTPUT INSERTED.LogId Values (@LogItemName, @StartTimeStamp, @EndTimeStamp, @LogItemMessage, @LogItemStatus)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@LogItemName", name);
                    command.Parameters.AddWithValue("@StartTimeStamp", DateTime.Now);
                    command.Parameters.AddWithValue("@EndTimeStamp", DateTime.Now);
                    command.Parameters.AddWithValue("@LogItemMessage", message);
                    command.Parameters.AddWithValue("@LogItemStatus", "Message");

                    connection.Open();
                    command.CommandTimeout = 1000;

                    command.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
        }

        public static void AddReportRun(string reportRunName, DateTime lastRunTime)
        {
            try
            {
                string insertSql = "INSERT INTO ReportRuns (ReportName, LastRunTime) Values (@reportRunName, @lastRunTime)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@reportRunName", reportRunName);
                    command.Parameters.AddWithValue("@lastRunTime", lastRunTime);

                    connection.Open();
                    command.CommandTimeout = 1000;

                    command.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
        }

        public static void AddExportQueItem(string fileToQue)
        {
            try
            {
                if(CheckQueItemExists(fileToQue))
                    return; 

                string insertSql = "INSERT INTO ExportItemQue (QueItemName) Values (@fileToQue)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@fileToQue", fileToQue);

                    connection.Open();
                    command.CommandTimeout = 1000;

                    command.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
        }

        public static void AddNonCoreWorkItemToQue(string workItem)
        {
            try
            {
                if (CheckQueWorkItemExists(workItem))
                    return; 

                string insertSql = "INSERT INTO WorkItemQue (WorkItem) Values (@workItem)";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnNonCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@workItem", workItem);

                    connection.Open();
                    command.CommandTimeout = 1000;

                    command.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
        }

        public static bool CheckQueWorkItemExists(string workItem)
        {
            try
            {
                string insertSql = "SELECT * FROM WorkItemQue WHERE WorkItem=@workItem";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnNonCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@workItem", workItem);

                    connection.Open();
                    command.CommandTimeout = 1000;

                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return (count > 0);
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
            return false;
        }
        public static bool CheckQueItemExists(string queItemName)
        {
            try
            {
                string insertSql = "SELECT * FROM ExportItemQue WHERE QueItemName=@queItemName";
                using (SqlConnection connection = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand command = new SqlCommand(insertSql);
                    command.Connection = connection;

                    command.Parameters.AddWithValue("@queItemName", queItemName);

                    connection.Open();
                    command.CommandTimeout = 1000;

                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return (count > 0);
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "Insert->AddLogStart", exception);
            }
            return false;
        }
    }
}
