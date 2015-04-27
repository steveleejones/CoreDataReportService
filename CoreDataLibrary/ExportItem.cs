using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Data;
using CoreDataLibrary.Exporters;

namespace CoreDataLibrary
{
    public class ExportItem
    {
        public SelectStatementBuilder SelectStatementBuilder { get; set; }
        public int ExportEnabled { get; set; }
        public int ExportItemId { get; set; }
        public string ExportItemName { get; set; }
        public string ExportItemData { get; set; }
        public int ExportItemFtpId { get; set; }
        public int ExportItemRunTime { get; set; }
        public string ExportType { get; set; }
        private readonly IExporter _exporter;

        public ExportItem()
        {
        }

        public ExportItem(int exportItemId, string name, string item, int ftpId, int exportItemRunTime, int exportEnabled, string exportType)
        {
            ExportItemId = exportItemId;
            ExportItemName = name;
            ExportItemData = item;
            ExportItemFtpId = ftpId;
            ExportItemRunTime = exportItemRunTime;
            ExportEnabled = exportEnabled;
            ExportType = exportType;
            _exporter = ExporterFactory.GetExporter(this);
        }

        public void Save()
        {
            ExportItem exportItem = Get.GetExportItem(ExportItemName);
            if (exportItem == null)
                Insert.AddExportItem(ExportItemName, SelectStatementBuilder.SerializeToXml(), ExportItemFtpId, ExportItemRunTime, ExportEnabled, ExportType);
            else
                Update.UpdateExportItem(this);
        }

        public void Export(ReportLogger reportLogger)
        {
            if (SelectStatementBuilder == null)
            {
                SelectStatementBuilder = new SelectStatementBuilder();
                SelectStatementBuilder = SelectStatementBuilder.LoadSelectStatementBuilder(ExportItemName);
            }
            _exporter.Export(reportLogger);
        }
    }
}
