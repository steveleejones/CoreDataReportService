using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Helpers
{
    public class FileDownloader
    {
        public static void DownloadFile(string url, string outputPath)
        {
            using (WebClient Client = new WebClient())
            {
                Client.DownloadFile(url, outputPath);
            }
        }
    }
}
