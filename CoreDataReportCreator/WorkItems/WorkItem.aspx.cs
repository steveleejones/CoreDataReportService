using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreDataLibrary.Models;

namespace CoreDataReportCreator.WorkItems
{
    public partial class WorkItem : System.Web.UI.Page
    {
        public List<CoreDataLibrary.Models.CoreDataWorkItem> coreDataWorkItems = new List<CoreDataWorkItem>();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                coreDataWorkItems = CoreDataLibrary.Data.Get.GetCoreDataWorkItems();
            }

        }
    }
}