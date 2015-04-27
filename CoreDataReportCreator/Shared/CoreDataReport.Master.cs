using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreDataReportCreator.Helpers;

namespace CoreDataReportCreator.Shared
{
    public partial class CoreDataReport : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserLogin();
            }
            else
            {
                UserLogin();
            }
        }

        private void UserLogin()
        {
            if (SessionManager.CurrentUser() != null && SessionManager.CurrentUser().LoggedIn)
            {
                SetUserUi();
            }
            else
            {
                SetUserUiLogout();
            }
        }

        private void SetUserUi()
        {
            CoreDataLibrary.Helpers.CoreDataUser user = SessionManager.CurrentUser();

            loginText.InnerText = "Logged in as : " + user.UserName;
            if (user.AccessLevel == 10
                | user.UserName == "patrick")
            {
                propertyexporter.Visible = true;
                offerloader.Visible = false;
            }
            else
            {
                propertyexporter.Visible = false;
                offerloader.Visible = false;
            }
            loginButton.Text = "Logout";
        }

        private void SetUserUiLogout()
        {
                loginText.InnerText = "";
                propertyexporter.Visible = false;
                offerloader.Visible = false;
                loginButton.Text = "Login";
        }

        protected void loginButton_OnClick(object sender, EventArgs e)
        {
            if (loginButton.Text == "Login")
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            else
            {
                loginButton.Text = "Login";
                SessionManager.LogoutUser();
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}