using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary;
using CoreDataLibrary.Data;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Reports
{
    public class IOLImportRunFtp : IReport
    {
        
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
            get { return "International Offer Import Info Tables"; }
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
            // Every day every 4 hours

            DateTime dateTimeNow = DateTime.Now;
            DateTime lastRunTime = LastRun;

            TimeSpan lastRunDateTime = dateTimeNow - lastRunTime;

            if(lastRunDateTime.Hours >= 4)
                return true;

            return false;
        }
    }
}
