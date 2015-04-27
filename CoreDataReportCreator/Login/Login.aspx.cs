using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreDataReportCreator.Helpers;

namespace CoreDataReportCreator.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                
            //}
            //else
            //{
            //    HttpContext.Current.Response.Redirect("~/Default.aspx", true); 
            //}
        }

        protected void ButtonLogin_OnClick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBoxUserName.Text) || !String.IsNullOrEmpty(TextBoxPassword.Text))
            {
                CoreDataLibrary.Helpers.CoreDataUser user = new CoreDataLibrary.Helpers.CoreDataUser(TextBoxUserName.Text, TextBoxPassword.Text);
                if (user.LoggedIn)
                {
                    SessionManager.InsertUser(user);
                    HttpContext.Current.Response.Redirect("~/Default.aspx", true);
                }
                else
                {
                    LiteralErrorMessage.Text = "Username & Password do not match";
                }
            }
            else
            {
                LiteralErrorMessage.Text = "Enter username & password";
            }
        }

        protected void ButtonCancel_OnClick(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect("~/Default.aspx", true);
        }
    }
}