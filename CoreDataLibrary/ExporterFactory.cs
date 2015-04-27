using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Exporters;

namespace CoreDataLibrary
{
    public class ExporterFactory
    {
        public static IExporter GetExporter(ExportItem exportItem)
        {
            switch (exportItem.ExportType)
            {
                case "csv":
                    return new CsvExporter(exportItem);
                case "xml":
                    return new XmlExporter(exportItem);
            }
            return null;
        }
    }
}
