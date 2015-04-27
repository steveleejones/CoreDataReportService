using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary
{
    public interface ICoreProcess
    {
        DateTime ProcessLastRun { get; set; }
        string ProcessName { get; }
        bool ProcessRun();
        bool ProcessDueToRun();
    }
}
