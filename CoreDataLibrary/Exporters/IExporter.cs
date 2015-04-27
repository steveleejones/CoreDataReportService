using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Exporters
{
    public interface IExporter
    {
        bool Export(ReportLogger reportLogger);
        string Name { get; }
    }
}
