using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreDataReportCreator.Helpers
{
    public class SessionManager
    {
        private static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        private static void AddObjectToSession(string objectName, object objectToAdd)
        {
            HttpContext.Current.Session.Add(objectName, objectToAdd);
        }

        private static object GetObjectFromSession(string objectName)
        {
            return HttpContext.Current.Session[objectName];
        }

        private static void RemoveObjectFromSession(string objectName)
        {
            HttpContext.Current.Session.Remove(objectName);
        }

        public static void InsertUser(CoreDataLibrary.Helpers.CoreDataUser user)
        {
            AddObjectToSession("currentuser", user);
        }

        public static CoreDataLibrary.Helpers.CoreDataUser CurrentUser()
        {
            return GetObjectFromSession("currentuser") as CoreDataLibrary.Helpers.CoreDataUser;
        }

        public static void LogoutUser()
        {
            HttpContext.Current.Session.Remove("currentuser");
        }
    }
}