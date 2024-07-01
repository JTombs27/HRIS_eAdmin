//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for User Profiling and creation
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// JOSEPH M TOMBO JR (JMTJR)     10/17/2018      Code Creation
//**********************************************************************************
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using HRIS_Common;
using System.Web;
using System.Drawing;
using System.Text.RegularExpressions;

namespace HRIS_eAdmin.View.cUserProfile
{
    public partial class cUserProfile : System.Web.UI.Page
    {
        //********************************************************************
        //  BEGIN - JMTJR- 09/20/2018 - Data Place holder creation 
        //********************************************************************

        DataTable dtSource
        {
            get
            {
                if ((DataTable)ViewState["dtSource"] == null) return null;
                return (DataTable)ViewState["dtSource"];
            }
            set
            {
                ViewState["dtSource"] = value;
            }
        }

        DataTable dtSource2
        {
            get
            {
                if ((DataTable)ViewState["dtSource2"] == null) return null;
                return (DataTable)ViewState["dtSource2"];
            }
            set
            {
                ViewState["dtSource2"] = value;
            }
        }

        DataTable dtaccesspages
        {
            get
            {
                if ((DataTable)ViewState["dtaccesspages"] == null) return null;
                return (DataTable)ViewState["dtaccesspages"];
            }
            set
            {
                ViewState["dtaccesspages"] = value;
            }
        }

        DataTable dataListGrid
        {
            get
            {
                if ((DataTable)ViewState["dataListGrid"] == null) return null;
                return (DataTable)ViewState["dataListGrid"];
            }
            set
            {
                ViewState["dataListGrid"] = value;
            }
        }

        DataTable dataEmployeeCombolist
        {
            get
            {
                if ((DataTable)ViewState["dataEmployeeCombolist"] == null) return null;
                return (DataTable)ViewState["dataEmployeeCombolist"];
            }
            set
            {
                ViewState["dataEmployeeCombolist"] = value;
            }
        }

        DataTable dataDeparatments
        {
            get
            {
                if ((DataTable)ViewState["dataDeparatments"] == null) return null;
                return (DataTable)ViewState["dataDeparatments"];
            }
            set
            {
                ViewState["dataDeparatments"] = value;
            }
        }

       public DataTable modulelist
        {
            get
            {
                if ((DataTable)ViewState["modulelist"] == null) return null;
                return (DataTable)ViewState["modulelist"];
            }
            set
            {
                ViewState["modulelist"] = value;
            }
        }

        //********************************************************************
        //  BEGIN - AEC- 09/12/2018 - Public Variable used in Add/Edit Mode
        //********************************************************************

        CommonDB MyCmn = new CommonDB();

        public string logged_user_id = "";
        public string msg_password_validator = "";
        
        //********************************************************************
        //  BEGIN - AEC- 09/20/2018 - Page Load method
        //********************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] != null)
                {
                logged_user_id = Session["ea_user_id"].ToString();
                if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "user_id";
                    ViewState["SortOrder"] = "ASC";

                }
            }
            else
            {
                
                Response.Redirect("~/login.aspx");
            }

        }

        void Page_LoadComplete(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ViewState["page_allow_view"] = Master.allow_view;
                if (Master.allow_view == "1")
                {
                    ViewState["page_allow_add"] = 0;
                    ViewState["page_allow_delete"] = 0;
                    ViewState["page_allow_edit"] = 0;
                    ViewState["page_allow_edit_history"] = 0;
                    ViewState["page_allow_print"] = 0;
                }
                else
                {
                    ViewState["page_allow_add"] = Master.allow_add;
                    ViewState["page_allow_delete"] = Master.allow_delete;
                    ViewState["page_allow_edit"] = Master.allow_edit;
                    ViewState["page_allow_edit_history"] = Master.allow_edit_history;
                    ViewState["page_allow_print"] = Master.allow_print;
                }
            }
        }
        //********************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialiazed Page 
        //********************************************************************
        private void InitializePage()
        {
            Session["sortdirection"] = SortDirection.Ascending.ToString();
            Session["cUserProfile"] = "cUserProfile";

            RetrieveDepartments();
            RetrieveDataListGrid();
            RetrieveModuleList();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_usersprofile_tbl_list","par_user_id",logged_user_id,"par_department_code",ddl_department.SelectedValue);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveModuleList()
        {
            modulelist = MyCmn.RetrieveData("sp_modules_tbl_list");
            foreach (DataRow row in modulelist.Rows)
            {
                DataTable temp_dtroles = new DataTable();
                ListItem li = new ListItem("Select Role", "");
                switch (row["module_id"].ToString())
                {
                    case "0": {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list","par_module_id",0);
                            ddl_module_0.DataTextField = "role_name";
                            ddl_module_0.DataValueField = "role_id";
                            ddl_module_0.DataSource = temp_dtroles;
                            ddl_module_0.DataBind();
                            ddl_module_0.Items.Insert(0, li);
                            break;
                        }
                    case "1":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 1);
                            ddl_module_1.DataTextField = "role_name";
                            ddl_module_1.DataValueField = "role_id";
                            ddl_module_1.DataSource = temp_dtroles;
                            ddl_module_1.DataBind();
                            ddl_module_1.Items.Insert(0, li);
                            break;
                        }
                    case "2":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 2);
                            ddl_module_2.DataTextField = "role_name";
                            ddl_module_2.DataValueField = "role_id";
                            ddl_module_2.DataSource = temp_dtroles;
                            ddl_module_2.DataBind();
                            ddl_module_2.Items.Insert(0, li);
                            break;
                        }
                    case "3":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 3);
                            ddl_module_3.DataTextField = "role_name";
                            ddl_module_3.DataValueField = "role_id";
                            ddl_module_3.DataSource = temp_dtroles;
                            ddl_module_3.DataBind();
                            ddl_module_3.Items.Insert(0, li);
                            break;
                        }
                    case "4":
                        {
                            //temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 4);
                            //ddl_module_4.DataTextField = "role_name";
                            //ddl_module_4.DataValueField = "role_id";
                            //ddl_module_4.DataSource = temp_dtroles;
                            //ddl_module_4.DataBind();
                            //ddl_module_4.Items.Insert(0, li);
                            break;
                        }
                    case "5":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 5);
                            ddl_module_5.DataTextField = "role_name";
                            ddl_module_5.DataValueField = "role_id";
                            ddl_module_5.DataSource = temp_dtroles;
                            ddl_module_5.DataBind();
                            ddl_module_5.Items.Insert(0, li);
                            break;
                        }
                    case "6":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 6);
                            ddl_module_6.DataTextField = "role_name";
                            ddl_module_6.DataValueField = "role_id";
                            ddl_module_6.DataSource = temp_dtroles;
                            ddl_module_6.DataBind();
                            ddl_module_6.Items.Insert(0, li);
                            break;
                        }
                    case "7":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 7);
                            ddl_module_7.DataTextField = "role_name";
                            ddl_module_7.DataValueField = "role_id";
                            ddl_module_7.DataSource = temp_dtroles;
                            ddl_module_7.DataBind();
                            ddl_module_7.Items.Insert(0, li);
                            break;
                        }
                    case "8":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 8);
                            ddl_module_8.DataTextField = "role_name";
                            ddl_module_8.DataValueField = "role_id";
                            ddl_module_8.DataSource = temp_dtroles;
                            ddl_module_8.DataBind();
                            ddl_module_8.Items.Insert(0, li);
                            break;
                        }
                    case "9":
                        {
                            temp_dtroles = MyCmn.RetrieveData("sp_securityaccessroles_per_module_list", "par_module_id", 9);
                            ddl_module_9.DataTextField = "role_name";
                            ddl_module_9.DataValueField = "role_id";
                            ddl_module_9.DataSource = temp_dtroles;
                            ddl_module_9.DataBind();
                            ddl_module_9.Items.Insert(0, li);
                            break;
                        }

                }
            }
        }

        //*************************************************************************
        //  BEGIN - Jade- 10/04/18 - Populate Combo List from Employees with TA
        //*************************************************************************
        private void RetrieveBindingEmplNameList()
        {
            ddl_empl_name.Items.Clear();
            dataEmployeeCombolist = MyCmn.RetrieveData("sp_personnelnames_combolist6", "par_user_id", logged_user_id, "par_department_code", ddl_department.SelectedValue.ToString().Trim());

            ddl_empl_name.DataTextField = "employee_name";
            ddl_empl_name.DataValueField = "empl_id";
            ddl_empl_name.DataSource = dataEmployeeCombolist;
            ddl_empl_name.DataBind();
            ListItem li = new ListItem("Select Employee","");
            ddl_empl_name.Items.Insert(0, li);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDepartments()
        {
            dataDeparatments = MyCmn.RetrieveData("sp_departments_tbl_list", "par_include_history","N");
            ddl_department.DataValueField = "department_code";
            ddl_department.DataTextField = "department_name1";
            ddl_department.DataSource = dataDeparatments;
            ddl_department.DataBind();
            ListItem li = new ListItem("Select Department", "");
            ddl_department.Items.Insert(0, li);
        }

        //**********************************************************************************
        //  BEGIN - AEC- 10/07/2018 - Validate Dropdownlist for Employee Name
        //*********************************************************************************
        protected void ddl_empl_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddl_empl_name.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_empl_name");
            }
            else
            {
                FieldValidationColorChanged(false, "ddl_empl_name");
                tbx_user_id.Text = "U" + ddl_empl_name.SelectedValue;
            }
            ddl_empl_name.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add button to trigger add/edit page
        //*************************************************************************
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            tbx_empl_name.Visible = false;
            ddl_empl_name.Visible = true;
            ddl_empl_name.Enabled = true;
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();
            AddNewRow();
            RetrieveBindingEmplNameList();
            LabelAddEdit.Text = "<strong>Add New Record</strong>";
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD);;
            FieldValidationColorChanged(false, "ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_password.Text = "";
            tbx_user_id.Text = "";
            ddl_empl_name.SelectedIndex = -1;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["user_id"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("user_id", typeof(System.String));
            dtSource.Columns.Add("user_password", typeof(System.String));
            dtSource.Columns.Add("empl_id", typeof(System.String));
            dtSource.Columns.Add("allow_edit_history", typeof(System.String));
            dtSource.Columns.Add("status", typeof(System.String));
            dtSource.Columns.Add("change_password", typeof(System.String));
            dtSource.Columns.Add("locked_account", typeof(System.String));
            dtSource.Columns.Add("created_date", typeof(System.String));
            dtSource.Columns.Add("created_by", typeof(System.String));
            dtSource.Columns.Add("last_updated_date", typeof(System.String));
            dtSource.Columns.Add("last_updated_by", typeof(System.String));
            dtSource.Columns.Add("user_accesslevel", typeof(System.String));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialized datasource fields/columns
        //*************************************************************************
        private void IniTableUserRoles()
        {
            dtSource2 = new DataTable();
            dtSource2.Columns.Add("user_id", typeof(System.String));
            dtSource2.Columns.Add("role_id", typeof(System.String));
        }
        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRowforUserRoles()
        {
            DataRow nrow = dtSource2.NewRow();
            nrow["user_id"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource2.Rows.Add(nrow);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeysForUserRoles()
        {
            dtSource2.TableName = "userroles_tbl";
            dtSource2.Columns.Add("action", typeof(System.Int32));
            dtSource2.Columns.Add("retrieve", typeof(System.Boolean));
           
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "usersprofile_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "user_id" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        protected void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string user_id = commandArgs[0];
            string appttypedescr = commandArgs[1];

            deleteRec1.Text = "Are you sure to delete this User Acces with the User ID = (" + user_id.Trim() + ") - " + appttypedescr.Trim() + " ?";
            lnkBtnYes.CommandArgument = user_id;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string deleteExpression = "user_id = '" + svalues + "'";
            MyCmn.DeleteBackEndData("usersprofile_tbl", "WHERE " + deleteExpression);
            MyCmn.DeleteBackEndData("userroles_tbl", "WHERE " + deleteExpression);
            DataRow[] row2Delete = dataListGrid.Select(deleteExpression);
            dataListGrid.Rows.Remove(row2Delete[0]);
            dataListGrid.AcceptChanges();
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalDelete();", true);
            up_dataListGrid.Update();
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Edit Row selection that will trigger edit page 
        //**************************************************************************
        protected void editRow_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string editExpression = "user_id = '" + svalues + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);
           
            InitializeTable();
            AddPrimaryKeys();
            RetrieveBindingEmplNameList();
           
            ClearEntry();
            DataRow nrow = dtSource.NewRow();
            nrow["user_id"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;

            nrow["user_id"] = tbx_user_id.Text.ToString().Trim();
            nrow["user_password"] = row2Edit[0]["user_password"].ToString();
            nrow["empl_id"] = row2Edit[0]["empl_id"].ToString();
            nrow["allow_edit_history"] = row2Edit[0]["allow_edit_history"];
            nrow["status"] = row2Edit[0]["status"].ToString();
            nrow["change_password"] = row2Edit[0]["change_password"].ToString();
            nrow["locked_account"] = row2Edit[0]["locked_account"].ToString();
            nrow["created_date"] = row2Edit[0]["created_date"].ToString();
            nrow["created_by"] = row2Edit[0]["created_by"].ToString();
            nrow["last_updated_date"] = row2Edit[0]["last_updated_date"].ToString();
            nrow["last_updated_by"] = row2Edit[0]["last_updated_by"].ToString();
            nrow["user_accesslevel"] = row2Edit[0]["user_accesslevel"].ToString();

            dtSource.Rows.Add(nrow);

            tbx_user_id.Text = row2Edit[0]["user_id"].ToString();
            tbx_empl_name.Text = row2Edit[0]["employee_name"].ToString();
            tbx_empl_name.Visible = true;
            ddl_empl_name.Visible = false;
            ddl_empl_name.Enabled = false;
            ddl_department.SelectedValue = row2Edit[0]["department_code"].ToString();
            ddl_user_accesslevel.SelectedValue = row2Edit[0]["user_accesslevel"].ToString().Trim() != "" ? row2Edit[0]["user_accesslevel"].ToString().Trim():"1";
            tbx_password.Text = MyCmn.DecryptString(row2Edit[0]["user_password"].ToString(),MyCmn.CONST_WORDENCRYPTOR);
            chkbx_allow_edit_history.Checked = row2Edit[0]["allow_edit_history"].ToString() == "True" ? true : false;
            ddl_module_0.SelectedValue = row2Edit[0]["role_id0"].ToString();
            ddl_module_1.SelectedValue = row2Edit[0]["role_id1"].ToString();
            ddl_module_2.SelectedValue = row2Edit[0]["role_id2"].ToString();
            ddl_module_3.SelectedValue = row2Edit[0]["role_id3"].ToString();
            //ddl_module_4.SelectedValue = row2Edit[0]["role_id4"].ToString();
            ddl_module_5.SelectedValue = row2Edit[0]["role_id5"].ToString();
            ddl_module_6.SelectedValue = row2Edit[0]["role_id6"].ToString();
            ddl_module_7.SelectedValue = row2Edit[0]["role_id7"].ToString();
            ddl_module_8.SelectedValue = row2Edit[0]["role_id8"].ToString();
            ddl_module_9.SelectedValue = row2Edit[0]["role_id9"].ToString();

            LabelAddEdit.Text = "Edit Record: " + tbx_empl_name.Text.Trim();
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);

            FieldValidationColorChanged(false, "ALL");

            //getModule_pages();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change Field Sort mode  
        //**************************************************************************
        protected void gv_dataListGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            sortingDirection = GetCurrentSortDir();

            if (sortingDirection == MyCmn.CONST_SORTASC)
            {
                SortDirectionVal = SortDirection.Descending;
                sortingDirection = MyCmn.CONST_SORTDESC;
            }
            else
            {
                SortDirectionVal = SortDirection.Ascending;
                sortingDirection = MyCmn.CONST_SORTASC;
            }

            ViewState["SortField"] = e.SortExpression;
            ViewState["SortOrder"] = sortingDirection;

            MyCmn.Sort(gv_dataListGrid, dataListGrid, e.SortExpression, sortingDirection);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Get Grid current sort order 
        //**************************************************************************
        private string GetCurrentSortDir()
        {
            string sortingDirection = string.Empty;

            if (SortDirectionVal == SortDirection.Ascending)
            {
                sortingDirection = MyCmn.CONST_SORTASC;
            }
            else
            {
                sortingDirection = MyCmn.CONST_SORTDESC;
            }

            return sortingDirection;
        }
      
        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Save New Record/Edited Record to back end DB
        //**************************************************************************
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string saveRecord = ViewState["AddEdit_Mode"].ToString();
            string scriptInsertUpdate = string.Empty;

            if (IsDataValidated())
            {
                    if (saveRecord == MyCmn.CONST_ADD)
                    {
                        dtSource.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                        dtSource.Rows[0]["user_password"] = MyCmn.EncryptString(tbx_password.Text.ToString(),MyCmn.CONST_WORDENCRYPTOR);
                        dtSource.Rows[0]["empl_id"] = ddl_empl_name.SelectedValue.ToString().Trim();
                        dtSource.Rows[0]["allow_edit_history"] = chkbx_allow_edit_history.Checked == true ? "1":"0";
                        dtSource.Rows[0]["status"] ="1";
                        dtSource.Rows[0]["change_password"] ="1";
                        dtSource.Rows[0]["locked_account"] ="0";
                        dtSource.Rows[0]["created_date"] = DateTime.Now.ToString("yyyy-MM-dd");
                        dtSource.Rows[0]["created_by"] = logged_user_id;
                        dtSource.Rows[0]["last_updated_date"] = DateTime.Now.ToString("yyyy-MM-dd");
                        dtSource.Rows[0]["last_updated_by"] = logged_user_id;
                        dtSource.Rows[0]["user_accesslevel"] = ddl_user_accesslevel.SelectedValue.ToString().Trim();

                        scriptInsertUpdate = MyCmn.get_insertscript(dtSource);
                    }
                    else if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        dtSource.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                        dtSource.Rows[0]["user_password"] = MyCmn.EncryptString(tbx_password.Text.ToString(), MyCmn.CONST_WORDENCRYPTOR);
                        dtSource.Rows[0]["status"] = "1";
                        dtSource.Rows[0]["allow_edit_history"] = chkbx_allow_edit_history.Checked == true ? "1" : "0";
                        dtSource.Rows[0]["last_updated_date"] = DateTime.Now.ToString("yyyy-MM-dd");
                        dtSource.Rows[0]["last_updated_by"] = logged_user_id;
                        dtSource.Rows[0]["user_accesslevel"] = ddl_user_accesslevel.SelectedValue.ToString().Trim();
                    scriptInsertUpdate = MyCmn.updatescript(dtSource);
                    }

                if (saveRecord == MyCmn.CONST_ADD || saveRecord == MyCmn.CONST_EDIT)
                {

                    
                        if (scriptInsertUpdate == string.Empty) return;
                        string msg = MyCmn.insertdata(scriptInsertUpdate);
                        if (msg == "") return;
                        if (msg.Substring(0, 1) == "X") return;

                        string deleteExpression = "user_id = '"+tbx_user_id.Text.ToString().Trim() +"'";
                        MyCmn.DeleteBackEndData("userroles_tbl", "WHERE " + deleteExpression);
                        for (int x = 0; x < modulelist.Rows.Count; x++)
                        {
                            scriptInsertUpdate = "";
                            IniTableUserRoles();
                            AddPrimaryKeysForUserRoles();
                            AddNewRowforUserRoles();

                            if ((modulelist.Rows[x]["module_id"].ToString() == "0") && (ddl_module_0.SelectedValue.ToString().Trim() !=""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_0.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            if ((modulelist.Rows[x]["module_id"].ToString() == "1") && (ddl_module_1.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_1.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            if ((modulelist.Rows[x]["module_id"].ToString() == "2") && (ddl_module_2.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_2.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            if ((modulelist.Rows[x]["module_id"].ToString() == "3") && (ddl_module_3.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_3.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            //if ((x == 4) && (ddl_module_4.SelectedValue.ToString().Trim() != ""))
                            //{
                            //    dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                            //    dtSource2.Rows[0]["role_id"] = ddl_module_4.SelectedValue;
                            //    scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            //}

                            if ((modulelist.Rows[x]["module_id"].ToString() == "5") && (ddl_module_5.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_5.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            if ((modulelist.Rows[x]["module_id"].ToString() == "6") && (ddl_module_6.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_6.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            if ((modulelist.Rows[x]["module_id"].ToString() == "7") && (ddl_module_7.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_7.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            if ((modulelist.Rows[x]["module_id"].ToString() == "8") && (ddl_module_8.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_8.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }

                            if ((modulelist.Rows[x]["module_id"].ToString() == "9") && (ddl_module_9.SelectedValue.ToString().Trim() != ""))
                            {
                                dtSource2.Rows[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                                dtSource2.Rows[0]["role_id"] = ddl_module_9.SelectedValue;
                                scriptInsertUpdate = MyCmn.get_insertscript(dtSource2);
                            }
                            MyCmn.insertdata(scriptInsertUpdate);
                        }


                    if (saveRecord == MyCmn.CONST_ADD)
                        {
                            DataRow nrow = dataListGrid.NewRow();
                            nrow["user_id"] = tbx_user_id.Text.ToString().Trim();
                            nrow["user_password"] = MyCmn.EncryptString(tbx_password.Text.ToString(), MyCmn.CONST_WORDENCRYPTOR);
                            nrow["empl_id"] = ddl_empl_name.SelectedValue.ToString();
                            nrow["change_password"] = true;
                            nrow["employee_name"] = ddl_empl_name.SelectedItem.Text.ToString();
                            nrow["allow_edit_history"] = chkbx_allow_edit_history.Checked == true ? true :false;
                            nrow["role_id0"] = ddl_module_0.SelectedValue;
                            nrow["role_id1"] = ddl_module_1.SelectedValue;
                            nrow["role_id2"] = ddl_module_2.SelectedValue;
                            nrow["role_id3"] = ddl_module_3.SelectedValue;
                            //nrow["role_id4"] = ddl_module_4.SelectedValue;
                            nrow["role_id5"] = ddl_module_5.SelectedValue;
                            nrow["role_id6"] = ddl_module_6.SelectedValue;
                            nrow["role_id7"] = ddl_module_7.SelectedValue;
                            nrow["role_id8"] = ddl_module_8.SelectedValue;
                            nrow["role_id9"] = ddl_module_9.SelectedValue;
                            nrow["department_code"] = ddl_department.SelectedValue;
                            nrow["user_accesslevel"] = ddl_user_accesslevel.SelectedValue;

                            dataListGrid.Rows.Add(nrow);
                            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
                            gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);

                            SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                        }
                        if (saveRecord == MyCmn.CONST_EDIT)
                        {
                            string editExpression = "user_id = '" + tbx_user_id.Text.ToString() + "'";
                            DataRow[] row2Edit = dataListGrid.Select(editExpression);
                            row2Edit[0]["user_id"] = tbx_user_id.Text.ToString().Trim();
                            row2Edit[0]["user_password"] = MyCmn.EncryptString(tbx_password.Text.ToString(), MyCmn.CONST_WORDENCRYPTOR);
                            row2Edit[0]["employee_name"] = tbx_empl_name.Text.ToString();
                            row2Edit[0]["allow_edit_history"] = chkbx_allow_edit_history.Checked == true ?  true : false;
                            row2Edit[0]["role_id0"] = ddl_module_0.SelectedValue;
                            row2Edit[0]["role_id1"] = ddl_module_1.SelectedValue;
                            row2Edit[0]["role_id2"] = ddl_module_2.SelectedValue;
                            row2Edit[0]["role_id3"] = ddl_module_3.SelectedValue;
                            //row2Edit[0]["role_id4"] = ddl_module_4.SelectedValue;
                            row2Edit[0]["role_id5"] = ddl_module_5.SelectedValue;
                            row2Edit[0]["role_id6"] = ddl_module_6.SelectedValue;
                            row2Edit[0]["role_id7"] = ddl_module_7.SelectedValue;
                            row2Edit[0]["role_id8"] = ddl_module_8.SelectedValue;
                            row2Edit[0]["role_id9"] = ddl_module_9.SelectedValue;
                            row2Edit[0]["department_code"] = ddl_department.SelectedValue;
                            row2Edit[0]["user_accesslevel"] = ddl_user_accesslevel.SelectedValue;
                            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
                            SaveAddEdit.Text = MyCmn.CONST_EDITREC;
                        }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    up_dataListGrid.Update();
                }
                ViewState.Remove("AddEdit_Mode");
                show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
            }
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/12/2018 - GridView Change Page Number
        //**************************************************************************
        protected void gridviewbind_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_dataListGrid.PageIndex = e.NewPageIndex;
            MyCmn.Sort(gv_dataListGrid, dataListGrid, ViewState["SortField"].ToString(), ViewState["SortOrder"].ToString());
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - AEC- 09/12/2018 - Change on Page Size (no. of row per page) on Gridview  
        //*********************************************************************************
        protected void DropDownListID_TextChanged(object sender, EventArgs e)
        {
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }


        //**********************************************************************************
        //  BEGIN - AEC- 10/07/2018 - Validate Dropdownlist for Departments 
        //*********************************************************************************
        protected void ddl_department_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_department.SelectedValue.ToString() != "")
            {
                RetrieveBindingEmplNameList();
                RetrieveDataListGrid();
                btn_add.Visible = true;
            }
            else {
                btn_add.Visible = false;
            }
        }

        //**********************************************************************************
        //  BEGIN - AEC- 09/12/2018 - Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        protected void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "user_id LIKE '%" + txt_search.Text.Trim() + "%' OR employee_name LIKE '%" + txt_search.Text.Trim() + "%' OR empl_id LIKE '%" + txt_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("user_id", typeof(System.String));
            dtSource1.Columns.Add("user_password", typeof(System.String));
            dtSource1.Columns.Add("employee_name", typeof(System.String));
            dtSource1.Columns.Add("empl_id", typeof(System.String));
            dtSource1.Columns.Add("allow_edit_history", typeof(System.String));
            dtSource1.Columns.Add("role_id0", typeof(System.String));
            dtSource1.Columns.Add("role_id1", typeof(System.String));
            dtSource1.Columns.Add("role_id2", typeof(System.String));
            dtSource1.Columns.Add("role_id3", typeof(System.String));
            dtSource1.Columns.Add("role_id4", typeof(System.String));
            dtSource1.Columns.Add("role_id5", typeof(System.String));
            dtSource1.Columns.Add("role_id6", typeof(System.String));
            dtSource1.Columns.Add("role_id7", typeof(System.String));
            dtSource1.Columns.Add("role_id8", typeof(System.String));
            dtSource1.Columns.Add("role_id9", typeof(System.String));
            dtSource1.Columns.Add("department_code", typeof(System.String));
            dtSource1.Columns.Add("status", typeof(System.String));
            dtSource1.Columns.Add("locked_account", typeof(System.String));
            dtSource1.Columns.Add("change_password", typeof(System.String));
            dtSource1.Columns.Add("created_date", typeof(System.String));
            dtSource1.Columns.Add("created_by", typeof(System.String));
            dtSource1.Columns.Add("last_updated_date", typeof(System.String));
            dtSource1.Columns.Add("last_updated_by", typeof(System.String));
            dtSource1.Columns.Add("user_accesslevel", typeof(System.String));

            DataRow[] rows = dataListGrid.Select(searchExpression);
            dtSource1.Clear();
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    dtSource1.ImportRow(row);
                }
            }

            gv_dataListGrid.DataSource = dtSource1;
            gv_dataListGrid.DataBind();
            up_dataListGrid.Update();
            txt_search.Attributes["onfocus"] = "var value = this.value; this.value = ''; this.value = value; onfocus = null;";
            txt_search.Focus();
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Define Property for Sort Direction  
        //*************************************************************************
        public SortDirection SortDirectionVal
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }

            set
            {
                ViewState["dirState"] = value;
            }
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            if (ddl_empl_name.SelectedValue.ToString().Trim() == "" && ViewState["AddEdit_Mode"].ToString() == MyCmn.CONST_ADD)
            {
                FieldValidationColorChanged(true, "ddl_empl_name");
                ddl_empl_name.Focus();
                validatedSaved = false;
            }
            else if (tbx_password.Text.ToString().Trim() == "")
            {
                FieldValidationColorChanged(true, "tbx_password");
                tbx_password.Focus();
                validatedSaved = false;
            }
            else {
                if (IsValidPassword(tbx_password.Text.ToString().Trim()))
                {
                    FieldValidationColorChanged(false, "ALL");

                }
                else
                {
                    FieldValidationColorChanged(true, "invalid-password");
                    validatedSaved = false;
                }
                    
            }
            return validatedSaved;
        }

        //*************************************************************************
        //  BEGIN Joseph M. Tombo Jr- 10/09/2018 - Password Validation
        //*************************************************************************
        public bool IsValidPassword(string inputed_password)
        {
            bool is_valid = true;
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasSpecialChar = new Regex(@"[$!@*_.]+");

            if (hasNumber.IsMatch(inputed_password) && hasUpperChar.IsMatch(inputed_password) && hasMinimum8Chars.IsMatch(inputed_password) && hasSpecialChar.IsMatch(inputed_password))
            {
                is_valid = true;
            }
            else
            {
                msg_password_validator = "Password required at least 8 characters, 1 uppercase, and 1 special character. ";
                is_valid = false;
            }
            return is_valid;
        }
        //**********************************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        protected void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            if (pMode)
                switch (pObjectName)
                {
                    case "ddl_empl_name":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            ddl_empl_name.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_password":
                        {
                            LblRequired6.Text = MyCmn.CONST_RQDFLD;
                            tbx_password.BorderColor = Color.Red;
                            break;
                        }
                    case "invalid-password":
                        {
                            LblRequired6.Text = msg_password_validator;
                            tbx_password.CssClass = "form-control form-control-sm required";
                            break;
                        }
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "ddl_empl_name":
                        {
                            if (LblRequired1.Text != "")
                            {
                                LblRequired1.Text = "";
                                ddl_empl_name.BorderColor = Color.LightGray;
                            }
                            break;
                        }
                    case "tbx_password":
                        {
                            LblRequired6.Text = "";
                            tbx_password.BorderColor = Color.LightGray;
                            break;
                        }
                    case "ALL":
                        {
                            LblRequired1.Text = "";
                            LblRequired6.Text = "";
                            ddl_empl_name.BorderColor = Color.LightGray;
                            tbx_password.BorderColor = Color.LightGray;
                            break;
                        }

                }
            }
        }

        protected void ddl_user_accesslevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}