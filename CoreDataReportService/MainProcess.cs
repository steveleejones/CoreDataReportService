using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreDataLibrary;
using CoreDataLibrary.Data;
using CoreDataLibrary.Exporters;
using CoreDataLibrary.Helpers;
using CoreDataLibrary.Objects;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;

namespace CoreDataReportService
{
    public class MainProcess
    {
        public static void Run()
        {

            if (!CoreDataLib.IsLive())
            {
                List<ExportItem> runList = Get.GetAllRunItems();
                foreach (ExportItem exportItem in runList)
                {
                    ReportLogger reportLogger = new ReportLogger(exportItem.ExportItemName);
                    try
                    {
                        //exportItem.Export(reportLogger);
                        if (exportItem.ExportItemName == "LCH_FullStock_HotelOnly_HC_ENG")
                        {
                            exportItem.Export(reportLogger);
                            //LchFullStockImagesEngTsmExportItem expItem = new LchFullStockImagesEngTsmExportItem(exportItem);
                            //expItem.Export(reportLogger);
                        }
                        //exportItem.Export(reportLogger);
                        //if (exportItem.ExportItemName.Contains("LCH_FullStock_HotelOnly_ENG_100R"))
                        //{
                        //    exportItem.Export(reportLogger);
                        //}
                        reportLogger.EndLog();

                    }
                    catch (Exception e)
                    {
                        reportLogger.EndLog(e);
                    }
                }
                //Parallel.ForEach(runList, currentExportItem =>
                //{
                //    ReportLogger reportLogger = new ReportLogger(currentExportItem.ExportItemName);
                //    try
                //    {
                //        reportLogger.StartLog(currentExportItem.ExportItemName);
                //        currentExportItem.Export(reportLogger);
                //        reportLogger.EndLog();
                //    }
                //    catch (Exception e)
                //    {
                //        reportLogger.EndLog(e);
                //        Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataReportService", "MainProcess->Run", e);
                //    }
                //});
            }
            else
            {
                List<ExportItem> runList = Get.GetRunItems(DateTime.Now.Hour);

                Parallel.ForEach(runList, currentExportItem =>
                {
                    ReportLogger reportLogger = new ReportLogger(currentExportItem.ExportItemName);
                    try
                    {
                        reportLogger.StartLog(currentExportItem.ExportItemName);
                        currentExportItem.Export(reportLogger);
                        reportLogger.EndLog();
                    }
                    catch (Exception e)
                    {
                        reportLogger.EndLog(e);
                        Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataReportService", "MainProcess->Run", e);
                    }
                });
            }
        }

        internal static void CheckQueItems()
        {
            List<string> queItems = CoreDataLibrary.Data.Get.GetQueItems();

            Parallel.ForEach(queItems, currentExportItem =>
            {
                ReportLogger reportLogger = new ReportLogger(currentExportItem);
                try
                {
                    ExportItem exportItem = Get.GetExportItem(currentExportItem);
                    reportLogger = new ReportLogger(exportItem.ExportItemName);
                    reportLogger.StartLog("Exporting : " + exportItem.ExportItemName);
                    exportItem.Export(reportLogger);
                    reportLogger.EndLog(exportItem.ExportItemName + " : Exported");
                }
                catch (Exception e)
                {
                    reportLogger.EndLog(e);
                    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataReportService", "MainProcess->Run", e);
                }
            });
        }
    }
}
