<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="cBarangays.aspx.cs" Inherits="HRIS_eAdmin.View.cBarangays"%>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="specific_css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="server">
    <form runat="server">

    <asp:ScriptManager ID="sm_Script" runat="server"> </asp:ScriptManager>
    <!-- The Modal - Add Confirmation -->
            <div class="modal fade" id="AddEditConfirm">
              <div class="modal-dialog modal-dialog">
                <div class="modal-content text-center">
                  <!-- Modal body -->
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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
                    <div class="modal-dialog modal-dialog">
                    <div class="modal-content text-center">
                    <!-- Modal body -->
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
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
        <asp:UpdatePanel ID="UpdatePanel7" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Modal Add/EditPage-->
                <div class="modal fade" id="add" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content" style="background-image:linear-gradient(white, lightblue)">
                        <div class="modal-header" style="background-image:linear-gradient(green, yellow);padding:8px!important;" >
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <h5 class="modal-title" id="AddEditPage"><asp:Label ID="LabelAddEdit" runat="server" Text="Add/Edit Page" forecolor="White"></asp:Label></h5>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        </div>
                        <div class="modal-body" runat="server">
                        <div class="row" runat="server">
                                <div class="form-group col-md-12" >
                                            <asp:Label ID="Label1" runat="server" CssClass="font-weight-bold" Text="Barangay Code:"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="tbx_barangaycode" runat="server" Width="15%" CssClass="form-control text-center" Enabled ="false"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>                            
                                <div class="form-group col-md-12">
                                    <asp:Label ID="Label3" runat="server" CssClass="font-weight-bold" Text="Barangay Name:"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="tbx_barangayname" runat="server" Width="100%" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                            <asp:Label ID="LblRequired1" CssClass="lbl_required" runat="server" Text=""></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                                <div class="form-group col-md-12">
                                    <asp:Label ID="Label5" runat="server" CssClass="font-weight-bold" Text="Province"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddl_province" runat="server"  Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_province_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:Label ID="LblRequired2" CssClass="lbl_required" runat="server" Text=""></asp:Label>
                                      </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="form-group col-md-12">
                                    <asp:Label ID="Label7" runat="server" CssClass="font-weight-bold" Text="Municipality"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                                <asp:DropDownList ID="ddl_municity" runat="server"  Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_municity_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:Label ID="LblRequired3" CssClass="lbl_required" runat="server" Text=""></asp:Label>
                              </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="LinkButton2"  runat="server" data-dismiss="modal" Text ="Cancel" CssClass="btn btn-danger cancel-icon icn"></asp:LinkButton>
                            <asp:Button ID="Button2" runat="server"  Text="Save" CssClass="btn btn-primary save-icon icn"  onClick="btnSave_Click" />
                        </div>
                    </div>
                    </div>
                </div>
            </ContentTemplate>
<%--            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Button2" />
            </Triggers>--%>
        </asp:UpdatePanel>
     <div class="col-12">
            <div class="row breadcrumb my-breadcrumb">
                <div class="col-4"><strong style="font-family:Arial;font-size:20px;color:white;"><%: Master.page_title %></strong></div>
                <div class="col-8">
                    <asp:UpdatePanel ID="UpdatePanel11" ChildrenAsTriggers="false" UpdateMode="Conditional"  runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbx_search" onInput="search_for(event);" runat="server" class="form-control" placeholder="Search.." Height="30px" 
                                Width="100%" OnTextChanged="tbx_search_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <script type="text/javascript">
                                function search_for(key) {
                                        __doPostBack("<%= tbx_search.ClientID %>", "");
                                }
                            </script>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    <div class="row">
        
        <div class="col-12">
            <table class="table table-bordered  table-scroll">
                <tbody class="my-tbody">
                    <tr>
                        <td>
                            <div class="row" style="margin-bottom:10px">
                                <div class="col-6">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:Label runat="server" Text="Show"></asp:Label>
                                                <asp:DropDownList ID="DropDownListID" runat="server" CssClass="form-control-sm" AppendDataBoundItems="true" AutoPostBack="True" OnTextChanged="DropDownListID_TextChanged" Width="15%" ToolTip="Show entries per page">
                                                    <asp:ListItem Text="5" Value="5" />
                                                    <asp:ListItem Text="10" Selected="True" Value="10" />
                                                    <asp:ListItem Text="15" Value="15" />
                                                    <asp:ListItem Text="25" Value="25" />
                                                    <asp:ListItem Text="50" Value="50" />
                                                    <asp:ListItem Text="100" Value="100" />

                                                </asp:DropDownList>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
                                                     <asp:Label ID="show_pagesx" runat="server" Text="Page: 9/9"></asp:Label>                                            <asp:Label runat="server" Text="Entries"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-6 text-right">
                                    <asp:UpdatePanel ID="UpdatePanel10" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                        <ContentTemplate>
                                            
                                            <% if (ViewState["page_allow_add"].ToString() == "1")
                                                {  %>
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary btn-sm add-icon icn"  Text="Add" OnClick="btnAdd_Click" />
                                            <% }
                                            else
                                             {   %>
                                            <%--<asp:Button ID="Button4" runat="server" CssClass="btn btn-primary btn-sm" Text="Add" Enabled="false" />--%>
                                            <% } %>
                                                                     
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="up_barangay" UpdateMode="Conditional" runat="server" >
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
                                            AlternatingRowStyle-Width="10%"
                                            CellPadding="2"
                                            ShowHeaderWhenEmpty="True"
                                            EmptyDataText="NO DATA FOUND"
                                            EmptyDataRowStyle-ForeColor="Red"
                                            EmptyDataRowStyle-CssClass="no-data-found"
                                            >
                                            <Columns>
                                                <asp:TemplateField HeaderText="CODE" SortExpression="barangay_code">
                                                    <ItemTemplate>
                                                        <%# Eval("barangay_code") %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BARANGAY NAME" SortExpression="barangay_name">
                                                    <ItemTemplate>
                                                        <%# Eval("barangay_name") %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MUNICIPALITY NAME" SortExpression="municipality_name">
                                                    <ItemTemplate>
                                                        <%# Eval("municipality_name") %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PROVINCE NAME" SortExpression="province_name">
                                                    <ItemTemplate>
                                                        <%# Eval("province_name") %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ACTION">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel12" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                                            <ContentTemplate>
                                                                <%--<asp:LinkButton ID="lnkEditRow" runat="server" CssClass="btn btn-danger btn-md" OnCommand="editRow_Command" CommandArgument='<%# Eval("barangay_code") %>'><i class="fa fa-edit"></i></asp:LinkButton>--%>
                                                                <% 
                                                                    if (ViewState["page_allow_edit"].ToString() == "1")
                                                                    {
                                                                %>
                                                                    <asp:ImageButton ID="imgbtn_editrow1" CssClass="btn btn-primary action" EnableTheming="true"  runat="server"  ImageUrl="~/ResourceImages/final_edit.png" OnCommand="editRow_Command" CommandArgument='<%# Eval("barangay_code") %>'/>
                                                        
                                                                <%   }
                                                                     else
                                                                     {
                                                                        
                                                                %>
                                                                    <%--<asp:ImageButton ID="ImageButton1" CssClass="btn btn-primary action" EnableTheming="true" Enabled="false"  runat="server"  ImageUrl="~/ResourceImages/final_edit.png" />--%>
                                                                <% 
                                                                    }
                                                                %>

                                                                <% if (ViewState["page_allow_delete"].ToString() == "1")
                                                                    {
                                                                %>
                                                                    <asp:ImageButton ID="lnkDeleteRow" CssClass="btn btn-danger action" EnableTheming="true" runat="server"  ImageUrl="~/ResourceImages/final_delete.png" OnCommand="deleteRow_Command" CommandArgument='<%# Eval("barangay_code") + ", " + Eval("barangay_name") %>'/>
                                                                <% }
                                                                    else
                                                                    {
                                                                %>
                                                                    <%--<asp:ImageButton ID="ImageButton2" CssClass="btn btn-danger action" EnableTheming="true" runat="server" Enabled="false"  ImageUrl="~/ResourceImages/final_delete.png" />--%>
                                                                <%
                                                                    } 
                                                                %>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
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
                                            <HeaderStyle BackColor="#507CD1" ForeColor="White" VerticalAlign="Middle" Font-Size="14px" CssClass="td-header" />
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
                                    <asp:AsyncPostBackTrigger ControlID="Button2" />
                                    <asp:AsyncPostBackTrigger ControlID="tbx_search" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListID" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</form>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="specific_scripts" runat="server">
</asp:Content>