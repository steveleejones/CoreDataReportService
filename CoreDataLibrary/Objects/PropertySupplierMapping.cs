using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CoreDataLibrary.Data;
using CoreDataLibrary.Helpers;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;

namespace CoreDataLibrary.Objects
{

    public class PropertySupplierMapping
    {
        string PathToExport = String.Empty;

        private readonly string fileName;

        public PropertySupplierMapping(string fileToProcess)
        {
            if(CoreDataLib.IsLive())
                PathToExport = "E:\\CoreData\\ExportFiles\\";
            else
                PathToExport = @"\\SVRsql4\E$\CoreData\ExportFiles\";

            fileName = fileToProcess;
            DoProcessing();
        }

        private void DoProcessing()
        {
            try
            {
                if (Process.PropertySupplierMapping())
                {
                    if (Process.ExportPropertySupplierMapping())
                    {
                        if (File.Exists(PathToExport + fileName + ".csv"))
                        {
                            AddHeaders();
                            BZipFile();
                        }
                        Process.RemoveTemporaryPropertySupplierMappingTable();
                        if (File.Exists(PathToExport + fileName + ".csv.bz2"))
                        {
                            FtpExportedFile();
                        }
                    }
                }
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Information ", "PropertySupplierMapping->DoProcessing : FINISHED");
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "PropertySupplierMapping->DoProcessing", e);
            }
            finally
            {
                CleanUpFiles();
            }
        }

        private void CleanUpFiles()
        {
            try
            {
                Process.RemoveTemporaryPropertySupplierMappingTable();

                if (File.Exists(PathToExport + "\\Temp\\" + fileName + ".csv"))
                    File.Delete(PathToExport + "\\Temp\\" + fileName + ".csv");

                if (File.Exists(PathToExport + "\\Temp\\" + fileName + ".csv.bz2"))
                    File.Delete(PathToExport + "\\Temp\\" + fileName + ".csv.bz2");

                if (File.Exists(PathToExport + fileName + ".csv"))
                    File.Delete(PathToExport + fileName + ".csv");

                if (File.Exists(PathToExport + fileName + ".csv.bz2"))
                    File.Delete(PathToExport + fileName + ".csv.bz2");
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "PropertySupplierMapping->CleanUpFiles", e);
            }
        }

        private void AddHeaders()
        {
            try
            {
                string line;

                if (File.Exists(PathToExport + "\\Temp\\" + fileName + ".csv"))
                    File.Delete(PathToExport + "\\Temp\\" + fileName + ".csv");

                File.Move(PathToExport + fileName + ".csv", PathToExport + "\\Temp\\" + fileName + ".csv");
                StreamWriter writer = null;
                StreamReader reader = null;
                FileStream fs = new FileStream(PathToExport + "\\Temp\\" + fileName + ".csv", FileMode.Open);
                FileStream temp = new FileStream(PathToExport + "\\Temp\\" + fileName + "-temp.csv", FileMode.Append);
                reader = new StreamReader(fs);
                writer = new StreamWriter(temp);

                writer.WriteLine("\"CountryCode\"|\"PropertyReferenceID\"|\"Source\"|\"SourceID\"");

                while ((line = reader.ReadLine()) != null)
                {
                    writer.WriteLine(line);
                }
                writer.Flush();
                writer.Close();
                reader.Close();

                File.Move(PathToExport + "\\Temp\\" + fileName + "-temp.csv", PathToExport + fileName + ".csv");

                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Information ",
                    "PropertySupplierMapping->AddHeaders");
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ",
                    "PropertySupplierMapping->AddHeaders", e);
            }
        }

        private void BZipFile()
        {
            try
            {
                File.Delete(PathToExport + fileName + ".csv.bz2");
                FileStream fs = File.Create(PathToExport + fileName + ".csv.bz2");
                FileStream csvFileToRead = File.OpenRead(PathToExport + fileName + ".csv");
                BZip2.Compress(csvFileToRead, fs, false, 1);
                fs.Close();
                csvFileToRead.Close();
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Information ", "PropertySupplierMapping->BZipFile");
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "PropertySupplierMapping->BZipFile", e);
            }
        }

        private void FtpExportedFile()
        {
            try
            {
                string ftpUploadUri = "";

                ftpUploadUri = "ftp://ftp.lowcosttravelgroup.com/SupplierMapping/";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUploadUri + fileName + ".csv.bz2");

                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential("XTGtravelgate", "LcH3rdparty");
                request.Timeout = 60 * 10000;

                FileStream fileStream = new FileStream(PathToExport + fileName + ".csv.bz2", FileMode.Open, FileAccess.Read);

                Stream requestStream = request.GetRequestStream();
                byte[] buffer = new byte[8092];
                int read = 0;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, read);

                requestStream.Flush();

                requestStream.Close();
                fileStream.Close();
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Information ", "PropertySupplierMapping->FtpExportedFile");
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataLibrary  [SVRSQL4]: Error ", "PropertySupplierMapping->FtpExportedFile", e);
            }
        }
    }
}
