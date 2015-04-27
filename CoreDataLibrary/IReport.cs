using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreDataLibrary
{
    public interface IReport
    {
        DateTime LastRun { get; set; }
        string ReportName { get; }
        bool RunReport();
        bool DueToRun();
    }
}
