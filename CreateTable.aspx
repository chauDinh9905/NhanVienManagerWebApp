<%@ Page Title="Create Table" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CreateTable.aspx.cs" Inherits="NhanVienWebApp.CreateTable" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-body">
            <h5><asp:Label runat="server" Text="<%$ Resources:Resource, Btn_CreateTable %>" /></h5>
            <div class="mb-3">
                <label><asp:Label runat="server" Text="<%$ Resources:Resource, Lbl_TableName %>" /></label>
                <asp:TextBox ID="txtTableName" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
            <table class="table" id="fieldsTable">
                <thead>
                    <tr><th>Tên trường</th>
                        <th>Kiểu dữ liệu</th>
                        <th>PK/Unique</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody id="fieldsBody">
                    <!-- ID luôn có sẵn, không cho sửa -->
                    <tr>
                        <td><input class="form-control" value="ID" disabled /></td>
                        <td><input class="form-control" value="INT AUTO_INCREMENT PRIMARY KEY" disabled /></td>
                        <td class="text-center"><input type="checkbox" checked disabled /></td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
            <button type="button" class="btn btn-outline-secondary btn-sm" onclick="addFieldRow()">Thêm trường</button>
            <div class="mt-3">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="Tạo bảng" OnClick="btnSubmit_Click" />
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="d-block mt-2" />
        </div>
    </div>
    <script>
        var allowedTypes = ["INT", "BIGINT", "VARCHAR(255)", "TEXT", "DATETIME", "DATE", "DOUBLE", "DECIMAL(10,2)", "BOOLEAN"];
        var rowIndex = 0;
        function addFieldRow() {
            rowIndex++;
            var tbody = document.getElementById("fieldsBody");
            var tr = document.createElement("tr");

            var tdName = document.createElement("td");
            var inpName = document.createElement("input");
            inpName.className = "form-control"; inpName.name = "fieldName_" + rowIndex;
            inpName.pattern = "^[a-zA-Z_][a-zA-Z0-9_]*$";
            inpName.required = true;
            tdName.appendChild(inpName);

            var tdType = document.createElement("td");
            var sel = document.createElement("select");
            sel.className = "form-select"; sel.name = "fieldType_" + rowIndex;
            allowedTypes.forEach(function(t) {
                var opt = document.createElement("option"); opt.value = t; opt.text = t; sel.appendChild(opt);
            });
            tdType.appendChild(sel);

            var tdPk = document.createElement("td"); tdPk.className = "text-center";
            var chk = document.createElement("input"); chk.type = "checkbox"; chk.name = "fieldPk_" + rowIndex; chk.value = "1";
            tdPk.appendChild(chk);

            var tdDel = document.createElement("td");
            var btnDel = document.createElement("button");
            btnDel.type = "button"; btnDel.className = "btn btn-sm btn-outline-danger"; btnDel.innerText = "Xoá";
            btnDel.onclick = function() { tr.remove(); };
            tdDel.appendChild(btnDel);

            tr.appendChild(tdName); tr.appendChild(tdType); tr.appendChild(tdPk); tr.appendChild(tdDel);
            tbody.appendChild(tr);

// lưu tổng số dòng vào hidden field để server biết cần đọc bao nhiêu dòng
    document.getElementById("hdnRowCount").value = rowIndex;
}
    window.onload = function() {
        var defaults = [
            ["Account", "VARCHAR(255)"], ["HoTen", "VARCHAR(255)"], ["QueQuan", "VARCHAR(255)"],
            ["NgaySinh", "DATETIME"], ["GioiTinh", "INT"], ["TruongHoc", "VARCHAR(255)"]
        ];
        defaults.forEach(function(d) {
            addFieldRow();
            var lastRow = document.getElementById("fieldsBody").lastChild;
            lastRow.querySelector("input[type=text], input.form-control").value = d[0];
            var select = lastRow.querySelector("select");
            select.value = d[1];
                });
            };
    </script>
    <asp:HiddenField ID="hdnRowCount" runat="server" ClientIDMode="Static" />
</asp:Content>
