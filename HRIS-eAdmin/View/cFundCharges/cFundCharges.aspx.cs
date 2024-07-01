//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Fund Charges Page
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
using System.Drawing;

namespace HRIS_eAdmin.View
{
    public partial class cFundCharges : System.Web.UI.Page
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
                    ViewState["SortField"] = "fund_code";
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
            Session["cFundCharges"] = "cFundCharges";
            RetrieveBindingNames();
            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_fundcharges_tbl_list");
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
            FieldValidationColorChanged(false,"ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_fund_code.Text = "";
            tbx_fund_short_description.Text = "";
            tbx_fund_description.Text = "";
            tbx_designation_head1.Text = "";
            tbx_designation_head2.Text = "";
            tbx_designation_accountant1.Text = "";
            tbx_designation_accountant2.Text = "";
            ddl_fundhead_name.SelectedIndex = 0;
            ddl_print_type.SelectedIndex = 0;
            ddl_accnt_name.SelectedIndex = 0;
            tbx_fund_code.ReadOnly = false;
            tbx_fund_code.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["fund_code"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "00000000";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["fund_code"].ToString();
            }

            int lastCodeInt = int.Parse(lastCode) + 1;
            string nextCode = lastCodeInt.ToString();
            nextCode = nextCode.PadLeft(8, '0');

            tbx_fund_code.Text = nextCode;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("fund_code", typeof(System.String));
            dtSource.Columns.Add("fund_short_description", typeof(System.String));
            dtSource.Columns.Add("fund_description", typeof(System.String));
            dtSource.Columns.Add("empl_id", typeof(System.String));
            dtSource.Columns.Add("designation_head1", typeof(System.String));
            dtSource.Columns.Add("designation_head2", typeof(System.String));
            dtSource.Columns.Add("empl_id_accountant", typeof(System.String));
            dtSource.Columns.Add("designation_accountant1", typeof(System.String));
            dtSource.Columns.Add("designation_accountant2", typeof(System.String));
            dtSource.Columns.Add("print_type", typeof(System.Int16));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "fundcharges_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "fund_code" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        protected void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string fund_cd1 = commandArgs[0];
            if (fund_cd1 == "00000001" || fund_cd1 == "00000002")
            {
                notify_header.Text = "Unable to Delete";
                lbl_editdeletenotify.Text = " - This Record is Protected, it can't be Deleted";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalNotify();", true);
            }
            else
            {
                deleteRec1.Text = "Are you sure to delete this Record ?";
                lnkBtnYes.CommandArgument = fund_cd1;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
            }
            }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string fund_cd1 = commandArgs[0];


            string deleteExpression = "fund_code = " + fund_cd1.ToString().Trim() ;

            MyCmn.DeleteBackEndData("fundcharges_tbl", "WHERE " + deleteExpression);

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
            string fund_cd2 = commandArgs[0];
            string editExpression = "fund_code = '" + fund_cd2 + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();

            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["fund_code"] = fund_cd2;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);
            FieldValidationColorChanged(false, "ALL");

            tbx_fund_code.Text = fund_cd2;
            tbx_fund_short_description.Text = row2Edit[0]["fund_short_description"].ToString();
            tbx_fund_description.Text = row2Edit[0]["fund_description"].ToString();
            ddl_print_type.SelectedValue = row2Edit[0]["print_type"].ToString();
            ddl_fundhead_name.SelectedValue = row2Edit[0]["empl_id"].ToString();
            tbx_designation_head1.Text = row2Edit[0]["designation_head1"].ToString();
            tbx_designation_head2.Text = row2Edit[0]["designation_head2"].ToString();
            ddl_accnt_name.SelectedValue = row2Edit[0]["empl_id_accountant"].ToString();
            tbx_designation_accountant1.Text = row2Edit[0]["designation_accountant1"].ToString();
            tbx_designation_accountant2.Text = row2Edit[0]["designation_accountant2"].ToString();

            tbx_fund_code.ReadOnly = true;
            tbx_fund_short_description.Focus();
            LabelAddEdit.Text = "Edit Record: " + tbx_fund_short_description.Text.Trim();
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);

            FieldValidationColorChanged(false,"ALL");

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

            if (IsDataValidated() )
            { 
                if (saveRecord == MyCmn.CONST_ADD)
                {
                    dtSource.Rows[0]["fund_code"] = tbx_fund_code.Text.ToString();
                    dtSource.Rows[0]["fund_short_description"] = tbx_fund_short_description.Text.ToString();
                    dtSource.Rows[0]["fund_description"] = tbx_fund_description.Text.ToString();
                    dtSource.Rows[0]["empl_id"] = ddl_fundhead_name.SelectedValue.ToString();
                    dtSource.Rows[0]["designation_head1"] = tbx_designation_head1.Text.ToString();
                    dtSource.Rows[0]["designation_head2"] = tbx_designation_head2.Text.ToString();
                    dtSource.Rows[0]["empl_id_accountant"] = ddl_accnt_name.SelectedValue.ToString();
                    dtSource.Rows[0]["designation_accountant1"] = tbx_designation_accountant1.Text.ToString();
                    dtSource.Rows[0]["designation_accountant2"] = tbx_designation_accountant2.Text.ToString();
                    dtSource.Rows[0]["print_type"] = ddl_print_type.SelectedValue.ToString();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["fund_code"] = tbx_fund_code.Text.ToString();
                    dtSource.Rows[0]["fund_short_description"] = tbx_fund_short_description.Text.ToString();
                    dtSource.Rows[0]["fund_description"] = tbx_fund_description.Text.ToString();
                    dtSource.Rows[0]["empl_id"] = ddl_fundhead_name.SelectedValue.ToString();
                    dtSource.Rows[0]["designation_head1"] = tbx_designation_head1.Text.ToString();
                    dtSource.Rows[0]["designation_head2"] = tbx_designation_head2.Text.ToString();
                    dtSource.Rows[0]["empl_id_accountant"] = ddl_accnt_name.SelectedValue.ToString();
                    dtSource.Rows[0]["designation_accountant1"] = tbx_designation_accountant1.Text.ToString();
                    dtSource.Rows[0]["designation_accountant2"] = tbx_designation_accountant2.Text.ToString();
                    dtSource.Rows[0]["print_type"] = ddl_print_type.SelectedValue.ToString();
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
                        nrow["fund_code"] = tbx_fund_code.Text.ToString();
                        nrow["fund_short_description"] = tbx_fund_short_description.Text.ToString();
                        nrow["fund_description"] = tbx_fund_description.Text.ToString();
                        nrow["empl_id"] = ddl_fundhead_name.SelectedValue.ToString();
                        nrow["designation_head1"] = tbx_designation_head1.Text.ToString();
                        nrow["designation_head2"] = tbx_designation_head2.Text.ToString();
                        nrow["empl_id_accountant"] = ddl_accnt_name.SelectedValue.ToString();
                        nrow["designation_accountant1"] = tbx_designation_accountant1.Text.ToString();
                        nrow["designation_accountant2"] = tbx_designation_accountant2.Text.ToString();
                        nrow["print_type"] = ddl_print_type.SelectedValue.ToString();
                        nrow["employee_name_format1"] = ddl_fundhead_name.SelectedItem.Text.ToString();
                        nrow["employee_name_format1_ac"] = ddl_accnt_name.SelectedItem.Text.ToString();

                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "fund_code = '" + tbx_fund_code.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["fund_code"] = tbx_fund_code.Text.ToString();
                        row2Edit[0]["fund_short_description"] = tbx_fund_short_description.Text.ToString();
                        row2Edit[0]["fund_description"] = tbx_fund_description.Text.ToString();
                        row2Edit[0]["empl_id"] = ddl_fundhead_name.SelectedValue.ToString();
                        row2Edit[0]["designation_head1"] = tbx_designation_head1.Text.ToString();
                        row2Edit[0]["designation_head2"] = tbx_designation_head2.Text.ToString();
                        row2Edit[0]["empl_id_accountant"] = ddl_accnt_name.SelectedValue.ToString();
                        row2Edit[0]["designation_accountant1"] = tbx_designation_accountant1.Text.ToString();
                        row2Edit[0]["designation_accountant2"] = tbx_designation_accountant2.Text.ToString();
                        row2Edit[0]["print_type"] = ddl_print_type.SelectedValue.ToString();
                        if (ddl_fundhead_name.SelectedValue == "")
                        {
                            row2Edit[0]["employee_name_format1"] = "";
                        }
                        else {
                            row2Edit[0]["employee_name_format1"] = ddl_fundhead_name.SelectedItem.Text.ToString();
                        }
                       
                        row2Edit[0]["employee_name_format1_ac"] = ddl_accnt_name.SelectedItem.Text.ToString();

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
            //CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
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
            string searchExpression = "fund_code LIKE '%" + tbx_search.Text.Trim() + "%' OR fund_description LIKE '%" + tbx_search.Text.Trim() + "%' OR employee_name_format1 LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("effective_date", typeof(System.String));
            dtSource1.Columns.Add("fund_code", typeof(System.String));
            dtSource1.Columns.Add("fund_short_description", typeof(System.String));
            dtSource1.Columns.Add("fund_description", typeof(System.String));
            dtSource1.Columns.Add("department_name2", typeof(System.String));
            dtSource1.Columns.Add("sort_order_dept", typeof(System.Int16));
            dtSource1.Columns.Add("print_type", typeof(System.Int16));
            dtSource1.Columns.Add("empl_id", typeof(System.String));
            dtSource1.Columns.Add("designation_head1", typeof(System.String));
            dtSource1.Columns.Add("designation_head2", typeof(System.String));
            dtSource1.Columns.Add("employee_name_format1", typeof(System.String));


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
  
        //*************************************************************************
        //  BEGIN - AEC- 09/09/2018 - Populate Combo list from Salary Grade Table
        //*************************************************************************
        private void RetrieveBindingNames()
        {
            ddl_fundhead_name.Items.Clear();
            DataTable dtEmpList = MyCmn.RetrieveData("sp_personnelnames_combolist");

            ddl_fundhead_name.DataSource = dtEmpList;
            ddl_fundhead_name.DataTextField = "employee_name";
            ddl_fundhead_name.DataValueField = "empl_id";
            ddl_fundhead_name.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_fundhead_name.Items.Insert(0, li);

            ddl_accnt_name.Items.Clear();
            ddl_accnt_name.DataSource = dtEmpList;
            ddl_accnt_name.DataTextField = "employee_name";
            ddl_accnt_name.DataValueField = "empl_id";
            ddl_accnt_name.DataBind();
            ListItem li2 = new ListItem("-- Select Here --", "");
            ddl_accnt_name.Items.Insert(0, li2);

        }

        //**************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            FieldValidationColorChanged(false, "ALL");

             if (tbx_fund_description.Text == "")
             {
                FieldValidationColorChanged(true, "tbx_fund_description");
                tbx_fund_description.Focus();
                validatedSaved = false;
             }
            return validatedSaved;
        }

        //**********************************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************

        protected void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            if (pMode)
                switch (pObjectName)
                {
                    case "tbx_fund_short_description":
                        {
                            LblRequired0.Text = MyCmn.CONST_RQDFLD;
                            tbx_fund_short_description.BorderColor = Color.Red;
                            break;
                        }
                    case "tbx_fund_description":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            tbx_fund_description.BorderColor = Color.Red;
                            break;
                        }
                    case "ddl_fundhead_name":
                        {
                            LblRequired2.Text = MyCmn.CONST_RQDFLD;
                            ddl_fundhead_name.BorderColor = Color.Red;
                            break;
                        }
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "tbx_fund_short_description":
                        {
                            LblRequired0.Text = "";
                            tbx_fund_short_description.BorderColor = Color.LightGray;
                            break;
                        }
 
                    case "tbx_fund_description":
                        {
                            LblRequired1.Text = "";
                            tbx_fund_description.BorderColor = Color.LightGray;
                            break;
                        }
 
                    case "ddl_fundhead_name":
                        {
                            LblRequired2.Text = "";
                            ddl_fundhead_name.BorderColor = Color.LightGray;
                            break;
                        }


                    case "ALL":
                        {
                            LblRequired0.Text = "";
                            LblRequired1.Text = "";
                            LblRequired2.Text = "";
                            tbx_fund_short_description.BorderColor = Color.LightGray;
                            tbx_fund_description.BorderColor = Color.LightGray;
                            ddl_fundhead_name.BorderColor = Color.LightGray;
                            break;
                        }
                }
            }
        }

        protected void ddl_fundhead_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_fundhead_name.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_fundhead_name");
            }
            else
            {
                FieldValidationColorChanged(false, "ddl_fundhead_name");
            }
            ddl_fundhead_name.Focus();
        }
    }
}