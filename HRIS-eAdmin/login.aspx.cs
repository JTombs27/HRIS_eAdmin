using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRIS_Common;

namespace HRIS_eAdmin
{
    public partial class login : System.Web.UI.Page
    {
        DataTable dataLogin
        {
            get
            {
                if ((DataTable)ViewState["dataLogin"] == null) return null;
                return (DataTable)ViewState["dataLogin"];
            }
            set
            {
                ViewState["dataLogin"] = value;
            }
        }

        CommonDB MyCmn = new CommonDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            if (!IsPostBack)
            {
                Session.RemoveAll();

            }
        }

        protected void tbx_username_TextChanged(object sender, EventArgs e)
        {
            if (tbx_username.Text.Trim() == "")
            {
                msg_logre.ForeColor = System.Drawing.Color.Red;
                msg_logre.Text = "User Name required.";
            }
            else
            {
                msg_logre.Text = "";
                msg_logre.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void tbx_password_TextChanged(object sender, EventArgs e)
        {

            if (tbx_password.Text.Trim() == "")
            {
                msg_logre.ForeColor = System.Drawing.Color.Red;
                msg_logre.Text = "Password required.";
            }
            else {
                msg_logre.Text = "";
                msg_logre.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btn_login_Command(object sender, CommandEventArgs e)
        {
            if (tbx_password.Text.Trim() == "" || tbx_username.Text.Trim() == "")
            {
                msg_logre.ForeColor = System.Drawing.Color.Red;
                msg_logre.Text = "User Name and/or Password required.";
            }
            else
            {
                Session.RemoveAll();
                dataLogin = MyCmn.RetrieveData("sp_user_login", "par_user_id", tbx_username.Text.Trim(), "par_user_password", MyCmn.EncryptString(tbx_password.Text.Trim(),MyCmn.CONST_WORDENCRYPTOR),"par_module_id","0");
                //dataLogin = MyCmn.RetrieveData("sp_userprofile_tbl_list2", "par_user_id", tbx_username.Text.Trim(), "par_user_password", tbx_password.Text.Trim());
                if (dataLogin.Rows.Count == 1)
                {
                    if (dataLogin.Rows[0]["change_password"].ToString() == "True")
                    {
                        msg_logre.ForeColor = System.Drawing.Color.Red;
                        msg_logre.Text = "<strong>FIRST LOGIN!</strong><br/>Login first to self service and changed your password.";
                    }
                    else
                    {
                        Session["ea_user_id"] = dataLogin.Rows[0]["user_id"].ToString();
                        Session["ea_user_profile"] = dataLogin.Rows[0]["empl_photo"].ToString().Trim();
                        Session["ea_empl_id"] = dataLogin.Rows[0]["empl_id"].ToString().Trim();
                        Session["ea_first_name"] = dataLogin.Rows[0]["first_name"].ToString().Trim();
                        Session["ea_last_name"] = dataLogin.Rows[0]["last_name"].ToString().Trim();
                        Session["ea_middle_name"] = dataLogin.Rows[0]["middle_name"].ToString().Trim();
                        Session["ea_suffix_name"] = dataLogin.Rows[0]["suffix_name"].ToString().Trim();
                        Session["ea_photo"] = dataLogin.Rows[0]["empl_photo"].ToString().Trim();
                        Session["ea_owner_fullname"] = dataLogin.Rows[0]["employee_name"].ToString().Trim();
                        Response.Redirect("~/");
                    }
                }
                else
                {
                    msg_logre.ForeColor = System.Drawing.Color.Red;
                    msg_logre.Text = "Incorrect username or passwsord.";
                }

            }
        }
    }
}