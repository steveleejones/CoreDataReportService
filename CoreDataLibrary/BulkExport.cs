using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using CoreDataLibrary.Data;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary
{
    public class BulkExport
    {
        public string FieldDelimeter { get; set; }
        public string RowDelimeter { get; set; }
        public FileInfo ExportFile { get; set; }
        public string DataConnecter { get; set; }
        public string SqlString { get; set; }
        public string ServerMachine { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public BulkExport(string fieldDelimeter, string rowDelimeter, FileInfo exportFile, string dataConnector, string sqlString, string server, string user, string password)
        {
            FieldDelimeter = fieldDelimeter;
            RowDelimeter = rowDelimeter;
            ExportFile = exportFile;
            DataConnecter = dataConnector;
            SqlString = sqlString;
            ServerMachine = server;
            User = user;
            Password = password;
            Export();
        }

        private void Export()
        {
            using (SqlConnection conn = new SqlConnection(DataConnecter))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandTimeout = 60*1000;
                sqlCommand.Connection = conn;

                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(@"DECLARE @bcpCmd as VARCHAR(8000);");
                sqlBuilder.Append(@"SET @bcpCmd= 'bcp ""SET NOCOUNT ON;");
                sqlBuilder.Append(SqlString);
                sqlBuilder.Append(@";SET NOCOUNT OFF;""");
                sqlBuilder.Append(" queryout ");
                sqlBuilder.Append(ExportFile.FullName);
                sqlBuilder.Append(" -t " + FieldDelimeter + " -r" + RowDelimeter);
                sqlBuilder.Append(" -c -C ACP -S " + ServerMachine + " -d " + Database + " -U " + User + " -P " + Password +
                                  "; EXEC master..xp_cmdshell @bcpCmd;");
                string sql = sqlBuilder.ToString();
                sqlCommand.CommandText = sql;
                conn.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
