using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Organizer_Web
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database.CreateConnection();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var isLoginPossible = Database.RunWithOpenConnection(Database.IsLoginPossible, tbUsername.Text);

            if (!isLoginPossible)
            {
                tbError.Text = "Nije moguce logovanje.";
                return;
            }

            Application["username"] = tbUsername.Text;
            Application["userId"] = Database.RunWithOpenConnection(Database.GetUserId, tbUsername.Text);

            Response.Redirect("dashboard.aspx");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (tbUsername.Text.Trim().Length == 0 || !Database.RunWithOpenConnection(Database.Register, tbUsername.Text))
            {
                tbError.Text = "Nije moguce registrovanje.";
                return;
            }

            Application["username"] = tbUsername.Text;
            Application["userId"] = Database.RunWithOpenConnection(Database.GetUserId, tbUsername.Text);

            Response.Redirect("dashboard.aspx");
        }
    }
}