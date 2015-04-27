using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Data;

namespace CoreDataLibrary.Helpers
{
    public class FileFtp
    {
        public static bool FtpFile(FileInfo fileToFtpInfo, ReportLogger reportLogger, ExportItem exportItem)
        {
            int stepId = reportLogger.AddStep();
            try
            {
                if (exportItem.ExportItemFtpId > 0)
                {
                    FtpItem ftpItem = Get.GetFtpItem(exportItem.ExportItemFtpId);

                    string ftpUploadUri = "";
                    if (!ftpItem.FtpAddress.StartsWith(@"ftp://"))
                        ftpUploadUri = @"ftp://" + ftpItem.FtpAddress.Trim();
                    else
                        ftpUploadUri = ftpItem.FtpAddress.Trim();

                    if (!ftpUploadUri.EndsWith(@"/"))
                        ftpUploadUri += @"/" + fileToFtpInfo.Name;
                    else
                        ftpUploadUri += fileToFtpInfo.Name;

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUploadUri);

                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(ftpItem.FtpUsername.Normalize(), ftpItem.FtpPassword.Normalize());
                    request.Timeout = 1800000;

                    FileStream fileStream = new FileStream(fileToFtpInfo.FullName, FileMode.Open, FileAccess.Read);

                    Stream requestStream = request.GetRequestStream();
                    byte[] buffer = new byte[8092];
                    int read = 0;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        requestStream.Write(buffer, 0, read);

                    requestStream.Flush();

                    requestStream.Close();
                }
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
                return false;
            }
            return true;
        }
    }
}
