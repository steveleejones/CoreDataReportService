using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary;

namespace CoreDataReportService.Helpers
{
    public class Process
    {
        public static void DailyRun()
        {
            List<ExportItem> runList = CoreDataLibrary.Data.Get.GetDailyRunItems();
            foreach (ExportItem exportItem in runList)
            {
                exportItem.SelectStatementBuilder = SelectStatementBuilder.LoadSelectStatementBuilder(exportItem.ExportItemName);
                CsvDataExporter dataExporter = new CsvDataExporter(@"E:\SteveJ\" + exportItem.ExportItemName + ".csv", exportItem);
                dataExporter.CsvExport();
                if (exportItem.ExportItemFtpId > 0)
                {
                    FtpItem ftpItem = CoreDataLibrary.Data.Get.GetFtpItem(exportItem.ExportItemFtpId);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.contoso.com/test.htm");
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

                    StreamReader sourceStream = new StreamReader("testfile.txt");
                    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    sourceStream.Close();
                    request.ContentLength = fileContents.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                    response.Close();
                }
            }
        }
    }
}
