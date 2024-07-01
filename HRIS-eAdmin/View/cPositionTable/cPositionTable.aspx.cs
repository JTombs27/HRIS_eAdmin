//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Position Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// ARIEL CABUNGCAL (AEC)      09/20/2018      Code Creation
//**********************************************************************************
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using HRIS_Common;
using System.Drawing;

namespace HRIS_eAdmin.View
{
    public partial class cPositionTable : System.Web.UI.Page
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
                    ViewState["SortField"] = "position_code";
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
            Session["cPositionTable"] = "cPositionTable";

            RetrieveBindingSalaryGrade();
            RetrieveBindingEmpType();

            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_positions_tbl_list");
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

            LabelAddEdit.Text = "Add New Record";
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD);;
            FieldValidationColorChanged(false, "ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_position_code.Text = "";
            tbx_position_short_title.Text = "";
            tbx_position_long_title.Text = "";
            tbx_position_title1.Text = "";
            tbx_position_title2.Text = "";

            ddl_salary_grade.SelectedIndex = 0;
            ddl_csc_level.SelectedIndex = 0;
            ddl_employment_type.SelectedIndex = 0;

            tbx_position_code.ReadOnly = false;
            tbx_position_code.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["position_code"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "0000";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["position_code"].ToString();
            }

            int lastCodeInt = int.Parse(lastCode) + 1;
            string nextCode = lastCodeInt.ToString();
            nextCode = nextCode.PadLeft(4, '0');

            tbx_position_code.Text = nextCode;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("position_code", typeof(System.String));
            dtSource.Columns.Add("position_short_title", typeof(System.String));
            dtSource.Columns.Add("position_long_title", typeof(System.String));
            dtSource.Columns.Add("position_title1", typeof(System.String));
            dtSource.Columns.Add("position_title2", typeof(System.String));
            dtSource.Columns.Add("salary_grade", typeof(System.String));
            dtSource.Columns.Add("csc_level", typeof(System.String));
            dtSource.Columns.Add("employment_type", typeof(System.String));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "positions_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "position_code" };
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

            deleteRec1.Text = "Are you sure to delete this Position = (" + appttype.Trim() + ") - " + appttypedescr.Trim() + " ?";
            lnkBtnYes.CommandArgument = appttype;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string deleteExpression = "position_code = '" + svalues + "'";

            MyCmn.DeleteBackEndData("positions_tbl", "WHERE " + deleteExpression);

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
            string svalues = e.CommandArgument.ToString();
            string editExpression = "position_code = '" + svalues + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();

            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["position_code"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);

            RetrieveBindingSalaryGrade();

            tbx_position_code.Text = svalues;
            tbx_position_short_title.Text = row2Edit[0]["position_short_title"].ToString();
            tbx_position_long_title.Text = row2Edit[0]["position_long_title"].ToString();
            tbx_position_title1.Text = row2Edit[0]["position_title1"].ToString();
            tbx_position_title2.Text = row2Edit[0]["position_title2"].ToString();
            ddl_salary_grade.SelectedValue = row2Edit[0]["salary_grade"].ToString();
            ddl_csc_level.SelectedValue = row2Edit[0]["csc_level"].ToString();
            if (row2Edit[0]["employment_type"].ToString() == string.Empty)
                ddl_employment_type.SelectedIndex = 0;
            else
                ddl_employment_type.SelectedValue = row2Edit[0]["employment_type"].ToString();

            tbx_position_code.ReadOnly = true;
            tbx_position_short_title.Focus();
            LabelAddEdit.Text = "Edit Record: " + tbx_position_short_title.Text.Trim();
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
                    dtSource.Rows[0]["position_code"] = tbx_position_code.Text.ToString();
                    dtSource.Rows[0]["position_short_title"] = tbx_position_short_title.Text.ToString();
                    dtSource.Rows[0]["position_long_title"] = tbx_position_long_title.Text.ToString();
                    dtSource.Rows[0]["position_title1"] = tbx_position_title1.Text.ToString();
                    dtSource.Rows[0]["position_title2"] = tbx_position_title2.Text.ToString();
                    dtSource.Rows[0]["salary_grade"] = ddl_salary_grade.SelectedValue.ToString();
                    dtSource.Rows[0]["csc_level"] = ddl_csc_level.SelectedValue.ToString();
                    dtSource.Rows[0]["employment_type"] = ddl_employment_type.SelectedValue.ToString();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["position_code"] = tbx_position_code.Text.ToString();
                    dtSource.Rows[0]["position_short_title"] = tbx_position_short_title.Text.ToString();
                    dtSource.Rows[0]["position_long_title"] = tbx_position_long_title.Text.ToString();
                    dtSource.Rows[0]["position_title1"] = tbx_position_title1.Text.ToString();
                    dtSource.Rows[0]["position_title2"] = tbx_position_title2.Text.ToString();
                    dtSource.Rows[0]["salary_grade"] = ddl_salary_grade.SelectedValue.ToString();
                    dtSource.Rows[0]["csc_level"] = ddl_csc_level.SelectedValue.ToString();
                    dtSource.Rows[0]["employment_type"] = ddl_employment_type.SelectedValue.ToString();
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
                        nrow["position_code"] = tbx_position_code.Text.ToString();
                        nrow["position_short_title"] = tbx_position_short_title.Text.ToString();
                        nrow["position_long_title"] = tbx_position_long_title.Text.ToString();
                        nrow["position_title1"] = tbx_position_title1.Text.ToString();
                        nrow["position_title2"] = tbx_position_title2.Text.ToString();
                        nrow["salary_grade"] = ddl_salary_grade.SelectedValue.ToString();
                        nrow["csc_level"] = ddl_csc_level.SelectedValue.ToString();
                        nrow["employment_type"] = ddl_employment_type.SelectedValue.ToString();

                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);
                        //gv_dataListGrid.SelectRow(gv_dataListGrid.Rows.Count - 1);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "position_code = '" + tbx_position_code.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["position_short_title"] = tbx_position_short_title.Text.ToString();
                        row2Edit[0]["position_long_title"] = tbx_position_long_title.Text.ToString();
                        row2Edit[0]["position_title1"] = tbx_position_title1.Text.ToString();
                        row2Edit[0]["position_title2"] = tbx_position_title2.Text.ToString();
                        row2Edit[0]["salary_grade"] = ddl_salary_grade.SelectedValue.ToString();
                        row2Edit[0]["csc_level"] = ddl_csc_level.SelectedValue.ToString();
                        row2Edit[0]["employment_type"] = ddl_employment_type.SelectedValue.ToString();
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
            string searchExpression = "position_code LIKE '%" + tbx_search.Text.Trim() + "%' OR position_long_title LIKE '%" + tbx_search.Text.Trim() + "%' OR position_short_title LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("position_code", typeof(System.String));
            dtSource1.Columns.Add("position_long_title", typeof(System.String));
            dtSource1.Columns.Add("position_short_title", typeof(System.String));
            dtSource1.Columns.Add("employmenttype_description", typeof(System.String));

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
        protected void ddl_salary_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddl_salary_grade.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_salary_grade");
            }
            else
            {
                FieldValidationColorChanged(false, "ddl_salary_grade");
            }
            ddl_salary_grade.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/09/2018 - Populate Combo list from Salary Grade Table
        //*************************************************************************
        private void RetrieveBindingSalaryGrade()
        {
            ddl_salary_grade.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_salaries_tbl_list");

            ddl_salary_grade.DataSource = dt;
            ddl_salary_grade.DataTextField = "salary_grade";
            ddl_salary_grade.DataValueField = "salary_grade";
            ddl_salary_grade.DataBind();
            ListItem li = new ListItem("-Select Here-", "");
            ddl_salary_grade.Items.Insert(0, li);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/09/2018 - Populate Combo list from Salary Grade Table
        //*************************************************************************
        private void RetrieveBindingEmpType()
        {
            ddl_employment_type.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_employmenttypes_tbl_list");

            ddl_employment_type.DataSource = dt;
            ddl_employment_type.DataTextField = "employmenttype_description";
            ddl_employment_type.DataValueField = "employment_type";
            ddl_employment_type.DataBind();
            ListItem li = new ListItem("-Select Here-", "");
            ddl_employment_type.Items.Insert(0, li);
        }
        

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            if (ddl_employment_type.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_employment_type");
                ddl_employment_type.Focus();
                validatedSaved = false;
            }
            else if (tbx_position_short_title.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_position_short_title");
                tbx_position_short_title.Focus();
                validatedSaved = false;
            }
            else if (tbx_position_long_title.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_position_long_title");
                tbx_position_long_title.Focus();
                validatedSaved = false;
            }
            else if (ddl_salary_grade.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_salary_grade");
                ddl_salary_grade.Focus();
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
                    case "ddl_employment_type":
                        {
                            LblRequired0.Text = MyCmn.CONST_RQDFLD;
                            ddl_employment_type.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_position_short_title":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            tbx_position_short_title.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_position_long_title":
                        {
                            LblRequired2.Text = MyCmn.CONST_RQDFLD;
                            tbx_position_long_title.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_position_title1":
                        {
                            LblRequired3.Text = MyCmn.CONST_RQDFLD;
                            tbx_position_title1.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_position_title2":
                        {
                            LblRequired4.Text = MyCmn.CONST_RQDFLD;
                            tbx_position_title2.BorderColor = Color.Red;
                            break;
                        }
                    case "ddl_salary_grade":
                        {
                            LblRequired5.Text = MyCmn.CONST_RQDFLD;
                            ddl_salary_grade.BorderColor = Color.Red;
                            break;
                        }
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "ddl_employment_type":
                        {
                            if (LblRequired0.Text != "")
                            {
                                LblRequired0.Text = "";
                                ddl_employment_type.BorderColor = Color.LightGray;
                            }
                            break;
                        }
                    case "tbx_position_short_title":
                        {
                            if (LblRequired1.Text != "")
                            {
                                LblRequired1.Text = "";
                                tbx_position_short_title.BorderColor = Color.LightGray;
                            }
                            break;
                        }
                    case "tbx_position_long_title":
                        {
                            LblRequired2.Text = "";
                            tbx_position_long_title.BorderColor = Color.LightGray;
                            break;
                        }
                    case "tbx_position_title1":
                        {
                            LblRequired3.Text = "";
                            tbx_position_title1.BorderColor = Color.LightGray;
                            break;
                        }
                    case "tbx_position_title2":
                        {
                            LblRequired4.Text = "";
                            tbx_position_title2.BorderColor = Color.LightGray;
                            break;
                        }
                    case "ddl_salary_grade":
                        {
                            LblRequired5.Text = "";
                            ddl_salary_grade.BorderColor = Color.LightGray;
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
                            ddl_employment_type.BorderColor = Color.LightGray;
                            tbx_position_short_title.BorderColor = Color.LightGray;
                            tbx_position_long_title.BorderColor = Color.LightGray;
                            tbx_position_title1.BorderColor = Color.LightGray;
                            tbx_position_title2.BorderColor = Color.LightGray;
                            ddl_salary_grade.BorderColor = Color.LightGray;
                            break;
                        }

                }
            }
        }

        protected void ddl_employment_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_employment_type.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_employment_type");
            }
            else
            {
                FieldValidationColorChanged(false, "ddl_employment_type");
            }
            ddl_employment_type.Focus();
        }
    }
}