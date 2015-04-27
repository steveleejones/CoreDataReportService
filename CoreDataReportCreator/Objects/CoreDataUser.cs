using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreDataReportCreator.Objects
{
    public class CoreDataUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool LoggedIn { get; set; }
        public int AccessLevel { get; set; }

        public CoreDataUser(string name, string password)
        {
            CoreDataUser user = CoreDataLibrary.Data.Get.CoreDataUser(name, password);
        }
    }
}