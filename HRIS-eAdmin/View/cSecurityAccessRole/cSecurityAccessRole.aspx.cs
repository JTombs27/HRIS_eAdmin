//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for scurity access roles
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// JOSEPH M TOMBO JR (JMTJR)     10/16/2018      Code Creation
//**********************************************************************************
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using HRIS_Common;
using System.Web;
using System.Drawing;

namespace HRIS_eAdmin.View.cSecurityAccessRole
{
    public partial class cSecurityAccessRole : System.Web.UI.Page
    {
        //********************************************************************
        //  BEGIN - JMTJR- 09/20/2018 - Data Place holder creation 
        //********************************************************************

        static string par_module_id;

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

        DataTable dataListGridPages
        {
            get
            {
                if ((DataTable)ViewState["dataListGridPages"] == null) return null;
                return (DataTable)ViewState["dataListGridPages"];
            }
            set
            {
                ViewState["dataListGridPages"] = value;
            }
        }

        DataTable modulelist
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

        //********************************************************************
        //  BEGIN - AEC- 09/20/2018 - Page Load method
        //********************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] != null)
            {
                if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "role_id";
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
            RetrieveModuleList();
            Session["sortdirection"] = SortDirection.Ascending.ToString();
            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_securityaccessroles_tbl_list");
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveModuleList()
        {
            modulelist = MyCmn.RetrieveData("sp_modules_tbl_list");
            ddl_modules.DataValueField = "module_id";
            ddl_modules.DataTextField = "module_name";
            ddl_modules.DataSource = modulelist;
            ddl_modules.DataBind();
            ListItem li = new ListItem("Select Module", "");
            ddl_modules.Items.Insert(0, li);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add button to trigger add/edit page
        //*************************************************************************
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            par_module_id = "NEW";
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();
            AddNewRow();
            getModule_pages();
            tbx_role_name.Enabled = true;
            tbx_role_name.ReadOnly = false;
            ddl_modules.Enabled = true;
            LabelAddEdit.Text = "<strong>Add New Record</strong>";
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD);;
            FieldValidationColorChanged(false, "ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void getModule_pages()
        {
            if (ddl_modules.SelectedValue.ToString().Trim() != "") {
                dataListGridPages = MyCmn.RetrieveData("sp_securityaccessroles_with_pages_list", "par_module_id", ddl_modules.SelectedValue, "par_role_id", par_module_id);
            }
            else {
                dataListGridPages = MyCmn.RetrieveData("sp_securityaccessroles_with_pages_list", "par_module_id", ddl_modules.SelectedValue, "par_role_id", "");
            }
           
            CommonCode.GridViewBind(ref this.gv_module_pages, dataListGridPages);
          
        }
        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_role_id.Text = "";
            tbx_role_name.Text = "";
            ddl_modules.SelectedIndex = 0;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["role_id"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "000";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["role_id"].ToString();
            }

            int lastCodeInt = int.Parse(lastCode) + 1;
            string nextCode = lastCodeInt.ToString();
            nextCode = nextCode.PadLeft(3, '0');
            tbx_role_id.Text = nextCode;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("role_id", typeof(System.String));
            dtSource.Columns.Add("role_name", typeof(System.String));
            dtSource.Columns.Add("module_id", typeof(System.String));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitTableAccessPages()
        {
            dtSource2 = new DataTable();
            dtSource2.Columns.Add("role_id", typeof(System.String));
            dtSource2.Columns.Add("url_name", typeof(System.String));
            dtSource2.Columns.Add("allow_add", typeof(System.String));
            dtSource2.Columns.Add("allow_edit", typeof(System.String));
            dtSource2.Columns.Add("allow_delete", typeof(System.String));
            dtSource2.Columns.Add("allow_print", typeof(System.String));
            dtSource2.Columns.Add("allow_view", typeof(System.String));
        }
        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeysAccessPages()
        {
            dtSource2.TableName = "securityaccessrolepages_tbl";
            dtSource2.Columns.Add("action", typeof(System.Int32));
            dtSource2.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "role_id","url_name" };
            dtSource2 = MyCmn.AddPrimaryKeys(dtSource2, col);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "securityaccessroles_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "role_id" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        protected void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string appttype = commandArgs[0];
            string appttypedescr = commandArgs[1];

            deleteRec1.Text = "Are you sure to delete this Roles with the Role Id = (" + appttype.Trim() + ") - " + appttypedescr.Trim() + " ?";
            lnkBtnYes.CommandArgument = appttype;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string deleteExpression = "role_id = '" + svalues + "'";

            MyCmn.DeleteBackEndData("securityaccessroles_tbl", "WHERE " + deleteExpression);
            MyCmn.DeleteBackEndData("securityaccessrolepages_tbl", "WHERE " + deleteExpression);

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
            par_module_id = svalues;
            string editExpression = "role_id = '" + svalues + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();

            InitializeTable();
            AddPrimaryKeys();
            
            DataRow nrow = dtSource.NewRow();
            nrow["role_id"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);

            tbx_role_id.Text = svalues;
            tbx_role_name.Text = row2Edit[0]["role_name"].ToString();
            ddl_modules.SelectedValue = row2Edit[0]["module_id"].ToString();

            tbx_role_id.ReadOnly = true;
            ddl_modules.Enabled = false;
            LabelAddEdit.Text = "Edit Record: " + tbx_role_name.Text.Trim();
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);

            FieldValidationColorChanged(false, "ALL");
            getModule_pages();
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
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change Field Sort mode  
        //**************************************************************************
        protected void gv_module_page_Sorting(object sender, GridViewSortEventArgs e)
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
            MyCmn.Sort(gv_module_pages, dataListGridPages, e.SortExpression, sortingDirection);
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
        protected void re_initialize(string saveRecord) {
            InitTableAccessPages();
            AddPrimaryKeysAccessPages();
            DataRow nrow2 = dtSource2.NewRow();
            if (saveRecord == MyCmn.CONST_ADD)
            {
                nrow2["role_id"] = string.Empty;
                nrow2["url_name"] = string.Empty;
                nrow2["action"] = 1;
                nrow2["retrieve"] = false;
            }
            else if (saveRecord == MyCmn.CONST_EDIT)
            {
                nrow2["role_id"] = string.Empty;
                nrow2["url_name"] = string.Empty;
                nrow2["action"] = 2;
                nrow2["retrieve"] = true;
            }

            dtSource2.Rows.Add(nrow2);
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
                    dtSource.Rows[0]["role_id"] = tbx_role_id.Text.ToString();
                    dtSource.Rows[0]["role_name"] = tbx_role_name.Text.ToString();
                    dtSource.Rows[0]["module_id"] = ddl_modules.Text.ToString();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);
                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["role_id"] = tbx_role_id.Text.ToString();
                    dtSource.Rows[0]["role_name"] = tbx_role_name.Text.ToString();
                    dtSource.Rows[0]["module_id"] = ddl_modules.Text.ToString();
                    scriptInsertUpdate = MyCmn.updatescript(dtSource);
                }

                if (saveRecord == MyCmn.CONST_ADD || saveRecord == MyCmn.CONST_EDIT)
                {
                    
                    //**************************************************
                    //CounterX will count the inserted accessrole pages
                    //****************************************************
                    int counterX = 0;

                    foreach (GridViewRow row in gv_module_pages.Rows)
                    {

                        string scriptInsertUpdateforpages = "";
                        re_initialize(saveRecord);

                        CheckBox temp_chkbx_allow_add = row.FindControl("chkbx_allow_add") as CheckBox;
                        CheckBox temp_chkbx_allow_edit = row.FindControl("chkbx_allow_edit") as CheckBox;
                        CheckBox temp_chkbx_allow_delete = row.FindControl("chkbx_allow_delete") as CheckBox;
                        CheckBox temp_chkbx_allow_print = row.FindControl("chkbx_allow_print") as CheckBox;
                        CheckBox temp_chkbx_view_only = row.FindControl("chkbx_view_only") as CheckBox;
                        Label hidden_url = row.FindControl("tbx_hidden") as Label;
                        if (temp_chkbx_view_only.Checked && (scriptInsertUpdate != string.Empty))
                            {
                                dtSource2.Rows[0]["role_id"] = tbx_role_id.Text.ToString();
                                dtSource2.Rows[0]["url_name"] = hidden_url.Text.ToString();
                                dtSource2.Rows[0]["allow_add"] = "0";
                                dtSource2.Rows[0]["allow_edit"] = "0";
                                dtSource2.Rows[0]["allow_delete"] = "0";
                                dtSource2.Rows[0]["allow_print"] = "0";
                                dtSource2.Rows[0]["allow_view"] = "1";
                                if (saveRecord == MyCmn.CONST_ADD) {
                                    scriptInsertUpdateforpages = MyCmn.get_insertscript(dtSource2);
                                }
                                else if(saveRecord == MyCmn.CONST_EDIT)
                                {
                                    scriptInsertUpdateforpages = MyCmn.updatescript(dtSource2);
                                }
                            string msg2 = "";
                                   msg2 = MyCmn.insertdata(scriptInsertUpdateforpages);
                                if(msg2 == "" && saveRecord == MyCmn.CONST_EDIT)
                                {
                                    re_initialize(MyCmn.CONST_ADD);
                                        dtSource2.Rows[0]["role_id"] = tbx_role_id.Text.ToString();
                                        dtSource2.Rows[0]["url_name"] = hidden_url.Text.ToString();
                                        dtSource2.Rows[0]["allow_add"] = "0";
                                        dtSource2.Rows[0]["allow_edit"] = "0";
                                        dtSource2.Rows[0]["allow_delete"] = "0";
                                        dtSource2.Rows[0]["allow_print"] = "0";
                                        dtSource2.Rows[0]["allow_view"] = "1";
                                    scriptInsertUpdateforpages = MyCmn.get_insertscript(dtSource2);
                                    MyCmn.insertdata(scriptInsertUpdateforpages);
                                }
                            counterX++;
                                    
                            }
                        else if ((temp_chkbx_allow_add.Checked || 
                                temp_chkbx_allow_edit.Checked || 
                                temp_chkbx_allow_print.Checked ||
                                temp_chkbx_allow_delete.Checked) && 
                                (scriptInsertUpdate != string.Empty))
                             {
                                dtSource2.Rows[0]["role_id"] = tbx_role_id.Text.ToString();
                                dtSource2.Rows[0]["url_name"] = hidden_url.Text.ToString();
                                dtSource2.Rows[0]["allow_add"] = temp_chkbx_allow_add.Checked == true ? "1" : "0";
                                dtSource2.Rows[0]["allow_edit"] = temp_chkbx_allow_edit.Checked == true ? "1" : "0";
                                dtSource2.Rows[0]["allow_delete"] = temp_chkbx_allow_delete.Checked == true ? "1" : "0";
                                dtSource2.Rows[0]["allow_print"] = temp_chkbx_allow_print.Checked == true ? "1" : "0";
                                dtSource2.Rows[0]["allow_view"] = temp_chkbx_view_only.Checked == true ? "1" : "0";
                                if (saveRecord == MyCmn.CONST_ADD)
                                {
                                    scriptInsertUpdateforpages = MyCmn.get_insertscript(dtSource2);
                                }
                                else if (saveRecord == MyCmn.CONST_EDIT)
                                {
                                    scriptInsertUpdateforpages = MyCmn.updatescript(dtSource2);
                                }
                                string msg2 = "";
                                       msg2 = MyCmn.insertdata(scriptInsertUpdateforpages);
                                if (msg2 == "" && saveRecord == MyCmn.CONST_EDIT)
                                {
                                    re_initialize(MyCmn.CONST_ADD);
                                        dtSource2.Rows[0]["role_id"] = tbx_role_id.Text.ToString();
                                        dtSource2.Rows[0]["url_name"] = hidden_url.Text.ToString();
                                        dtSource2.Rows[0]["allow_add"] = temp_chkbx_allow_add.Checked == true ? "1" : "0";
                                        dtSource2.Rows[0]["allow_edit"] = temp_chkbx_allow_edit.Checked == true ? "1" : "0";
                                        dtSource2.Rows[0]["allow_delete"] = temp_chkbx_allow_delete.Checked == true ? "1" : "0";
                                        dtSource2.Rows[0]["allow_print"] = temp_chkbx_allow_print.Checked == true ? "1" : "0";
                                        dtSource2.Rows[0]["allow_view"] = temp_chkbx_view_only.Checked == true ? "1" : "0";
                                    scriptInsertUpdateforpages = MyCmn.get_insertscript(dtSource2);
                                    MyCmn.insertdata(scriptInsertUpdateforpages);
                                }
                            counterX++;
                        }
                    }

                    if (counterX > 0)
                    {
                        if (scriptInsertUpdate == string.Empty) return;
                        string msg = MyCmn.insertdata(scriptInsertUpdate);
                        if (msg == "") return;
                        if (msg.Substring(0, 1) == "X") return;
                    }
                    else
                    {
                        at_least.Text = "Plss Check atleast 1 checkbox below.";
                        
                        return;
                    }
                   

                    if (saveRecord == MyCmn.CONST_ADD)
                    {
                        DataRow nrow = dataListGrid.NewRow();
                        nrow["role_id"] = tbx_role_id.Text.ToString();
                        nrow["role_name"] = tbx_role_name.Text.ToString();
                        nrow["module_id"] = ddl_modules.SelectedValue.ToString();
                        nrow["module_name"] = ddl_modules.SelectedItem.Text.ToString();
                        dataListGrid.Rows.Add(nrow);
                        CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "role_id = '" + tbx_role_id.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["role_id"] = tbx_role_id.Text.ToString();
                        row2Edit[0]["role_name"] = tbx_role_name.Text.ToString();
                        row2Edit[0]["module_id"] = ddl_modules.SelectedValue.ToString();
                        row2Edit[0]["module_name"] = ddl_modules.SelectedItem.Text.ToString();
                        CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
                        SaveAddEdit.Text = MyCmn.CONST_EDITREC;
                    }

                    at_least.Text = "";
                    ViewState.Remove("AddEdit_Mode");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    up_dataListGrid.Update();
                }
            }
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/12/2018 - GridView Change Page Number
        //**************************************************************************
        protected void gridviewbind_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_dataListGrid.PageIndex = e.NewPageIndex;
            MyCmn.Sort(gv_dataListGrid, dataListGrid, ViewState["SortField"].ToString(), ViewState["SortOrder"].ToString());
        }

        //**********************************************************************************
        //  BEGIN - AEC- 09/12/2018 - Change on Page Size (no. of row per page) on Gridview  
        //*********************************************************************************
        protected void DropDownListID_TextChanged(object sender, EventArgs e)
        {
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
        }

        //**********************************************************************************
        //  BEGIN - AEC- 09/12/2018 - Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        protected void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "role_id LIKE '%" + tbx_search.Text.Trim() + "%' OR role_name LIKE '%" + tbx_search.Text.Trim() + "%' OR module_name LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("role_id", typeof(System.String));
            dtSource1.Columns.Add("role_name", typeof(System.String));
            dtSource1.Columns.Add("module_name", typeof(System.String));
            dtSource1.Columns.Add("module_id", typeof(System.String));

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
            tbx_search.Attributes["onfocus"] = "var value = this.value; this.value = ''; this.value = value; onfocus = null;";
            tbx_search.Focus();
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

            if (tbx_role_name.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_role_name");
                tbx_role_name.Focus();
                validatedSaved = false;
            }
            else if (ddl_modules.SelectedValue.ToString().Trim() == "")
            {
                FieldValidationColorChanged(true, "ddl_modules");
                ddl_modules.Focus();
                validatedSaved = false;
            }
            return validatedSaved;
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Check if Object already contains value  
        //*************************************************************************
        protected void CheckInputValue(object sender, EventArgs e)
        {
            TextBox TextBox1 = (TextBox)sender;
            string checkValue = TextBox1.Text;
            string checkName = TextBox1.ID;

            if (checkValue.ToString() != "")
            {
                FieldValidationColorChanged(false, checkName);
            }
            TextBox1.Attributes["onfocus"] = "var value = this.value; this.value = ''; this.value = value; onfocus = null;";
            TextBox1.Focus();
        }

        //**********************************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        protected void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            if (pMode)
                switch (pObjectName)
                {
                    case "tbx_role_name":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            tbx_role_name.BorderColor = Color.Red;
                            break;
                        }
                    case "ddl_modules":
                        {
                            LblRequired2.Text = MyCmn.CONST_RQDFLD;
                            ddl_modules.BorderColor = Color.Red;
                            break;
                        }
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "tbx_role_name":
                        {
                            if (LblRequired1.Text != "")
                            {
                                LblRequired1.Text = "";
                                tbx_role_name.BorderColor = Color.LightGray;
                            }
                            break;
                        }
                    case "ddl_modules":
                        {
                            LblRequired2.Text = "";
                            ddl_modules.BorderColor = Color.LightGray;
                            break;
                        }
                    case "ALL":
                        {
                            LblRequired1.Text = "";
                            LblRequired2.Text = "";
                            tbx_role_name.BorderColor = Color.LightGray;
                            ddl_modules.BorderColor = Color.LightGray;
                            break;
                        }

                }
            }
        }

        protected void ddl_modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            getModule_pages();
        }

        protected void gv_module_pages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexvalue = gv_module_pages.SelectedIndex;
            bool svalues = (gv_module_pages.Rows[indexvalue].FindControl("chkbx_view_only") as CheckBox).Checked;

            if (svalues)
            {
                (gv_module_pages.Rows[indexvalue].FindControl("chkbx_allow_add") as CheckBox).Checked = true;
            }
            else
            {
                (gv_module_pages.Rows[indexvalue].FindControl("chkbx_allow_add") as CheckBox).Checked = false;
            }
        }

        protected void gv_module_pages_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            int indexvalue = gv_module_pages.SelectedIndex;
            bool svalues = (gv_module_pages.Rows[indexvalue].FindControl("chkbx_view_only") as CheckBox).Checked;

            if (svalues)
            {
                (gv_module_pages.Rows[indexvalue].FindControl("chkbx_allow_add") as CheckBox).Checked = true;
            }
            else
            {
                (gv_module_pages.Rows[indexvalue].FindControl("chkbx_allow_add") as CheckBox).Checked = false;
            }
        }

    }
}