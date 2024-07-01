<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="cUserProfile.aspx.cs" Inherits="HRIS_eAdmin.View.cUserProfile.cUserProfile" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="specific_css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="server">
    <form runat="server" >

    <asp:ScriptManager ID="sm_Script" runat="server"></asp:ScriptManager>

        <!-- The Modal - Generating Report -->
                <div class="modal fade" id="Loading">
                  <div class="modal-dialog modal-dialog-centered modal-lg">
                    <div class="modal-content text-center">
                      <!-- Modal body -->
                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                  <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                   <ContentTemplate>
                                           <div class="col-12 text-center">
                                                <img src="/ResourceImages/loadingwithlogo.gif" style="width:100%;"/>
                                            </div>
                                    </ContentTemplate>
                               </asp:UpdatePanel>
                              </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                      <!-- Modal footer -->
                      <div style="margin-bottom:30px">
                      </div>
                    </div>
                  </div>
                </div>

         <!-- The Modal - Add Confirmation -->
            <div class="modal fade" id="AddEditConfirm">
              <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content text-center">
                  <!-- Modal body -->
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
 
                    <div class="modal-body">
                      <i class="fa-5x fa fa-check-circle text-success"></i>
                      <h2 >Successfully</h2>
                       <h6><asp:Label ID="SaveAddEdit" runat="server" Text="Save"></asp:Label></h6>
                  </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                  <!-- Modal footer -->
                  <div style="margin-bottom:30px">
                  </div>
                </div>
              </div>
            </div>
        <asp:UpdatePanel ID="delete_confirm_popup" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <!-- Modal Delete -->
                <div class="modal fade" id="deleteRec">
                    <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content text-center">
                    <!-- Modal body -->
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <i class="fa-5x fa fa-question text-danger"></i>
                                        <h2 >Delete this Record</h2>
                                        <h6><asp:Label ID="deleteRec1" runat="server" Text="Are you sure to delete this Record"></asp:Label></h6>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <!-- Modal footer -->
                        <div style="margin-bottom:50px">
                            <asp:LinkButton ID="lnkBtnYes" runat="server"  CssClass="btn btn-danger" OnCommand="btnDelete_Command"> <i class="fa fa-check"></i> Yes, Delete it </asp:LinkButton>
                            <asp:LinkButton ID="LinkButton3"  runat="server" data-dismiss="modal"  CssClass="btn btn-dark"> <i class="fa fa-times"></i> No, Keep it! </asp:LinkButton>
                        </div>
                    </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
       
         <asp:UpdatePanel ID="edit_delete_notify" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <!-- Modal Add/EditPage-->
                <div class="modal fade" id="editdeletenotify" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <div class="modal-header" style="background-color: #dc3545;color: white;">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <h5 class="modal-title" ><asp:Label ID="notify_header" runat="server" Text=""></asp:Label></h5>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <div class="modal-body text-center">
                                        
                                        <h6><i class="fa fa-times-circle text-danger">  </i><asp:Label ID="lbl_editdeletenotify" runat="server"></asp:Label></h6>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                     </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="add_edit_panel" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <!-- Modal Add/EditPage-->
                <div class="modal fade" id="add" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                    <div class="modal-content modal-content-add-edit">
                        <div class="modal-header modal-header-add-edit">
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>
                                <h5 class="modal-title" id="AddEditPage"><asp:Label ID="LabelAddEdit" runat="server" Text="Add/Edit Page"></asp:Label></h5>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        </div>
                        <div class="modal-body with-background" style="height:100%;padding-bottom:0px;">
                            <div class="row" style="margin-top:0px;margin-bottom:0px">
                                <div class="col-8">
                                     <div class="form-group row mt5">
                                        <label class="col-12 col-md-4 col-form-label"><strong>Employee Name:</strong></label>
                                        <div class="col-12 col-md-8">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                      <asp:TextBox ID="tbx_empl_name" MaxLength="100" runat="server" Width="100%" CssClass="form-control form-control-sm" Enabled="false" Visible="false"></asp:TextBox>
                                                      <asp:DropDownList ID="ddl_empl_name" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddl_empl_SelectedIndexChanged" Width="100%"> </asp:DropDownList>   
                                                      <asp:Label ID="LblRequired1" runat="server" CssClass="lbl_required" Text=""></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="form-group row mt5">
                                        <label class="col-12 col-md-4 col-form-label"><strong>User Password:</strong></label>
                                        <div class="col-12 col-md-8">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                     <asp:TextBox ID="tbx_password" runat="server" MaxLength="30" Width="100%" CssClass="form-control form-control-sm"></asp:TextBox>
                                                <asp:Label ID="LblRequired6" runat="server" CssClass="lbl_required" Text=""></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="form-group row mt5">
                                        <label class="col-12 col-md-4 col-form-label"><strong>User Access Level:</strong></label>
                                        <div class="col-12 col-md-8">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                      <asp:TextBox ID="TextBox1" MaxLength="100" runat="server" Width="100%" CssClass="form-control form-control-sm" Enabled="false" Visible="false"></asp:TextBox>
                                                      <asp:DropDownList ID="ddl_user_accesslevel" runat="server" CssClass="form-control form-control-sm" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddl_user_accesslevel_SelectedIndexChanged" Width="100%">
                                                          <asp:ListItem Text="SS User" Value="1"></asp:ListItem>
                                                          <asp:ListItem Text="System User" Value="2"></asp:ListItem>
                                                          <asp:ListItem Text="System Administrator" Value="3"></asp:ListItem>
                                                          <asp:ListItem Text="System Supervisor" Value="4"></asp:ListItem>
                                                      </asp:DropDownList>   
                                                      <asp:Label ID="Label1" runat="server" CssClass="lbl_required" Text=""></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                 <div class="col-4">
                                     <div class="form-group row mt5">
                                        <label class="col-12 col-md-5 col-form-label"><strong>User ID:</strong></label>
                                        <div class="col-12 col-md-7">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                       <asp:TextBox ID="tbx_user_id" runat="server"  MaxLength="10" Width="100%" Enabled="false"  CssClass="form-control form-control-sm"></asp:TextBox>
                                                       <asp:Label ID="LblRequired2" runat="server" CssClass="lbl_required" Text=""></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                     <div class="form-group row mt5">
                                        <label class="col-12 col-md-7 col-form-label"><strong>Include History:</strong></label>
                                        <div class="col-12 col-md-4">
                                            <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                <ContentTemplate>
                                                       <asp:CheckBox CssClass="form-check-input" style="margin-top:7px;" ID="chkbx_allow_edit_history" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr style="padding:0px;margin-top:5px;margin-bottom:5px;" />
                            <div class="row">
                                <div class="col-6">
<%
                                int x = 0;
                                for (x = 0; x < modulelist.Rows.Count; x++)
                                    {
%> 
                                       <div class="row">
                                           <div class="col-12 mt5"  style="padding-bottom:0px !important;margin-bottom:0px !important;" >
                                               <label><%: modulelist.Rows[x]["module_name"] %></label>
                                           </div>
                                       </div>
<%
                                    } 
%>
                                </div>
                                <div class="col-6">
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_0" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_1" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                 </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_2" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_3" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel> 
                                        </div>
                                    </div>
                                    <%--<div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_4" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>--%>
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_5" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_6" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_7" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt5">
                                            <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_8" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt5" style="padding-bottom:0px !important;margin-bottom:0px !important;">
                                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_module_9" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true"  Width="100%"> </asp:DropDownList> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="padding:5px;margin-top:0px !important;">
                           <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                              <ContentTemplate>
                                      <asp:Button ID="btn_cancel"  runat="server" data-dismiss="modal" Text ="Cancel" CssClass="btn btn-danger cancel-icon icn"></asp:Button>
                                      <asp:Button ID="btn_save"  runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary save-icon icn" />
                                  </div>
                              </ContentTemplate>
                          </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="col-12">
            <div class="row breadcrumb my-breadcrumb">
                <div class="col-12 col-md-4"><strong style="font-family:Arial;font-size:20px;color:white;"><%: Master.page_title %></strong></div>
                <div class="col-8">
                    <asp:UpdatePanel ID="UpdatePanel39" ChildrenAsTriggers="false" UpdateMode="Conditional"  runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_search" onInput="search_for(event);" runat="server" class="form-control" placeholder="Search.." Height="30px" 
                        Width="100%" OnTextChanged="tbx_search_TextChanged" AutoPostBack="true"></asp:TextBox>
                           <script type="text/javascript">
                                function search_for(key)
                                {
                                        __doPostBack("<%= txt_search.ClientID %>", "");
                                }
                            </script>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <%-- This Part Update Panel is For Gridview Display --%>
                <table class="table table-bordered  table-scroll">
                    <tbody class="my-tbody">
                        <tr>
                            <td>
                                <div class="row" style="margin-bottom:10px">
                                    <div class="col-4">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Label runat="server" Text="Show"></asp:Label>
                                                <asp:DropDownList ID="DropDownListID" OnTextChanged="DropDownListID_TextChanged" CssClass="form-control-sm" runat="server" AppendDataBoundItems="true" AutoPostBack="True" Width="20%" ToolTip="Show entries per page">
                                                    <asp:ListItem Text="5" Value="5" />
                                                    <asp:ListItem Text="10" Selected="True" Value="10" />
                                                    <asp:ListItem Text="25" Value="25" />
                                                    <asp:ListItem Text="50" Value="50" />
                                                    <asp:ListItem Text="100" Value="100" />
                                                </asp:DropDownList>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="show_pagesx" runat="server" Text="Page: 0/0"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                   <div class="col-6">
                                        <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                            <ContentTemplate>
                                                    <asp:Label runat="server" Text="Department:"></asp:Label>
                                                    <asp:DropDownList ID="ddl_department" Width="75%" runat="server" AutoPostBack="true" CssClass="form-control form-control-sm" OnSelectedIndexChanged="ddl_department_SelectedIndexChanged" ></asp:DropDownList>
                                                </div>
                                                </ContentTemplate>
                                        </asp:UpdatePanel>
 
                                        <div class="col-sm-2 text-right">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                <ContentTemplate>
                                                    <% if (ViewState["page_allow_add"].ToString() == "1") { %>
                                                        <asp:Button ID="btn_add"  runat="server" autoPostback="true" CssClass="btn btn-primary btn-sm add-icon icn" Visible="false" Text="Add" OnClick="btnAdd_Click"/>
                                                    <%} %>
                                                    <%--<asp:Button ID="btn_print" runat="server" CssClass="btn btn-success btn-sm" Text="Print" />--%> 
                                                </ContentTemplate>
                                                <Triggers> 
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_department" />
                                                </Triggers>
                                            </asp:UpdatePanel>                       
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="up_dataListGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView 
                                        ID="gv_dataListGrid"
                                        runat="server" 
                                        allowpaging="True" 
                                        AllowSorting="True" 
                                        AutoGenerateColumns="False" 
                                        EnableSortingAndPagingCallbacks="True"
                                        ForeColor="#333333" 
                                        GridLines="Both" height="100%" 
                                        onsorting="gv_dataListGrid_Sorting"  
                                        OnPageIndexChanging="gridviewbind_PageIndexChanging"
                                        PagerStyle-Width="3" 
                                        PagerStyle-Wrap="false" 
                                        pagesize="10"
                                        Width="100%" 
                                        Font-Names="Century gothic"
                                        Font-Size="Medium" 
                                        RowStyle-Width="5%" 
                                        AlternatingRowStyle-Width="10%" CellPadding="2" ShowHeaderWhenEmpty="true"
                                     EmptyDataText="NO DATA FOUND" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-CssClass="no-data-found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="USER ID" SortExpression="user_id">
                                                <ItemTemplate>
                                                    <%# Eval("user_id") %>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EMPLOYEE NAME" SortExpression="employee_name">
                                                <ItemTemplate>
                                                    &nbsp;<span><%# Eval("employee_name") %></span>
                                                </ItemTemplate>
                                                <ItemStyle Width="54%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="left" CssClass="gg" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EDIT HISTORY" SortExpression="allow_edit_history">
                                                <ItemTemplate>
                                                    &nbsp;<span><%# Eval("allow_edit_history").ToString() == "True" ? "Allowed":"Not Allowed" %></span>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" CssClass="gg" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="LEVEL" SortExpression="user_accesslevel">
                                                <ItemTemplate>
                                                    &nbsp;<span><%# (Eval("user_accesslevel")) %></span>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" CssClass="gg" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ACTION">
                                                <ItemTemplate>
                                                    <% if (ViewState["page_allow_edit"].ToString() == "1") { %>
                                                        <asp:ImageButton ID="imgbtn_editrow1" CssClass="btn btn-primary action" EnableTheming="true"  runat="server"  ImageUrl="~/ResourceImages/final_edit.png" OnCommand="editRow_Command" CommandArgument='<%# Eval("user_id") %>' ImageAlign="Middle" />
                                                    <%} %>
                                                    <% if (ViewState["page_allow_edit"].ToString() == "1") { %>
                                                        <asp:ImageButton ID="lnkDeleteRow" CssClass="btn btn-danger action" EnableTheming="true" runat="server"  ImageUrl="~/ResourceImages/final_delete.png" OnCommand="deleteRow_Command" CommandArgument='<%# Eval("user_id")+","+Eval("employee_name") %> ' />
                                                    <%} %>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings  
                                            Mode="NumericFirstLast" 
                                            FirstPageText="First" 
                                            PreviousPageText="Previous" 
                                            NextPageText="Next" 
                                            LastPageText="Last" 
                                            PageButtonCount="1" 
                                            Position="Bottom" 
                                            Visible="True" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="10%" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" VerticalAlign="Middle" />
                                        <PagerStyle CssClass="pagination-ys" BackColor="#2461BF" ForeColor="White" HorizontalAlign="right" VerticalAlign="NotSet" Wrap="True" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btn_add" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListID" />
                                    <asp:AsyncPostBackTrigger ControlID="txt_search" />
                                    <asp:AsyncPostBackTrigger ControlID="ddl_department" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="specific_scripts" runat="server">
    <script type="text/javascript">
        function openModal() {
            $('#add').modal({
                keyboard: false,
                backdrop:"static"
            });
           
        };
        

    </script>
    <script type="text/javascript">
        function closeModal() {
            $('#add').modal("hide");
             $('#AddEditConfirm').modal({
                 keyboard: false,
                backdrop:"static"
            });
            setTimeout(function () {
                $('#AddEditConfirm').modal("hide");
                $('.modal-backdrop.show').remove();
            }, 800);
         };
    </script>

    <script type="text/javascript">
        function openModalDelete() {
            $('#deleteRec').modal({
                keyboard: false,
                backdrop:"static"
            });
       };
    </script>
    <script type="text/javascript">
        function closeModalDelete() {
            $('#deleteRec').modal('hide');
         };
    </script>

    <script type="text/javascript">
        function openModalNotify() {
            $('#editdeletenotify').modal({
                keyboard: false,
                backdrop:"static"
            });
       };
    </script>
    <script type="text/javascript">
        function closeModalNotify() {
            $('#editdeletenotify').modal('hide');
         };
    </script>
    <script type="text/javascript">
        function openLoading(url) {
           
            $('#Loading').modal({
                keyboard: false,
                backdrop:"static"
            });
            setTimeout(function () {
                window.location.replace(url);
            }, 1000);
       }
    </script>
</asp:Content>