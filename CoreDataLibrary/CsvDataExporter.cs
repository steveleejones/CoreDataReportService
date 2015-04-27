using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml;
using CoreDataLibrary.Data;
using CoreDataLibrary.Helpers;
using Newtonsoft.Json;

namespace CoreDataLibrary
{
    public class CsvDataExporter
    {
        private StringBuilder logger = new StringBuilder();
        private string ServerPath;
        private string ExportServerTemp;
        private string TempServerPath;

        private ReportLogger PropertyExporterLogger;

        public const string TEMPFILE_APPENDING = "_Temp.csv";

        private string m_selectClause;
        private string m_pathAndFileName;
        private string m_serverPathAndFile;
        private char m_delimitor = '|';
        private string m_del = "!|!";
        private readonly SelectStatementBuilder m_selectStatementBuilder;

        public ExportItem ExportItem { get; set; }

        public CsvDataExporter(ExportItem exportItem)
        {
            PropertyExporterLogger = new ReportLogger(exportItem.ExportItemName);

            if (!CoreDataLib.IsLive())
            {
                ServerPath = @"\\SVRsql4\E$\CoreData\ExportFiles\Test\";
                ExportServerTemp = @"E:\CoreData\ExportFiles\Test\Temp\";
                TempServerPath = ServerPath + @"Temp\";
            }
            else
            {
                ServerPath = "E:\\CoreData\\ExportFiles\\";
                ExportServerTemp = @"E:\CoreData\ExportFiles\Temp\";
                TempServerPath = ServerPath + @"Temp\";
            }
            ExportItem = exportItem;
            m_pathAndFileName = TempServerPath + exportItem.ExportItemName + ".csv";
            m_selectClause = exportItem.SelectStatementBuilder.SelectStatement();
            m_selectStatementBuilder = exportItem.SelectStatementBuilder;
        }

        public CsvDataExporter()
        {
        }

        public string SelectClause
        {
            get { return m_selectClause; }
            set { m_selectClause = value; }
        }

        public char Delimitor
        {
            get { return m_delimitor; }
            set { m_delimitor = value; }
        }

        public string PathAndFileName
        {
            get { return m_pathAndFileName; }
            set { m_pathAndFileName = value; }
        }

        public string ServerPathAndFile
        {
            get { return m_serverPathAndFile; }
            set { m_serverPathAndFile = value; }
        }

        public string JsonExport()
        {
            FileInfo fileInfo = new FileInfo(m_pathAndFileName);
            //DataTable tbl = new DataTable();

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                int PageSize = 100;
                int CurrentStartPage = 0;
                try
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

                    string sql = ExportItem.SelectStatementBuilder.SelectStatement();
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.CommandTimeout = 720000;
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    //da.Fill(tbl);
                    DataSet dataSet = new DataSet();
                    while (da.Fill(dataSet, CurrentStartPage, PageSize, "Properties") != 0)
                    {
                        foreach (DataRow dr in dataSet.Tables[0].Rows)
                        {
                            var row = new Dictionary<string, object>();
                            foreach (DataColumn col in dataSet.Tables[0].Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                        CurrentStartPage = CurrentStartPage + (PageSize + 1);
                    }
                    return serializer.Serialize(rows);
                }
                catch (Exception e)
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->PropertySupplierMapping", e);
                }
            }
            return "";
        }

        //public string JsonExport2()
        //{
        //    FileInfo fileInfo = new FileInfo(m_pathAndFileName);
        //    DataTable tbl = new DataTable();

        //    using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
        //    {
        //        try
        //        {
        //            string sql = ExportItem.SelectStatementBuilder.SelectStatement();
        //            SqlCommand command = new SqlCommand(sql, conn);
        //            command.CommandTimeout = 720000;
        //            conn.Open();

        //            SqlDataReader reader = command.ExecuteReader();

        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                }
        //            }
        //            //SqlDataAdapter da = new SqlDataAdapter(command);
        //            //da.Fill(tbl);
        //            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //            //serializer.MaxJsonLength = Int32.MaxValue;

        //            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //            //Dictionary<string, object> row;
        //            //foreach (DataRow dr in tbl.Rows)
        //            //{
        //            //    row = new Dictionary<string, object>();
        //            //    foreach (DataColumn col in tbl.Columns)
        //            //    {
        //            //        row.Add(col.ColumnName, dr[col]);
        //            //    }
        //            //    rows.Add(row);
        //            //}
        //            //return serializer.Serialize(rows);
        //        }
        //        catch (Exception e)
        //        {
        //            Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "PropertySupplierMapping : Error ", "CoreDataLibrary.Data->Process->PropertySupplierMapping", e);
        //        }
        //    }
        //    return "";
        //}

        public string ConvertDataTabletoString()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Data Source=SureshDasari;Initial Catalog=master;Integrated Security=true"))
            {
                using (SqlCommand cmd = new SqlCommand("select title=City,lat=latitude,lng=longitude,description from LocationDetails", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return serializer.Serialize(rows);
                }
            }
        }
        public void XmlExport(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            try
            {
                FileInfo fileInfo = new FileInfo(m_pathAndFileName);
                FileInfo serverPathAndFile = new FileInfo(ServerPath + fileInfo.Name);

                CoreDataLibrary.Data.Process.ExportItemToXml(ExportItem, serverPathAndFile);
                reportLogger.EndStep(stepId);
            }
            catch (Exception exception)
            {
                reportLogger.EndStep(stepId, exception);
            }
        }

        public void CsvExport(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            try
            {
                FileInfo fileInfo = new FileInfo(m_pathAndFileName);
                FileInfo serverPathAndFile = new FileInfo(ServerPath + fileInfo.Name);
                FileInfo serverPath = new FileInfo(ServerPath);
                FileInfo tempDirectory = new FileInfo(ServerPath + @"Temp\");
                string filename = fileInfo.Name.Split('.')[0];

                ServerPathAndFile = serverPathAndFile.FullName;

                StringBuilder headers = new StringBuilder();

                logger.AppendLine("Create Headers");
                foreach (string header in m_selectStatementBuilder.GetOutputHeaders)
                {
                    headers.Append(header + "|");
                }

                headers.Remove(headers.Length - 1, 1);

                logger.AppendLine("Headers Created");
                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 60000;
                    sqlCommand.Connection = conn;
                    StringBuilder sqlBuilder = new StringBuilder();
                    sqlBuilder.Append(@"DECLARE @bcpCmd as VARCHAR(8000);");
                    sqlBuilder.Append(@"SET @bcpCmd= 'bcp ""SET NOCOUNT ON;");
                    sqlBuilder.Append(m_selectClause);
                    sqlBuilder.Append(@";SET NOCOUNT OFF;""");
                    sqlBuilder.Append(" queryout ");
                    sqlBuilder.Append(ExportServerTemp + fileInfo.Name);
                    sqlBuilder.Append(" -t \"" + m_del + "\" -r(!!!!!!!!!!)");

                    if (CoreDataLib.IsLive())
                        sqlBuilder.Append(" -c -C ACP -S SVRsql4 -d CoreData -U CoreData -P CoreD@T@'; EXEC master..xp_cmdshell @bcpCmd;");
                    else
                        sqlBuilder.Append(" -c -C ACP -S SVRsql4 -d CoreData_Test -U CoreData -P CoreD@T@'; EXEC master..xp_cmdshell @bcpCmd;");

                    sqlBuilder.Replace("@LangId", m_selectStatementBuilder.LanguageId);
                    string sql = sqlBuilder.ToString();
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    sqlCommand.ExecuteNonQuery();

                    if (File.Exists(tempDirectory + fileInfo.Name))
                    {
                        CreateCsvFile(tempDirectory, filename, fileInfo, headers, serverPathAndFile, serverPath, m_selectStatementBuilder.LanguageId);
                    }
                    else
                    {
                        Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CsvDataExporter [SVRSQL4]: - Error : ", "File not found : " + tempDirectory);// + fileInfo.Name + " : " + DateTime.Now.ToLongDateString() + " : " + DateTime.Now.ToLongTimeString());
                    }
                    reportLogger.EndStep(stepId);

                //    StringBuilder sqlBuilder = new StringBuilder();
                //    sqlBuilder.Append(m_selectClause);

                //    sqlBuilder.Replace("@LangId", m_selectStatementBuilder.LanguageId);
                //    string sql = sqlBuilder.ToString();
                //    sqlCommand.CommandText = sql;
                //    conn.Open();
                //    DataTable dataTable = new DataTable();
                //    dataTable.Load(sqlCommand.ExecuteReader());

                //    foreach (DataRow row in dataTable.Rows)
                //    {
                //        for (int i = 0; i <= dataTable.Columns.Count - 1; i++)
                //        {
                //            if (row[i].ToString().Contains("|"))
                //            {
                //                string newValue = row[i].ToString().Replace("|", "");
                //                row[i] = newValue;
                //            }
                //        }
                //    }

                //    if (File.Exists(tempDirectory + fileInfo.Name))
                //    {
                ////CsvToXmlParser xmlParser = new CsvToXmlParser(serverPathAndFile);

                //        CoreDataLib.CreateCSVfile(dataTable, serverPathAndFile.FullName, "|");
                //    }
                //    else
                //    {
                //        Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CsvDataExporter [SVRSQL4]: - Error : ", "File not found : " + tempDirectory);// + fileInfo.Name + " : " + DateTime.Now.ToLongDateString() + " : " + DateTime.Now.ToLongTimeString());
                //    }
                //    reportLogger.EndStep(stepId);
                }
            }
            catch (Exception exception)
            {
                reportLogger.EndStep(stepId, exception);
            }
        }

        private void CreateCsvFile(FileInfo tempDirectory, string filename, FileInfo fileInfo, StringBuilder headers,
            FileInfo serverPathAndFile, FileInfo serverPath)
        {
            Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CsvDataExporter [SVRSQL4]:", "CsvExport File Exists");
            try
            {
                string line;

                if (File.Exists(tempDirectory + filename + TEMPFILE_APPENDING))
                    File.Delete(tempDirectory + filename + TEMPFILE_APPENDING);
                StreamWriter writer = null;
                StreamReader reader = null;
                FileStream fs = new FileStream(tempDirectory + fileInfo.Name, FileMode.Open);
                FileStream outStream = new FileStream(tempDirectory + filename + TEMPFILE_APPENDING, FileMode.Append);
                reader = new StreamReader(fs, Encoding.Default);

                writer = new StreamWriter(outStream, Encoding.GetEncoding("windows-1250"));

                writer.WriteLine(headers);

                string[] stringSeperators = { "(!!!!!!!!!!)" };
                string[] fieldSeperators = { "!|!" };
                string lastEntry = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] lines = line.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length > 0)
                    {
                        for (int i = 0; i <= lines.Length - 1; i++)
                        {
                            if (lines[i].Split(fieldSeperators, StringSplitOptions.None).Length - 1 ==
                                headers.ToString().Split('|').Length - 1)
                            {
                                string row = lines[i].Replace("!|!", "|");
                                writer.WriteLine(row);
                            }
                            else
                            {
                                lastEntry = lastEntry + lines[i];
                                if (lastEntry.Split(fieldSeperators, StringSplitOptions.None).Length - 1 ==
                                    headers.ToString().Split('|').Length - 1)
                                {
                                    lastEntry = lastEntry.Replace("!|!", "|");
                                    writer.WriteLine(lastEntry);
                                    lastEntry = "";
                                }
                            }
                        }
                    }
                }

                writer.Flush();

                reader.Close();
                writer.Close();

                string pathAndFileToFtp = "";

                File.Delete(tempDirectory + fileInfo.Name);
                if (File.Exists(serverPathAndFile.FullName))
                    File.Delete(serverPathAndFile.FullName);

                File.Move(tempDirectory + filename + TEMPFILE_APPENDING, serverPathAndFile.FullName);
                pathAndFileToFtp = ExportItem.ExportItemName + ".csv";

                File.Delete(tempDirectory + filename + TEMPFILE_APPENDING);
                if (ExportItem.ExportItemFtpId > 0)
                    FtpExportedFile(pathAndFileToFtp);
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CsvDataExporter   [SVRSQL4]: Error ", "CreateCsvFile", exception);
            }
        }

        private void CreateCsvFile(FileInfo tempDirectory, string filename, FileInfo fileInfo, StringBuilder headers,
            FileInfo serverPathAndFile, FileInfo serverPath, string languageEncoding)
        {
            int stepId = PropertyExporterLogger.AddStep();
            try
            {
                //CoreDataLibrary.Data.Process.ExportItemToXml(ExportItem, serverPathAndFile);
                //CsvToXmlParser xmlParser = new CsvToXmlParser(serverPathAndFile);

                string line;

                if (File.Exists(tempDirectory + filename + TEMPFILE_APPENDING))
                    File.Delete(tempDirectory + filename + TEMPFILE_APPENDING);

                StreamWriter writer = null;
                StreamReader reader = null;
                FileStream fs = new FileStream(tempDirectory + fileInfo.Name, FileMode.Open);
                FileStream outStream = new FileStream(tempDirectory + filename + TEMPFILE_APPENDING, FileMode.Append);
                reader = new StreamReader(fs, Encoding.Default);
                string encoding = Get.GetLanguageEncoding(languageEncoding);
                if (String.IsNullOrEmpty(encoding))
                    writer = new StreamWriter(outStream, Encoding.GetEncoding("windows-1252"));
                else
                    writer = new StreamWriter(outStream, Encoding.GetEncoding(encoding));

                writer.WriteLine(headers);

                string[] stringSeperators = { "(!!!!!!!!!!)" };
                string[] fieldSeperators = { "!|!" };
                string lastEntry = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] lines = line.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length > 0)
                    {
                        for (int i = 0; i <= lines.Length - 1; i++)
                        {
                            if (lines[i].Split(fieldSeperators, StringSplitOptions.None).Length - 1 ==
                                headers.ToString().Split('|').Length - 1)
                            {
                                string row = lines[i].Replace("!|!", "|");
                                writer.WriteLine(row);
                            }
                            else
                            {
                                lastEntry = lastEntry + lines[i];
                                if (lastEntry.Split(fieldSeperators, StringSplitOptions.None).Length - 1 ==
                                    headers.ToString().Split('|').Length - 1)
                                {
                                    lastEntry = lastEntry.Replace("!|!", "|");
                                    writer.WriteLine(lastEntry);
                                    lastEntry = "";
                                }
                            }
                        }
                    }
                }

                writer.Flush();

                reader.Close();
                writer.Close();

                string pathAndFileToFtp = "";

                File.Delete(tempDirectory + fileInfo.Name);
                if (File.Exists(serverPathAndFile.FullName))
                    File.Delete(serverPathAndFile.FullName);

                File.Move(tempDirectory + filename + TEMPFILE_APPENDING, serverPathAndFile.FullName);
                pathAndFileToFtp = ExportItem.ExportItemName + ".csv";

                File.Delete(tempDirectory + filename + TEMPFILE_APPENDING);
                if (CoreDataLib.IsLive())
                {
                    if (ExportItem.ExportItemFtpId > 0)
                        FtpExportedFile(pathAndFileToFtp);
                }

                PropertyExporterLogger.EndStep(stepId);
            }
            catch (Exception exception)
            {
                PropertyExporterLogger.EndStep(stepId, exception);
            }
        }

        private string TimeStampFile()
        {
            string newName = "";

            FileInfo fileInfo = new FileInfo(m_pathAndFileName);
            newName = DateTime.Now.ToShortDateString().Replace("/", "") + DateTime.Now.ToShortTimeString().Replace(":", "") + fileInfo.Name;
            return newName;
        }

        private void FtpExportedFile(string ftpExportItem)
        {
            int stepId = PropertyExporterLogger.AddStep();
            try
            {
                if (ExportItem.ExportItemFtpId > 0)
                {
                    FtpItem ftpItem = Get.GetFtpItem(ExportItem.ExportItemFtpId);

                    string ftpUploadUri = "";
                    if (!ftpItem.FtpAddress.StartsWith(@"ftp://"))
                        ftpUploadUri = @"ftp://" + ftpItem.FtpAddress.Trim();
                    else
                        ftpUploadUri = ftpItem.FtpAddress.Trim();

                    if (!ftpUploadUri.EndsWith(@"/"))
                        ftpUploadUri += @"/" + ftpExportItem;
                    else
                        ftpUploadUri += ftpExportItem;

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUploadUri);

                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(ftpItem.FtpUsername.Normalize(), ftpItem.FtpPassword.Normalize());
                    request.Timeout = 1800000;

                    FileStream fileStream = new FileStream(ServerPathAndFile, FileMode.Open, FileAccess.Read);

                    Stream requestStream = request.GetRequestStream();
                    byte[] buffer = new byte[8092];
                    int read = 0;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        requestStream.Write(buffer, 0, read);

                    requestStream.Flush();

                    requestStream.Close();
                }
                PropertyExporterLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                PropertyExporterLogger.EndStep(stepId, e);
            }
        }
    }
}