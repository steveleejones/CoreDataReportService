using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreDataLibrary.Objects;
using CoreDataReportCreator.Helpers;

namespace CoreDataReportCreator
{
    public partial class LogDetails : System.Web.UI.Page
    {
        private List<LogItemStep> logSteps = new List<LogItemStep>();
        private LogEntry m_logEntry;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);

                m_logEntry = CoreDataLibrary.Data.Get.GetLogEntry(id);
                logSteps = CoreDataLibrary.Data.Get.GetLogSteps(id);

                LogLabelName.Text = m_logEntry.LogItemName;
                LogLabelStartTime.Text = m_logEntry.StartTimeStamp.ToLongTimeString();

                if (m_logEntry.LogItemStatus == "Active")
                    LogLabelEndTime.Text = String.Empty;
                else
                    LogLabelEndTime.Text = m_logEntry.EndTimeStamp.ToLongTimeString();

                LogLabelStatus.Text = m_logEntry.LogItemStatus;
                LogMessages.Text = m_logEntry.LogItemMessage;

                this.LogDetailsRepeater.DataSource = logSteps;
                LogDetailsRepeater.DataBind();
                if (SessionManager.CurrentUser() != null && SessionManager.CurrentUser().AccessRole == 10)
                {
                    ButtonRemoveLogItem.Visible = true;
                    ButtonCancel.Visible = true;
                }
                else
                {
                    ButtonRemoveLogItem.Visible = false;
                    ButtonCancel.Visible = false;
                }
            }
        }
        private static int id;

        protected void LogDetailsRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Panel panel = ((Panel)e.Item.FindControl("paneltitle"));
                TextBox textBoxMessage = ((TextBox)e.Item.FindControl("Messages"));
                Label labelEndTime = ((Label)e.Item.FindControl("LabelEndTime"));
                Label labelTimetaken = ((Label)e.Item.FindControl("LabelTimeTaken"));
                LogItemStep logStep = ((LogItemStep)e.Item.DataItem);
                textBoxMessage.Text = logStep.Messages;
                if (logStep.Status == "Active")
                {
                    labelEndTime.Text = "";
                    labelTimetaken.Text = "";
                }
                else
                {
                    labelEndTime.Text = logStep.EndTimeStamp.ToLongTimeString();
                    labelTimetaken.Text = logStep.TimeTakenForStep.ToString();
                }

                if (logStep.Messages.Contains(("Error")))
                {
                    panel.CssClass = "panel panel-danger";
                }
                else if (logStep.Status == "Active")
                {
                    panel.CssClass = "panel panel-success";
                }
                else if (logStep.Status == "InActive")
                {
                    panel.CssClass = "panel panel-warning";
                }
            }
        }

        protected void ButtonCancel_OnClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            CoreDataLibrary.Data.Process.CancelLog(id);
            Response.Redirect("Default.aspx");
        }

        protected void ButtonRemoveLogItem_OnClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            CoreDataLibrary.Data.Process.MarkLogItemAsRemoved(id);
            Response.Redirect("Default.aspx");
        }
    }
}