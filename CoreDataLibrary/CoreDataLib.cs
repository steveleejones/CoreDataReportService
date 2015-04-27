using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using CoreDataLibrary.Data;
using CoreDataLibrary.Exporters;

namespace CoreDataLibrary
{
    public class CoreDataLib
    {
        public static bool IsLive()
        {
            //FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            //string file = "intest.txt";

            //if (File.Exists(fileInfo.Directory + "\\" + file))
            //    return false;

            //if (System.Diagnostics.Debugger.IsAttached)
            //    return false;
            return true;
        }

        public static bool LchAndLcbAvailableOnIVDB()
        {
            return LcbAvailableOnIvdb() && LchAvailableOnIvdb();
        }

        public static bool LcbAvailableOnIvdb()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 6000;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT TOP 1 PropertyTypeID, PropertyType FROM [IVDB].[LCB].[dbo].[PropertyType]";
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public static bool LchAvailableOnIvdb()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 6000;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT TOP 1 PropertyTypeID, PropertyType FROM [IVDB].[LCH].[dbo].[PropertyType]";
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public static bool CoreDataRunning()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 6000;
                    sqlCommand.Connection = conn;

                    string sql = @"SELECT TOP 1 [LogId] ,[LogItemName] ,[StartTimeStamp] ,[EndTimeStamp] ,[LogItemMessage] ,[LogItemStatus] ,[LogItemRemoved] FROM [CoreData].[dbo].[Logs] WHERE LogItemName = 'CoreData' ORDER BY StartTimeStamp DESC ";
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public static void FtpFile(FileInfo fileToFtp, string ftpTarget)
        {
            ReportLogger ftpFileReportLogger = new ReportLogger("FtpFile");

            int stepId = ftpFileReportLogger.AddStep();

            try
            {
                //ftpUploadUri = "ftp://ftp.lowcosttravelgroup.com/BusinessRules/";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpTarget + fileToFtp.Name);

                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential("XTGtravelgate", "LcH3rdparty");
                request.Timeout = 60 * 10000;

                FileStream fileStream = new FileStream(fileToFtp.FullName, FileMode.Open, FileAccess.Read);

                Stream requestStream = request.GetRequestStream();
                byte[] buffer = new byte[8092];
                int read = 0;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, read);

                requestStream.Flush();

                requestStream.Close();
                fileStream.Close();
                ftpFileReportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                ftpFileReportLogger.EndStep(stepId, e);
            }
            ftpFileReportLogger.EndLog();
        }

        public static void RunPropertyExportFileNow(string name)
        {
            ReportLogger reportLogger = null;
            try
            {
                ExportItem exportItem = Get.GetExportItem(name);
                reportLogger = new ReportLogger(exportItem.ExportItemName);
                reportLogger.StartLog("Exporting : " + exportItem.ExportItemName);
                exportItem.Export(reportLogger);
                reportLogger.EndLog(exportItem.ExportItemName + " : Exported");
            }
            catch (Exception exception)
            {
                if (reportLogger != null) reportLogger.EndLog(exception);
            }
        }

        public static bool FileCompare(FileInfo fileInfo1, FileInfo fileInfo2)
        {
            bool result = false;

            try
            {
                if (fileInfo1.Length != fileInfo2.Length)
                {
                    result = false;
                }
                else
                {
                    using (var file1 = fileInfo1.OpenRead())
                    {
                        using (var file2 = fileInfo2.OpenRead())
                        {
                            result = StreamsContentsAreEqual(file1, file2);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return result;
            }

            return result;
        }

        private static bool StreamsContentsAreEqual(Stream stream1, Stream stream2)
        {
            const int bufferSize = 2048 * 2;
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                int count1 = stream1.Read(buffer1, 0, bufferSize);
                int count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0)
                {
                    return true;
                }

                int iterations = (int)Math.Ceiling((double)count1 / sizeof(Int64));
                for (int i = 0; i < iterations; i++)
                {
                    if (BitConverter.ToInt64(buffer1, i * sizeof(Int64)) != BitConverter.ToInt64(buffer2, i * sizeof(Int64)))
                    {
                        return false;
                    }
                }
            }
        }
    }
}
