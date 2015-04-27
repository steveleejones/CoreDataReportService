using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary
{
    public interface ICoreDataWorkItem
    {
        DateTime LastRun { get; set; }
        string Name { get; }
        bool Run();
        bool DueToRun();
        void PersistToDb();
        void ReadFromDb();
    }
}
