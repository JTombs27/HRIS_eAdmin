using System;
using System.Web;
using System.Configuration;
using System.Globalization;
using image = System.Drawing.Image;
using System.IO;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;

namespace HRIS_Common
{
    public class CommonCode
    {
        enum typesof
        {
            textbox,
            label,
            dropdropdownlist,
            radiobuttomlist,
            buttom
        }

        enum photoexttype
        {
            png,
            jpeg,
            gif,
            bmp,
            tiff
        }
        enum ddl_nationality
        {

        }
        enum gvboolean
        {
            FALSE = 0
        }
        enum convertoboolean
        {
            FALSE = 0,
            TRUE = 1
        }
        public static bool inttoboolean(int value)
        {
            return value == 0 ? false : true;
        }
        public static int booleantoint(bool value)
        {
            return value ? 1 : 0;
        }
        public static string userlogin;

        public static string[] paytypename = new string[] { "D", "WEEK", "SM", "M" };
        //public static string[] monthname = new string[] { "January", "February", "March", "April","May","June","July","August","September","October","November","December"};
        string[] phototype = new string[] { "image/png", "image/jpeg", "image/jif" };
        public static string[] dayname = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        public static string[] leavetype = new string[] { "VL", "SL", "ML", "PL", "SI", "BL", "EL" };
        public static Boolean[] truefalse = new Boolean[] { false, true };
        public static string photoemploloc = "~/hr/photoemp/";
        public static string photomenu = "~/photo/";
        public static string adminpwd = "stymike09141968";
        public static string subject_class = "CL-001";
        public static string assessment_class = "CL-001";
        public static void monthdropdown(ref DropDownList ddl)
        {
            string[] monthname = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for (int i = 0; i < 12; i++)
            {
                ddl.Items.Insert(i, new ListItem(monthname[i], (i + 1).ToString()));
            }

        }

        public static Boolean SavePhotoToFolder(string FileName, byte[] Data)

        {
            BinaryWriter Writer = null;
            string Name = @"D:\stymicz_sofware\stymiczwebsite\stymiczwebsite\photoemp\m1.png";

            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));

                // Writer raw data                
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }

            return true;
        }
        public static int country_id()
        {
            int id = Convert.ToInt32(ConfigurationManager.ConnectionStrings["countryid"].ConnectionString);
            return id;
        }
        public static string compcode()
        {
            string comcode = ConfigurationManager.ConnectionStrings["comp_code"].ConnectionString;
            return comcode;
        }
        public static Stream getimage(byte[] parmvalue)
        {
            MemoryStream ms = new MemoryStream(parmvalue);
            return ms;
        }
        public static byte[] photoupload(FileUpload upload)
        {
            try
            {
                int length = upload.PostedFile.ContentLength;
                byte[] pic = new byte[length];
                upload.PostedFile.InputStream.Read(pic, 0, length);
                return pic;
            }
            catch (Exception)
            {
                return null;

            }
        }

        //}
        public static string savephotofolder(FileUpload upload, string location)
        {
            try
            {
                string imgName = location + upload.FileName;
                upload.SaveAs(System.Web.HttpContext.Current.Server.MapPath(imgName));
                return imgName;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static Boolean Checkimagetype(string imagetype)
        {
            photoexttype namevalue = (photoexttype)Enum.Parse(typeof(photoexttype), imagetype);

            if (namevalue == photoexttype.jpeg) return true;
            if (namevalue == photoexttype.png) return true;
            if (namevalue == photoexttype.gif) return true;
            if (namevalue == photoexttype.bmp) return true;
            if (namevalue == photoexttype.tiff) return true;

            return false;
        }
        public static int lastday(int year, int month)
        {
            int[] lastdayofmonth = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            if (month == 2)
            {
                if (year % 4 == 0)
                {
                    return 29;
                }

            }
            int val = lastdayofmonth[month - 1];
            return val;
        }
        public static int gvbool(string parmbool)
        {
            gvboolean lb_bol = (gvboolean)Enum.Parse(typeof(gvboolean), parmbool.ToUpper());
            int retvalue = 1;
            if (lb_bol == gvboolean.FALSE)
            {
                retvalue = 0;
            }
            return retvalue;
        }
        public static void getclient(ref string comp_code, ref string branch_code, ref int franchise)
        {

            comp_code = ConfigurationManager.ConnectionStrings["comp_code"].ConnectionString;
            branch_code = ConfigurationManager.ConnectionStrings["branch_code"].ConnectionString;
            franchise = Convert.ToInt32(ConfigurationManager.ConnectionStrings["franchise"].ConnectionString);
        }
        public static int lastdayofmonth(int year, int months)
        {
            int lastday;

            if (months == 1)
            {
                lastday = 31;
            }
            else if (months == 2)
            {

                if ((year % 4) == 0)
                {
                    lastday = 29;
                }
                else
                {
                    lastday = 28;
                };
            }
            else if (months == 3)
            {
                lastday = 31;
            }
            else if (months == 4)
            {
                lastday = 30;
            }
            else if (months == 5)
            {
                lastday = 31;
            }
            else if (months == 6)
            {
                lastday = 30;
            }
            else if (months == 7)
            {
                lastday = 31;
            }
            else if (months == 8)
            {
                lastday = 31;
            }
            else if (months == 9)
            {
                lastday = 30;
            }
            else if (months == 10)
            {
                lastday = 31;
            }
            else if (months == 11)
            {
                lastday = 30;
            }
            else if (months == 12)
            {
                lastday = 31;
            }
            else
            {
                lastday = 0;
            }
            return lastday;

        }
        public static Boolean onemonthallowed(DateTime dt_from, DateTime dt_to)
        {
            Boolean retvalue;
            retvalue = false;

            TimeSpan day = dt_to.Subtract(dt_from);
            int months = Convert.ToInt32(dt_from.Month.ToString());
            int years = Convert.ToInt32(dt_from.Year.ToString());
            int lastday = lastdayofmonth(years, months);

            int nodays = Convert.ToInt32(day.TotalDays.ToString()) + 1;
            if (nodays <= lastday)
            {
                retvalue = true;
            }
            return retvalue;
        }
        public static string monthname(int months)
        {
            string name;

            if (months == 1)
            {
                name = "January";
            }
            else if (months == 2)
            {
                name = "February";
            }
            else if (months == 3)
            {
                name = "March";
            }
            else if (months == 4)
            {
                name = "April";
            }
            else if (months == 5)
            {
                name = "May";
            }
            else if (months == 6)
            {
                name = "June";
            }
            else if (months == 7)
            {
                name = "July";
            }
            else if (months == 8)
            {
                name = "August";
            }
            else if (months == 9)
            {
                name = "September";
            }
            else if (months == 10)
            {
                name = "October";
            }
            else if (months == 11)
            {
                name = "November";
            }
            else if (months == 12)
            {
                name = "December";
            }
            else
            {
                name = "";
            }
            return name;
        }
        public static CultureInfo ResolveCulture()
        {
            string[] languages = HttpContext.Current.Request.UserLanguages;

            if (languages == null || languages.Length == 0)
                return null;

            try
            {
                string language = languages[0].ToLowerInvariant().Trim();
                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
        public static RegionInfo ResolveCountry()
        {
            CultureInfo culture = ResolveCulture();
            if (culture != null)
                return new RegionInfo(culture.LCID);

            return null;
        }
        public static void displaydropdownlist(ref DropDownList ddl, string values)
        {
            ddl.ClearSelection();
            if ((values == null) || (values == ""))
            {
                try
                {
                    ddl.Items.FindByValue("0").Selected = false;
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    ddl.Items.FindByValue(values).Selected = true;
                }
                catch (Exception)
                {
                    //    ddl.Items.FindByValue("0").Selected = true;
                }

            }
        }
        public static void displayradiobuttomlist(ref RadioButtonList rbl, string values)
        {
            rbl.ClearSelection();
            if ((values == null) || (values == ""))
            {
                //  rbl.Items.FindByValue("0").Selected = false;
            }
            else
            {
                try
                {
                    rbl.Items.FindByValue(values).Selected = true;
                }
                catch (Exception)
                {

                }

            }
        }
        public static string dropdownlistvalue(DropDownList ddl)
        {
            try
            {
                int index = ddl.SelectedIndex;

                return ddl.Items[index].Value.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string dropdownlisttextvalue(DropDownList ddl)
        {
            try
            {
                int index = ddl.SelectedIndex;
                return ddl.Items[index].Text;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string radionbottomlistvalue(RadioButtonList rbl)
        {
            try
            {
                int index = rbl.SelectedIndex;
                return rbl.Items[index].Value.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string radionbottomlisttextvalue(RadioButtonList rbl)
        {
            try
            {
                int index = rbl.SelectedIndex;
                return rbl.Items[index].Text;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Boolean checkisdecimal(TextBox tb)
        {
            Boolean retvalue = true;
            decimal parm;
            retvalue = decimal.TryParse(tb.Text, out parm);

            return retvalue;
        }

        public static Boolean isnumber(string svalue)
        {
            Boolean retvalue = true;
            int parm;
            retvalue = int.TryParse(svalue, out parm);
            return retvalue;
        }
        public static Boolean isCheckNumber(TextBox tb)
        {
            Boolean retvalue = true;
            int parm;
            retvalue = int.TryParse(tb.Text, out parm);

            return retvalue;
        }
        public static Boolean checkisdatetime(TextBox tb)
        {
            Boolean retvalue = true;
            DateTime parm;
            retvalue = DateTime.TryParse(tb.Text, out parm);
            return retvalue;
        }
        public static Boolean checkisdatetime(string msg)
        {
            Boolean retvalue = true;
            DateTime parm;
            retvalue = DateTime.TryParse(msg, out parm);
            return retvalue;
        }
        public static void showmessage(ref Label lb, string msg, Boolean parmbool)
        {
            lb.Text = msg;
            lb.Visible = parmbool;
        }
        public static DateTime fromdate(DateTime dateend, double valueparm)
        {
            DateTime retvalue;
            double tempvar = -1 * valueparm;
            retvalue = dateend.AddDays(tempvar);
            return retvalue;
        }
        public static DateTime todate(DateTime fromdate, double valueparm)
        {
            DateTime retvalue;
            double tempvar = valueparm;
            retvalue = fromdate.AddDays(tempvar);
            return retvalue;
        }

        public static string whereclause(Boolean nobranch)
        {
            string whereclause = "where ";
            //  whereclause = whereclause + " comp_code = '" + comp_code + "' and";
            if (nobranch)
            {
                whereclause = whereclause + " branch_cOde = 'X' and";
                whereclause = whereclause + " franchise = 0";
            }
            else
            {
                //whereclause = whereclause + " branch_cOde = '" + branch_code + "' and";
                //whereclause = whereclause + " franchise = " + franchise.ToString();
            }
            return whereclause;

        }
        public static void GetImageDimension(FileUpload img, ref int h, ref int w)
        {
            System.IO.Stream stream = img.PostedFile.InputStream;
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

            int height = image.Height;
            int width = image.Width;

            h = height;
            w = width;
        }
        public static Boolean get_imagefromupload(FileUpload img, ref System.Web.UI.WebControls.Image image, string location)
        {

            Boolean retvalue = false;
            if (img.HasFile)
            {
                byte[] photo = null;
                string imgloc = "";
                string contentype = "";

                if (Checkimagetype(img.PostedFile.ContentType.Substring(6)))
                {
                    photo = photoupload(img);

                    if (photo != null)
                    {
                        retvalue = true;

                        //  resizeimage.getImageFromBytes(photo);

                        imgloc = CommonCode.savephotofolder(img, location);

                        if (imgloc != null)
                        {
                            contentype = img.PostedFile.ContentType;
                            image.ImageUrl = imgloc;
                            //   string tostringvar = CommonCode.ConvertBytesToString(photo);
                            // image.ImageUrl = "~/photoprocessbytes.ashx?id=" + contentype + "@" + tostringvar;

                        }
                    }
                }


            }

            return retvalue;
        }
        public static string cropimage(string pathfile, string destpath, int x, int y, int w, int h)
        {
            //image outputfile = image.FromFile(pathfile);
            Rectangle cropcoordinate = new Rectangle(x, y, w, h);
            //      string confilename;
            string confilepath = "";
            //        Bitmap bitmap = new Bitmap(cropcoordinate.Width, cropcoordinate.Height, outputfile.PixelFormat);
            //        Graphics grapics = Graphics.FromImage(bitmap);
            //        grapics.DrawImage(outputfile, new Rectangle(0, 0, bitmap.Width, bitmap.Height), cropcoordinate, GraphicsUnit.Pixel);
            ////confilename = filname;
            //        confilepath = destpath;
            //        bitmap.Save(confilepath);
            return "";
            //return confilepath;
        }

        public static Boolean get_imagefromupload(FileUpload img, ref System.Web.UI.WebControls.Image image, ref DataTable dt)
        {

            Boolean retvalue = false;
            adddatatablecolumn(ref dt);
            if (img.HasFile)
            {
                byte[] photo = null;
                string imgloc = "";
                string contentype = "";

                if (Checkimagetype(img.PostedFile.ContentType.Substring(6)))
                {
                    photo = photoupload(img);

                    if (photo != null)
                    {
                        retvalue = true;
                        imgloc = CommonCode.savephotofolder(img, CommonCode.photoemploloc);

                        if (imgloc != null)
                        {
                            contentype = img.PostedFile.ContentType;
                            image.ImageUrl = imgloc;
                            //   string tostringvar = CommonCode.ConvertBytesToString(photo);
                            // image.ImageUrl = "~/photoprocessbytes.ashx?id=" + contentype + "@" + tostringvar;
                            if (dt != null)
                            {
                                DataRow dtrow = dt.NewRow();
                                dtrow["photo"] = photo;
                                dtrow["location"] = imgloc;
                                dtrow["phototype"] = contentype;
                                dt.Rows.Add(dtrow);
                            }
                        }
                    }
                }


            }

            return retvalue;
        }
        private static void adddatatablecolumn(ref DataTable dt)
        {
            dt = new DataTable();
            dt.Columns.Add("photo", typeof(byte[]));
            dt.Columns.Add("location", typeof(string));
            dt.Columns.Add("phototype", typeof(string));

        }
        public static string ConvertBytesToString(byte[] bytes)
        {
            string output = String.Empty;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    output = reader.ReadToEnd();
                }
            }
            return output;
        }
        public static byte[] ToByteArray(string input)
        {
            byte[] outByte = new byte[] { };
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(input);
                    writer.Flush();
                }
                outByte = stream.ToArray();
            }
            return outByte;
        }
        public static string gettextvalue(object sender)
        {

            if (sender.GetType() == typeof(TextBox))
            {
                TextBox tb = (TextBox)sender;
                return tb.Text;
            }
            if (sender.GetType() == typeof(Label))
            {
                Label tb = (Label)sender;
                return tb.Text;
            }
            if (sender.GetType() == typeof(DropDownList))
            {
                DropDownList ddl = (DropDownList)sender;
                return dropdownlistvalue(ddl);
            }
            if (sender.GetType() == typeof(RadioButtonList))
            {
                RadioButtonList rbl = (RadioButtonList)sender;
                return radionbottomlistvalue(rbl);
            }
            return "";
        }

        public static void setobjectvalue(ref object sender, string valueparm)
        {

            if (sender.GetType() == typeof(TextBox))
            {
                TextBox tb = (TextBox)sender;
                tb.Text = valueparm;
            }
            if (sender.GetType() == typeof(Label))
            {
                Label tb = (Label)sender;
                tb.Text = valueparm;

            }
            if (sender.GetType() == typeof(DropDownList))
            {
                DropDownList ddl = (DropDownList)sender;
                displaydropdownlist(ref ddl, valueparm);
            }
            if (sender.GetType() == typeof(RadioButtonList))
            {
                RadioButtonList rbl = (RadioButtonList)sender;
                displayradiobuttomlist(ref rbl, valueparm);
            }
            if (sender.GetType() == typeof(System.Web.UI.WebControls.Image))
            {
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)sender;
                img.ImageUrl = valueparm;
            }
        }
        public static Boolean objectisempty(object sender)
        {

            if (sender.GetType() == typeof(TextBox))
            {
                TextBox tb = (TextBox)sender;
                if (tb.Text.Trim().Length == 0)
                {
                    return true;
                }
            }
            if (sender.GetType() == typeof(Label))
            {
                Label tb = (Label)sender;
                if (tb.Text.Trim().Length == 0)
                {
                    return true;
                }

            }
            if (sender.GetType() == typeof(DropDownList))
            {
                DropDownList ddl = (DropDownList)sender;
                if (dropdownlistvalue(ddl) == "0")
                {
                    return true;
                }
            }
            if (sender.GetType() == typeof(RadioButtonList))
            {
                RadioButtonList rbl = (RadioButtonList)sender;
                if (rbl == null)
                {
                    return true;
                }
            }

            return false;
        }

        public static void setobjectvalue(ref object[] sender, string strparm)
        {
            for (int i = 0; i < sender.Length; i++)
            {
                if (sender[i].GetType() == typeof(TextBox))
                {
                    TextBox tb = (TextBox)sender[i];
                    object obj = tb;
                    setobjectvalue(ref obj, strparm);
                }
                if (sender[i].GetType() == typeof(Label))
                {
                    Label tb = (Label)sender[i];
                    object obj = tb;
                    setobjectvalue(ref obj, strparm);

                }
                if (sender[i].GetType() == typeof(DropDownList))
                {
                    DropDownList tb = (DropDownList)sender[i];
                    object obj = tb;
                    setobjectvalue(ref obj, "0");
                }
                if (sender.GetType() == typeof(RadioButtonList))
                {
                    RadioButtonList tb = (RadioButtonList)sender[i];
                    object obj = tb;
                    setobjectvalue(ref obj, "0");
                }
                if (sender[i].GetType() == typeof(System.Web.UI.WebControls.Image))
                {
                    System.Web.UI.WebControls.Image tb = (System.Web.UI.WebControls.Image)sender[i];
                    object obj = tb;
                    setobjectvalue(ref obj, strparm);
                }
            }
        }

        public static void objectenabled(ref object[] sender, Boolean boolparm)
        {
            for (int i = 0; i < sender.Length; i++)
            {
                if (sender[i].GetType() == typeof(TextBox))
                {
                    TextBox tb = (TextBox)sender[i];
                    object obj = tb;
                    objectenabled(ref obj, boolparm);

                }
                if (sender[i].GetType() == typeof(Label))
                {
                    Label tb = (Label)sender[i];
                    object obj = tb;
                    objectenabled(ref obj, boolparm);

                }
                if (sender[i].GetType() == typeof(DropDownList))
                {
                    DropDownList tb = (DropDownList)sender[i];
                    object obj = tb;
                    objectenabled(ref obj, boolparm);
                }
                if (sender[i].GetType() == typeof(RadioButtonList))
                {
                    RadioButtonList tb = (RadioButtonList)sender[i];
                    object obj = tb;
                    objectenabled(ref obj, boolparm);
                }
                if (sender[i].GetType() == typeof(ImageButton))
                {
                    ImageButton tb = (ImageButton)sender[i];
                    object obj = tb;
                    objectenabled(ref obj, boolparm);
                }
            }
        }

        internal static void objectenabled(ref object sender, bool p)
        {

            if (sender.GetType() == typeof(TextBox))
            {
                TextBox tb = (TextBox)sender;
                tb.Enabled = p;
            }
            if (sender.GetType() == typeof(Label))
            {
                Label tb = (Label)sender;
                tb.Enabled = p;

            }
            if (sender.GetType() == typeof(DropDownList))
            {
                DropDownList tb = (DropDownList)sender;
                tb.Enabled = p;
            }
            if (sender.GetType() == typeof(RadioButtonList))
            {
                RadioButtonList tb = (RadioButtonList)sender;
                tb.Enabled = p;
            }
            if (sender.GetType() == typeof(ImageButton))
            {
                ImageButton tb = (ImageButton)sender;
                tb.Enabled = p;
            }

        }
        public static void gridview_RowCreated(object sender, GridViewRowEventArgs e, ref GridView dv)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Pager)
                {
                    DropDownList ddlPage = (DropDownList)e.Row.FindControl("ddlPage");
                    LinkButton lnkprevious = (LinkButton)e.Row.FindControl("lnkPrevious");
                    LinkButton lnknext = (LinkButton)e.Row.FindControl("lnkNext");
                    LinkButton lnkfirst = (LinkButton)e.Row.FindControl("lnkfirst");
                    LinkButton lnklast = (LinkButton)e.Row.FindControl("lnkLast");
                    int pageindex = dv.PageIndex;
                    int PageCount = dv.PageCount;

                    if (pageindex == 0)
                    {
                        lnkprevious.Visible = false;
                        lnkfirst.Visible = false;
                    }
                    else
                    {
                        lnkprevious.Visible = true;
                        lnkfirst.Visible = true;

                    }
                    if (pageindex == PageCount - 1)
                        lnknext.Visible = false;
                    else
                        lnknext.Visible = true;

                    if (pageindex >= PageCount - 1)
                    {
                        lnklast.Visible = false;
                        lnknext.Visible = false;
                    }
                    else
                    {
                        lnknext.Visible = true;
                        lnklast.Visible = true;

                    }

                    ddlPage.Items.Clear();
                    for (int i = 1; i <= PageCount; i++)
                        ddlPage.Items.Add(i.ToString());
                    ddlPage.SelectedIndex = pageindex;
                }
            }
            catch (Exception ex)
            {
                ///  WriteLog.writeLog(ex, this);
            }
        }

        internal static void setgridviewpagefromddl(object sender, ref GridView gv_tablist, DataTable dt)
        {
            DropDownList ddl = (DropDownList)sender;
            gv_tablist.PageIndex = Convert.ToInt32(dropdownlistvalue(ddl));
            GridViewBind(ref gv_tablist, dt);
        }

        public static void GridViewBind(ref GridView gv, DataTable dt)
        {

            gv.DataSource = dt;
            gv.DataBind();
        }

        internal static void gridview_oncommandindexpagechange(ref object sender, GridViewCommandEventArgs e, DataTable productview)
        {
            GridView gv_tablist = (GridView)sender;

            if (e.CommandName.ToLower() == "first")
            {
                gv_tablist.PageIndex = 0;
                GridViewBind(ref gv_tablist, productview);

            }
            else if (e.CommandName.ToLower() == "next")
            {
                if (gv_tablist.PageIndex <= gv_tablist.PageCount)
                {
                    gv_tablist.PageIndex = gv_tablist.PageIndex + 1;
                    GridViewBind(ref gv_tablist, productview);
                }
            }
            else if (e.CommandName.ToLower() == "previous")
            {
                if (gv_tablist.PageIndex > 0)
                {
                    gv_tablist.PageIndex = gv_tablist.PageIndex - 1;
                    GridViewBind(ref gv_tablist, productview);
                }
            }
            else if (e.CommandName.ToLower() == "last")
            {
                gv_tablist.PageIndex = gv_tablist.PageCount;
                GridViewBind(ref gv_tablist, productview);
            }

        }
        public static Boolean toboolean(string parm)
        {
            if (parm == "1")
                return true;
            return false;
        }
        public static Boolean toboolean(int parm)
        {
            if (parm == 1)
                return true;
            return false;
        }
        public static String sendmail(string fromaddress, string frompwd, string toaddress, string subject, string body)
        {
            //var fromaddress = "noreply@stymicz-co.com";
            //var toaddress = send_address.Text.ToString();

            //const string topassword = "noreply@2016";
            //string subject = send_subject.Text.ToString();
            //body = "From: " + fromaddress + "\n";
            //body += "Email: " + send_address.Text + "\n\n";
            //body += "Subject: " + send_subject.Text + "\n\n";
            //body += "Questions/Comment: \n" + send_msg.Text + "\n";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromaddress);
                mail.To.Add(toaddress);
                mail.Subject = subject;
                mail.Body = body;

                SmtpClient smtp = new SmtpClient("mail.stymicz-co.com", 25);
                NetworkCredential credentials = new NetworkCredential(fromaddress, frompwd);
                smtp.Credentials = credentials;
                smtp.Send(mail);
                return "Successfully Send";
            }
            catch (Exception)
            {
                return "Error in Send mail";
            }
        }
        public static string generatepassword(int max)
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < max; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }

        public static string encrypt(string stringvalue)
        {
            string _result = string.Empty;
            char[] temp = stringvalue.ToCharArray();
            foreach (var _singleChar in temp)
            {
                var i = (int)_singleChar;
                i = i + 3;
                _result += (char)i;
            }
            return _result;
        }
        public static string decrypt(string stringvalue)
        {
            string _result = string.Empty;
            char[] temp = stringvalue.ToCharArray();
            foreach (var _singleChar in temp)
            {
                var i = (int)_singleChar;
                i = i - 3;
                _result += (char)i;
            }
            return _result;
        }
        public static string militarytime(DateTime dt)
        {
            string timevalue = dt.ToString("hh\\:mm");
            string tvalues = "";
            string[] svalue = dt.ToString().Split(' ');
            if (svalue[2] == "PM")
            {
                svalue = timevalue.Split(':');
                if (svalue[0] == "12")
                {
                    tvalues = svalue[0] + ":" + svalue[1];
                }
                else
                {
                    tvalues = Convert.ToString(Convert.ToInt32(svalue[0]) + 12) + ":" + svalue[1];
                }
                timevalue = tvalues;
            }
            else if (svalue[2] == "AM")
            {
                svalue = timevalue.Split(':');
                if (svalue[0] == "12")
                {
                    tvalues = "00:" + svalue[1];
                }
                else
                {
                    tvalues = svalue[0] + ":" + svalue[1];
                }
                timevalue = tvalues;
            }
            return timevalue;
        }
        public static string militarytime(TextBox tbx)
        {
            if (tbx.Text.Trim() == "") return "";
            DateTime dt = Convert.ToDateTime(tbx.Text);
            string timevalue = dt.ToString("hh\\:mm");
            string tvalues = "";
            string[] svalue = dt.ToString().Split(' ');
            if (svalue[2] == "PM")
            {
                svalue = timevalue.Split(':');
                if (svalue[0] == "12")
                {
                    tvalues = svalue[0] + ":" + svalue[1];
                }
                else
                {
                    tvalues = Convert.ToString(Convert.ToInt32(svalue[0]) + 12) + ":" + svalue[1];
                }
                timevalue = tvalues;
            }
            else if (svalue[2] == "AM")
            {
                svalue = timevalue.Split(':');
                if (svalue[0] == "12")
                {
                    tvalues = "00:" + svalue[1];
                }
                else
                {
                    tvalues = svalue[0] + ":" + svalue[1];
                }
                timevalue = tvalues;
            }
            return timevalue;
        }
        public static DateTime getcurrenttime(string systemtimezone)
        {
            DateTime thisTime = DateTime.Now;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            DateTime tziTime = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, tzi);
            return tziTime;
        }
        public static bool find(string svalue, string spattern)
        {
            return Regex.IsMatch(svalue.ToLower(), spattern);
        }
        public static int getnuminweek(DateTime dt)
        {
            int nodayofweek = 0;
            DayOfWeek dow = dt.DayOfWeek;
            //double toad = dates.ToOADate();
            switch (dow.ToString().ToLower())
            {
                case "sunday":
                    nodayofweek = 1;
                    break;
                case "monday":
                    nodayofweek = 2;
                    break;
                case "tuesday":
                    nodayofweek = 3;
                    break;
                case "wednesday":
                    nodayofweek = 4;
                    break;
                case "thursday":
                    nodayofweek = 5;
                    break;
                case "friday":
                    nodayofweek = 6;
                    break;
                case "saturday":
                    nodayofweek = 7;
                    break;
            }
            return nodayofweek;
        }
        public static double computehrs(DateTime dtin, DateTime dtout)
        {
            double totalhrs;
            int ivalue = DateTime.Compare(dtout, dtin);
            if (ivalue == -1) dtout = dtout.AddDays(1);
            totalhrs = (dtout - dtin).TotalHours;
            return totalhrs;
        }
        public static DateTime computemonths(DateTime dtstart, int months)
        {
            return dtstart.AddMonths(months);
        }
        public static double computedays(DateTime dtstart, DateTime dtend)
        {
            double totaldays;
            int ivalue = DateTime.Compare(dtend, dtstart);
            if (ivalue == -1) return ivalue;
            totaldays = (dtend - dtstart).TotalDays;
            return totaldays;
        }
        public static string whereclause(string accountno, int countryid, string comp_code, string branch_code, int franchise)
        {
            string whereclause = "where cust_acct_no = '" + accountno + "' and";
            whereclause += " countryid = " + countryid + " and";
            whereclause += " comp_code ='" + comp_code + "' and ";
            whereclause += "branch_code = '" + branch_code + "' and ";
            whereclause += "franchise = " + franchise;
            return whereclause;

        }
        //public static void GetAllPrinterList(ref DropDownList ddl)
        //{
        //    ManagementScope objScope = new ManagementScope(ManagementPath.DefaultPath); //For the local Access
        //    objScope.Connect();
        //    int ivalue = 0;
        //    SelectQuery selectQuery = new SelectQuery();
        //    selectQuery.QueryString = "Select * from win32_Printer";
        //    ManagementObjectSearcher MOS = new ManagementObjectSearcher(objScope, selectQuery);
        //    ManagementObjectCollection MOC = MOS.Get();
        //    foreach (ManagementObject mo in MOC)
        //    {
        //        ddl.Items.Insert(ivalue, new ListItem(mo["Name"].ToString(), mo["Name"].ToString()));
        //        //                lstPrinterList.Items.Add(mo["Name"].ToString());
        //    }
        //    ddl.DataBind();
        //}
        public static void printpreview(Page page, string url)
        {
            string s = "window.open('" + url + "', 'popup_window', 'width=1024,height=768,left=100,top=100,resizable=yes');";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "script", s, true);
        }

        internal static double computenightpremiumn(DateTime dtamin, DateTime dtamout, DateTime dtpmin, DateTime dtpmout)
        {
            DateTime datetimensdstart;
            DateTime datetimensdend;
            double nsd = 0;
            DateTime comparnsdout = Convert.ToDateTime("1900/01/01 6:00 Am");
            DateTime comperefrom = Convert.ToDateTime("1900/01/01 " + dtamin.ToShortTimeString());
            if (comperefrom < comparnsdout)
            {
                datetimensdstart = Convert.ToDateTime(dtamin.ToShortDateString() + " 12:00 Am");
                datetimensdend = Convert.ToDateTime(dtamin.ToShortDateString() + " 6:00 Am");
            }
            else
            {
                datetimensdstart = Convert.ToDateTime(dtamin.ToShortDateString() + " 22:00 pm");
                DateTime tempvalue = dtamin.AddDays(1);
                datetimensdend = Convert.ToDateTime(tempvalue.ToShortDateString() + " 6:00 Am");
            }
            if (dtamin < datetimensdstart && dtamout < datetimensdstart)
            {
                if (dtpmin < datetimensdstart && dtpmout < datetimensdstart)
                {
                    return 0;
                }
                if (dtpmin < datetimensdstart && dtpmout >= datetimensdstart)
                {
                    return computehrs(datetimensdstart, dtpmout);
                }

                if (dtpmin >= datetimensdstart && dtpmin >= datetimensdstart)
                {
                    return computehrs(dtpmin, dtpmout);
                }
            }
            else if (dtamin < datetimensdstart && dtamout >= datetimensdstart)
            {
                if (dtamout >= datetimensdstart)
                {
                    nsd = computehrs(datetimensdstart, dtamout);
                }
                nsd += computehrs(dtpmin, dtpmout);
                return nsd;
            }
            else
            {
                if (dtamin >= datetimensdstart && dtamout <= datetimensdend)
                {
                    nsd = computehrs(dtamin, dtamout);
                }
                if (dtamin >= datetimensdstart && dtamout <= datetimensdend)
                {
                    // dtamout
                    // nsd = computehrs(dtamin, datetimensdend);
                    nsd = computehrs(dtamin, dtamout);
                }
                if (dtpmin >= datetimensdstart && dtpmout <= datetimensdend)
                {
                    nsd += computehrs(dtpmin, dtpmout);
                }
                if (dtpmin >= datetimensdstart && dtpmout > datetimensdend)
                {
                    nsd += computehrs(dtpmin, datetimensdend);
                }
                return nsd;
            }
            return nsd;
        }
        internal static double computeotnightpremiumn(DateTime dtamin, DateTime dtamout)
        {
            double nsd = 0;
            DateTime datetimensdstart;
            DateTime datetimensdend;
            DateTime comparnsdout = Convert.ToDateTime("1900/01/01 6:00 Am");
            DateTime comperefrom = Convert.ToDateTime("1900/01/01 " + dtamin.ToShortTimeString());
            if (comperefrom < comparnsdout)
            {
                datetimensdstart = Convert.ToDateTime(dtamin.ToShortDateString() + " 12:00 Am");
                datetimensdend = Convert.ToDateTime(dtamin.ToShortDateString() + " 6:00 Am");
            }
            else
            {
                datetimensdstart = Convert.ToDateTime(dtamin.ToShortDateString() + " 22:00 pm");
                DateTime tempvalue = dtamin.AddDays(1);
                datetimensdend = Convert.ToDateTime(tempvalue.ToShortDateString() + " 6:00 Am");
            }
            if (dtamin < datetimensdstart && dtamout < datetimensdstart)
            {
                return nsd;
            }
            else if (dtamin < datetimensdstart && dtamout >= datetimensdstart)
            {
                if (dtamout <= datetimensdend)
                {
                    nsd = computehrs(datetimensdstart, dtamout);
                    return nsd;
                }
                if (dtamout > datetimensdend)
                {
                    nsd = computehrs(datetimensdstart, datetimensdend);
                    return nsd;
                }
                return nsd;
            }
            else if (dtamin >= datetimensdstart && dtamout <= datetimensdstart)
            {
                return computehrs(dtamin, dtamout);
            }
            else
            {
                if (dtamin >= datetimensdstart && dtamout <= datetimensdend)
                {
                    nsd = computehrs(dtamin, dtamout);
                }
                if (dtamin >= datetimensdstart && dtamout > datetimensdend)
                {
                    nsd = computehrs(dtamin, datetimensdend);
                }
                return nsd;
            }

        }
        public static void addnewrow(ref DataTable dt, string accountno, int countryid, string comp_code, string branch_code, int franchise)
        {
            DataRow nrow = dt.NewRow();
            nrow["cust_acct_no"] = accountno;
            nrow["countryid"] = countryid;
            nrow["comp_code"] = comp_code;
            nrow["branch_code"] = branch_code;
            nrow["franchise"] = franchise;
            dt.Rows.Add(nrow);

        }
        public static DataTable addnewrow(DataTable dt, string accountno, int countryid, string comp_code, string branch_code, int franchise)
        {
            DataRow nrow = dt.NewRow();
            nrow["cust_acct_no"] = accountno;
            nrow["countryid"] = countryid;
            nrow["comp_code"] = comp_code;
            nrow["branch_code"] = branch_code;
            nrow["franchise"] = franchise;

            dt.Rows.Add(nrow);
            return dt;

        }
        public static string f_convert_decimal_text_amount(decimal a_amount)
        {
            //string samount = "";
            //string stringamt = "";
            string ls_textvalue = "";
            //string decimalplace = "";
            //string wholenumber = "";
            string[] ls_textonce = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] ls_textoncemore = new string[] { "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] ls_textteen = new string[] { "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eigthy", "Ninety" };
            //int li_len;
            decimal li_value;
            string retvalue;


            if ((Int32)(a_amount / 1000000000000) > 0)
            {
                li_value = (Int32)(a_amount / 1000000000000);

                ls_textvalue = f_convert_decimal_text_amount(li_value) + " Trillion " + f_convert_decimal_text_amount((a_amount % 1000000000000));

                return ls_textvalue;
            }


            //if int( a_amount / 1000000000) > 0 then
            //    li_value = int(a_amount / 1000000000)

            //    ls_textvalue = f_convert_decimal_text_amount(li_value) + " Billion " +  f_convert_decimal_text_amount(mod(a_amount , 1000000000))

            //        return ls_textvalue

            //end if

            if ((Int32)(a_amount / 1000000) > 0)
            {
                li_value = (Int32)(a_amount / 1000000);

                ls_textvalue = f_convert_decimal_text_amount(li_value) + " Million " + f_convert_decimal_text_amount((a_amount % 1000000));

                return ls_textvalue;
            }

            if ((Int32)(a_amount / 1000) > 0)
            {
                li_value = (Int32)(a_amount / 1000);

                ls_textvalue = f_convert_decimal_text_amount(li_value) + " Thousand " + f_convert_decimal_text_amount((a_amount % 1000));

                return ls_textvalue;
            }

            if ((Int32)(a_amount / 100) > 0)
            {
                li_value = (Int32)(a_amount / 100);

                ls_textvalue = f_convert_decimal_text_amount(li_value) + " Hundred " + f_convert_decimal_text_amount((a_amount % 100));

                return ls_textvalue;
            }

            if ((Int32)(a_amount / 10) > 0)
            {
                li_value = (Int32)(a_amount / 10);

                if (li_value == 0)
                    f_convert_decimal_text_amount(li_value);
                else
                {
                    if (((Int32)(a_amount) < 20) && ((Int32)(a_amount) > 10))
                    {
                        ls_textvalue = ls_textoncemore[(Int32)((a_amount) - 10) - 1];
                        return ls_textvalue;
                    }
                }

                retvalue = f_convert_decimal_text_amount((a_amount % 10));
                //	if retvalue > "" then retvalue = " and " + retvalue
                ls_textvalue = ls_textteen[(Int32)(a_amount / 10) - 1] + " " + retvalue;
                return ls_textvalue;

            }

            if ((Int32)(a_amount / 1) > 0)
            {
                li_value = (Int32)(a_amount / 1);
                if ((a_amount % 1) == 0)
                {
                    ls_textvalue = ls_textonce[Convert.ToInt32(li_value) - 1] + f_convert_decimal_text_amount((a_amount % 1));
                }
                else
                {
                    ls_textvalue = ls_textonce[Convert.ToInt32(li_value) - 1] + " and " + f_convert_decimal_text_amount((a_amount % 1));
                }
                return ls_textvalue;
            }


            if (a_amount == 0) return "";

            ls_textvalue = f_convert_decimal_text_amount(a_amount * 2);


            return ls_textvalue;

        }
        public static void multiview_activeindex(ref MultiView mv, int dtr_recieve)
        {
            mv.ActiveViewIndex = dtr_recieve;
        }
        public static Int32 multiview_activeindex(MultiView mv)
        {
            return mv.ActiveViewIndex;
        }
        public static void checkmanualschedule(string timein, string timeout, ref int hrin, ref int hrout)
        {
            DateTime dtimein = Convert.ToDateTime(timein);
            DateTime dtimeout = Convert.ToDateTime(timeout);

            if (dtimein > Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 04:00") && dtimein < Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 07:00"))
            {
                hrin = 6;
                hrout = 14;
            }
            if (dtimein >= Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 07:00") && dtimein < Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 09:00"))
            {
                hrin = 8;
                hrout = 17;
            }
            if (dtimein >= Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 09:00") && dtimein < Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 11:00"))
            {
                hrin = 10;
                hrout = 18;
            }
            if (dtimein >= Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 11:00") && dtimein < Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 15:00"))
            {
                hrin = 14;
                hrout = 22;
            }

            if (dtimein >= Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 15:45") && dtimein < Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 19:00"))
            {
                hrin = 18;
                hrout = 02;
            }
            if (dtimein >= Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 18:30") && dtimein < Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 20:39"))
            {
                hrin = 20;
                hrout = 04;
            }
            if (dtimein >= Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 20:40") && dtimein < Convert.ToDateTime(Convert.ToDateTime(dtimein).ToShortDateString() + " 23:30"))
            {
                hrin = 22;
                hrout = 6;
            }
        }
        public static void dropdownlistselecthere(ref DropDownList ddl, string selectall)
        {
            ddl.Items.Insert(0, new ListItem(selectall, ""));
        }

    }
}