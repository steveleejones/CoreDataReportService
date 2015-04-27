using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataReportService.Helpers
{
    public class CsvDataExporter
    {
        private string m_selectClause;
        private string m_pathAndFileName;
        private char m_delimitor = '|';
        private string m_del = "!|!";
        private readonly SelectStatementBuilder m_selectStatementBuilder;

        public CsvDataExporter(string pathAndFileName, SelectStatementBuilder selectStatementBuilder)
        {
            m_pathAndFileName = pathAndFileName;
            m_selectClause = selectStatementBuilder.SelectStatement();
            m_selectStatementBuilder = selectStatementBuilder;
        }

        public CsvDataExporter(string pathAndFileName, ExportItem exportItem)
        {
            m_pathAndFileName = pathAndFileName;
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

        public void CsvExport()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(m_pathAndFileName);
                FileInfo serverFileAndPath = new FileInfo(@"\\mssqldev\E$\SteveJ\" + fileInfo.Name);
                FileInfo tempFile = new FileInfo(@"\\mssqldev\E$\SteveJ\Temp.csv");

                StringBuilder headers = new StringBuilder();

                foreach (string header in m_selectStatementBuilder.GetOutputHeaders)
                {
                    headers.Append(header + "|");
                }

                headers.Remove(headers.Length - 1, 1);

                using (SqlConnection conn = new SqlConnection(DataConnection.SqlConnCoreData))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 60 * 30;
                    sqlCommand.Connection = conn;

                    StringBuilder sqlBuilder = new StringBuilder();
                    sqlBuilder.Append(@"DECLARE @bcpCmd as VARCHAR(8000);");
                    sqlBuilder.Append(@"SET @bcpCmd= 'bcp ""SET NOCOUNT ON;");
                    sqlBuilder.Append(m_selectClause);
                    sqlBuilder.Append(@";SET NOCOUNT OFF;""");
                    sqlBuilder.Append(" queryout ");
                    sqlBuilder.Append(m_pathAndFileName);
                    sqlBuilder.Append(" -t \"" + m_del + "\" -r(!!!!!!!!!!)");
                    sqlBuilder.Append(" -c -S MSSQLDEV -d CoreData -U CoreDataMaintain -P c0reMa1nta1n'; EXEC master..xp_cmdshell @bcpCmd;");
                    sqlBuilder.Replace("@LangId", m_selectStatementBuilder.LanguageId);

                    string sql = sqlBuilder.ToString();
                    sqlCommand.CommandText = sql;
                    conn.Open();
                    sqlCommand.ExecuteNonQuery();


                    if (File.Exists(serverFileAndPath.FullName))
                    {
                        string line;

                        if (File.Exists(tempFile.FullName))
                            File.Delete(tempFile.FullName);

                        FileStream fs = new FileStream(serverFileAndPath.FullName, FileMode.Open);
                        FileStream outStream = new FileStream(tempFile.FullName, FileMode.Append);
                        StreamReader reader = new StreamReader(fs);
                        StreamWriter writer = new StreamWriter(outStream);

                        writer.WriteLine(headers);

                        string[] stringSeperators = new string[] { "(!!!!!!!!!!)" };
                        string[] fieldSeperators = new string[] { "!|!" };
                        string lastEntry = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] lines = line.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries); ;

                            if(lines.Length > 0)
                            {
                                for (int i = 0; i <= lines.Length - 1; i++)
                                {
                                    if (lines[i].Split(fieldSeperators, StringSplitOptions.None).Length - 1 == headers.ToString().Split('|').Length - 1)
                                    {
                                        string row = lines[i].Replace("!|!", "|");
                                        writer.WriteLine(row);
                                    }
                                    else
                                    {
                                        lastEntry = lastEntry + lines[i];
                                        if (lastEntry.Split(fieldSeperators, StringSplitOptions.None).Length - 1 == headers.ToString().Split('|').Length - 1)
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

                        File.Delete(serverFileAndPath.FullName);
                        File.Move(tempFile.FullName, serverFileAndPath.FullName);

                        File.Delete(tempFile.FullName);
                    }
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("StevenJones@lowcostbeds.com", "CVS Data Exporter - Error : ", "CVSExport", exception);
            }
        }
    }
}
