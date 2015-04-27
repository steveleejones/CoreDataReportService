using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreDataLibrary;
using CoreDataLibrary.Objects;

namespace CoreDataReportCreator
{
    public partial class CoreData : System.Web.UI.Page
    {
        private readonly DateTime m_dateTime = DateTime.Now;
        private static bool s_showActive;
        private static bool s_showInActive;
        private static bool s_showError;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //m_dateTime = DateTime.Now;
                //txtDate.Text = m_dateTime.ToShortDateString();
                BindDaysLogs();
                BindMessages();
            }
        }

        protected void UpDateTimer_Elapsed(object sender, EventArgs e)
        {
            BindDaysLogs();
            BindMessages();
            //m_dateTime = DateTime.Now;
            //txtDate.Text = m_dateTime.ToShortDateString();
        }

        protected void Details_OnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.CommandArgument.ToString();

            Response.Redirect("~/LogDetails.aspx?id=" + id);
        }

        protected void RepeaterInActiveLogs_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Panel panel = ((Panel)e.Item.FindControl("paneltitle"));
                LogEntry logEntry = ((LogEntry)e.Item.DataItem);
                if (logEntry.LogItemMessage.Contains(("Error")))
                {
                    panel.CssClass = "panel panel-danger";
                }
                else if (logEntry.LogItemStatus.StartsWith("Active"))
                {
                    panel.CssClass = "panel panel-success";
                }
                else if (logEntry.LogItemStatus.StartsWith("InActive"))
                {
                    panel.CssClass = "panel panel-warning";
                }
                else if (logEntry.LogItemStatus.StartsWith("Message"))
                {
                    Label messageLabel = ((Label)e.Item.FindControl("LabelMessage"));
                    if (messageLabel != null)
                        messageLabel.Text = logEntry.LogItemMessage;

                    panel.CssClass = "panel panel-info";
                }
                List<LogItemStep> steps = logEntry.GetAllSteps();
                foreach (LogItemStep logItemStep in steps)
                {
                    if (logItemStep.Messages.Contains("Error"))
                        panel.CssClass = "panel panel-danger";
                }
                if (logEntry.LogItemStatus == "Cancelled")
                {
                    panel.CssClass = "panel panel-warning"; 
                }
            }
        }

        protected void ButtonUpdate_OnClick(object sender, EventArgs e)
        {
            //m_dateTime = DateTime.Parse(txtDate.Text);
            BindDaysLogs();
        }

        private void BindDaysLogs()
        {
            try
            {
                this.ActiveLogRepeater.DataSource = CoreDataLibrary.Data.Get.GetDayLogItems(m_dateTime);
                this.ActiveLogRepeater.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void BindMessages()
        {
            try
            {
                this.MessagesRepeater.DataSource = CoreDataLibrary.Data.Get.GetDayMessages(m_dateTime);
                this.MessagesRepeater.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected void MessagesRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                LogEntry logEntry = ((LogEntry)e.Item.DataItem);
                Panel panel = ((Panel)e.Item.FindControl("panelMessageTitle"));
                if(panel != null)
                    panel.CssClass = "panel panel-info";

                Label messageLabel = ((Label)e.Item.FindControl("LabelMessageSidebar"));
                if (messageLabel != null)
                    messageLabel.Text = logEntry.LogItemMessage;
            }
        }

        protected void CheckBoxActive_OnCheckedChanged(object sender, EventArgs e)
        {
            //ShowActive = CheckBoxActive.Checked;
        }

        protected void CheckBoxInActive_OnCheckedChanged(object sender, EventArgs e)
        {
            //ShowInActive = CheckBoxInActive.Checked;
        }

        protected void CheckBoxError_OnCheckedChanged(object sender, EventArgs e)
        {
            //ShowError = CheckBoxError.Checked;
        }
    }
}