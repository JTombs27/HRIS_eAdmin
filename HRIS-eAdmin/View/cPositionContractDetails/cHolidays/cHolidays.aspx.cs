//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Appointment Type Page
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
    public partial class cHolidays : System.Web.UI.Page
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
                    ViewState["SortField"] = "holiday_date";
                    ViewState["SortOrder"] = "ASC";
                }
            }
            else
            {
                Response.Redirect("~/login.aspx");
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
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
            Session["cAppointmentType"] = "cAppointmentType";
            loop_year();
            RetrieveDataListGrid();
            
        }

        public void loop_year() {
            ddl_year.Items.Clear();
            int years = Convert.ToInt32(DateTime.Now.Year);
            int prev_year = years - 9;
            ListItem li = new ListItem("Select Year", "");
            ddl_year.Items.Insert(0, li);
            for (int x = 1;x < 12; x++)
            {
                ListItem li2 = new ListItem(prev_year.ToString(),prev_year.ToString());
                ddl_year.Items.Insert(x, li2);
                if (prev_year == years)
                {
                    ListItem li3 = new ListItem((years+1).ToString(), (years + 1).ToString());
                    ddl_year.Items.Insert(x+1, li3);
                    ddl_year.SelectedValue = years.ToString();
                    break;
                }
                prev_year = prev_year + 1;
            }
          
        }
        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_holidays_tbl_list","par_year",ddl_year.SelectedValue);
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
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD);
            FieldValidationColorChanged(false, "ALL");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SHowDate", "show_date();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_date.Text = "";
            tbx_date.Enabled = true;
            tbx_holiday_name.Text = "";
            ddl_holiday_type.SelectedValue = "";
            chckbx_casual.Checked = false;
            chckbx_regular.Checked = false;
            chckbx_jo.Checked = false;
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["holiday_date"] = string.Empty;
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
            dtSource.Columns.Add("holiday_date", typeof(System.String));
            dtSource.Columns.Add("holiday_name", typeof(System.String));
            dtSource.Columns.Add("holiday_type", typeof(System.String));
            dtSource.Columns.Add("with_pay_regular", typeof(System.String));
            dtSource.Columns.Add("with_pay_casual", typeof(System.String));
            dtSource.Columns.Add("with_pay_joborder", typeof(System.String));
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "holidays_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "holiday_date" };
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

            deleteRec1.Text = "Are you sure to delete this Annual Holiday = (" + appttype.Trim() + ") - " + appttypedescr.Trim() + " ?";
            lnkBtnYes.CommandArgument = appttype;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
            up_dataListGrid.Update();
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";

        }

        //*************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string deleteExpression = "holiday_date = '" + svalues + "'";

            MyCmn.DeleteBackEndData("holidays_tbl", "WHERE " + deleteExpression);

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
            string editExpression = "holiday_date = '" + svalues + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();

            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["holiday_date"] = svalues;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);

            tbx_date.Text = svalues;
            tbx_date.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDate", "show_date();", true);

            tbx_holiday_name.Text = row2Edit[0]["holiday_name"].ToString();
            ddl_holiday_type.SelectedValue = row2Edit[0]["holiday_type"].ToString();
            chckbx_regular.Checked = row2Edit[0]["with_pay_regular"].ToString() == "True" ? true : false;
            chckbx_casual.Checked = row2Edit[0]["with_pay_casual"].ToString() == "True" ? true : false;
            chckbx_jo.Checked = row2Edit[0]["with_pay_joborder"].ToString() == "True" ? true : false;
            LabelAddEdit.Text = "Edit Record: " + tbx_holiday_name.Text.Trim();
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
                    dtSource.Rows[0]["holiday_date"] = tbx_date.Text.ToString();
                    dtSource.Rows[0]["holiday_name"] = tbx_holiday_name.Text.ToString();
                    dtSource.Rows[0]["holiday_type"] = ddl_holiday_type.SelectedValue.ToString().Trim();
                    dtSource.Rows[0]["with_pay_regular"] = chckbx_regular.Checked ? "True":"False";
                    dtSource.Rows[0]["with_pay_casual"] = chckbx_casual.Checked ? "True":"False";
                    dtSource.Rows[0]["with_pay_joborder"] = chckbx_jo.Checked ? "True":"False";
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);
                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["holiday_name"] = tbx_holiday_name.Text.ToString();
                    dtSource.Rows[0]["holiday_type"] = ddl_holiday_type.SelectedValue.ToString().Trim();
                    dtSource.Rows[0]["with_pay_regular"] = chckbx_regular.Checked ? "True" : "False";
                    dtSource.Rows[0]["with_pay_casual"] = chckbx_casual.Checked ? "True" : "False";
                    dtSource.Rows[0]["with_pay_joborder"] = chckbx_jo.Checked ? "True" : "False";
                    scriptInsertUpdate = MyCmn.updatescript(dtSource);
                }

                if (saveRecord == MyCmn.CONST_ADD || saveRecord == MyCmn.CONST_EDIT)
                {
                    if (scriptInsertUpdate == string.Empty) return;
                    string msg = MyCmn.insertdata(scriptInsertUpdate);
                    if (msg == "") return;
                    if (msg.Substring(0, 1) == "X")
                    {
                        FieldValidationColorChanged(true, "already-exist");
                        return;
                    }


                    if (saveRecord == MyCmn.CONST_ADD)
                    {
                        DataRow nrow = dataListGrid.NewRow();
                        nrow["holiday_date"] = tbx_date.Text.ToString();
                        nrow["holiday_name"] = tbx_holiday_name.Text.ToString();
                        nrow["holiday_type"] = ddl_holiday_type.SelectedValue.ToString();
                        nrow["holiday_type_descr"] = ddl_holiday_type.SelectedItem.Text.ToString();
                        nrow["with_pay_regular"] = chckbx_regular.Checked ? true:false;
                        nrow["with_pay_casual"] = chckbx_casual.Checked ? true : false;
                        nrow["with_pay_joborder"] = chckbx_jo.Checked ? true : false;
                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);
                        //gv_dataListGrid.SelectRow(gv_dataListGrid.Rows.Count - 1);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "holiday_date = '" + tbx_date.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["holiday_name"] = tbx_holiday_name.Text.ToString();
                        row2Edit[0]["holiday_type"] = ddl_holiday_type.SelectedValue.ToString();
                        row2Edit[0]["holiday_type_descr"] = ddl_holiday_type.SelectedItem.Text.ToString();
                        row2Edit[0]["with_pay_regular"] = chckbx_regular.Checked ? true : false;
                        row2Edit[0]["with_pay_casual"] = chckbx_casual.Checked ? true : false;
                        row2Edit[0]["with_pay_joborder"] = chckbx_jo.Checked ? true : false;
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
            string searchExpression = "holiday_date LIKE'%" + tbx_search.Text.Trim() +"%' OR holiday_name LIKE '%" + tbx_search.Text.Trim() + "%' OR holiday_type_descr LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("holiday_date", typeof(System.String));
            dtSource1.Columns.Add("holiday_name", typeof(System.String));
            dtSource1.Columns.Add("holiday_type", typeof(System.String));
            dtSource1.Columns.Add("holiday_type_descr", typeof(System.String));
            dtSource1.Columns.Add("with_pay_regular", typeof(System.String));
            dtSource1.Columns.Add("with_pay_casual", typeof(System.String));
            dtSource1.Columns.Add("with_pay_joborder", typeof(System.String));

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
            try
            {
                DateTime converted_date =  DateTime.Parse(tbx_date.Text.Trim());
                FieldValidationColorChanged(false, "ALL");
                if (converted_date.Year.ToString() != ddl_year.SelectedValue)
                {
                    FieldValidationColorChanged(true, "invalid-year");
                    validatedSaved = false;
                }
                else if (ddl_holiday_type.SelectedValue.Trim() == "")
                {
                    FieldValidationColorChanged(true, "ddl_holiday_type");
                    ddl_holiday_type.Focus();
                    validatedSaved = false;
                }
                else if (tbx_holiday_name.Text.Trim() == "")
                {
                    FieldValidationColorChanged(true, "tbx_holiday_name");
                    tbx_holiday_name.Focus();
                    validatedSaved = false;
                }

            }
            catch (Exception b)
            {
                if (tbx_date.Text == "")
                {
                    FieldValidationColorChanged(true, "tbx_date");
                }
                else
                {
                    FieldValidationColorChanged(true, "invalid-date");
                }
                tbx_date.Focus();
                return false;
            }
            return validatedSaved;
        }


        //**********************************************************************************************
        //  BEGIN - AEC- 09/20/2018 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        protected void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            if (pMode)
            {
                switch (pObjectName)
                {

                    case "tbx_date":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            tbx_date.CssClass = "form-control form-control-sm my-date required";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "show_date();", true);
                            break;
                        }
                    case "invalid-year":
                        {
                            LblRequired1.Text = "<b>Invalid Year! ("+(Convert.ToDateTime(tbx_date.Text.ToString())).ToString("yyyy")+")</b>.<br/> Valid Date is under the year selected in the main page.";
                            tbx_date.CssClass = "form-control form-control-sm my-date required";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "show_date();", true);
                            break;
                        }
                    case "tbx_holiday_name":
                        {
                            LblRequired3.Text = MyCmn.CONST_RQDFLD;
                            tbx_holiday_name.CssClass = "form-control form-control-sm required";
                            break;
                        }
                    case "ddl_holiday_type":
                        {
                            LblRequired2.Text = MyCmn.CONST_RQDFLD;
                            ddl_holiday_type.CssClass = "form-control form-control-sm required";
                            break;
                        }
                    case "invalid-date":
                        {
                            LblRequired1.Text = "Invalid Date Value";
                            tbx_date.CssClass = "form-control form-control-sm my-date required";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "show_date();", true);
                            break;
                        }
                    case "already-exist":
                        {
                            LblRequired1.Text = "Holiday date already exist.";
                            tbx_date.CssClass = "form-control form-control-sm my-date required";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "show_date();", true);
                            break;
                        }
                }
            }
            else if (!pMode)
            {
                switch (pObjectName)
                {

                    case "tbx_date":
                        {
                            LblRequired1.Text = "";
                            tbx_date.CssClass = "form-control form-control-sm my-date";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "show_date();", true);
                            break;
                        }
                    case "tbx_holiday_name":
                        {
                            LblRequired3.Text = "";
                            tbx_holiday_name.CssClass = "form-control form-control-sm";
                            break;
                        }
                    case "ddl_holiday_type":
                        {
                            LblRequired2.Text = "";
                            ddl_holiday_type.CssClass = "form-control form-control-sm";
                            break;
                        }
                    case "invalid-date":
                        {
                            LblRequired1.Text = "";
                            tbx_date.CssClass = "form-control form-control-sm my-date";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "show_date();", true);
                            break;
                        }
                    case "ALL":
                        {
                            LblRequired1.Text = "";
                            LblRequired2.Text = "";
                            LblRequired3.Text = "";
                            tbx_date.CssClass = "form-control form-control-sm my-date";
                            tbx_holiday_name.CssClass = "form-control form-control-sm";
                            ddl_holiday_type.CssClass = "form-control form-control-sm";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "show_date();", true);
                            break;
                        }

                }
            }
        }

        protected void ddl_year_TextChanged(object sender, EventArgs e)
        {
            RetrieveDataListGrid();
        }
    }
}