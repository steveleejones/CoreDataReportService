using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace CoreDataReportService.Helpers
{
    class FtpHelper
    {
        private const string FtpUsername = "lowcostbeds.co.uk_ppcengine";
        private const string FtpPassword = "R2tv5Q39";
        private const string ServerPath = "ftp://ftp.lowcostbeds.co.uk/Trip/";

        public static string DownloadTripEquityFileFromFtp(string countryCode)
        {
            try
            {
                string filename = GetTripEquityFileName(DateTime.Now, countryCode);
                if (!String.IsNullOrEmpty(filename))
                {
                    DownloadFtpFile(filename, false);
                }
                else
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "TRIP Filename Came back blank", "Country Code" + countryCode);
                }
                return filename;
            }
            catch (Exception ex)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "TRIP Daily Equity Sales FTP Import Failed" + DateTime.Now.ToString(), ex.InnerException.ToString());
                return "";
            }
        }
        public static string DownloadTripBookingReferencesFileFromFtp(string countryCode)
        {
            try
            {
                string filename = GetTripBookingReferenceFileName(DateTime.Now, countryCode);
                if (!String.IsNullOrEmpty(filename))
                {
                    DownloadFtpFile(filename, false);
                }
                else
                {
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", countryCode + " TRIP Booking Reference File Name Came back blank", "");
                }
                return filename;
            }
            catch (Exception ex)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "TRIP Booking Reference File FTP Import Failed" + DateTime.Now.ToString(), ex.InnerException.ToString());
                return "";
            }
        }
        private static void DownloadFtpFile(string fileName, bool fileFromTrip)
        {
            FtpWebRequest requestFileDownload;
            if (fileFromTrip)
            {
                requestFileDownload = (FtpWebRequest)WebRequest.Create("ftp://ftp.lowcostbeds.co.uk/TripReports/" + fileName);
            }
            else
            {
                requestFileDownload = (FtpWebRequest)WebRequest.Create("ftp://ftp.lowcostbeds.co.uk/Trip/" + fileName);
            }

            requestFileDownload.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
            requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
            Stream responseStream = responseFileDownload.GetResponseStream();
            FileStream writeStream = new FileStream("C:\\Temp\\" + fileName, FileMode.Create);
            int Length = 2048;
            Byte[] buffer = new Byte[Length];
            int bytesRead = responseStream.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = responseStream.Read(buffer, 0, Length);
            }
            responseStream.Close();
            writeStream.Close();
            requestFileDownload = null;
            responseFileDownload = null;

        }
        private static string GetTripEquityFileName(DateTime date, string countryCode)
        {
            string fileName = "";

            List<String> fileList = GetFtpFileListing(ServerPath);

            //string targetDate = date.ToString("20150530");
            string targetDate = date.ToString("yyyyMMdd");
            foreach (string name in fileList)
            {
                if (name.ToLower().Contains("trip_automated_report_" + countryCode.ToLower()) && name.ToLower().Contains(targetDate))
                {
                    fileName = name;
                    break;
                }
            }
            return fileName;
        }


        private static string GetTripBookingReferenceFileName(DateTime date, string countryCode)
        {
            string fileName = "";

            List<String> fileList = GetFtpFileListing(ServerPath);

            string targetDate = date.ToString("yyyyMMdd");
            foreach (string name in fileList)
            {
                if (name.ToLower().Contains("trip_order_id_" + countryCode.ToLower()) && name.ToLower().Contains(targetDate))
                {
                    fileName = name;
                    break;
                }
            }
            return fileName;
        }

        private static List<string> GetFtpFileListing(string path)
        {
            List<string> fileList = new List<string>();
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(path);
            ftp.Credentials = new System.Net.NetworkCredential(FtpUsername, FtpPassword);
            ftp.KeepAlive = false;
            ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                StreamReader responseStream = new StreamReader(stream);
                string responseLine = responseStream.ReadLine();
                while (responseLine != null)
                {
                    fileList.Add(responseLine);
                    responseLine = responseStream.ReadLine();
                }
                responseStream.Close();
            }
            return fileList;
        }

        public static bool AreTripFilesPresentInRootFolder()
        {
            return GetFtpFileListing("ftp://ftp.lowcostbeds.co.uk/").Count != 0;
        }

        public static List<string> TripFilesInRootDirectory()
        {
            List<string> fileList = GetFtpFileListing("ftp://ftp.lowcostbeds.co.uk/");
            return TripFilesInFileList(fileList);
        }

        private static List<string> TripFilesInFileList(List<string> fileList)
        {
            IEnumerable<string> fileQuery =
                from file in fileList
                where file.Contains("Trip") & (file.EndsWith("xls") | file.EndsWith("csv"))
                select file;

            return fileQuery.ToList();
        }

        public static bool FileMissingOnFtpSite(string fileName)
        {
            bool fileMissing = false;
            var request = (FtpWebRequest)WebRequest.Create(fileName);
            request.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    fileMissing = true;
                }
            }
            return fileMissing;
        }

        public static List<string> GetCatchUpFiles()
        {
            List<String> fileList = new List<String>();

            try
            {
                string catchUpPath = ServerPath + @"CatchUp/";
                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(catchUpPath);
                ftp.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
                ftp.KeepAlive = false;
                ftp.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    StreamReader responseStream = new StreamReader(stream);
                    string responseLine = responseStream.ReadLine();
                    while (responseLine != null)
                    {
                        fileList.Add(responseLine);
                        responseLine = responseStream.ReadLine();
                    }
                    responseStream.Close();
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "Error reading in Catchup files", e.Message);
            }
            return fileList;
        }
    }
}
