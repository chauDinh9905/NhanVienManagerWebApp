<%@ Page Title="Edit Table" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="EditTableStructure.aspx.cs" Inherits="NhanVienWebApp.EditTableStructure" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
     <asp:UpdatePanel ID="upMain" runat="server">
         <ContentTemplate>
             <h5><asp:Label runat="server" Text="<%$ Resources:Resource, Btn_EditTable %>" /> — <asp:Label ID="lblTableName" runat="server" /></h5>
             <asp:GridView ID="gvColumns" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"
                DataKeyNames="Name" OnRowCommand="gvColumns_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Tên cột" />
                    <asp:BoundField DataField="DataType" HeaderText="Kiểu dữ liệu" />
                    <asp:BoundField DataField="ColumnKey" HeaderText="Khoá" />
                    <asp:TemplateField HeaderText="Thao tác">
                        <ItemTemplate>
                            <asp:Button runat="server" CssClass="btn btn-sm btn-secondary me-1" Text="Sửa"
                                CommandName="Edit" CommandArgument='<%# Eval("Name") %>'
                                Enabled='<%# !string.Equals(Eval("Name"), "ID") %>' />
                            <asp:Button runat="server" CssClass="btn btn-sm btn-danger" Text="Xoá"
                                CommandName="DeleteCol" CommandArgument='<%# Eval("Name") %>'
                                Enabled='<%# !string.Equals(Eval("Name"), "ID") %>'
                                OnClientClick='<%# "return confirm(\x27Xoá cột " + Eval("Name") + "?\x27);" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Panel ID="pnlEdit" runat="server" Visible="false" CssClass="card mt-3">
                <div class="card-body">
                    <h6>Sửa cột: <asp:Label ID="lblOldName" runat="server" /></h6>
                    <div class="mb-2">
                        <label>Tên cột mới</label>
                        <asp:TextBox ID="txtNewName" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-2">
                        <label>Kiểu dữ liệu mới</label>
                        <asp:DropDownList ID="ddlNewType" runat="server" CssClass="form-select" />
                    </div>
                    <asp:Button ID="btnSaveEdit" runat="server" CssClass="btn btn-primary" Text="Lưu" OnClick="btnSaveEdit_Click" />
                </div>
            </asp:Panel>
            <asp:Label ID="lblMessage" runat="server" CssClass="d-block mt-2" />
         </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>
