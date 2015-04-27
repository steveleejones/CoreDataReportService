using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreDataLibrary.Objects
{
    public class LogEntry
    {
        private List<LogItemStep> LogItemSteps = new List<LogItemStep>();
        private int m_logId;
        private bool m_errors = false;

        [Key]
        public string LogItemName { get; set; }
        public DateTime StartTimeStamp { get; set; }
        public DateTime EndTimeStamp { get; set; }
        public string LogItemMessage { get; set; }
        public string LogItemStatus { get; set; }

        public string StartTime
        {
            get
            {
                return StartTimeStamp.ToLongTimeString();
            }
        }

        public string EndTime
        {
            get
            {
                if (EndTimeStamp.ToLongTimeString() != "01:01:01")
                    return EndTimeStamp.ToLongTimeString();
                return "";
            }
        }

        public string Duration
        {
            get
            {
                if (StartTimeStamp.ToLongTimeString() == "01:01:01" && EndTimeStamp.ToLongTimeString() == "01:01:01")
                    return "";
                else if (StartTimeStamp.ToLongTimeString() != "01:01:01" && EndTimeStamp.ToLongTimeString() == "01:01:01")
                {
                    TimeSpan currentTimeTaken = DateTime.Now - StartTimeStamp;
                    return currentTimeTaken.Hours.ToString().PadLeft(2, '0') + ":" + currentTimeTaken.Minutes.ToString().PadLeft(2, '0') + ":" + currentTimeTaken.Seconds.ToString().PadLeft(2, '0');
                }
                else
                {
                    TimeSpan timeSpan = EndTimeStamp - StartTimeStamp;
                    return timeSpan.ToString("c");
                }
            }
        }

        public bool InActive
        {
            get
            {
                if (LogItemStatus == "InActive")
                    return true;
                return false;
            }
        }

        public bool Succeded
        {
            get
            {
                return m_errors;
            }
        }

        public int LogId
        {
            get
            {
                return m_logId;
            }
            set
            {
                m_logId = value;
                GetLogItemSteps();
            }
        }

        public List<LogItemStep> GetAllSteps()
        {
            return LogItemSteps;
        }

        public List<LogItemStep> GetActiveSteps()
        {
            if (LogItemSteps.Count > 0)
            {
                return (List<LogItemStep>)LogItemSteps.Where(logItemStep => logItemStep.Status == "Active").ToList();
            }
            return new List<LogItemStep>();
        }

        public List<LogItemStep> GetInActiveSteps()
        {
            if (LogItemSteps.Count > 0)
            {
                return (List<LogItemStep>)LogItemSteps.Where(logItemStep => logItemStep.Status == "InActive").ToList();
            }
            return new List<LogItemStep>();
        }

        private void GetLogItemSteps()
        {
            LogItemSteps = CoreDataLibrary.Data.Get.GetLogSteps(m_logId);
            foreach (LogItemStep logItemStep in LogItemSteps)
            {
                if (logItemStep.Messages.Contains("Error"))
                {
                    m_errors = true;
                    break;
                }
            }
        }
    }
}
