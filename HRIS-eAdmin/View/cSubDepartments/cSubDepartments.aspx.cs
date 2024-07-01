//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Department Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// JORGE RUSTOM VILLANUEVA      01/29/2019      Code Creation
//**********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRIS_Common;
using System.Drawing;
using System.Data;

namespace HRIS_eAdmin.View
{
    public partial class cSubDepartments : System.Web.UI.Page
    {

        //********************************************************************
        //  BEGIN - AEC- Data Place holder creation 
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
        //  BEGIN - AEC- Public Variable used in Add/Edit Mode
        //********************************************************************

        public string var_include_history;

        CommonDB MyCmn = new CommonDB();
        
        //********************************************************************
        //  BEGIN - AEC- Page Load method
        //********************************************************************
        public void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] != null)
            {
                if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "subdepartment_code";
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
        //  BEGIN - AEC- Initialiazed Page 
        //********************************************************************
        private void InitializePage()
        {
            var_include_history = "N";
            Session["sortdirection"] = SortDirection.Ascending.ToString();
            Session["cSubDepartments"] = "cSubDepartments";
            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - AEC- Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_subdepartments_tbl_list");
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //*************************************************************************
        //  BEGIN - AEC- Add button to trigger add/edit page
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
        //  BEGIN - AEC- Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_effective_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tbx_subdepartment_code.Text = "";
            tbx_subdepartment_short_name.Text = "";
            tbx_subdepartment_name1.Text = "";
            tbx_subdepartment_name2.Text = "";
            // tbx_designation_head1.Text = "";
            //tbx_designation_head2.Text = "";
            //ddl_depHead_name.SelectedIndex = 0;
            tbx_subdepartment_code.ReadOnly = false;
            tbx_subdepartment_code.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["effective_date"] = DateTime.Now.ToString("yyyy-MM-dd");
            nrow["subdepartment_code"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "00";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["subdepartment_code"].ToString();
            }

            int lastCodeInt = int.Parse(lastCode) + 1;
            string nextCode = lastCodeInt.ToString();
            nextCode = nextCode.PadLeft(2, '0');

            tbx_subdepartment_code.Text = nextCode;
        }

        //*************************************************************************
        //  BEGIN - AEC- Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("effective_date", typeof(System.String));
            dtSource.Columns.Add("subdepartment_code", typeof(System.String));
            dtSource.Columns.Add("subdepartment_short_name", typeof(System.String));
            dtSource.Columns.Add("subdepartment_name1", typeof(System.String));
            dtSource.Columns.Add("subdepartment_name2", typeof(System.String));
 
        }

        //*************************************************************************
        //  BEGIN - AEC- Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "subdepartments_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "effective_date", "subdepartment_code" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        public void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string subdept_cd1 = commandArgs[0];
            string eff_dt1 = commandArgs[1];

            deleteRec1.Text = "Are you sure to delete this Record ?";
            lnkBtnYes.CommandArgument = subdept_cd1 + ", " + eff_dt1;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- Delete Data to back-end Database
        //*************************************************************************
        public void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string subdept_cd2 = commandArgs[0];
            string eff_dt2 = commandArgs[1];

            string deleteExpression = "subdepartment_code = '" + subdept_cd2 + "' AND CONVERT(date, effective_date) = CONVERT(date,'" + eff_dt2.ToString().Trim() + "')";
            string deleteExpression1 = "subdepartment_code = '" + subdept_cd2 + "' AND effective_date = '" + eff_dt2.ToString().Trim() + "'";

            MyCmn.DeleteBackEndData("subdepartments_tbl", "WHERE " + deleteExpression);

            DataRow[] row2Delete = dataListGrid.Select(deleteExpression1);
            dataListGrid.Rows.Remove(row2Delete[0]);
            dataListGrid.AcceptChanges();
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalDelete();", true);
            up_dataListGrid.Update();
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**************************************************************************
        //  BEGIN - AEC- Edit Row selection that will trigger edit page 
        //**************************************************************************
        public void editRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string dept_cd2 = commandArgs[0];
            string eff_dt2 = commandArgs[1];
            string editExpression = "subdepartment_code = '" + dept_cd2 + "' AND effective_date = '" + eff_dt2.ToString().Trim() + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();

            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["effective_date"] = eff_dt2;
            nrow["subdepartment_code"] = dept_cd2;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);
            FieldValidationColorChanged(false, "ALL");

            tbx_subdepartment_code.Text = dept_cd2;
            tbx_effective_date.Text = eff_dt2;
            tbx_subdepartment_short_name.Text = row2Edit[0]["subdepartment_short_name"].ToString();
            tbx_subdepartment_name1.Text = row2Edit[0]["subdepartment_name1"].ToString();
            tbx_subdepartment_name2.Text = row2Edit[0]["subdepartment_name2"].ToString();
            // ddl_depHead_name.SelectedValue = row2Edit[0]["empl_id"].ToString();
            // tbx_designation_head1.Text = row2Edit[0]["designation_head1"].ToString();
            // tbx_designation_head2.Text = row2Edit[0]["designation_head2"].ToString();

            tbx_subdepartment_code.ReadOnly = true;
            tbx_subdepartment_short_name.Focus();
            LabelAddEdit.Text = "Edit Record: " + tbx_subdepartment_short_name.Text.Trim();

            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);
           
            FieldValidationColorChanged(false,"ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //**************************************************************************
        //  BEGIN - AEC- Change Field Sort mode  
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
        //  BEGIN - AEC- Get Grid current sort order 
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
    //  BEGIN - AEC-- Save New Record/Edited Record to back end DB
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
                    dtSource.Rows[0]["subdepartment_code"] = tbx_subdepartment_code.Text.ToString();
                    dtSource.Rows[0]["subdepartment_short_name"] = tbx_subdepartment_short_name.Text.ToString();
                    dtSource.Rows[0]["subdepartment_name1"] = tbx_subdepartment_name1.Text.ToString();
                    dtSource.Rows[0]["subdepartment_name2"] = tbx_subdepartment_name2.Text.ToString();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);
                    up_dataListGrid.Update();

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["effective_date"] = tbx_effective_date.Text.ToString();
                    dtSource.Rows[0]["subdepartment_code"] = tbx_subdepartment_code.Text.ToString();
                    dtSource.Rows[0]["subdepartment_short_name"] = tbx_subdepartment_short_name.Text.ToString();
                    dtSource.Rows[0]["subdepartment_name1"] = tbx_subdepartment_name1.Text.ToString();
                    dtSource.Rows[0]["subdepartment_name2"] = tbx_subdepartment_name2.Text.ToString();
                    scriptInsertUpdate = MyCmn.updatescript(dtSource);
                    up_dataListGrid.Update();
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
                        nrow["subdepartment_code"] = tbx_subdepartment_code.Text.ToString();
                        nrow["subdepartment_short_name"] = tbx_subdepartment_short_name.Text.ToString();
                        nrow["subdepartment_name1"] = tbx_subdepartment_name1.Text.ToString();
                        nrow["subdepartment_name2"] = tbx_subdepartment_name2.Text.ToString();

                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "subdepartment_code = '" + tbx_subdepartment_code.Text.ToString() + "' AND effective_date = '" + tbx_effective_date.Text.ToString().Trim() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["subdepartment_short_name"] = tbx_subdepartment_short_name.Text.ToString();
                        row2Edit[0]["subdepartment_name1"] = tbx_subdepartment_name1.Text.ToString();
                        row2Edit[0]["subdepartment_name2"] = tbx_subdepartment_name2.Text.ToString();
 
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
    //  BEGIN - AEC- GridView Change Page Number
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
        //  BEGIN - AEC- Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        public void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "subdepartment_code LIKE '%" + tbx_search.Text.Trim() + "%' OR subdepartment_name1 LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("effective_date", typeof(System.String));
            dtSource1.Columns.Add("subdepartment_code", typeof(System.String));
            dtSource1.Columns.Add("subdepartment_short_name", typeof(System.String));
            dtSource1.Columns.Add("subdepartment_name1", typeof(System.String));
            dtSource1.Columns.Add("subdepartment_name2", typeof(System.String));
  

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
        //  BEGIN - AEC- Define Property for Sort Direction  
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
        //  BEGIN - AEC - Set Include History Variable
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
        //  BEGIN - AEC- Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            FieldValidationColorChanged(false, "ALL");

           if (tbx_subdepartment_name1.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_subdepartment_name1");
                tbx_subdepartment_name1.Focus();
                validatedSaved = false;
            }

            return validatedSaved;
        }

        //**********************************************************************************************
        //  BEGIN - AEC - Change/Toggle Mode for Object Appearance during validation  
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
        //  BEGIN - AEC - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        public void FieldValidationColorChanged_Actual(bool pMode, string pObjectName,string err_msg )
       {
            if (pMode)
                switch (pObjectName)
                {
                  
                    case "tbx_subdepartment_name1":
                        {
                            LblRequired1.Text = err_msg;
                            tbx_subdepartment_name1.BorderColor = Color.Red;
                            break;
                        }
                   
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    
                    case "tbx_subdepartment_name1":
                        {
                            LblRequired1.Text = "";
                            tbx_subdepartment_name1.BorderColor = Color.LightGray;
                            break;
                        }
                   

                    case "ALL":
                        {
                            LblRequired0.Text = "";
                            LblRequired1.Text = "";
                            tbx_subdepartment_short_name.BorderColor = Color.LightGray;
                            tbx_subdepartment_name1.BorderColor = Color.LightGray;
                           
                            break;
                        }

                }
            }
        }

        //**********************************************************************************************
        //  End of Code  
        //**********************************************************************************************

    }
}