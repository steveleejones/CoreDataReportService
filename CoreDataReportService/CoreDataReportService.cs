using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CoreDataLibrary;
using CoreDataLibrary.Helpers;
using CoreDataLibrary.Objects;
using Timer = System.Timers.Timer;

namespace CoreDataReportService
{
    public partial class CoreDataReportService : ServiceBase
    {
        private const int MILLISECONDS_IN_60_MINUTES = 3600000;
        private static Timer coreDataReportTimer;
        private static Timer checkTimer;

        public CoreDataReportService()
        {
            if (Environment.UserInteractive)
            {
                MainProcess.CheckQueItems();
                MainProcess.Run();
            }
            else
                InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ReportLogger.AddMessage("CoreData Report Service", "Started");
            coreDataReportTimer = new Timer(MILLISECONDS_IN_60_MINUTES);
            coreDataReportTimer.Enabled = true;
            coreDataReportTimer.Elapsed += coreDataReportTimer_Elapsed;
            checkTimer = new Timer(60000);
            checkTimer.Enabled = true;
            checkTimer.Elapsed += CheckTimerOnElapsed;
        }

        private void CheckTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            MainProcess.CheckQueItems();
        }

        private void StopTimers()
        {
            coreDataReportTimer.Stop();
        }

        private void StartTimers()
        {
            coreDataReportTimer.Start();
        }

        void coreDataReportTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StopTimers();
            //Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataReportService", "CoreDataReportService Timer Elapsed");
            MainProcess.Run();
            StartTimers();
        }

        protected override void OnStop()
        {
            ReportLogger.AddMessage("CoreData Report Service", "Stopped");
        }
    }
}
