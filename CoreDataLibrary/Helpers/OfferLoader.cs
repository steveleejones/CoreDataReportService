using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary;
using CoreDataLibrary.Data;
using CoreDataLibrary.Models;

namespace CoreDataLibrary.Helpers
{
    public class OfferLoader
    {
        public static void ImportRunFtp()
        {
            ReportLogger importRunFtpLogger = new ReportLogger("ImportRunFtpLogger");

            importRunFtpLogger.StartLog("Start of Run");

            try
            {
                SaveCriteria(importRunFtpLogger);
                UpdateIncludeTable(importRunFtpLogger);
                LoadFlightCostCache(importRunFtpLogger);
                LoadPropertyPriceCache(importRunFtpLogger);
                CreateAllOutputFiles(importRunFtpLogger);
                SendEmails("Dear All, the InternationalOfferLoader has successfully completed an import and load run for");
                importRunFtpLogger.EndLog();
            }
            catch (Exception e)
            {
                importRunFtpLogger.EndLog(e);
            }
        }

        public static void ImportAllPackagesData()
        {
            ReportLogger importAllPackagesData = new ReportLogger("ImportAllPackagesData");
            LoadFlightCostCache(importAllPackagesData);
            LoadPropertyPriceCache(importAllPackagesData);
        }

        public static void FirstLoad()
        {
            ReportLogger firstLoadReportLogger = new ReportLogger("FirstLoad");
            SaveCriteria(firstLoadReportLogger);
            UpdateIncludeTable(firstLoadReportLogger);
            CreateAllOutputFiles(firstLoadReportLogger);
            SendEmails("Dear All, the InternationalOfferLoader has successfully completed its first run of the day for");
        }

        public static void ImportInfoTables()
        {
            ReportLogger importInfoTablesLogger = new ReportLogger("ImportInfoTables");
            ClearOutSurplusReportTables(importInfoTablesLogger);
            SaveCriteria(importInfoTablesLogger);
            LoadInfoTables(importInfoTablesLogger);
        }

        public static void RunFtpMultipleLoad()
        {
            ReportLogger runFtpMultipleLoad = new ReportLogger("RunFtpMultipleLoad");
            LoadInfoTables(runFtpMultipleLoad);
            CreateAllOutputFiles(runFtpMultipleLoad);
            SendEmails("Dear All, the InternationalOfferLoader has successfully completed a recurring run for");
        }

        public static void PropertyMapping()
        {
            ReportLogger propertyMappingReportLogger = new ReportLogger("PropertyMapping");
            Extract(propertyMappingReportLogger);
        }

        static void SaveCriteria(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "uspExportAllCriteria");
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        static void UpdateIncludeTable(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "uspImportNewInclude");
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        static void LoadFlightCostCache(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "uspFlightCostCacheOut");
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        static void LoadPropertyPriceCache(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "uspPropertyPriceCacheOut");
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        static void CreateAllOutputFiles(ReportLogger reportLogger)
        {
            List<ConfigOPFile> internationalOfferLoaderFiles = new List<ConfigOPFile>();

            reportLogger.StartLog("Load Files to process");
            internationalOfferLoaderFiles = Get.GetInternationalOfferLoaderFiles();
            reportLogger.EndLog("Files Loaded");

            reportLogger.StartLog("Processing International OfferLoader Files");
            try
            {
                foreach (ConfigOPFile configOpFile in internationalOfferLoaderFiles)
                {
                    if (configOpFile.Template.Trim() == "Long")
                    {
                        ProcessLongOutput(configOpFile.FileName, reportLogger);
                    }
                    else if (configOpFile.Template.Trim() == "Detail")
                    {
                        ProcessDetailOutput(configOpFile.FileName, reportLogger);
                    }
                    else if (configOpFile.Template.Trim() == "Short")
                    {
                        ProcessShortOutput(configOpFile.FileName, reportLogger);
                    }
                }
                reportLogger.EndLog("End of International OfferLoader Files");
            }
            catch (Exception e)
            {
                reportLogger.EndLog(e);
            }
        }

        private static void ProcessShortOutput(string fileName, ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep("International Offer Loader - " + fileName);

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.InternationalOfferLoaderConnection))
                {
                    SqlCommand cmd = new SqlCommand("dbo.uspOutputShort", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@psFileName", fileName);
                    cmd.CommandTimeout = 720000;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    reportLogger.EndStep(stepId);
                }
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        private static void ProcessDetailOutput(string fileName, ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep("International Offer Loader - " + fileName);

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.InternationalOfferLoaderConnection))
                {
                    SqlCommand cmd = new SqlCommand("dbo.uspOutputDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@psFileName", fileName);
                    cmd.CommandTimeout = 72000;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    reportLogger.EndStep(stepId);
                }
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        private static void ProcessLongOutput(string fileName, ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep("International Offer Loader - " + fileName);

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection.InternationalOfferLoaderConnection))
                {
                    SqlCommand cmd = new SqlCommand("dbo.uspOutputLong", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@psFileName", fileName);
                    cmd.CommandTimeout = 72000;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    reportLogger.EndStep(stepId);
                }
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }
        private static void SendEmails(string message)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.Add("@profile_name", SqlDbType.VarChar).Value = "MIS";
            cmd.Parameters.Add("@recipients", SqlDbType.VarChar).Value = "alex.gisbert@lowcostholidays.com";
            cmd.Parameters.Add("@copy_recipients", SqlDbType.VarChar).Value = "Steven.Jones@lowcostholidays.com;michael.gaylard@lowcostholidays.com";
            cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "InternationalOfferLoader Completed Load - " + DateTime.Now;
            cmd.Parameters.Add("@body", SqlDbType.NVarChar).Value = message + " - " + DateTime.Now;

            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "sp_send_dbmail");
            }
            catch (Exception e)
            {
                Console.WriteLine("Sending success email failed. Error : " + e.Message);
            }
        }

        static void ClearOutSurplusReportTables(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "uspZ_aaa_ClearOutReportTables");
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        static void LoadInfoTables(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "uspImportInfoTables");
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }

        static void Extract(ReportLogger reportLogger)
        {
            int stepId = reportLogger.AddStep();
            SqlCommand cmd = new SqlCommand();
            SqlConnection scon = new SqlConnection("Data Source=MSSQLDEV;Initial Catalog=ReportingDB;Integrated Security=True");
            try
            {
                DataAccess.ExecuteNonQuery(ref cmd, CommandType.StoredProcedure, "mcgspInternationalOfferLoaderPropertyMapping", scon);
                reportLogger.EndStep(stepId);
            }
            catch (Exception e)
            {
                reportLogger.EndStep(stepId, e);
            }
        }
    }
}
