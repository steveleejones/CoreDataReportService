using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataReportService.Helpers
{
    public static class Emailer
    {
        private const string UserName = "LOWCOSTBEDS\\SQLAdmin";
        private const string Password = "SQL4dmin123";
        private const string HostName = "svrexch1.lowcostbeds.com";
        private const string FromName = "PPCEngine@lowcostholidays.com";

        private const int Port = 587;

        public static void SendEmail(string toEmail, string subject, string message)
        {
            MailMessage mail = new MailMessage(FromName, toEmail, subject, message);
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Priority = MailPriority.Normal;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(UserName, Password);
            client.Port = Port;
            client.Host = HostName;
            client.EnableSsl = false;
            client.UseDefaultCredentials = true;

            try
            {
                client.Send(mail);
            }
            catch (Exception)
            {
            }
        }

        public static void SendEmail(string toEmail, string subject, string function, Exception exception)
        {
            string error = "Error occured in : " + function + " : " + Environment.NewLine + Environment.NewLine;
            error += CreateExceptionMessage(exception);
            SendEmail(toEmail, subject, error);
        }

        private static void SendEmail(string toEmail, string subject, Exception exception)
        {
            SendEmail(toEmail, subject, CreateExceptionMessage(exception));
        }

        private static string CreateExceptionMessage(Exception exception)
        {
            string exceptionMessage;

            exceptionMessage = exception.Message + Environment.NewLine + Environment.NewLine;
            exceptionMessage += exception.StackTrace + Environment.NewLine + Environment.NewLine;

            if (exception.InnerException != null)
            {
                exceptionMessage = "InnerException" + Environment.NewLine + Environment.NewLine;
                exceptionMessage += exception.InnerException.Message + Environment.NewLine + Environment.NewLine;
                exceptionMessage += exception.InnerException.StackTrace + Environment.NewLine + Environment.NewLine;
            }
            return exceptionMessage;
        }
    }
}
