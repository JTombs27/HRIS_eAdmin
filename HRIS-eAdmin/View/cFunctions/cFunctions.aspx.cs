//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Department Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// VINCET JADE ALIVIO       03/26/2019       Code Creation
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
    public partial class cFunctions : System.Web.UI.Page
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
                    ViewState["SortField"] = "function_code";
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
            Session["cFunctions"] = "cFunctions";
            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_functions_tbl_list");
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

            tbx_function_code.Enabled = true;
            tbx_function_code.ReadOnly = false;
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
            tbx_function_code.Text = "";
            tbx_function_code.Focus();

            tbx_function_details.Text = "";
            tbx_function_name.Text = "";
            tbx_function_program.Text = "";
            tbx_function_shortname.Text = "";
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["function_code"] = string.Empty;
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
            dtSource.Columns.Add("function_code", typeof(System.String));
            dtSource.Columns.Add("function_shortname", typeof(System.String));
            dtSource.Columns.Add("function_name", typeof(System.String));
            dtSource.Columns.Add("function_detail", typeof(System.String));
            dtSource.Columns.Add("function_program", typeof(System.String));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "functions_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "function_code"};
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        public void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string dept_cd1 = commandArgs[0];
            deleteRec1.Text = "Are you sure to delete this Record ?";
            lnkBtnYes.CommandArgument = dept_cd1;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        public void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string function_code = commandArgs[0];

            string deleteExpression = "function_code = '" + function_code + "'";
            
            MyCmn.DeleteBackEndData("functions_tbl", "WHERE " + deleteExpression);

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
        public void editRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string func_code = commandArgs[0];
            string editExpression = "function_code = '" + func_code + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();

            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["function_code"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);
            FieldValidationColorChanged(false, "ALL");

            tbx_function_code.Text = func_code;
            tbx_function_shortname.Text = row2Edit[0]["function_shortname"].ToString();
            tbx_function_name.Text = row2Edit[0]["function_name"].ToString();
            tbx_function_details.Text = row2Edit[0]["function_detail"].ToString();
            tbx_function_program.Text = row2Edit[0]["function_program"].ToString();

            tbx_function_code.ReadOnly = true;
            LabelAddEdit.Text = "Edit Record: " + tbx_function_code.Text.Trim();

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
                    dtSource.Rows[0]["function_code"]         = tbx_function_code.Text.ToString().Trim();
                    dtSource.Rows[0]["function_shortname"]    = tbx_function_shortname.Text.ToString().Trim();
                    dtSource.Rows[0]["function_name"]         = tbx_function_name.Text.ToString().Trim();
                    dtSource.Rows[0]["function_detail"]       = tbx_function_details.Text.ToString().Trim();
                    dtSource.Rows[0]["function_program"]      = tbx_function_program.Text.ToString().Trim();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["function_code"] = tbx_function_code.Text.ToString().Trim();
                    dtSource.Rows[0]["function_shortname"] = tbx_function_shortname.Text.ToString().Trim();
                    dtSource.Rows[0]["function_name"] = tbx_function_name.Text.ToString().Trim();
                    dtSource.Rows[0]["function_detail"] = tbx_function_details.Text.ToString().Trim();
                    dtSource.Rows[0]["function_program"] = tbx_function_program.Text.ToString().Trim();
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
                        nrow["function_code"] = tbx_function_code.Text.ToString().Trim();
                        nrow["function_shortname"] = tbx_function_shortname.Text.ToString().Trim();
                        nrow["function_name"] = tbx_function_name.Text.ToString().Trim();
                        nrow["function_detail"] = tbx_function_details.Text.ToString().Trim();
                        nrow["function_program"] = tbx_function_program.Text.ToString().Trim();
                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "function_code = '" + tbx_function_code.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);

                        row2Edit[0]["function_code"] = tbx_function_code.Text.ToString().Trim();
                        row2Edit[0]["function_shortname"] = tbx_function_shortname.Text.ToString().Trim();
                        row2Edit[0]["function_name"] = tbx_function_name.Text.ToString().Trim();
                        row2Edit[0]["function_detail"] = tbx_function_details.Text.ToString().Trim();
                        row2Edit[0]["function_program"] = tbx_function_program.Text.ToString().Trim();

                        CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
                        SaveAddEdit.Text = MyCmn.CONST_EDITREC;
                    }
                    up_dataListGrid.Update();
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
            string searchExpression = "function_code LIKE '%" + tbx_search.Text.Trim() + "%' OR function_shortname LIKE '%" + tbx_search.Text.Trim() + "%' OR function_name LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("function_code", typeof(System.String));
            dtSource1.Columns.Add("function_shortname", typeof(System.String));
            dtSource1.Columns.Add("function_name", typeof(System.String));
            dtSource1.Columns.Add("function_detail", typeof(System.String));
            dtSource1.Columns.Add("function_program", typeof(System.String));

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
        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            FieldValidationColorChanged(false, "ALL");

            if (tbx_function_code.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_function_code");
                tbx_function_code.Focus();
                validatedSaved = false;
            }
            else if (tbx_function_shortname.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_function_shortname");
                tbx_function_shortname.Focus();
                validatedSaved = false;
            }
            else if (tbx_function_name.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_function_name");
                tbx_function_name.Focus();
                validatedSaved = false;
            }
            else if (tbx_function_details.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_function_details");
                tbx_function_details.Focus();
                validatedSaved = false;
            }
            else if (tbx_function_program.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_function_program");
                tbx_function_program.Focus();
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
                    case "tbx_function_code":
                        {
                            LblRequired0.Text = err_msg;
                            tbx_function_code.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_function_shortname":
                        {
                            LblRequired1.Text = err_msg;
                            tbx_function_shortname.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_function_name":
                        {
                            LblRequired2.Text = err_msg;
                            tbx_function_name.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_function_details":
                        {
                            LblRequired3.Text = err_msg;
                            tbx_function_details.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_function_program":
                        {
                            LblRequired4.Text = err_msg;
                            tbx_function_program.BorderColor = Color.Red;
                            break;
                        }
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "ALL":
                        {
                            LblRequired0.Text = "";
                            LblRequired1.Text = "";
                            LblRequired2.Text = "";
                            LblRequired3.Text = "";
                            LblRequired4.Text = "";
                            tbx_function_code.BorderColor = Color.LightGray;
                            tbx_function_shortname.BorderColor = Color.LightGray;
                            tbx_function_name.BorderColor = Color.LightGray;
                            tbx_function_details.BorderColor = Color.LightGray;
                            tbx_function_program.BorderColor = Color.LightGray;
                            break;
                        }

                }
            }
        }
    }
}