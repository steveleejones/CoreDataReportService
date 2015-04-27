using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreDataLibrary.Data;
using CoreDataReportCreator.Helpers;

namespace CoreDataReportCreator.Login
{
    public partial class Account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void ButtonChangePassword_OnClick(object sender, EventArgs e)
        {
            if(TextBoxOriginalPassword.Text != "" && TextBoxNewPassword.Text != "" && TextBoxNewPassword2.Text != "")
            {
                if (TextBoxNewPassword.Text != TextBoxNewPassword2.Text)
                {
                    LiteralErrorMessage.Text = "New passwords do not match.";
                }
                else
                {
                    if (Update.ChangeUserPassword(SessionManager.CurrentUser().UserName,
                        TextBoxNewPassword.Text, TextBoxOriginalPassword.Text))
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        LiteralErrorMessage.Text = "Unable to change password.";
                    }
                }
            }
            else
            {
                LiteralErrorMessage.Text = "Please enter password information";
            }
        }
    }
}