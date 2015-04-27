using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreDataLibrary.Helpers;

namespace CoreDataLibrary.Reports
{
    
    public class IOLPropertyMapping : IReport
    {
        //Extract
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
            get { return "International Offer Property Mapping"; }
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
            // Every week on sunday at 13:00
            DateTime dateTimeNow = DateTime.Now;

            if (dateTimeNow.DayOfWeek == DayOfWeek.Sunday)
            {
                if(dateTimeNow.Hour == 13)
                    return true;
            }
            return false;
        }
    }
}
