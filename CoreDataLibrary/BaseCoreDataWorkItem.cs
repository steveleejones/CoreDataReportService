using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDataLibrary.Data;

namespace CoreDataLibrary
{
    public abstract class BaseCoreDataWorkItem : ICoreDataWorkItem
    {
        public bool m_quedToRun = false;

        public virtual DateTime LastRun
        {
            get
            {
                if (!String.IsNullOrEmpty(Name))
                    return CoreDataLibrary.Data.Get.GetWorkItemLastRunDate(Name);
                return new DateTime();
            }
            set
            {
                if (!String.IsNullOrEmpty(Name))
                    if (value.Year == 1)
                        value = DateTime.Now;
                Process.SaveWorkItemLastRunDateTime(Name, value);
            }
        }

        public abstract string Name { get; }
        public abstract bool Run();

        public virtual bool DueToRun()
        {
            ReadFromDb();
            var dateTime = DateTime.Now;
            var timeSpan = dateTime - LastRun;

            if (timeSpan.TotalMinutes >= 60 * 24 && dateTime.Hour == 1)
            {
                LastRun = dateTime;
                return true;
            }
            return false;
        }

        public virtual void PersistToDb()
        {
            if (LastRun.Year == 1)
                LastRun = DateTime.Now;

            Process.SaveWorkItemLastRunDateTime(Name, LastRun);
        }

        public virtual void ReadFromDb()
        {
            LastRun = Get.ReadCoreDataWorkItemLastRunTimeFromDb(Name);
        }

        public bool QuedToRun
        {
            get { return m_quedToRun; }
            set { m_quedToRun = value; }
        }
    }
}
