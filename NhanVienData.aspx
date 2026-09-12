<%@ Page Title="Nhan Vien" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="NhanVienData.aspx.cs" Inherits="NhanVienWebApp.NhanVienData" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="d-flex justify-content-between align-items-center mb-3">
                <h5>Danh sách nhân viên — <asp:Label ID="lblTableName" runat="server" /></h5>
                <div>
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-success"
                        Text="<%$ Resources:Resource, Btn_Add %>" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-secondary"
                        Text="<%$ Resources:Resource, Btn_Edit %>" OnClick="btnEdit_Click" />
                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger"
                        Text="<%$ Resources:Resource, Btn_Delete %>" OnClick="btnDelete_Click" />
                </div>
            </div>

            <asp:Label ID="lblToast" runat="server" CssClass="d-block mb-2" />

            <asp:GridView ID="gvNhanVien" runat="server" CssClass="table table-striped table-hover"
                AutoGenerateColumns="false" DataKeyNames="ID">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="Account" HeaderText="Account" />
                    <asp:BoundField DataField="HoTen" HeaderText="Họ tên" />
                    <asp:BoundField DataField="QueQuan" HeaderText="Quê quán" />
                    <asp:BoundField DataField="NgaySinh" HeaderText="Ngày sinh" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Giới tính">
                        <ItemTemplate><%# Convert.ToInt32(Eval("GioiTinh")) == 1 ? "Nam" : "Nữ" %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TruongHoc" HeaderText="Trường học" />
                </Columns>
            </asp:GridView>

            <!--  MODAL Thêm/Sửa (dùng chung 1 form)  -->
            <asp:Panel ID="pnlModal" runat="server" CssClass="modal-backdrop-custom" Style="display:none;">
                <div class="card mx-auto mt-5" style="max-width:480px;">
                    <div class="card-body">
                        <h6><asp:Label ID="lblModalTitle" runat="server" /></h6>
                        <asp:HiddenField ID="hdnEditingId" runat="server" />

                        <div class="mb-2"><label>Account</label>
                            <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control" /></div>
                        <div class="mb-2"><label>Họ tên</label>
                            <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" /></div>
                        <div class="mb-2"><label>Quê quán</label>
                            <asp:TextBox ID="txtQueQuan" runat="server" CssClass="form-control" /></div>
                        <div class="mb-2"><label>Ngày sinh</label>
                            <asp:TextBox ID="txtNgaySinh" runat="server" CssClass="form-control" TextMode="Date" /></div>
                        <div class="mb-2"><label>Giới tính</label>
                            <asp:DropDownList ID="ddlGioiTinh" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Nữ" Value="0" />
                                <asp:ListItem Text="Nam" Value="1" />
                            </asp:DropDownList></div>
                        <div class="mb-3"><label>Trường học</label>
                            <asp:TextBox ID="txtTruongHoc" runat="server" CssClass="form-control" /></div>

                        <asp:Label ID="lblFormError" runat="server" CssClass="text-danger d-block mb-2" />
                        <asp:Button ID="btnSaveModal" runat="server" CssClass="btn btn-primary" Text="OK" OnClick="btnSaveModal_Click" />
                        <asp:Button ID="btnCancelModal" runat="server" CssClass="btn btn-outline-secondary" Text="Huỷ" OnClick="btnCancelModal_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>

            <!--  Xác nhận xoá  -->
            <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modal-backdrop-custom" Style="display:none;">
                <div class="card mx-auto mt-5" style="max-width:420px;">
                    <div class="card-body text-center">
                        <asp:Label ID="lblConfirmMsg" runat="server" CssClass="d-block mb-3" />
                        <asp:Button ID="btnConfirmDeleteOk" runat="server" CssClass="btn btn-danger me-2" Text="OK" OnClick="btnConfirmDeleteOk_Click" />
                        <asp:Button ID="btnConfirmDeleteCancel" runat="server" CssClass="btn btn-outline-secondary" Text="Huỷ" OnClick="btnConfirmDeleteCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <style>
        .modal-backdrop-custom {
            position: fixed; top:0; left:0; width:100%; height:100%;
            background: rgba(0,0,0,.5); z-index: 1050;
        }
    </style>
</asp:Content>