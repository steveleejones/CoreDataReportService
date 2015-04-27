using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Data
{
    public class DataConnection
    {
        private const string SqlConnCoreData_Live = "Data Source=svrsql4;Initial Catalog=CoreData;uid=CoreData;password=CoreD@T@";
        private const string SqlConnCoreData_Test = "Data Source=svrsql4;Initial Catalog=CoreData_test;uid=CoreData;password=CoreD@T@";
        private const string SqlConnNonCoreData_Live = "Data Source=SVRSQL4;Initial Catalog=NonCoreData;Persist Security Info=True;User ID=CoreData;Password=CoreD@T@";
        private const string SqlConnMssqldev = "Data Source=MSSQLDEV;Initial Catalog=ReportingDB;uid=lcbuser;password=lcbuser";
        private const string SqlConnInternationalOfferLoader = "Data Source=MSSQLDEV;Initial Catalog=InternationalOfferLoader;uid=lcbuser;password=lcbuser";

        public static string SqlConnCoreData
        {
            get
            {
                // Need the test database set up before we can use this!
                if (!CoreDataLib.IsLive())
                    return SqlConnCoreData_Test;
                return SqlConnCoreData_Live;
            }
        }

        public static string SqlConnNonCoreData
        {
            get
            {
                return SqlConnNonCoreData_Live;
            }
        }

        public static string MssqldevConnection
        {
            get
            {
                return SqlConnMssqldev; 
            }
        }

        public static string InternationalOfferLoaderConnection
        {
            get
            {
                return SqlConnInternationalOfferLoader; 
            }
        }
    }
}
