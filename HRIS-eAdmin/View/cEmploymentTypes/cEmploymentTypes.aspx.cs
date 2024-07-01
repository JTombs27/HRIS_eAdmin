﻿//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for EmploymentTypes Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// JORGE RUSTOM VILLANUEVA  01/22/2019      Code Creation
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

namespace HRIS_eAdmin.View.cEmploymentTypes
{
    public partial class cEmploymentTypes : System.Web.UI.Page
    {
        //********************************************************************
        //  BEGIN - AEC- 01/22/2018 - Data Place holder creation 
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
        //  BEGIN - AEC- 01/22/2019 - Public Variable used in Add/Edit Mode
        //********************************************************************

        CommonDB MyCmn = new CommonDB();

        //********************************************************************
        //  BEGIN - AEC- 01/22/2019 - Page Load method
        //********************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] != null)

            {
                if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "employment_type";
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
        //  BEGIN - AEC- 01/22/2019 - Initialiazed Page 
        //********************************************************************
        private void InitializePage()
        {

            Session["sortdirection"] = SortDirection.Ascending.ToString();
            Session["cEmploymentTypes"] = "cEmploymentTypes";
            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_employmenttypes_tbl_list");
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Add button to trigger add/edit page
        //*************************************************************************
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();
            AddNewRow();
        
            LabelAddEdit.Text = "Add New Record";
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD); ;
            FieldValidationColorChanged(false, "ALL");
            tbx_employment_type.Enabled = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_employment_type.Text = "";
            tbx_employment_description.Text = "";
            tbx_employment_type.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["employment_type"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "000";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["employment_type"].ToString();
            }

           
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("employment_type", typeof(System.String));
            dtSource.Columns.Add("employmenttype_description", typeof(System.String));
           
         
        }

        //*************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "employmenttypes_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "employment_type" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Triggers Delete Confirmation Pop-up Dialog Box
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
        //  BEGIN - AEC- 01/22/2019 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string deleteExpression = "employment_type = '" + svalues + "'";

            MyCmn.DeleteBackEndData("employmenttypes_tbl", "WHERE " + deleteExpression);

            DataRow[] row2Delete = dataListGrid.Select(deleteExpression);
            dataListGrid.Rows.Remove(row2Delete[0]);
            dataListGrid.AcceptChanges();
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalDelete();", true);
            up_dataListGrid.Update();
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Edit Row selection that will trigger edit page 
        //**************************************************************************
        protected void editRow_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string editExpression = "employment_type = '" + svalues + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();
         
            tbx_employment_type.Enabled = false;
            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["employment_type"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);

            tbx_employment_type.Text = svalues;
            tbx_employment_description.Text = row2Edit[0]["employmenttype_description"].ToString();
            tbx_employment_description.Focus();
            LabelAddEdit.Text = "Edit Record: " + tbx_employment_description.Text.Trim();
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);

            FieldValidationColorChanged(false, "ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //**************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Change Field Sort mode  
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
        //  BEGIN - AEC- 01/22/2019 - Get Grid current sort order 
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
        //  BEGIN - AEC- 01/22/2019 - Save New Record/Edited Record to back end DB
        //**************************************************************************
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string saveRecord = ViewState["AddEdit_Mode"].ToString();
            string scriptInsertUpdate = string.Empty;

            if (IsDataValidated())
            {
           

                if (saveRecord == MyCmn.CONST_ADD)
                {
                    dtSource.Rows[0]["employment_type"] = tbx_employment_type.Text.ToString().ToUpper();
                    dtSource.Rows[0]["employmenttype_description"] = tbx_employment_description.Text.ToString();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);

                }
                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["employment_type"] = tbx_employment_type.Text.ToString().ToUpper();
                    dtSource.Rows[0]["employmenttype_description"] = tbx_employment_description.Text.ToString();
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
                        nrow["employment_type"] = tbx_employment_type.Text.ToString().ToUpper();
                        nrow["employmenttype_description"] = tbx_employment_description.Text.ToString();
                       
                        dataListGrid.Rows.Add(nrow);
                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);
                 
                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        
                        string editExpression = "employment_type = '" + tbx_employment_type.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["employmenttype_description"] = tbx_employment_description.Text.ToString();
                        row2Edit[0]["employment_type"] = tbx_employment_type.Text.ToString().ToUpper();
                       
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
        //  BEGIN - AEC- 01/22/2019 - GridView Change Page Number
        //**************************************************************************
        protected void gridviewbind_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_dataListGrid.PageIndex = e.NewPageIndex;
            MyCmn.Sort(gv_dataListGrid, dataListGrid, ViewState["SortField"].ToString(), ViewState["SortOrder"].ToString());
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Change on Page Size (no. of row per page) on Gridview  
        //*********************************************************************************
        protected void DropDownListID_TextChanged(object sender, EventArgs e)
        {
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        protected void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "employment_type LIKE '%" + tbx_search.Text.Trim() + "%' OR employmenttype_description LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("employment_type", typeof(System.String));
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
        //  BEGIN - AEC- 01/22/2019 - Define Property for Sort Direction  
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
        //  BEGIN - AEC- 01/22/2019 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            if (tbx_employment_description.Text == "")
            {
                FieldValidationColorChanged(true, "tbx_employment_description");
                tbx_employment_description.Focus();
                validatedSaved = false;
            }

            return validatedSaved;
        }

        //**********************************************************************************************
        //  BEGIN - AEC- 01/22/2019 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        protected void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            if (pMode)
                switch (pObjectName)
                {
                    case "tbx_employment_description":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            tbx_employment_description.BorderColor = Color.Red;
                            break;
                        }
                    default:
                        break;
                }
            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "tbx_employment_description":
                        {
                            if (LblRequired1.Text != "")
                            {
                                LblRequired1.Text = "";
                                tbx_employment_description.BorderColor = Color.LightGray;
                            }
                            break;
                        }

                    case "ALL":
                        {
                            LblRequired1.Text = "";
                            tbx_employment_description.BorderColor = Color.LightGray;
                         
                            break;
                        }

                }
            }
        }
    }
}