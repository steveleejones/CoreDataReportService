using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CoreDataLibrary
{
    public class ReportLogger
    {
        private int Id = 0;
        private readonly string LoggerName;
        private int callerId = 2;

        public ReportLogger(string name)
        {
            LoggerName = name;
        }

        public int LogId
        {
            get
            {
                return Id;
            }
        }

        public static void AddMessage(string name, string message)
        {
            Data.Insert.AddMessage(name, message);
        }

        public static void AddMessage(string name, Exception exception)
        {
            Data.Insert.AddMessage(name, "Error : " + ExceptionMessage(exception));    
        }

        public int StartLog(string message)
        {
            Id = Data.Insert.AddLogStart(LoggerName, DateTime.Now, message);
            return Id;
        }

        public int AddStep(string step)
        {
            return Data.Insert.AddLogStep(LogId, DateTime.Now, step);
        }

        public int AddStep()
        {
            return Data.Insert.AddLogStep(LogId, DateTime.Now, GetCallingMethod(callerId)); 
        }

        public void EndLog()
        {
            EndLog("Finished");
        }

        public void EndLog(string message)
        {
            Data.Insert.AddLogEnd(LogId, DateTime.Now, message);
        }

        public void EndLog(Exception exception)
        {
            Data.Insert.AddLogEnd(LogId, DateTime.Now, "Error : " + ExceptionMessage(exception));
        }

        public void EndStep(int id)
        {
            Data.Insert.AddLogStepEnd(id, DateTime.Now);
        }

        public void EndStep(int id, Exception exception)
        {
            Data.Insert.AddLogStepEnd(id, DateTime.Now, "Error : " + ExceptionMessage(exception));
        }

        public void EndStep(int id, string message)
        {
            Data.Insert.AddLogStepEnd(id, DateTime.Now, message);
        }

        private static string ExceptionMessage(Exception exception)
        {
            string exceptionMessage = exception.Message + Environment.NewLine + Environment.NewLine;
            exceptionMessage += exception.StackTrace + Environment.NewLine + Environment.NewLine;

            if (exception.InnerException != null)
            {
                exceptionMessage = "InnerException" + Environment.NewLine + Environment.NewLine;
                exceptionMessage += exception.InnerException.Message + Environment.NewLine + Environment.NewLine;
                exceptionMessage += exception.InnerException.StackTrace + Environment.NewLine + Environment.NewLine;
            }
            return exceptionMessage;
        }

        private static string GetCallingMethod(int caller)
        {
            StackTrace stackTrace = new StackTrace();

            StackFrame stackFrame = stackTrace.GetFrame(caller);
            return stackFrame.GetMethod().Name;
        }
    }
}
