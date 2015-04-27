using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Reports
{
    
    public class IOLImportAllPackageData : IReport
    {
        // LoadFlightCostCache, LoadPropertyPriceCache
        public DateTime LastRun
        {
            get
            {
                return CoreDataLibrary.Data.Get.GetReportLastRunDateTime(ReportName);
            }
            set
            {
                CoreDataLibrary.Data.Update.UpdateReportLastRunDateTime(ReportName, DateTime.Now);
            }
        }

        public string ReportName
        {
            get { return "International Offer Import All Package Data"; }
        }

        public bool RunReport()
        {
            ReportLogger reportLogger = new ReportLogger(ReportName);

            int stepId = reportLogger.AddStep();
            try
            {
                if (DueToRun())
                {
                    OfferLoader.ImportAllPackagesData();
                    LastRun = DateTime.Now;
                    reportLogger.EndStep(stepId);
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

        public bool DueToRun()
        {
            // Every day at 21:57
            DateTime dateTimeNow = DateTime.Now;

            if(dateTimeNow.Hour == 22)
                return true;

            return false;
        }
    }
}
