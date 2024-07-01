using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRIS_Common;
using System.Net;
using System.Web.Services;
using System.Web.Security;

namespace HRIS_eAdmin
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        //********************************************************************
        //  BEGIN - JMTJR- 09/03/2018 - Data Place holder creation 
        //********************************************************************
        public string menu_active_level;
        public string current_ruet;
        public string active_menu_id;
        public string active_parent_id;
        public string page_title;
        public string budget_code;
        public string allow_add;
        public string allow_edit;
        public string allow_delete;
        public string allow_print;
        public string allow_view;

        public string allow_edit_history;
        public static string third_url;
        public static string user_defaults;

        DataTable dtMenuSource
        {
            get
            {
                if ((DataTable)ViewState["dtMenuSource"] == null) return null;
                return (DataTable)ViewState["dtMenuSource"];
            }
            set
            {
                ViewState["dtMenuSource"] = value;
            }
        }
        DataTable userdefaults
        {
            get
            {
                if ((DataTable)ViewState["userdefaults"] == null) return null;
                return (DataTable)ViewState["userdefaults"];
            }
            set
            {
                ViewState["userdefaults"] = value;
            }
        }

        CommonDB MyCmn = new CommonDB();

        //********************************************************************
        //  BEGIN - JMTJR- 09/03/2018 - Menu List Variable Initialization 
        //********************************************************************
        public class page_menus
        {
            public int id;
            public string menu_name;
            public int menu_id_link;
            public string url_name;
            public string page_title;
            public string menu_icon;
            public int menu_level;
        }
        public List<page_menus> menus = new List<page_menus>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ea_user_id"] == null)
                {
                    Response.Redirect("~/logoff.aspx");
                }
              
                initialize();
            }
            Page.LoadComplete += Page_LoadComplete;
        }
        private void Page_LoadComplete(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            try
            {

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
                Response.Expires = -1000;
                Response.CacheControl = "no-cache";
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void initialize()
        {
            //dtMenuSource = MyCmn.RetrieveData("sp_menus_tbl_list", "module_id", 0);
            dtMenuSource = MyCmn.RetrieveData("sp_user_menu_access_role_list", "par_user_id", Session["ea_user_id"].ToString(), "par_module_id", 0);
            menus.Clear();
            current_ruet = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            string[] extentions = Request.QueryString.AllKeys;
            string url_params = "";

            if (extentions.Length > 0)
            {
                foreach (string keys in extentions)
                {
                    url_params = url_params + "&" + keys + "=" + Request.QueryString[keys].ToString();
                }
            }
            url_params = url_params.Trim('&');
            url_params = url_params.Trim() != "" ? "?" + url_params : "";
            if (url_params != "" && url_params != null)
            {
                current_ruet = current_ruet + "" + url_params;
            }
            DataRow[] MenuRows = dtMenuSource.Select();
            foreach (DataRow row in MenuRows)
            {

                if (row["url_name"].ToString() == current_ruet)
                {
                    page_title = row["page_title"].ToString();
                    active_menu_id = row["id"].ToString();
                    active_parent_id = row["menu_id_link"].ToString();
                    menu_active_level = row["menu_level"].ToString();
                    allow_add = row["allow_add"].ToString();
                    allow_edit = row["allow_edit"].ToString();
                    allow_delete = row["allow_delete"].ToString();
                    allow_print = row["allow_print"].ToString();
                    allow_view = row["allow_view"].ToString();
                    allow_edit_history = row["allow_edit_history"].ToString();
                }


                page_menus getMenusFromDB = new page_menus();
                getMenusFromDB.id = Convert.ToInt32(row["id"]);
                getMenusFromDB.menu_name = row["menu_name"].ToString();
                getMenusFromDB.menu_icon = WebUtility.HtmlDecode(row["menu_icon"].ToString());
                getMenusFromDB.url_name = row["url_name"].ToString();
                getMenusFromDB.page_title = row["page_title"].ToString();
                getMenusFromDB.menu_id_link = Convert.ToInt32(row["menu_id_link"]);
                getMenusFromDB.menu_level = Convert.ToInt32(row["menu_level"]);
                menus.Add(getMenusFromDB);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("~/login.aspx");
        }
    }
}