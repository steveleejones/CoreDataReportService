using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Reports
{
    
    public class IOLRunFtpMultipleLoad : IReport
    {
        // LoadInfotables, runMultipleDayLoading
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
            get { return "International Offer Loader Run FTP Multiple Load"; }
        }

        public bool RunReport()
        {
            ReportLogger reportLogger = new ReportLogger(ReportName);

            int stepId = reportLogger.AddStep();
            try
            {
                if (DueToRun())
                {
                    OfferLoader.FirstLoad();
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
            // Every day every 2 hours between 08:00 and 18:59
            DateTime dateTimeNow = DateTime.Now;
            DateTime lastRunTime = LastRun;

            if (dateTimeNow.Hour > 8 && dateTimeNow.Hour < 19)
            {
                TimeSpan sinceLastRun = dateTimeNow - lastRunTime;
                if (sinceLastRun.Hours > 2)
                    return true;
            }
            return false;
        }
    }
}
