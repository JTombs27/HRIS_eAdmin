//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Reasons Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// JORGE RUSTOM VILLANUEVA  01/24/2019      Code Creation
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

namespace HRIS_eAdmin.View.cReasons
{
    public partial class cReasons : System.Web.UI.Page
    {
        //********************************************************************
        //  BEGIN - AEC- 01/24/2019 - Data Place holder creation 
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

        DataTable transactionlist
        {
            get
            {
                if ((DataTable)ViewState["reasonslist"] == null) return null;
                return (DataTable)ViewState["reasonslist"];
            }
            set
            {
                ViewState["reasonslist"] = value;
            }
        }

        //********************************************************************
        //  BEGIN - AEC- 01/24/2019 - Public Variable used in Add/Edit Mode
        //********************************************************************

        CommonDB MyCmn = new CommonDB();

        //********************************************************************
        //  BEGIN - AEC- 01/24/2019 - Page Load method
        //********************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] != null)

            {
                if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "reason_code";
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
        //  BEGIN - AEC- 01/24/2019 - Initialiazed Page 
        //********************************************************************
        private void InitializePage()
        {

            Session["sortdirection"] = SortDirection.Ascending.ToString();
            Session["cReasons"] = "cReasons";
            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_reasons_tbl_list");
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Add button to trigger add/edit page
        //*************************************************************************
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();
            AddNewRow();
            RetrieveBindingtransactionsdescr();

            LabelAddEdit.Text = "Add New Record";
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD); ;
            FieldValidationColorChanged(false, "ALL");
            

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_reason_code.Text = "";
            tbx_reasons_description.Text = "";
            tbx_reasons_subject_description.Text = "";
            tbx_reason_code.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["reason_code"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "00";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["reason_code"].ToString();
            }

            int lastCodeInt = int.Parse(lastCode) + 1;
            string nextCode = lastCodeInt.ToString();
            nextCode = nextCode.PadLeft(3, '0');

            tbx_reason_code.Text = nextCode;


        }

        //*************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {

            dtSource = new DataTable();
            dtSource.Columns.Add("reason_code", typeof(System.String));
            dtSource.Columns.Add("reason_descr", typeof(System.String));
            dtSource.Columns.Add("transaction_code", typeof(System.String));
            dtSource.Columns.Add("reason_report_subject", typeof(System.String));

        }

        //*************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "reasons_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "reason_code" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        protected void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string appttype = commandArgs[0];
            string appttypedescr = commandArgs[1];

            deleteRec1.Text = "Are you sure to delete this Record = (" + appttype.Trim() + ") - " + appttypedescr.Trim() + " ?";
            lnkBtnYes.CommandArgument = appttype;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string deleteExpression = "reason_code = '" + svalues + "'";

            MyCmn.DeleteBackEndData("reasons_tbl", "WHERE " + deleteExpression);

            DataRow[] row2Delete = dataListGrid.Select(deleteExpression);
            dataListGrid.Rows.Remove(row2Delete[0]);
            dataListGrid.AcceptChanges();
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalDelete();", true);
            up_dataListGrid.Update();
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Edit Row selection that will trigger edit page 
        //**************************************************************************
        protected void editRow_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string editExpression = "reason_code = '" + svalues + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            RetrieveBindingtransactionsdescr();
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();

            DataRow nrow = dtSource.NewRow();
            nrow["reason_code"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);

            tbx_reason_code.Text = svalues;
            tbx_reasons_description.Text = row2Edit[0]["reason_descr"].ToString();
            tbx_reasons_subject_description.Text = row2Edit[0]["reason_report_subject"].ToString();
            ddl_transaction_descr.Text = row2Edit[0]["transaction_code"].ToString();
          



            tbx_reasons_description.Focus();
            LabelAddEdit.Text = "Edit Record: " + tbx_reasons_description.Text.Trim();
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);

            FieldValidationColorChanged(false, "ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //**************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Change Field Sort mode  
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

        //******************************************************************************
        //  BEGIN - AEC- 09/09/2018 - Populate Combo list from transactions table
        //******************************************************************************
        private void RetrieveBindingtransactionsdescr()
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

        protected void ddl_transaction_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            if (ddl_transaction_descr.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(false, "transaction_descr");
                ddl_transaction_descr.Focus();
            }

        }

        //**************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Get Grid current sort order 
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

        protected void ddl_transaction_descr_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (ddl_transaction_descr.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(false, "ddl_transaction_descr");
                ddl_transaction_descr.Focus();
            }
        }

        //**************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Save New Record/Edited Record to back end DB
        //**************************************************************************
        protected void btnSave_Click(object sender, EventArgs e)
        {

            string saveRecord = ViewState["AddEdit_Mode"].ToString();
            string scriptInsertUpdate = string.Empty;

            if (IsDataValidated())
            {


                if (saveRecord == MyCmn.CONST_ADD)
                {
                    

                    dtSource.Rows[0]["reason_code"] = tbx_reason_code.Text.ToString();
                    dtSource.Rows[0]["reason_descr"] = tbx_reasons_description.Text.ToString();
                    dtSource.Rows[0]["reason_report_subject"] = tbx_reasons_subject_description.Text.ToString();
                    dtSource.Rows[0]["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    
                    dtSource.Rows[0]["reason_code"] = tbx_reason_code.Text.ToString();
                    dtSource.Rows[0]["reason_descr"] = tbx_reasons_description.Text.ToString();
                    dtSource.Rows[0]["reason_report_subject"] = tbx_reasons_subject_description.Text.ToString();
                    dtSource.Rows[0]["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
              
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
                        nrow["reason_code"] = tbx_reason_code.Text.ToString();
                        nrow["reason_descr"] = tbx_reasons_description.Text.ToString();
                        nrow["reason_report_subject"] = tbx_reasons_subject_description.Text.ToString();
                        nrow["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {

                        string editExpression = "reason_code = '" + tbx_reason_code.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);

                        row2Edit[0]["reason_code"] = tbx_reason_code.Text.ToString();
                        row2Edit[0]["reason_descr"] = tbx_reasons_description.Text.ToString();
                        row2Edit[0]["reason_report_subject"] = tbx_reasons_subject_description.Text.ToString();
                        row2Edit[0]["transaction_descr"] = ddl_transaction_descr.SelectedItem.ToString();
                        row2Edit[0]["transaction_code"] = ddl_transaction_descr.SelectedValue.ToString();
                        
                        CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
                        SaveAddEdit.Text = MyCmn.CONST_EDITREC;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                }
                ViewState.Remove("AddEdit_Mode");
                RetrieveDataListGrid();
                retaininfo();
                show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
            }
        }

        //**************************************************************************
        //  BEGIN - JORGE RUSTOM VILLANUEVA- 01/29/2019 - Retain Gridview for edit/add
        //**************************************************************************

        public void retaininfo()
        {

            if (tbx_search.Text != "")
            {
                string searchExpression = "reason_code LIKE '%" + tbx_search.Text.Trim() + "%' OR reason_descr LIKE '%" + tbx_search.Text.Trim() + "%' OR transaction_descr LIKE '%" + tbx_search.Text.Trim() + "%' OR reason_report_subject LIKE '%" + tbx_search.Text.Trim() + "%'";


                DataTable dtSource1 = new DataTable();
                dtSource1.Columns.Add("reason_code", typeof(System.String));
                dtSource1.Columns.Add("reason_descr", typeof(System.String));
                dtSource1.Columns.Add("transaction_descr", typeof(System.String));
                dtSource1.Columns.Add("reason_report_subject", typeof(System.String));
                dtSource1.Columns.Add("transaction_code", typeof(System.String));

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

        }

                //**************************************************************************
                //  BEGIN - AEC- 01/24/2019 - GridView Change Page Number
                //**************************************************************************
                protected void gridviewbind_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_dataListGrid.PageIndex = e.NewPageIndex;
            MyCmn.Sort(gv_dataListGrid, dataListGrid, ViewState["SortField"].ToString(), ViewState["SortOrder"].ToString());
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Change on Page Size (no. of row per page) on Gridview  
        //*********************************************************************************
        protected void DropDownListID_TextChanged(object sender, EventArgs e)
        {
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        protected void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "reason_code LIKE '%" + tbx_search.Text.Trim() + "%' OR reason_descr LIKE '%" + tbx_search.Text.Trim() + "%' OR transaction_descr LIKE '%" + tbx_search.Text.Trim() + "%' OR reason_report_subject LIKE '%" + tbx_search.Text.Trim() + "%'";


            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("reason_code", typeof(System.String));
            dtSource1.Columns.Add("reason_descr", typeof(System.String));
            dtSource1.Columns.Add("transaction_descr", typeof(System.String));
            dtSource1.Columns.Add("reason_report_subject", typeof(System.String));
            dtSource1.Columns.Add("transaction_code", typeof(System.String));

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
        //  BEGIN - AEC- 01/24/2019 - Define Property for Sort Direction  
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
        //  BEGIN - AEC- 01/24/2019 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            FieldValidationColorChanged(false, "ALL");

            if (tbx_reasons_description.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_reasons_description");
                tbx_reasons_description.Focus();
                validatedSaved = false;
            }

            else if (tbx_reasons_subject_description.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_reasons_subject_description");
                tbx_reasons_subject_description.Focus();
                validatedSaved = false;
            }

            else if(ddl_transaction_descr.SelectedItem.Text== "-- Select Here --") {

                FieldValidationColorChanged(true, "ddl_transaction_descr");
                ddl_transaction_descr.Focus();
                validatedSaved = false;
            }

            return validatedSaved;
        }

        //**********************************************************************************************
        //  BEGIN - AEC- 01/24/2019 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        protected void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            if (pMode)
                switch (pObjectName)
                {
                    case "tbx_reasons_description":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            tbx_reasons_description.BorderColor = Color.Red;
                            break;
                        }

                    case "tbx_reasons_subject_description":
                        {
                            LblRequired2.Text = MyCmn.CONST_RQDFLD;
                            tbx_reasons_subject_description.BorderColor = Color.Red;
                            break;
                        }

                    case "ddl_transaction_descr":
                        {
                            LblRequired3.Text = MyCmn.CONST_RQDFLD;
                            ddl_transaction_descr.BorderColor = Color.Red;
                            break;
                        }


                    default:
                        break;
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "tbx_reasons_description":
                        {
                            if (LblRequired1.Text != "")
                            {
                                LblRequired1.Text = "";
                                tbx_reasons_description.BorderColor = Color.LightGray;
                            }
                            break;
                        }

                    case "tbx_reasons_subject_description":
                        {
                            if (LblRequired2.Text != "")
                            {
                                LblRequired2.Text = "";
                                tbx_reasons_subject_description.BorderColor = Color.LightGray;
                            }
                            break;
                        }

                    case "ddl_transaction_descr":
                        {
                            if (LblRequired3.Text != "")
                            {
                                LblRequired3.Text = "";
                                ddl_transaction_descr.BorderColor = Color.LightGray;
                            }
                            break;
                        }

                    case "ALL":
                        {
                            LblRequired1.Text = "";
                            tbx_reasons_description.BorderColor = Color.LightGray;
                            LblRequired2.Text = "";
                            tbx_reasons_subject_description.BorderColor = Color.LightGray;
                            LblRequired3.Text = "";
                            ddl_transaction_descr.BorderColor = Color.LightGray;

                            break;
                        }

                }
            }
        }
    }
}