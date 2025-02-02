﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using HRIS_Common;
using System.Configuration;
using System.Data.SqlClient;

namespace HRIS_eAdmin.View
{
    /// <summary>
    /// Summary description for image_dbretriever1
    /// </summary>
    public class image_dbretriever1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Int32 empno;
            if (context.Request.QueryString["id"] != null)
                empno = Convert.ToInt32(context.Request.QueryString["id"]);
            else
                throw new ArgumentException("No parameter specified");

            context.Response.ContentType = "image/jpeg";
            Stream strm = ShowEmpImage(empno);
            byte[] buffer = new byte[4096];
            if (strm != null)
            {
                int byteSeq = strm.Read(buffer, 0, 4096);

                while (byteSeq > 0)
                {
                    context.Response.OutputStream.Write(buffer, 0, byteSeq);
                    byteSeq = strm.Read(buffer, 0, 4096);
                }
            }
            else
            {
                context.Response.WriteFile("~/ResourceImages/upload_profile.png");
            }

        }


        public Stream ShowEmpImage(int empno)
        {
            string conn = ConfigurationManager.ConnectionStrings["hrisConn"].ConnectionString;
            SqlConnection connection = new SqlConnection(conn);
            string sql = "SELECT empl_photo_img FROM personnel_tbl WHERE empl_id = @ID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", empno);
            connection.Open();
            object img = cmd.ExecuteScalar();
            try
            {
                return new MemoryStream((byte[])img);
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}