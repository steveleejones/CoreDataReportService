using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CoreDataLibrary.Data;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Exporters
{
    public class XmlExporter : BaseExporter
    {
        private string _serverPath;
        private string _exportServerTemp;
        private string _tempServerPath;
        private string _pathAndFileName;
        public const string TEMPFILE_APPENDING = "_Temp.xml";
        private string _extension = ".xml";
        private ReportLogger _reportLogger;

        public XmlExporter(ExportItem exportItem)
            : base(exportItem, ".xml")
        {
        }

        public override bool Export(ReportLogger reportLogger)
        {
            _reportLogger = reportLogger;
            SetupExport();

            int stepId = reportLogger.AddStep();
            try
            {
                FileInfo fileInfo = new FileInfo(PathAndFileName);

                FileInfo serverPathAndFile = new FileInfo(_serverPath + fileInfo.Name);
                FileInfo serverPath = new FileInfo(_serverPath);
                FileInfo tempDirectory = new FileInfo(_serverPath + @"Temp\");
                string filename = fileInfo.Name.Split('.')[0];

                XmlExport(fileInfo);

                if (File.Exists(serverPathAndFile.FullName))
                    File.Delete(serverPathAndFile.FullName);

                File.Move(tempDirectory + filename + _extension, serverPathAndFile.FullName);

                if (CoreDataLib.IsLive())
                {
                    if (ExportItem.ExportItemFtpId > 0)
                        FileFtp.FtpFile(serverPathAndFile, reportLogger, ExportItem);
                }

                reportLogger.EndStep(stepId);
            }
            catch (Exception exception)
            {
                _reportLogger.EndStep(stepId, exception);
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
        }

        private void XmlExport(FileInfo fileInfo)
        {
            int stepId = _reportLogger.AddStep();

            using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
            {
                try
                {
                    string sql = ExportItem.SelectStatementBuilder.SelectStatement();
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.CommandTimeout = 720000;
                    conn.Open();

                    using (XmlTextWriter xWriter = new XmlTextWriter(fileInfo.Directory + "\\" + ExportItem.ExportItemName + ".xml", Encoding.UTF8))
                    {
                        DataTable tbl = new DataTable();
                        tbl.TableName = "Property";
                        tbl.Load(command.ExecuteReader());
                        foreach (DataRow row in tbl.Rows)
                        {
                            foreach (DataColumn column in tbl.Columns)
                            {
                                if (column.ColumnName == "Country" || column.ColumnName == "Region" ||
                                    column.ColumnName == "Resort")
                                {
                                    string value = row[column].ToString();
                                    if (value.Contains("City"))
                                    {
                                        string parsedValue = value.Replace("City", "").Replace("Centre", "").Trim();
                                        row[column] = parsedValue;
                                    }
                                }
                            }
                        }
                        tbl.WriteXml(xWriter, XmlWriteMode.IgnoreSchema);
                        _reportLogger.EndStep(stepId);
                    }
                }
                catch (Exception e)
                {
                    _reportLogger.EndStep(stepId);
                }
            }
        }
    }
}
