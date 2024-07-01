<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logoff.aspx.cs" Inherits="HRIS_eAdmin.logoff" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <script type="text/javascript" language="javascript">
             function DisableBackButton() {
               window.history.forward()
              }
             DisableBackButton();
             window.onload = DisableBackButton;
             window.onpageshow = function(evt) { if (evt.persisted) DisableBackButton() }
             window.onunload = function() { void (0) }
         </script>
        <meta charset="utf-8" />
        <meta http-equiv="Expires" content="0" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <meta http-equiv="Cache-Control" content="no-cache" />
        <meta http-equiv="Pragma" content="no-cache" />
        <title>LOG OFF</title>
        <style>
            div.styled {
                margin-top:10%;
                margin-bottom:auto;
                margin-right:100px;
                margin-left:100px;
                min-height:50px;
                padding:50px;
                border-radius:8px;
                border:1px solid #d9d9d9;
                text-align:center;
            }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager EnablePageMethods="true" ID="MainSM" runat="server" ></asp:ScriptManager>
        <div class="styled">
            <h1>THANK YOU FOR USING HRIS!</h1>
        </div>
        <script type="text/javascript">
           <%-- $(document).ready(function () {
                var mySession = '<%= Page.Session["ss_user_id"]%>';
                setTimeout(function () {
                    if (mySession != null || mySession != '') {
                         PageMethods.SetDownloadPath();
                 
                    }
                    else {
                        document.location.replace("~/");
                    }
                  document.location.reload(true);
                },500);
            });--%>

            function reloader() {
                setTimeout(function () {
                        document.location.replace('<%= ResolveUrl("~/login.aspx")%>');
                },1500);
              
            }
        </script>
    </form>
</body>
<script src='<%= ResolveUrl("~/vendor/jquery/jquery.min.js") %>'></script>

</html>