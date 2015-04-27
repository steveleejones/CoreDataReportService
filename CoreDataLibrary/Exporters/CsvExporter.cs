using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Data;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Exporters
{
    public class CsvExporter : BaseExporter
    {
        private readonly string m_delimeter;
        private string _serverPath;
        private string _exportServerTemp;
        private string _tempServerPath;

        private ReportLogger _csvExporteLogger;

        public const string TEMPFILE_APPENDING = "_Temp.csv";

        private string _selectClause;
        private string _pathAndFileName;
        private string _serverPathAndFile;
        private char _delimiter = '|';
        private string _del = "!|!";
        private string _extension = ".csv";

        private SelectStatementBuilder _selectStatementBuilder;

        public CsvExporter(ExportItem exportItem, bool includeHeaders = false, string delimeter = "|")
            : base(exportItem, ".csv")
        {
            m_delimeter = delimeter;
            ExportItem = exportItem;
        }

        public override bool Export(ReportLogger reportLogger)
        {
            _csvExporteLogger = reportLogger;
            SetupExport();

            int stepId = _csvExporteLogger.AddStep();
            try
            {
                BcpExport();
            }
            catch (Exception exception)
            {
                _csvExporteLogger.EndStep(stepId, exception);
                return false;
            }
            return true;
        }

        private void SetupExport()
        {
            if (!CoreDataLib.IsLive())
            {
                _serverPath = @"\\SVRsql4\E$\CoreData\ExportFiles\Test\";
                _exportServerTemp = @"E:\CoreData\ExportFiles\Test\Temp\";
                _tempServerPath = _serverPath + @"Temp\";
            }
            else
            {
                _serverPath = "E:\\CoreData\\ExportFiles\\";
                _exportServerTemp = @"E:\CoreData\ExportFiles\Temp\";
                _tempServerPath = _serverPath + @"Temp\";
            }
            _pathAndFileName = _tempServerPath + ExportItem.ExportItemName + ".csv";
            _selectClause = ExportItem.SelectStatementBuilder.SelectStatement();
            _selectStatementBuilder = ExportItem.SelectStatementBuilder;
        }

        private void BcpExport()
        {
            int stepId = _csvExporteLogger.AddStep();
            FileInfo fileInfo = new FileInfo(_pathAndFileName);
            FileInfo serverPathAndFile = new FileInfo(_serverPath + fileInfo.Name);
            FileInfo serverPath = new FileInfo(_serverPath);
            FileInfo tempDirectory = new FileInfo(_serverPath + @"Temp\");
            string filename = fileInfo.Name.Split('.')[0];

            _serverPathAndFile = serverPathAndFile.FullName;

            StringBuilder headers = new StringBuilder();

            foreach (string header in _selectStatementBuilder.GetOutputHeaders)
            {
                headers.Append(header + "|");
            }

            headers.Remove(headers.Length - 1, 1);

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandTimeout = 720000;
                sqlCommand.Connection = conn;
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(@"DECLARE @bcpCmd as VARCHAR(8000);");
                sqlBuilder.Append(@"SET @bcpCmd= 'bcp ""SET NOCOUNT ON;");
                sqlBuilder.Append(_selectClause);
                sqlBuilder.Append(@";SET NOCOUNT OFF;""");
                sqlBuilder.Append(" queryout ");
                sqlBuilder.Append(_exportServerTemp + fileInfo.Name);
                sqlBuilder.Append(" -t \"" + _del + "\" -r(!!!!!!!!!!)");

                if (CoreDataLib.IsLive())
                    sqlBuilder.Append(
                        " -c -C ACP -S SVRsql4 -d CoreData -U CoreData -P CoreD@T@'; EXEC master..xp_cmdshell @bcpCmd;");
                else
                    sqlBuilder.Append(
                        " -c -C ACP -S SVRsql4 -d CoreData_Test -U CoreData -P CoreD@T@'; EXEC master..xp_cmdshell @bcpCmd;");

                sqlBuilder.Replace("@LangId", _selectStatementBuilder.LanguageId);
                string sql = sqlBuilder.ToString();
                sqlCommand.CommandText = sql;
                conn.Open();
                sqlCommand.ExecuteNonQuery();

                if (File.Exists(tempDirectory + fileInfo.Name))
                {
                    CreateCsvFile(tempDirectory, filename, fileInfo, headers, serverPathAndFile, serverPath,
                        _selectStatementBuilder.LanguageId);
                }
                else
                {
                    _csvExporteLogger.EndStep(stepId, new Exception("File not found"));
                }
                _csvExporteLogger.EndStep(stepId);
            }
        }

        private void CreateCsvFile(FileInfo tempDirectory, string filename, FileInfo fileInfo, StringBuilder headers,
                                   FileInfo serverPathAndFile, FileInfo serverPath, string languageEncoding)
        {
            int stepId = _csvExporteLogger.AddStep();
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
                string encoding = Get.GetLanguageEncoding(languageEncoding);
                if (String.IsNullOrEmpty(encoding))
                    writer = new StreamWriter(outStream, Encoding.GetEncoding("windows-1252"));
                else
                    writer = new StreamWriter(outStream, Encoding.GetEncoding(encoding));

                if (headers.Length > 0)
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

                File.Delete(tempDirectory + fileInfo.Name);
                if (File.Exists(serverPathAndFile.FullName))
                    File.Delete(serverPathAndFile.FullName);

                File.Move(tempDirectory + filename + TEMPFILE_APPENDING, serverPathAndFile.FullName);

                File.Delete(tempDirectory + filename + TEMPFILE_APPENDING);
                if (CoreDataLib.IsLive())
                {
                    if (ExportItem.ExportItemFtpId > 0)
                        FileFtp.FtpFile(serverPathAndFile, _csvExporteLogger, ExportItem);
                        //FtpExportedFile(pathAndFileToFtp);
                }

                _csvExporteLogger.EndStep(stepId);
            }
            catch (Exception exception)
            {
                _csvExporteLogger.EndStep(stepId, exception);
            }
        }

        //private void FtpExportedFile(string ftpExportItem)
        //{
        //    int stepId = _csvExporteLogger.AddStep();
        //    try
        //    {
        //        if (ExportItem.ExportItemFtpId > 0)
        //        {
        //            FtpItem ftpItem = Get.GetFtpItem(ExportItem.ExportItemFtpId);

        //            string ftpUploadUri = "";
        //            if (!ftpItem.FtpAddress.StartsWith(@"ftp://"))
        //                ftpUploadUri = @"ftp://" + ftpItem.FtpAddress.Trim();
        //            else
        //                ftpUploadUri = ftpItem.FtpAddress.Trim();

        //            if (!ftpUploadUri.EndsWith(@"/"))
        //                ftpUploadUri += @"/" + ftpExportItem;
        //            else
        //                ftpUploadUri += ftpExportItem;

        //            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUploadUri);

        //            request.Method = WebRequestMethods.Ftp.UploadFile;

        //            request.Credentials = new NetworkCredential(ftpItem.FtpUsername.Normalize(), ftpItem.FtpPassword.Normalize());
        //            request.Timeout = 1800000;

        //            FileStream fileStream = new FileStream(_serverPathAndFile, FileMode.Open, FileAccess.Read);

        //            Stream requestStream = request.GetRequestStream();
        //            byte[] buffer = new byte[8092];
        //            int read = 0;
        //            while ((read = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        //                requestStream.Write(buffer, 0, read);

        //            requestStream.Flush();

        //            requestStream.Close();
        //        }
        //        _csvExporteLogger.EndStep(stepId);
        //    }
        //    catch (Exception e)
        //    {
        //        _csvExporteLogger.EndStep(stepId, e);
        //    }
        //}
    }
}
