//**********************************************************************************
// PROJECT NAME     :   HRIS - eComval
// VERSION/RELEASE  :   HRIS Release #1
// PURPOSE          :   Code Behind for Barangay Page
//**********************************************************************************
// REVISION HISTORY
//**********************************************************************************
// AUTHOR                    DATE            PURPOSE
//----------------------------------------------------------------------------------
// ARIEL CABUNGCAL (AEC)      09/09/2018      Code Creation
//**********************************************************************************
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using HRIS_Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Drawing;

namespace HRIS_eAdmin
{
    public partial class _default : System.Web.UI.Page
    {
        CommonDB MyCmn = new CommonDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ea_user_id"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
        }
    }
}