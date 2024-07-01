//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Transactions Reviewer and Approver
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// ARIEL CABUNGCAL (AEC)      10/04/2018      Code Creation
//**********************************************************************************
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using HRIS_Common;
using System.Web;
using System.Drawing;

namespace HRIS_eAdmin.View
{
    public partial class cTransactionsApprover : System.Web.UI.Page
    {

        //********************************************************************
        //  BEGIN - AEC- 09/20/2018 - Data Place holder creation 
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

        DataTable dtEmpNameList
        {
            get
            {
                if ((DataTable)ViewState["dtEmpNameList"] == null) return null;
                return (DataTable)ViewState["dtEmpNameList"];
            }
            set
            {
                ViewState["dtEmpNameList"] = value;
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
 //           if (HttpContext.Current.Session["ea_user_id"] != null)
                if (Session["ea_user_id"] != null)
                {
                    if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "empl_id";
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
            Session["cTransactionsApprover"] = "cTransactionsApprover";
            RetrieveDataListGrid();
            RetrieveBindingEmplNameList();
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";

            RetrieveBindingTransDesc();
            RetrieveDataListGrid();

            RetrieveBindingDep();
            RetrieveBindingSubDep();
            RetrieveBindingDivision();
            RetrieveBindingSection();

            btn_add.Visible = false;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_transactionsapprover_tbl_list", "par_transaction_code", ddl_transaction_descr.SelectedValue.Trim());
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add button to trigger add/edit page
        //*************************************************************************
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();
            AddNewRow();

            txtb_trans_descr.Text = ddl_transaction_descr.SelectedItem.Text.ToString(); 

            LabelAddEdit.Text = "Add New Record";
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD);;
            FieldValidationColorChanged(false,"ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            ddl_empl_name.SelectedIndex = 0;
            ddl_workflow_authority.SelectedIndex = 0;
            
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["transaction_code"] = string.Empty;
            nrow["empl_id"] = string.Empty;
            nrow["workflow_authority"] = string.Empty;
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
            dtSource.Columns.Add("transaction_code", typeof(System.String));
            dtSource.Columns.Add("empl_id", typeof(System.String));
            dtSource.Columns.Add("workflow_authority", typeof(System.String));
            dtSource.Columns.Add("department_code", typeof(System.String));
            dtSource.Columns.Add("subdepartment_code", typeof(System.String));
            dtSource.Columns.Add("division_code", typeof(System.String));
            dtSource.Columns.Add("section_code", typeof(System.String));
            dtSource.Columns.Add("with_self_service_approval", typeof(System.Boolean));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "transactionsapprover_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "transaction_code", "empl_id" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        protected void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string v_transaction_code = commandArgs[0];
            string v_empl_id = commandArgs[1];
            string v_transaction_descr = commandArgs[1];
            
            deleteRec1.Text = "Are you sure to delete this Record = (" + v_transaction_code.Trim() + ") - " + v_transaction_descr.Trim() + " ?";
            lnkBtnYes.CommandArgument = v_transaction_code + ", " + v_empl_id;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string v_transaction_code = commandArgs[0];
            string v_empl_id = commandArgs[1];
            string deleteExpression = "transaction_code = '" + v_transaction_code.Trim() + "' AND empl_id = '" + v_empl_id.Trim() + "'";

            MyCmn.DeleteBackEndData("transactionsapprover_tbl", "WHERE " + deleteExpression);

            DataRow[] row2Delete = dataListGrid.Select(deleteExpression);
            dataListGrid.Rows.Remove(row2Delete[0]);
            dataListGrid.AcceptChanges();
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalDelete();", true);
            up_dataListGrid.Update();
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Edit Row selection that will trigger edit page 
        //**************************************************************************
        protected void editRow_Command(object sender, CommandEventArgs e)
        {
            
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string v_transaction_code = commandArgs[0];
            string v_empl_id = commandArgs[1];
            string v_transaction_descr = commandArgs[2];
            string editExpression = "transaction_code = '" + v_transaction_code + "' AND empl_id = '" + v_empl_id.Trim() + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["transaction_code"] = string.Empty;
            nrow["empl_id"] = string.Empty;
            nrow["workflow_authority"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);

            RetrieveBindingTransDesc();

            ddl_transaction_descr.SelectedValue = row2Edit[0]["transaction_code"].ToString();
            ddl_empl_name.SelectedValue = row2Edit[0]["empl_id"].ToString();
            ddl_workflow_authority.SelectedValue = row2Edit[0]["workflow_authority"].ToString();
            ddl_empl_name.Visible = false;

            ddl_dep.SelectedValue = row2Edit[0]["department_code"].ToString();
            RetrieveBindingSubDep();
            ddl_subdep.SelectedValue = row2Edit[0]["subdepartment_code"].ToString();
            RetrieveBindingDivision();
            ddl_division.SelectedValue = row2Edit[0]["division_code"].ToString();
            RetrieveBindingSection();
            ddl_section.SelectedValue = row2Edit[0]["section_code"].ToString();
            if (row2Edit[0]["with_self_service_approval"].ToString().ToUpper() == "TRUE")
                chkbox_with_ss.Checked = true;
            else chkbox_with_ss.Checked = false;

            txtb_trans_descr.Text = v_transaction_descr;

            txtb_empl_name.Visible = true;
            
            txtb_empl_name.Text = row2Edit[0]["employee_name"].ToString();
            ddl_workflow_authority.Focus();
            LabelAddEdit.Text = "Edit Record: " + ddl_empl_name.Text.Trim();
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);

            FieldValidationColorChanged(false, "ALL");

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
                    dtSource.Rows[0]["empl_id"] = ddl_empl_name.Text.ToString();
                    dtSource.Rows[0]["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
                    dtSource.Rows[0]["workflow_authority"] = ddl_workflow_authority.SelectedValue.ToString();

                    dtSource.Rows[0]["department_code"] = ddl_dep.SelectedValue.ToString();
                    dtSource.Rows[0]["subdepartment_code"] = ddl_subdep.SelectedValue.ToString();
                    dtSource.Rows[0]["division_code"] = ddl_division.SelectedValue.ToString();
                    dtSource.Rows[0]["section_code"] = ddl_section.SelectedValue.ToString();

                    if (chkbox_with_ss.Checked == true)
                        dtSource.Rows[0]["with_self_service_approval"] = 1;
                    else dtSource.Rows[0]["with_self_service_approval"] = 0;

                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["empl_id"] = ddl_empl_name.Text.ToString();
                    dtSource.Rows[0]["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
                    dtSource.Rows[0]["workflow_authority"] = ddl_workflow_authority.SelectedValue.ToString();

                    dtSource.Rows[0]["department_code"] = ddl_dep.SelectedValue.ToString();
                    dtSource.Rows[0]["subdepartment_code"] = ddl_subdep.SelectedValue.ToString();
                    dtSource.Rows[0]["division_code"] = ddl_division.SelectedValue.ToString();
                    dtSource.Rows[0]["section_code"] = ddl_section.SelectedValue.ToString();

                    if (chkbox_with_ss.Checked == true)
                        dtSource.Rows[0]["with_self_service_approval"] = 1;
                    else dtSource.Rows[0]["with_self_service_approval"] = 0;

                    scriptInsertUpdate = MyCmn.updatescript(dtSource);
                }

                if (saveRecord == MyCmn.CONST_ADD || saveRecord == MyCmn.CONST_EDIT)
                {
                    if (scriptInsertUpdate == string.Empty) return;
                    string msg = MyCmn.insertdata(scriptInsertUpdate);
                    if (msg == "") return;
                    if (msg.Substring(0, 1) == "X") return;


                    if (saveRecord == MyCmn.CONST_ADD)
                    {
                        DataRow nrow = dataListGrid.NewRow();
                        nrow["empl_id"] = ddl_empl_name.SelectedValue.ToString();
                        nrow["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
                        nrow["workflow_authority"] = ddl_workflow_authority.SelectedValue.ToString();
                        nrow["employee_name"] = ddl_empl_name.SelectedItem.Text.ToString();
                        nrow["transaction_descr"] = ddl_transaction_descr.SelectedItem.Text.ToString();
                        nrow["workflow_authority_descr"] = ddl_workflow_authority.SelectedItem.Text.ToString();

                        nrow["department_code"] = ddl_dep.SelectedValue.ToString();
                        nrow["subdepartment_code"] = ddl_subdep.SelectedValue.ToString();
                        nrow["division_code"] = ddl_division.SelectedValue.ToString();
                        nrow["section_code"] = ddl_section.SelectedValue.ToString();
                        nrow["with_self_service_approval"] = dtSource.Rows[0]["with_self_service_approval"].ToString();

                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);
                        //gv_dataListGrid.SelectRow(gv_dataListGrid.Rows.Count - 1);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "empl_id = '" + ddl_empl_name.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
                        row2Edit[0]["workflow_authority"] = ddl_workflow_authority.SelectedValue.ToString();
                        row2Edit[0]["employee_name"] = txtb_empl_name.Text;
                        row2Edit[0]["workflow_authority_descr"] = ddl_workflow_authority.SelectedItem.Text.ToString();

                        row2Edit[0]["department_code"] = ddl_dep.SelectedValue.ToString();
                        row2Edit[0]["subdepartment_code"] = ddl_subdep.SelectedValue.ToString();
                        row2Edit[0]["division_code"] = ddl_division.SelectedValue.ToString();
                        row2Edit[0]["section_code"] = ddl_section.SelectedValue.ToString();
                        row2Edit[0]["with_self_service_approval"] = dtSource.Rows[0]["with_self_service_approval"].ToString();

                        CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
                        SaveAddEdit.Text = MyCmn.CONST_EDITREC;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
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
        //  BEGIN - AEC- 09/12/2018 - Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        protected void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "transaction_descr LIKE '%" + tbx_search.Text.Trim() + "%' OR employee_name LIKE '%" + tbx_search.Text.Trim() + "%'  OR workflow_authority_descr LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("transaction_descr", typeof(System.String));
            dtSource1.Columns.Add("employee_name", typeof(System.String));
            dtSource1.Columns.Add("workflow_authority_descr", typeof(System.String));
            dtSource1.Columns.Add("empl_id", typeof(System.String));
            dtSource1.Columns.Add("transaction_code", typeof(System.String));
            dtSource1.Columns.Add("workflow_authority", typeof(System.String));

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

        //*****************************************************************************
        //  BEGIN - AEC- 09/09/2018 - Populate Combo list from Transactions Ref Table
        //*****************************************************************************
        private void RetrieveBindingTransDesc()
        {
            ddl_transaction_descr.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_transactionsref_tbl_list");

            ddl_transaction_descr.DataSource = dt;
            ddl_transaction_descr.DataTextField = "transaction_descr";
            ddl_transaction_descr.DataValueField = "transaction_code";
            ddl_transaction_descr.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_transaction_descr.Items.Insert(0, li);
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            if (ddl_transaction_descr.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_transaction_descr");
                ddl_transaction_descr.Focus();
                validatedSaved = false;
            }
            else if (ddl_empl_name.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_empl_name");
                ddl_empl_name.Focus();
                validatedSaved = false;
            }
            else if (ddl_workflow_authority.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_workflow_authority");
                ddl_workflow_authority.Focus();
                validatedSaved = false;
            }
            return validatedSaved;
        }

        //**********************************************************************************
        //  BEGIN - AEC- 10/04/2018 - Validate Dropdownlist for Transactions
        //*********************************************************************************
        protected void ddl_transaction_descr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_transaction_descr.SelectedValue.ToString() != "")
            {
                RetrieveDataListGrid();
                btn_add.Visible = true;
            }
        }

        //**********************************************************************************
        //  BEGIN - AEC- 10/04/2018 - Validate Dropdownlist for Employees 
        //*********************************************************************************
        protected void ddl_empl_name_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddl_empl_name.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_empl_name");
            }
            else
            {
                FieldValidationColorChanged(false, "ddl_empl_name");
            }
            ddl_empl_name.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 10/04/2018 - Populate Combo List from Employees 
        //*************************************************************************
        private void RetrieveBindingEmplNameList()
        {
            ddl_empl_name.Items.Clear();
            dtEmpNameList = MyCmn.RetrieveData("sp_personnelnames_combolist");

            ddl_empl_name.DataSource = dtEmpNameList;
            ddl_empl_name.DataTextField = "employee_name";
            ddl_empl_name.DataValueField = "empl_id";
            ddl_empl_name.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_empl_name.Items.Insert(0, li);

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
                            ddl_empl_name.CssClass = "form-control-sm required";
                            break;
                        }
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "ddl_empl_name":
                        {
                            LblRequired1.Text = "";
                            ddl_empl_name.CssClass = "form-control-sm";
                            break;
                        }
                    case "ALL":
                        {
                            LblRequired1.Text = "";
                            ddl_transaction_descr.BorderColor = Color.LightGray;
                            ddl_empl_name.BorderColor = Color.LightGray;
                            break;
                        }

                }
            }
        }

        protected void ddl_dep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_dep.SelectedValue.ToString() == "")
            {
            }
            else
            {
                RetrieveBindingDivision();
                RetrieveBindingSection();
            }
            ddl_dep.Focus();
        }

        protected void ddl_subdep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_subdep.SelectedValue.ToString() == "")
            {
            }
            else
            {
                RetrieveBindingDivision();
                RetrieveBindingSection();
            }
            ddl_subdep.Focus();
        }
        protected void ddl_division_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_division.SelectedValue.ToString() == "")
            {
            }
            else
            {
                RetrieveBindingSection();
            }
            ddl_division.Focus();
        }

        //*****************************************************************************
        //  BEGIN - AEC- 11/16/2018 - Populate Combo list from Departments Table
        //*****************************************************************************
        private void RetrieveBindingDep()
        {
            ddl_dep.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_departments_tbl_list", "par_include_history", "N");

            ddl_dep.DataSource = dt;
            ddl_dep.DataValueField = "department_code";
            ddl_dep.DataTextField = "department_name1";
            ddl_dep.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_dep.Items.Insert(0, li);
        }

        //*****************************************************************************
        //  BEGIN - AEC- 11/16/2018 - Populate Combo list from Sub-Departments Table
        //*****************************************************************************
        private void RetrieveBindingSubDep()
        {
            ddl_subdep.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_subdepartments_tbl_list");

            ddl_subdep.DataSource = dt;
            ddl_subdep.DataValueField = "subdepartment_code";
            ddl_subdep.DataTextField = "subdepartment_short_name";
            ddl_subdep.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_subdep.Items.Insert(0, li);
        }

        //*****************************************************************************
        //  BEGIN - AEC- 11/16/2018 - Populate Combo list from Division Table
        //*****************************************************************************
        private void RetrieveBindingDivision()
        {
            ddl_division.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_divisions_tbl_combolist", "par_department_code", ddl_dep.SelectedValue.ToString(),"par_subdepartment_code", ddl_subdep.SelectedValue.ToString());

            ddl_division.DataSource = dt;
            ddl_division.DataValueField = "division_code";
            ddl_division.DataTextField = "division_name1";
            ddl_division.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_division.Items.Insert(0, li);
        }

        //*****************************************************************************
        //  BEGIN - AEC- 11/16/2018 - Populate Combo list from Section Table
        //*****************************************************************************
        private void RetrieveBindingSection()
        {
            ddl_section.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_sections_tbl_combolist", "par_department_code", ddl_dep.SelectedValue.ToString().Trim(), "par_subdepartment_code", ddl_subdep.SelectedValue.ToString(), "par_division_code", ddl_division.SelectedValue.ToString().Trim());

            ddl_section.DataSource = dt;
            ddl_section.DataValueField = "section_code";
            ddl_section.DataTextField = "section_name1";
            ddl_section.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_section.Items.Insert(0, li);
        }

        // End of Code

    }
}