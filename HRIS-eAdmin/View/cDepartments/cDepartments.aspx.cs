//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Department Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// ARIEL CABUNGCAL (AEC)      10/23/2018      Code Creation
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
    public partial class cDepartments : System.Web.UI.Page
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

        
        //********************************************************************
        //  BEGIN - AEC- 09/12/2018 - Public Variable used in Add/Edit Mode
        //********************************************************************

        public string var_include_history;

        CommonDB MyCmn = new CommonDB();
        
        //********************************************************************
        //  BEGIN - AEC- 09/20/2018 - Page Load method
        //********************************************************************
        public void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] != null)
            {
                if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "department_code";
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
            var_include_history = "N";
            Session["sortdirection"] = SortDirection.Ascending.ToString();
            Session["cDepartments"] = "cDepartments";
            RetrieveBindingNames();
            RetrieveDataListGrid();
            RetriveFunction();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_departments_tbl_list", "par_include_history", var_include_history);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }
        private void RetriveFunction()
        {
            ddl_function_code.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_functions_tbl_list");

            ddl_function_code.DataSource = dt;
            ddl_function_code.DataTextField = "function_name";
            ddl_function_code.DataValueField = "function_code";
            ddl_function_code.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_function_code.Items.Insert(0, li);
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

            LabelAddEdit.Text = "Add New Record";
            FieldValidationColorChanged(false,"ALL");
            ViewState["AddEdit_Mode"] = MyCmn.CONST_ADD;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_effective_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tbx_department_code.Text = "";
            tbx_department_short_name.Text = "";
            tbx_department_name1.Text = "";
            tbx_department_name2.Text = "";
            tbx_designation_head1.Text = "";
            tbx_designation_head2.Text = "";
            ddl_depHead_name.SelectedIndex = 0;
            ddl_print_group.SelectedIndex = 0;
            ddl_function_code.SelectedIndex = 0;
            tbx_department_code.ReadOnly = false;
            tbx_department_code.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["effective_date"] = DateTime.Now.ToString("yyyy-MM-dd");
            nrow["department_code"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "00";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["department_code"].ToString();
            }

            int lastCodeInt = int.Parse(lastCode) + 1;
            string nextCode = lastCodeInt.ToString();
            nextCode = nextCode.PadLeft(2, '0');

            tbx_department_code.Text = nextCode;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("effective_date", typeof(System.String));
            dtSource.Columns.Add("department_code", typeof(System.String));
            dtSource.Columns.Add("department_short_name", typeof(System.String));
            dtSource.Columns.Add("department_name1", typeof(System.String));
            dtSource.Columns.Add("department_name2", typeof(System.String));
            dtSource.Columns.Add("sort_order_dept", typeof(System.Int16));
            dtSource.Columns.Add("print_group", typeof(System.Int16));
            dtSource.Columns.Add("empl_id", typeof(System.String));
            dtSource.Columns.Add("designation_head1", typeof(System.String));
            dtSource.Columns.Add("designation_head2", typeof(System.String));
            dtSource.Columns.Add("function_code", typeof(System.String));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "departments_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "effective_date", "department_code" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        public void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string dept_cd1 = commandArgs[0];
            string eff_dt1 = commandArgs[1];

            deleteRec1.Text = "Are you sure to delete this Record ?";
            lnkBtnYes.CommandArgument = dept_cd1 + ", " + eff_dt1;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        public void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string dept_cd2 = commandArgs[0];
            string eff_dt2 = commandArgs[1];

            string deleteExpression = "department_code = '" + dept_cd2 + "' AND CONVERT(date, effective_date) = CONVERT(date,'" + eff_dt2.ToString().Trim() + "')";
            string deleteExpression1 = "department_code = '" + dept_cd2 + "' AND effective_date = '" + eff_dt2.ToString().Trim() + "'";

            MyCmn.DeleteBackEndData("departments_tbl", "WHERE " + deleteExpression);

            DataRow[] row2Delete = dataListGrid.Select(deleteExpression1);
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
        public void editRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string dept_cd2 = commandArgs[0];
            string eff_dt2 = commandArgs[1];
            string editExpression = "department_code = '" + dept_cd2 + "' AND effective_date = '" + eff_dt2.ToString().Trim() + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();

            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["effective_date"] = eff_dt2;
            nrow["department_code"] = dept_cd2;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);
            FieldValidationColorChanged(false, "ALL");

            tbx_department_code.Text = dept_cd2;
            tbx_effective_date.Text = eff_dt2;
            tbx_department_short_name.Text = row2Edit[0]["department_short_name"].ToString();
            tbx_department_name1.Text = row2Edit[0]["department_name1"].ToString();
            tbx_department_name2.Text = row2Edit[0]["department_name2"].ToString();
            tbx_sort_order_dept.Text = row2Edit[0]["sort_order_dept"].ToString();
            ddl_print_group.SelectedValue = row2Edit[0]["print_group"].ToString();
            ddl_depHead_name.SelectedValue = row2Edit[0]["empl_id"].ToString();
            tbx_designation_head1.Text = row2Edit[0]["designation_head1"].ToString();
            tbx_designation_head2.Text = row2Edit[0]["designation_head2"].ToString();
            ddl_function_code.SelectedValue = row2Edit[0]["function_code"].ToString();
  
            tbx_department_code.ReadOnly = true;
            tbx_department_short_name.Focus();
            LabelAddEdit.Text = "Edit Record: " + tbx_department_short_name.Text.Trim();

            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);
           
            FieldValidationColorChanged(false,"ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change Field Sort mode  
        //**************************************************************************
        public void gv_dataListGrid_Sorting(object sender, GridViewSortEventArgs e)
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
        public void btnSave_Click(object sender, EventArgs e)
        {
            string saveRecord = ViewState["AddEdit_Mode"].ToString();
            string scriptInsertUpdate = string.Empty;

            if (IsDataValidated() )
            { 
                if (saveRecord == MyCmn.CONST_ADD)
                {
                    dtSource.Rows[0]["effective_date"] = tbx_effective_date.Text.ToString();
                    dtSource.Rows[0]["department_code"] = tbx_department_code.Text.ToString();
                    dtSource.Rows[0]["department_short_name"] = tbx_department_short_name.Text.ToString();
                    dtSource.Rows[0]["department_name1"] = tbx_department_name1.Text.ToString();
                    dtSource.Rows[0]["department_name2"] = tbx_department_name2.Text.ToString();
                    dtSource.Rows[0]["sort_order_dept"] = tbx_sort_order_dept.Text.ToString();
                    dtSource.Rows[0]["print_group"] = ddl_print_group.SelectedValue.ToString();
                    dtSource.Rows[0]["empl_id"] = ddl_depHead_name.SelectedValue.ToString();
                    dtSource.Rows[0]["designation_head1"] = tbx_designation_head1.Text.ToString();
                    dtSource.Rows[0]["designation_head2"] = tbx_designation_head2.Text.ToString();
                    dtSource.Rows[0]["function_code"] = ddl_function_code.SelectedValue.ToString().Trim();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["effective_date"] = tbx_effective_date.Text.ToString();
                    dtSource.Rows[0]["department_code"] = tbx_department_code.Text.ToString();
                    dtSource.Rows[0]["department_short_name"] = tbx_department_short_name.Text.ToString();
                    dtSource.Rows[0]["department_name1"] = tbx_department_name1.Text.ToString();
                    dtSource.Rows[0]["department_name2"] = tbx_department_name2.Text.ToString();
                    dtSource.Rows[0]["sort_order_dept"] = tbx_sort_order_dept.Text.ToString();
                    dtSource.Rows[0]["print_group"] = ddl_print_group.SelectedValue.ToString();
                    dtSource.Rows[0]["empl_id"] = ddl_depHead_name.SelectedValue.ToString();
                    dtSource.Rows[0]["designation_head1"] = tbx_designation_head1.Text.ToString();
                    dtSource.Rows[0]["designation_head2"] = tbx_designation_head2.Text.ToString();
                    dtSource.Rows[0]["function_code"] = ddl_function_code.SelectedValue.ToString().Trim();
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
                        nrow["effective_date"] = tbx_effective_date.Text.ToString();
                        nrow["department_code"] = tbx_department_code.Text.ToString();
                        nrow["department_short_name"] = tbx_department_short_name.Text.ToString();
                        nrow["department_name1"] = tbx_department_name1.Text.ToString();
                        nrow["department_name2"] = tbx_department_name2.Text.ToString();
                        nrow["sort_order_dept"] = tbx_sort_order_dept.Text.ToString();
                        nrow["print_group"] = ddl_print_group.SelectedValue.ToString();
                        nrow["empl_id"] = ddl_depHead_name.SelectedValue.ToString();
                        nrow["designation_head1"] = tbx_designation_head1.Text.ToString();
                        nrow["designation_head2"] = tbx_designation_head2.Text.ToString();
                        nrow["employee_name_format1"] = ddl_depHead_name.SelectedItem.Text.ToString();
                        nrow["function_code"] = ddl_function_code.SelectedValue.ToString().Trim();
                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "department_code = '" + tbx_department_code.Text.ToString() + "' AND effective_date = '" + tbx_effective_date.Text.ToString().Trim() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["department_short_name"] = tbx_department_short_name.Text.ToString();
                        row2Edit[0]["department_name1"] = tbx_department_name1.Text.ToString();
                        row2Edit[0]["department_name2"] = tbx_department_name2.Text.ToString();
                        row2Edit[0]["sort_order_dept"] = tbx_sort_order_dept.Text.ToString();
                        row2Edit[0]["print_group"] = ddl_print_group.SelectedValue.ToString();
                        row2Edit[0]["empl_id"] = ddl_depHead_name.SelectedValue.ToString();
                        row2Edit[0]["designation_head1"] = tbx_designation_head1.Text.ToString();
                        row2Edit[0]["designation_head2"] = tbx_designation_head2.Text.ToString();
                        row2Edit[0]["employee_name_format1"] = ddl_depHead_name.SelectedItem.Text.ToString();
                        row2Edit[0]["function_code"] = ddl_function_code.SelectedValue.ToString().Trim();
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
    public void gridviewbind_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_dataListGrid.PageIndex = e.NewPageIndex;
            MyCmn.Sort(gv_dataListGrid, dataListGrid, ViewState["SortField"].ToString(), ViewState["SortOrder"].ToString());
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - AEC- 09/12/2018 - Change on Page Size (no. of row per page) on Gridview  
        //*********************************************************************************
        public void DropDownListID_TextChanged(object sender, EventArgs e)
        {
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - AEC- 09/12/2018 - Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        public void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "department_code LIKE '%" + tbx_search.Text.Trim() + "%' OR department_name1 LIKE '%" + tbx_search.Text.Trim() + "%' OR employee_name_format1 LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("effective_date", typeof(System.String));
            dtSource1.Columns.Add("department_code", typeof(System.String));
            dtSource1.Columns.Add("department_short_name", typeof(System.String));
            dtSource1.Columns.Add("department_name1", typeof(System.String));
            dtSource1.Columns.Add("department_name2", typeof(System.String));
            dtSource1.Columns.Add("sort_order_dept", typeof(System.Int16));
            dtSource1.Columns.Add("print_group", typeof(System.Int16));
            dtSource1.Columns.Add("empl_id", typeof(System.String));
            dtSource1.Columns.Add("designation_head1", typeof(System.String));
            dtSource1.Columns.Add("designation_head2", typeof(System.String));
            dtSource1.Columns.Add("employee_name_format1", typeof(System.String));
            dtSource1.Columns.Add("function_code", typeof(System.String));

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

        //**********************************************************************************
        //  BEGIN - AEC- 09/12/2018 - Validate Dropdownlist for Salary Grade 
        //*********************************************************************************
        public void ddl_depHead_name_SelectedIndexChanged(object sender, EventArgs e)
        {
 
            if (ddl_depHead_name.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_depHead_name");
            }
            else
            {
                FieldValidationColorChanged(false, "ddl_depHead_name");
            }
            ddl_depHead_name.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/09/2018 - Populate Combo list from Salary Grade Table
        //*************************************************************************
        private void RetrieveBindingNames()
        {
            ddl_depHead_name.Items.Clear();
            DataTable dtEmpList = MyCmn.RetrieveData("sp_personnelnames_department_combolist", "par_user_id", Session["ea_user_id"].ToString().Trim());

            ddl_depHead_name.DataSource = dtEmpList;
            ddl_depHead_name.DataTextField = "employee_name";
            ddl_depHead_name.DataValueField = "empl_id";
            ddl_depHead_name.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_depHead_name.Items.Insert(0, li);
        }

        //**********************************************************************************
        //  BEGIN - Ariel Cabungcal- 10/24/2018 - Set Include History Variable
        //*********************************************************************************
        public void chkIncludeHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIncludeHistory.Checked)
            {
                var_include_history = "Y";
            }
            else
            {
                var_include_history = "N";
            }
            RetrieveDataListGrid();
        }
        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            FieldValidationColorChanged(false, "ALL");

            if (tbx_department_short_name.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_department_short_name");
                tbx_department_short_name.Focus();
                validatedSaved = false;
            }
            else if (tbx_department_name1.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_department_name1");
                tbx_department_name1.Focus();
                validatedSaved = false;
            }
            else if (tbx_sort_order_dept.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_sort_order_dept");
                tbx_sort_order_dept.Focus();
                validatedSaved = false;
            }
            else if (CommonCode.isCheckNumber(tbx_sort_order_dept)==false)
            {
                FieldValidationColorChanged(true, "tbx_sort_order_dept",MyCmn.CONST_INVALID_NUMERIC);
                tbx_sort_order_dept.Focus();
                validatedSaved = false;
            }
            else if (ddl_print_group.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_print_group");
                ddl_print_group.Focus();
                validatedSaved = false;
            }
            else if (ddl_depHead_name.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_depHead_name");
                ddl_depHead_name.Focus();
                validatedSaved = false;
            }
            else if (tbx_designation_head1.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_designation_head1");
                tbx_designation_head1.Focus();
                validatedSaved = false;
            }
            return validatedSaved;
        }

        //**********************************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        public void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            FieldValidationColorChanged_Actual(pMode, pObjectName, MyCmn.CONST_RQDFLD);
        }

        public void FieldValidationColorChanged(bool pMode, string pObjectName, string err_msg)
        {
            FieldValidationColorChanged_Actual(pMode, pObjectName, err_msg);

        }

        //**********************************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        public void FieldValidationColorChanged_Actual(bool pMode, string pObjectName,string err_msg )
       {
            if (pMode)
                switch (pObjectName)
                {
                    case "tbx_department_short_name":
                        {
                            LblRequired0.Text = err_msg;
                            tbx_department_short_name.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_department_name1":
                        {
                            LblRequired1.Text = err_msg;
                            tbx_department_name1.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_sort_order_dept":
                        {
                            LblRequired2.Text = err_msg;
                            tbx_sort_order_dept.BorderColor = Color.Red;
                            break;
                        }
                    case "ddl_print_group":
                        {
                            LblRequired3.Text = err_msg;
                            ddl_print_group.BorderColor = Color.Red;
                            break;
                        }
                    case "ddl_depHead_name":
                        {
                            LblRequired4.Text = err_msg;
                            ddl_depHead_name.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_designation_head1":
                        {
                            LblRequired5.Text = err_msg;
                            tbx_designation_head1.BorderColor = Color.Red;
                            break;
                        }
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "tbx_department_short_name":
                        {
                            LblRequired0.Text = "";
                            tbx_department_short_name.BorderColor = Color.LightGray;
                            break;
                        }
                    case "tbx_department_name1":
                        {
                            LblRequired1.Text = "";
                            tbx_department_name1.BorderColor = Color.LightGray;
                            break;
                        }
                    case "tbx_sort_order_dept":
                        {
                            LblRequired1.Text = "";
                            tbx_sort_order_dept.BorderColor = Color.LightGray;
                            break;
                        }
                    case "ddl_print_group":
                        {
                            LblRequired3.Text = "";
                            ddl_print_group.BorderColor = Color.LightGray;
                            break;
                        }
                    case "ddl_depHead_name":
                        {
                            LblRequired4.Text = "";
                            ddl_depHead_name.BorderColor = Color.LightGray;
                            break;
                        }
                    case "tbx_designation_head1":
                        {
                            LblRequired5.Text = "";
                            tbx_designation_head1.BorderColor = Color.LightGray;
                            break;
                        }

                    case "ALL":
                        {
                            LblRequired0.Text = "";
                            LblRequired1.Text = "";
                            LblRequired2.Text = "";
                            LblRequired3.Text = "";
                            LblRequired4.Text = "";
                            LblRequired5.Text = "";
                            tbx_department_short_name.BorderColor = Color.LightGray;
                            tbx_department_name1.BorderColor = Color.LightGray;
                            tbx_sort_order_dept.BorderColor = Color.LightGray;
                            ddl_print_group.BorderColor = Color.LightGray;
                            tbx_designation_head1.BorderColor = Color.LightGray;
                            ddl_depHead_name.BorderColor = Color.LightGray;
                            break;
                        }

                }
            }
        }
    }
}