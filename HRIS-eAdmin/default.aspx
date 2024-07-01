<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HRIS_eAdmin._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="specific_css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="server">
    <form runat="server">
        <asp:ScriptManager runat="server" ID="sm_script"></asp:ScriptManager>
<%--        <div class="col-12">
            <div class="row breadcrumb my-breadcrumb">
                <div class="col-12"><strong style="font-family:Arial;font-size:20px;color:white;">LANDING PAGE</strong></div>
                --<div class="col-8">
                    <asp:UpdatePanel ID="UpdatePanel11" ChildrenAsTriggers="false" UpdateMode="Conditional"  runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbx_search" onInput="search_for(event);" runat="server" class="form-control" placeholder="Search.." Height="30px" 
                                Width="100%" ></asp:TextBox>
                            <script type="text/javascript">
                                function search_for(key) {
                                        __doPostBack("<%= tbx_search.ClientID %>", "");
                                }
                            </script>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-12" >
                <!-- Icon Cards-->
                <div class="row">
                    <div class="col-9" style="padding:0px 10px 10px 10px;">
                        <div class="row">
                            <div class="col-12">
                                <asp:Image ID="Image1" Width="100%" Height="550px" ImageUrl="~/ResourceImages/background.png" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-3 mt5">
                        <%--<div class="row ">
                            <div class="col-12">
                                <div class="card text-white bg-info o-hidden h-100">
                                <div class="card-body">
                                    <div class="mr-5" style="color:white;">
                                        <h3><strong>255 <i class="fa fa-user"></i></strong></h3>
                                    </div>
                                    <div class="mr-5" style="color:black;">
                                        <small>VACANT POSITION</small>
                                    </div>
                                    <div class="card-body-icon">
                                    <i class="fa fa-fw fa-address-card"></i>
                                    </div>
                                </div>
                                <a class="card-footer text-white clearfix small z-1" href="#">
                                    <span class="float-left">View Details</span>
                                    <span class="float-right">
                                    <i class="fa fa-angle-right"></i>
                                    </span>
                                </a>
                                </div>
                            </div>
                        </div>--%>
                        <div class="row" style="padding:0px 15px 2px 4px;">
                            <div class="col-12" style="background-color:#e9ecef;padding:10px 10px 10px 10px;border-radius:5px;">
                                <div class="row">
                                    <div class="col-12">
                                        <strong>Direct Message</strong>
                                    </div>
                                    <div class="form-group col-md-12">
                                    <asp:Label ID="Label3" runat="server" Text="Employee:"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="tbx_barangayname" runat="server" Width="100%" MaxLength="100" CssClass="form-control form-control-sm"></asp:TextBox>
                                                <asp:Label ID="LblRequired1" CssClass="text-danger" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="form-group col-md-12">
                                    <asp:Label ID="Label1" runat="server" Text="Message:"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Width="100%" TextMode="MultiLine" MaxLength="100" CssClass="form-control form-control-sm"></asp:TextBox>
                                                <asp:Label ID="Label2" CssClass="text-danger" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-12 text-right">
                                        <asp:Button ID="btn_send" runat="server" Text="Send" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-12">
                                       <strong>CALENDAR</strong>
                                    </div>
                                </div>
                                <asp:Calendar ID="Calendar1" CssClass="mt5--%>" runat="server"></asp:Calendar>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
       </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="specific_scripts" runat="server">
</asp:Content>
