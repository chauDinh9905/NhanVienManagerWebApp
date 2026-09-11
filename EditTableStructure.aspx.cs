using NhanVienWebApp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NhanVienWebApp
{
    public partial class EditTableStructure : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DbSessionHelper.HasDatabaseSelected()) 
            { 
                Response.Redirect("~/SelectDatabase.aspx"); return; 
            }
            lblTableName.Text = DbSessionHelper.CurrentTable;
            if (!IsPostBack) BindColumns();
        }
        private void BindColumns()
        {
            gvColumns.DataSource = SchemaHelper.GetColumns(DbSessionHelper.CurrentTable);
            gvColumns.DataBind();
        }
        protected void gvColumns_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            string colName = e.CommandArgument.ToString();

            if (e.CommandName == "Edit")
            {
                pnlEdit.Visible = true;
                lblOldName.Text = colName;
                txtNewName.Text = colName;
                ddlNewType.Items.Clear();
                foreach (var t in ValidationHelper.AllowedDataTypes) ddlNewType.Items.Add(t);
                ViewState["EditingColumn"] = colName;
            }
            else if (e.CommandName == "DeleteCol")
            {
                try
                {
                    SchemaHelper.DropColumn(DbSessionHelper.CurrentTable, colName);
                    lblMessage.CssClass = "text-success d-block mt-2";
                    lblMessage.Text = "Đã xoá cột " + colName + ".";
                    BindColumns();
                }
                catch (Exception ex)
                {
                    lblMessage.CssClass = "text-danger d-block mt-2";
                    lblMessage.Text = "Lỗi: " + ex.Message;
                }
            }
        }
        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            string oldName = (string)ViewState["EditingColumn"];
            try
            {
                SchemaHelper.RenameOrChangeColumn(DbSessionHelper.CurrentTable, oldName, txtNewName.Text.Trim(), ddlNewType.SelectedValue);
                lblMessage.CssClass = "text-success d-block mt-2";
                lblMessage.Text = "Đã cập nhật cột thành công.";
                pnlEdit.Visible = false;
                BindColumns();
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "text-danger d-block mt-2";
                lblMessage.Text = "Lỗi: " + ex.Message;
            }
        }
    }
}