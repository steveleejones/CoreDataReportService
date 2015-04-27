using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace CoreDataLibrary.Objects
{
    public class LogDisplay
    {
        private List<LogEntry> ActiveLogEntries = new List<LogEntry>();
        private List<LogEntry> DaysLogEntries = new List<LogEntry>();
        private List<LogEntry> InActiveLogEntries = new List<LogEntry>();
        private List<LogEntry> ErrorLogEntries = new List<LogEntry>();
        private System.Timers.Timer updateTimer = new Timer();

        public LogDisplay()
        {
            ActiveLogEntries = CoreDataLibrary.Data.Get.GetSuccesfulActiveLogItems();
            InActiveLogEntries = CoreDataLibrary.Data.Get.GetInActiveLogItems();
            ErrorLogEntries = CoreDataLibrary.Data.Get.GetErrorLogItems();
            DaysLogEntries = CoreDataLibrary.Data.Get.GetDayLogItems(DateTime.Now);

            //GetLogItemSteps();
            updateTimer.Interval = 1000;
            updateTimer.Elapsed += updateTimer_Elapsed;
            updateTimer.Enabled = true;
        }

        //private void GetLogItemSteps()
        //{
        //    foreach (LogEntry logEntry in ActiveLogEntries)
        //    {
        //        logEntry
        //    }
        //}
        public List<LogEntry> GetDaysLogs(DateTime dateTime)
        {
            DaysLogEntries = CoreDataLibrary.Data.Get.GetDayLogItems(dateTime);
            return DaysLogEntries;
        }

        public List<LogEntry> GetActiveLogs()
        {
            return ActiveLogEntries;
        }

        public List<LogEntry> GetInActiveLogs()
        {
            return InActiveLogEntries;
        }

        public List<LogEntry> GetErrorLogs()
        {
            return ErrorLogEntries;
        }

        public LogEntry GetLogEntry(int logId)
        {
            foreach (LogEntry logEntry in ActiveLogEntries)
            {
                if (logEntry.LogId == logId)
                    return logEntry;
            }
            return null;
        }

        void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            updateTimer.Stop();
            System.Console.WriteLine("...... Tick");
            ActiveLogEntries = CoreDataLibrary.Data.Get.GetSuccesfulActiveLogItems();
            InActiveLogEntries = CoreDataLibrary.Data.Get.GetInActiveLogItems();
            //foreach (LogEntry logEntry in ActiveLogEntries)
            //{
            //    System.Console.WriteLine(logEntry.LogItemName + " : " + logEntry.LogItemMessage);
            //    foreach (LogItemStep logItemStep in logEntry.GetActiveSteps())
            //    {
            //        System.Console.WriteLine("......" + logItemStep.Step);
            //    }
            //}
            updateTimer.Start();
        }
    }
}
