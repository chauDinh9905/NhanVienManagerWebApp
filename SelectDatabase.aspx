<%@ Page Title="Select DB" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SelectDatabase.aspx.cs" Inherits="NhanVienWebApp.SelectDatabase" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="card mx-auto" style="max-width:520px;">
                <div class="card-body">
                    <div class="mb-2">
                        <label><asp:Label runat="server" Text="<%$ Resources:Resource, Lbl_DatabaseName %>" /></label>
                        <asp:TextBox ID="txtDbName" runat="server" CssClass="form-control" Text="NhanVien" />
                    </div>
                    <div class="mb-2">
                        <label><asp:Label runat="server" Text="<%$ Resources:Resource, Lbl_TableName %>" /></label>
                        <asp:TextBox ID="txtTableName" runat="server" CssClass="form-control" Text="tb_nhanvien" />
                    </div>
                    <asp:Button ID="btnCheck" runat="server" CssClass="btn btn-primary"
                        Text="<%$ Resources:Resource, Btn_Check %>" OnClick="btnCheck_Click" />
                    <asp:Panel ID="pnlResult" runat="server" CssClass="mt-3" Visible="false">
                        <asp:Label ID="lblStatus" runat="server" CssClass="d-block mb-2 fw-bold" />
                        <asp:Button ID="btnCreateDb" runat="server" CssClass="btn btn-success me-2"
                            Text="<%$ Resources:Resource, Btn_CreateDb %>" OnClick="btnCreateDb_Click" Visible="false" />
                        <asp:Button ID="btnCreateTable" runat="server" CssClass="btn btn-success me-2"
                            Text="<%$ Resources:Resource, Btn_CreateTable %>" OnClick="btnCreateTable_Click" Visible="false" />
                        <asp:Button ID="btnEditTable" runat="server" CssClass="btn btn-secondary me-2"
                            Text="<%$ Resources:Resource, Btn_EditTable %>" OnClick="btnEditTable_Click" Visible="false" />
                        <asp:Button ID="btnViewData" runat="server" CssClass="btn btn-primary"
                            Text="<%$ Resources:Resource, Btn_ViewData %>" OnClick="btnViewData_Click" Visible="false" />
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
