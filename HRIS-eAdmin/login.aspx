<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="HRIS_eAdmin.login" %>
<!DOCTYPE html>
<html>
<head runat="server">
      <meta charset="utf-8">
      <meta http-equiv="X-UA-Compatible" content="IE=edge">
      <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
      <meta name="description" content="">
      <meta name="author" content="">
      <title>HRIS</title>

<script type="text/javascript">
    function DisableBackButton() {
    window.history.forward()
    }
    DisableBackButton();
    window.onload = DisableBackButton;
    window.onpageshow = function(evt) { if (evt.persisted) DisableBackButton() }
    window.onunload = function() { void (0) }
</script>

      <!-- Bootstrap core CSS-->
      <link rel="stylesheet"  href="~/vendor/bootstrap/css/bootstrap.min.css">
      <!-- Custom fonts for this template-->
      <link  rel="stylesheet" href="~/vendor/font-awesome/css/font-awesome.min.css">
      <!-- Page level plugin CSS-->
      <link  rel="stylesheet" href="~/vendor/datatables/dataTables.bootstrap4.css">
      <!-- Custom styles for this template-->
      <link href="~/css/sb-admin.css" rel="stylesheet">
      <link href="~/css/common.css" rel="stylesheet">
      <style type="text/css">
          
          .login-bg {
              background-image:url('<%= ResolveUrl("~/ResourceImages/login_bg.png") %>');
              background-position:center;
              background-repeat:no-repeat;
              background-size:cover;
              height: 100vh;
          }
          .my-form {
              padding:15%;
          }
          .my-form > .form-container {
                border: 1px solid #54ac39;
                padding: 20px;
                border-radius: 10px 10px 10px 10px;
                background: linear-gradient(180deg, #2d8e27 0%,#9de45c 100%);
                box-shadow: 4px 6px 13px 2px #72673c;
            }
          .input-group div#pannel_password, .input-group div#pannel_username {
              width:85%;
          }
          .form-group {
                margin-bottom:3px;
            }
          #message_loger {
              text-align:center !important;
          }
          .show-caps {
            font-size:14px;
            font-weight:bold;
            color:darkred;
            visibility:hidden;
          }
          .input-group input {
                 border-radius: 0px 5px 5px 0px !important;
          }
         .grad1 
         {
            background-color: #1fc8db;
            background-image: linear-gradient(90deg, #0e8200 0%, #ffd333 50%,#16c701 80%, #098fc7 95%);
            color: white;
            opacity: 0.95;
         }
         
      </style>
</head>
<body >
    
         
        <%--<nav class="navbar navbar-expand-lg navbar-light bg-light">
          <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">
              
            </ul>
            <ul class="navbar-nav mr-auto ">
                <li class="nav-item text-right">
                    <a class="nav-link" href="#"></a>
                </li>
            </ul>
          </div>
        </nav>--%>
        <div class="row">
            <div class="col-12 login-bg">
                <div class="row">
                    <div class="col-12 col-md-6">
                        <div class="row my-form">
                            <div class="col-12">
                               <%--<asp:Image ID="Image2" Height="350px" Width="600px"  runat="server" ImageUrl="~/ResourceImages/login_left.png" />--%>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-6">
                        <div class="row my-form">
                            <div class="col-12 form-container">
                                <form id="form1" runat="server">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                    <div class="row">
                                        <div class="col-12 text-center">
                                            <h2 style="color:white;font-stretch:expanded;">COMVAL e-ADMIN</h2>
                                        </div>
                                        <div class="col-12 text-center" style="padding-top:10px;padding-bottom:20px;">
                                            <asp:Image ID="Image1" Height="150px" Width="150px"  runat="server" ImageUrl="~/ResourceImages/com_logo.png" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 text-center">
                                            <asp:UpdatePanel ID="message_loger" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="msg_logre" Cssclass="col-form-label" runat="server" Text=""></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btn_login" />
                                                    <asp:AsyncPostBackTrigger ControlID="tbx_username" />
                                                    <asp:AsyncPostBackTrigger ControlID="tbx_password" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                      <asp:Label ID="Label1" Cssclass="col-12 col-sm-3 col-form-label" runat="server" Text="Username:" Font-Bold="True"></asp:Label>
                                      <div class=" input-group col-12 col-sm-9">
                                        <div class="input-group-append" style="z-index:9999">
                                            <label class="btn-outline-secondary" style="padding:7px 13px;background-color:gray;color:white;margin-right:-2px;z-index:900 !important;border-radius:5px 0px 0px 5px"><i class="fa fa-user fa-wd"></i></label>
                                        </div>
                                        <asp:UpdatePanel ID="pannel_username" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <asp:TextBox ID="tbx_username" runat="server" onInput="go_postBack(event)" OnTextChanged="tbx_username_TextChanged" AutoPostBack="false" CssClass="form-control" Width="100%" ></asp:TextBox>
                                                <script type="text/javascript">
                                                    function go_postBack(key) {
                                                        if (key.which != 13) {
                                                            __doPostBack("<%= tbx_username.ClientID %>", "");
                                                        }
                                                            
                                                    }
                                                </script>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                      </div>
                                    </div>
                                    <div class="form-group row" style="padding-bottom:0px;margin-bottom:0px;">
                                      <asp:Label ID="Label3" Cssclass="col-12 col-sm-3 col-form-label" runat="server" Text="Password:" Font-Bold="True"></asp:Label>
                                      <div class=" input-group col-12 col-sm-9" >
                                        <div class="input-group-append" style="z-index:9999">
                                            <label class="btn-outline-secondary"  style="height:40px;padding:7px 13px;background-color:gray;color:white;margin-right:-2px;z-index:900 !important;border-radius:5px 0px 0px 5px"><i class="fa fa-lock fa-wd"></i></label>
                                        </div>
                                        <asp:UpdatePanel ID="pannel_password" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <asp:TextBox ID="tbx_password" type="password" TextMode="Password" onInput="go_postBack2(event)" onKeyPress="capLock(event)" OnTextChanged="tbx_password_TextChanged" AutoPostBack="false"  runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                                                <asp:Label ID="show_caps" CssClass="show-caps" runat="server">Capslock is on.</asp:Label>
                                                <script type="text/javascript">
                                                    function go_postBack2(key)
                                                    {
                                                        if (key.which != 13)
                                                        {
                                                            __doPostBack("<%= tbx_password.ClientID %>", "");
                                                        }
                                                    }
                                                </script>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                      </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-6">
<%--                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server">Create Account</asp:LinkButton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                        </div>
                                        <div class="col-6 text-right" style="padding-right:32px;padding-top:0px;">
                                           <asp:LinkButton ID="btn_login"  CssClass="btn btn-primary"  OnCommand="btn_login_Command" runat="server"><i class="fa fa-fw fa-sign-out"></i>Login</asp:LinkButton>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</body>
<script src='<%= ResolveUrl("~/vendor/jquery/jquery.min.js") %>'></script>
   
<script src='<%= ResolveUrl("~/vendor/popper/popper.min.js") %>'></script>
<script src='<%= ResolveUrl("~/vendor/bootstrap/js/bootstrap.min.js") %>'></script>
<!-- Core plugin JavaScript-->
<script src='<%= ResolveUrl("~/vendor/jquery-easing/jquery.easing.min.js") %>'></script>
<!-- Page level plugin JavaScript-->
<script src='<%= ResolveUrl("~/vendor/datatables/jquery.dataTables.js") %>' ></script>
<script src='<%= ResolveUrl("~/vendor/datatables/dataTables.bootstrap4.js") %>'></script>
<!-- Custom scripts for all pages-->
<script src='<%= ResolveUrl("~/js/sb-admin.min.js") %>'></script>
<!-- Custom scripts for this page-->
<script src='<%= ResolveUrl("~/js/sb-admin-datatables.min.js") %>'></script>
<script type="text/javascript">
    document.addEventListener("keydown", function (event)
    {
        if (event.which == 13) {
            __doPostBack("<%= btn_login.ClientID %>", "");
        }
        else  if (event.which == 20) {
            capLock(event);
        }
    });

    function capLock(e)
    {
      var kc = e.keyCode ? e.keyCode : e.which;
        var sk = e.shiftKey ? e.shiftKey : kc === 16;
        console.log(kc + " -- " + sk);
        if (e.which == 13) {
             __doPostBack("<%= btn_login.ClientID %>", "");
        }
        else
        {
            var visibility = ((kc >= 65 && kc <= 90) && !sk) || 
            ((kc >= 97 && kc <= 122) && sk) ? 'visible' : 'hidden';
            console.log(visibility);
            $("#<%: show_caps.ClientID%>").css("visibility", visibility);
        }
      
    }
</script>
</html>
