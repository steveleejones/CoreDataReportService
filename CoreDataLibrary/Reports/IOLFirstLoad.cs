using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreDataLibrary;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Reports
{
    // Every day at 00:13
    public class IolFirstLoad : IReport
    {
        // SaveCriteria, UpdateIncludeTable, CreateAllOutputFiles

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
            get { return "International Offer Loader First Load"; }
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
            // Every day at 00:13
            DateTime dateTimeNow = DateTime.Now;
 
            if(dateTimeNow.Hour == 0)
                return true;

            return false;
        }
    }
}
