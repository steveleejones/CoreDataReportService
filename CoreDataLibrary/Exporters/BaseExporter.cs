using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Exporters
{
    public abstract class BaseExporter : IExporter
    {
        private string _tempServerPath;
        internal ExportItem ExportItem { get; set; }
        public string PathAndFileName { get; private set; }
        public string Extension { get; set; }

        protected BaseExporter(ExportItem exportItem, string extension)
        {
            ExportItem = exportItem;
            Extension = extension;
            SetUpExporter();
        }

        public abstract bool Export(ReportLogger reportLogger);

        public string Name
        {
            get
            {
                return ExportItem.ExportItemName;
            }
        }

        private void SetUpExporter()
        {
            if (!CoreDataLib.IsLive())
                _tempServerPath = @"\\SVRsql4\E$\CoreData\ExportFiles\Test\Temp\";
            else
                _tempServerPath = @"E:\CoreData\ExportFiles\Temp\";

            PathAndFileName = _tempServerPath + ExportItem.ExportItemName + Extension;
        }
    }
}
