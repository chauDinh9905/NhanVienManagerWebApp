<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NhanVienWebApp.Login" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="card mx-auto" style="max-width:420px;">
        <div class="card-body">
            <h5 class="card-title"><asp:Label runat="server" Text="<%$ Resources:Resource, Lbl_Login %>" /></h5>
            <div class="mb-2">
                <label><asp:Label runat="server" Text="<%$ Resources:Resource, Lbl_Server %>" /></label>
                <asp:TextBox ID="txtServer" runat="server" CssClass="form-control" Text="127.0.0.1" />
            </div>
            <div class="mb-2">
                <label>Port</label>
                <asp:TextBox ID="txtPort" runat="server" CssClass="form-control" Text="3306" />
            </div>
            <div class="mb-2">
                <label><asp:Label runat="server" Text="<%$ Resources:Resource, Lbl_Account %>" /></label>
                <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ControlToValidate="txtAccount" runat="server"
                    ErrorMessage="<%$ Resources:Resource, Msg_RequiredAccount %>" CssClass="text-danger" Display="Dynamic" />
            </div>
            <div class="mb-3">
                <label><asp:Label runat="server" Text="<%$ Resources:Resource, Lbl_Password %>" /></label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>
            <asp:Button ID="btnConnect" runat="server" CssClass="btn btn-primary w-100"
                Text="<%$ Resources:Resource, Btn_Connect %>" OnClick="btnConnect_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mt-2" />
        </div>
    </div>
</asp:Content>
