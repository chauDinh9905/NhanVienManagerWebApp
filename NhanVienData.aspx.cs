using NhanVienWebApp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NhanVienWebApp
{
	public partial class NhanVienData : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!DbSessionHelper.HasDatabaseSelected()) { Response.Redirect("~/SelectDatabase.aspx"); return; }
            lblTableName.Text = DbSessionHelper.CurrentTable;
            if (!IsPostBack) BindGrid();
        }
        private void BindGrid()
        {
            gvNhanVien.DataSource = NhanVienDAL.GetAll(DbSessionHelper.CurrentTable);
            gvNhanVien.DataBind();
        }
        private List<int> GetSelectedIds()
        {
            var ids = new List<int>();
            foreach (GridViewRow row in gvNhanVien.Rows)
            {
                var chk = row.FindControl("chkSelect") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    ids.Add(Convert.ToInt32(gvNhanVien.DataKeys[row.RowIndex].Value));
                }
            }
            return ids;
        }

        private void ClearModalForm()
        {
            txtAccount.Text = txtHoTen.Text = txtQueQuan.Text = txtNgaySinh.Text = txtTruongHoc.Text = "";
            ddlGioiTinh.SelectedIndex = 0;
            lblFormError.Text = "";
        }

        private void ShowFormError(string msg)
        {
            lblFormError.Text = msg;
        }

        private void ShowToast(string msg, bool success)
        {
            lblToast.CssClass = success ? "text-success d-block mb-2" : "text-danger d-block mb-2";
            lblToast.Text = msg;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearModalForm();
            lblModalTitle.Text = "Thêm nhân viên";
            hdnEditingId.Value = "";
            pnlModal.Style["display"] = "block";
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            List<int> selected = GetSelectedIds();
            if (selected.Count != 1)
            {
                ShowToast((string)GetGlobalResourceObject("Resource", "Msg_SelectOneRow"), false);
                return;
            }

            var nv = NhanVienDAL.GetById(DbSessionHelper.CurrentTable, selected[0]);
            lblModalTitle.Text = "Sửa nhân viên";
            hdnEditingId.Value = nv.ID.ToString();
            txtAccount.Text = nv.Account;
            txtHoTen.Text = nv.HoTen;
            txtQueQuan.Text = nv.QueQuan;
            txtNgaySinh.Text = nv.NgaySinh.ToString("yyyy-MM-dd");
            ddlGioiTinh.SelectedValue = nv.GioiTinh.ToString();
            txtTruongHoc.Text = nv.TruongHoc;
            lblFormError.Text = "";
            pnlModal.Style["display"] = "block";
        }
        protected void btnSaveModal_Click(object sender, EventArgs e)
        {
            string err;
            err = ValidationHelper.ValidateAccount(txtAccount.Text.Trim()); if (err != null) { ShowFormError(err); return; }
            err = ValidationHelper.ValidateHoTen(txtHoTen.Text.Trim()); if (err != null) { ShowFormError(err); return; }
            err = ValidationHelper.ValidateQueQuan(txtQueQuan.Text.Trim()); if (err != null) { ShowFormError(err); return; }
            err = ValidationHelper.ValidateTruongHoc(txtTruongHoc.Text.Trim()); if (err != null) { ShowFormError(err); return; }

            DateTime ngaySinh;
            err = ValidationHelper.ValidateNgaySinh(txtNgaySinh.Text.Trim(), out ngaySinh); if (err != null) { ShowFormError(err); return; }

            int gioiTinh;
            err = ValidationHelper.ValidateGioiTinh(ddlGioiTinh.SelectedValue, out gioiTinh); if (err != null) { ShowFormError(err); return; }

            var nv = new NhanVien
            {
                Account = txtAccount.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                QueQuan = txtQueQuan.Text.Trim(),
                NgaySinh = ngaySinh,
                GioiTinh = gioiTinh,
                TruongHoc = txtTruongHoc.Text.Trim()
            };

            bool isEdit = !string.IsNullOrEmpty(hdnEditingId.Value);
            try
            {
                if (isEdit)
                {
                    nv.ID = int.Parse(hdnEditingId.Value);
                    NhanVienDAL.Update(DbSessionHelper.CurrentTable, nv);
                    ShowToast((string)GetGlobalResourceObject("Resource", "Msg_EditSuccess"), true);
                }
                else
                {
                    NhanVienDAL.Insert(DbSessionHelper.CurrentTable, nv);
                    ShowToast((string)GetGlobalResourceObject("Resource", "Msg_AddSuccess"), true);
                }
                pnlModal.Style["display"] = "none";
                BindGrid(); // cập nhật lại lưới ngay lập tức (AJAX, không reload trang)
            }
            catch (Exception ex)
            {
                ShowFormError("Lỗi khi lưu dữ liệu: " + ex.Message);
            }
        }
        protected void btnCancelModal_Click(object sender, EventArgs e)
        {
            pnlModal.Style["display"] = "none";
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> selected = GetSelectedIds();
            if (selected.Count == 0)
            {
                ShowToast((string)GetGlobalResourceObject("Resource", "Msg_SelectAtLeastOne"), false);
                return;
            }

            string template = (string)GetGlobalResourceObject("Resource", "Msg_ConfirmDelete");
            lblConfirmMsg.Text = string.Format(template, selected.Count); // hiện đúng số lượng x bản ghi
            ViewState["PendingDeleteIds"] = selected;
            pnlConfirmDelete.Style["display"] = "block";
        }
        protected void btnConfirmDeleteOk_Click(object sender, EventArgs e)
        {
            var ids = (List<int>)ViewState["PendingDeleteIds"];
            int count = NhanVienDAL.DeleteMany(DbSessionHelper.CurrentTable, ids);
            pnlConfirmDelete.Style["display"] = "none";
            ShowToast((string)GetGlobalResourceObject("Resource", "Msg_DeleteSuccess") + " (" + count + ")", true);
            BindGrid();
        }

        protected void btnConfirmDeleteCancel_Click(object sender, EventArgs e)
        {
            pnlConfirmDelete.Style["display"] = "none";
        }
    }
}