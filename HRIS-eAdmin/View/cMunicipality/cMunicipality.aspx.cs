//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Provinces Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// JORGE RUSTOM VILLANUEVA (JRV)   01/30/2019      Code Creation
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

namespace HRIS_eAdmin.View.cMunicipality
{
    public partial class cMunicipality : System.Web.UI.Page
    {
        //********************************************************************
        //  BEGIN - JRV- 01/30/2019 - Data Place holder creation 
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

        DataTable provincelist
        {
            get
            {
                if ((DataTable)ViewState["provincelist"] == null) return null;
                return (DataTable)ViewState["provincelist"];
            }
            set
            {
                ViewState["provincelist"] = value;
            }
        }

        DataTable municipalitieslist
        {
            get
            {
                if ((DataTable)ViewState["municipalitieslist"] == null) return null;
                return (DataTable)ViewState["municipalitieslist"];
            }
            set
            {
                ViewState["municipalitieslist"] = value;
            }
        }

        //********************************************************************
        //  BEGIN - JRV- 01/30/2019 - Public Variable used in Add/Edit Mode
        //********************************************************************

        CommonDB MyCmn = new CommonDB();

        //********************************************************************
        //  BEGIN - JRV- 09/09/2018 - Page Load method
        //********************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] != null)
            {

                if (!IsPostBack)
                {
                    InitializePage();
                    ViewState["SortField"] = "municipality_code";
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
        //  BEGIN - JRV- 01/30/2019 - Initialiazed Page 
        //********************************************************************
        private void InitializePage()
        {

            Session["sortdirection"] = SortDirection.Ascending.ToString();
            Session["cMunicipalty"] = "cMunicipalty";
            RetrieveDataListGrid();
        }

        //*************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Retrieve back end data and load to GridView
        //*************************************************************************
        private void RetrieveDataListGrid()
        {
            dataListGrid = MyCmn.RetrieveData("sp_municipalities_tbl_list");
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //*************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Add button to trigger add/edit page
        //*************************************************************************
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEntry();
            InitializeTable();
            AddPrimaryKeys();
            AddNewRow();
            RetrieveBindingProvince();


            LabelAddEdit.Text = "Add New Record";
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_ADD); ;
            FieldValidationColorChanged(false, "ALL");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //*************************************************************************
        //  BEGIN - JRV - 01/30/2019 - Clear Add/Edit Page Fields
        //*************************************************************************
        private void ClearEntry()
        {
            tbx_municipalitycode.Text = "";
            tbx_municipalityname.Text = "";

            tbx_municipalitycode.ReadOnly = false;
            tbx_municipalitycode.Focus();
        }

        //*************************************************************************
        //  BEGIN - AEC- 09/09/2018 - Add new row to datatable object
        //*************************************************************************
        private void AddNewRow()
        {
            DataRow nrow = dtSource.NewRow();
            nrow["municipality_code"] = string.Empty;
            nrow["action"] = 1;
            nrow["retrieve"] = false;
            dtSource.Rows.Add(nrow);

            int dtRowCont = dataListGrid.Rows.Count - 1;
            string lastCode = "000";

            if (dtRowCont > -1)
            {
                DataRow lastRow = dataListGrid.Rows[dtRowCont];
                lastCode = lastRow["municipality_code"].ToString();
            }

            int lastCodeInt = int.Parse(lastCode) + 1;
            string nextCode = lastCodeInt.ToString();
            nextCode = nextCode.PadLeft(3, '0');

            tbx_municipalitycode.Text = nextCode;
        }

        //*************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Initialized datasource fields/columns
        //*************************************************************************
        private void InitializeTable()
        {
            dtSource = new DataTable();
            dtSource.Columns.Add("municipality_code", typeof(System.String));
            dtSource.Columns.Add("municipality_name", typeof(System.String));
            dtSource.Columns.Add("province_code", typeof(System.String));
        }

        //*************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Add Primary Key Field to datasource
        //*************************************************************************
        private void AddPrimaryKeys()
        {
            dtSource.TableName = "municipalities_tbl";
            dtSource.Columns.Add("action", typeof(System.Int32));
            dtSource.Columns.Add("retrieve", typeof(System.Boolean));
            string[] col = new string[] { "municipality_code" };
            dtSource = MyCmn.AddPrimaryKeys(dtSource, col);
        }

        //***************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Triggers Delete Confirmation Pop-up Dialog Box
        //***************************************************************************
        protected void deleteRow_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string brgcode = commandArgs[0];
            string brgname = commandArgs[1];

            deleteRec1.Text = "Are you sure to delete this Municipality = (" + brgcode.Trim() + ") - " + brgname.Trim() + " ?";
            lnkBtnYes.CommandArgument = brgcode;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //*************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Delete Data to back-end Database
        //*************************************************************************
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string deleteExpression = "municipality_code = '" + svalues + "'";

            MyCmn.DeleteBackEndData("municipalities_tbl", "WHERE " + deleteExpression);

            DataRow[] row2Delete = dataListGrid.Select(deleteExpression);
            dataListGrid.Rows.Remove(row2Delete[0]);
            dataListGrid.AcceptChanges();
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalDelete();", true);
            up_dataListGrid.Update();
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**************************************************************************
        //  BEGIN - JRV - 01/30/2019 - Edit Row selection that will trigger edit page 
        //**************************************************************************
        protected void editRow_Command(object sender, CommandEventArgs e)
        {
            string svalues = e.CommandArgument.ToString();
            string editExpression = "municipality_code = '" + svalues + "'";

            DataRow[] row2Edit = dataListGrid.Select(editExpression);

            ClearEntry();
            RetrieveBindingProvince();
            InitializeTable();
            AddPrimaryKeys();
            DataRow nrow = dtSource.NewRow();
            nrow["municipality_code"] = string.Empty;
            nrow["action"] = 2;
            nrow["retrieve"] = true;
            dtSource.Rows.Add(nrow);

            tbx_municipalitycode.Text = svalues;
            tbx_municipalityname.Text = row2Edit[0]["municipality_name"].ToString();
            ddl_province.SelectedValue = row2Edit[0]["province_code"].ToString();


            tbx_municipalitycode.ReadOnly = true;
            tbx_municipalityname.Focus();
            LabelAddEdit.Text = "Edit Record: " + tbx_municipalityname.Text.Trim();
            ViewState.Add("AddEdit_Mode", MyCmn.CONST_EDIT);

            FieldValidationColorChanged(false, "ALL");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        //**************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Change Field Sort mode  
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
        //  BEGIN - JRV- 01/30/2019 - Get Grid current sort order 
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
        //  BEGIN - JRV - 01/30/2019 - Save New Record/Edited Record to back end DB
        //**************************************************************************
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string saveRecord = ViewState["AddEdit_Mode"].ToString();

            string scriptInsertUpdate = string.Empty;
            if (IsDataValidated())
            {
                if (saveRecord == MyCmn.CONST_ADD)
                {
                    dtSource.Rows[0]["municipality_code"] = tbx_municipalitycode.Text.ToString();
                    dtSource.Rows[0]["municipality_name"] = tbx_municipalityname.Text.ToString();
                    dtSource.Rows[0]["province_code"] = ddl_province.SelectedValue.ToString();
                    scriptInsertUpdate = MyCmn.get_insertscript(dtSource);
                }

                else if (saveRecord == MyCmn.CONST_EDIT)
                {
                    dtSource.Rows[0]["municipality_code"] = tbx_municipalitycode.Text.ToString();
                    dtSource.Rows[0]["municipality_name"] = tbx_municipalityname.Text.ToString();
                    dtSource.Rows[0]["province_code"] = ddl_province.SelectedValue.ToString();
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
                        nrow["municipality_code"] = tbx_municipalitycode.Text.ToString();
                        nrow["municipality_name"] = tbx_municipalityname.Text.ToString();
                        nrow["province_code"] = ddl_province.SelectedValue.ToString();
                        dataListGrid.Rows.Add(nrow);

                        gv_dataListGrid.SetPageIndex(gv_dataListGrid.PageCount);
                       

                        SaveAddEdit.Text = MyCmn.CONST_NEWREC;
                    }
                    if (saveRecord == MyCmn.CONST_EDIT)
                    {
                        string editExpression = "municipality_code = '" + tbx_municipalitycode.Text.ToString() + "'";
                        DataRow[] row2Edit = dataListGrid.Select(editExpression);
                        row2Edit[0]["municipality_name"] = tbx_municipalityname.Text.ToString();
                        row2Edit[0]["province_code"] = ddl_province.SelectedValue.ToString();
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
                string searchExpression = "municipality_code LIKE '%" + tbx_search.Text.Trim() + "%' OR municipality_name LIKE '%" + tbx_search.Text.Trim() + "%' OR province_name LIKE '%" + tbx_search.Text.Trim() + "%'";

                DataTable dtSource1 = new DataTable();
                dtSource1.Columns.Add("municipality_code", typeof(System.String));
                dtSource1.Columns.Add("municipality_name", typeof(System.String));
                dtSource1.Columns.Add("province_name", typeof(System.String));

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
            tbx_search.Focus();
        }





        //**************************************************************************
        //  BEGIN - JRV- 01/30/2019 - GridView Change Page Number
        //**************************************************************************
        protected void gridviewbind_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_dataListGrid.PageIndex = e.NewPageIndex;
            MyCmn.Sort(gv_dataListGrid, dataListGrid, ViewState["SortField"].ToString(), ViewState["SortOrder"].ToString());
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Change on Page Size (no. of row per page) on Gridview  
        //*********************************************************************************
        protected void DropDownListID_TextChanged(object sender, EventArgs e)
        {
            gv_dataListGrid.PageSize = Convert.ToInt32(DropDownListID.Text);
            CommonCode.GridViewBind(ref this.gv_dataListGrid, dataListGrid);
            show_pagesx.Text = "Page: <b>" + (gv_dataListGrid.PageIndex + 1) + "</b>/<strong style='color:#B7B7B7;'>" + gv_dataListGrid.PageCount + "</strong>";
        }

        //**********************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Search Data Bind to Grid View on every KeyInput  
        //*********************************************************************************
        protected void tbx_search_TextChanged(object sender, EventArgs e)
        {
            string searchExpression = "municipality_code LIKE '%" + tbx_search.Text.Trim() + "%' OR municipality_name LIKE '%" + tbx_search.Text.Trim() + "%' OR province_name LIKE '%" + tbx_search.Text.Trim() + "%'";

            DataTable dtSource1 = new DataTable();
            dtSource1.Columns.Add("municipality_code", typeof(System.String));
            dtSource1.Columns.Add("municipality_name", typeof(System.String));
            dtSource1.Columns.Add("province_name", typeof(System.String));

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


        //**********************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Bind Municipality Dropdownlist for selected Province  
        //*********************************************************************************
        protected void ddl_province_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (ddl_province.SelectedValue.ToString() == "")
            {
                FieldValidationColorChanged(false, "ddl_province");
                ddl_province.Focus();
            }
        }

        //**************************************************************************
        //  BEGIN - AEC- 01/30/2019 - Populate Combo list from Province table
        //*************************************************************************
        private void RetrieveBindingProvince()
        {
            ddl_province.Items.Clear();
            DataTable dt = MyCmn.RetrieveData("sp_provinces_tbl_list");

            ddl_province.DataSource = dt;
            ddl_province.DataTextField = "province_name";
            ddl_province.DataValueField = "province_code";
            ddl_province.DataBind();
            ListItem li = new ListItem("-- Select Here --", "");
            ddl_province.Items.Insert(0, li);
        }




        //**************************************************************************
        //  BEGIN - JRV- 001/30/2019 - Define Property for Sort Direction  
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
        //  BEGIN - JRV- 01/30/2019 - Objects data Validation
        //*************************************************************************
        private bool IsDataValidated()
        {
            bool validatedSaved = true;

            FieldValidationColorChanged(false, "ALL");

            if (tbx_municipalityname.Text.ToString() == "")
            {
                FieldValidationColorChanged(true, "tbx_municipalityname");
                tbx_municipalityname.Focus();
                validatedSaved = false;
            }

            else if (ddl_province.Text.ToString() == "")
            {
                FieldValidationColorChanged(true, "ddl_province");
                ddl_province.Focus();
                validatedSaved = false;
            }

            return validatedSaved;
        }

        //**********************************************************************************************
        //  BEGIN - JRV- 01/30/2019 - Change/Toggle Mode for Object Appearance during validation  
        //**********************************************************************************************
        protected void FieldValidationColorChanged(bool pMode, string pObjectName)
        {
            if (pMode)
                switch (pObjectName)
                {
                    case "tbx_municipalityname":
                        {
                            LblRequired1.Text = MyCmn.CONST_RQDFLD;
                            tbx_municipalityname.BorderColor = Color.Red;
                            tbx_municipalityname.Focus();
                            break;
                        }

                    case "ddl_province":
                        {
                            LblRequired2.Text = MyCmn.CONST_RQDFLD;
                            ddl_province.BorderColor = Color.Red;
                            ddl_province.Focus();
                            break;
                        }
                }

            else if (!pMode)
            {
                switch (pObjectName)
                {
                    case "tbx_municipalityname":
                        {
                            LblRequired1.Text = "";
                            tbx_municipalityname.BorderColor = Color.LightGray;
                            break;
                        }


                    case "ALL":
                        {
                            LblRequired1.Text = "";
                            LblRequired2.Text = "";
                            tbx_municipalityname.BorderColor = Color.LightGray;
                            ddl_province.BorderColor = Color.LightGray;
                            break;
                        }

                }
            }
        }
    }
}