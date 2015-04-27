using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataReportService.Data;

namespace CoreDataReportService.Helpers
{
    public class ExportItem
    {
        public SelectStatementBuilder m_selectStatementBuilder { get; set; }
        public int ExportItemId { get; set; }
        public string ExportItemName { get; set; }
        public string ExportItemData { get; set; }
        public int ExportItemFtpId { get; set; }

        public ExportItem()
        {
        }

        public ExportItem(int exportItemId, string name, string item, int ftpId)
        {
            ExportItemId = exportItemId;
            ExportItemName = name;
            ExportItemData = item;
            ExportItemFtpId = ftpId;
            //m_selectStatementBuilder = SelectStatementBuilder.LoadSelectStatementBuilder(ExportItemName);
        }

        public SelectStatementBuilder SelectStatementBuilder
        {
            get { return m_selectStatementBuilder; }
            set { m_selectStatementBuilder = value; }
        }

        public void Save()
        {
            ExportItem exportItem = Get.GetExportItem(ExportItemName);
            if (exportItem == null)
                Insert.AddExportItem(ExportItemName, SelectStatementBuilder.LoadSelectStatementBuilder(exportItem.ExportItemName).SelectStatement() ,ExportItemFtpId);
            else
                Update.UpdateExportItem(this);
        }
    }
}
