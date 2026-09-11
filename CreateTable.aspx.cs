using NhanVienWebApp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NhanVienWebApp
{
    public partial class CreateTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DbSessionHelper.HasDatabaseSelected()) { Response.Redirect("~/SelectDatabase.aspx"); return; }
            txtTableName.Text = DbSessionHelper.CurrentTable;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int rowCount;
            int.TryParse(Request.Form["hdnRowCount"], out rowCount);

            var columns = new List<Tuple<string, string, bool>>();
            for (int i = 1; i <= rowCount; i++)
            {
                string name = Request.Form["fieldName_" + i];
                string type = Request.Form["fieldType_" + i];
                bool isPk = Request.Form["fieldPk_" + i] == "1";
                if (string.IsNullOrWhiteSpace(name)) continue; // dòng đã bị người dùng bấm "Xoá" trên UI
                columns.Add(Tuple.Create(name.Trim(), type, isPk));
            }

            try
            {
                SchemaHelper.CreateTable(DbSessionHelper.CurrentTable, columns);
                Response.Redirect("~/NhanVienData.aspx");
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "text-danger d-block mt-2";
                lblMessage.Text = "Tạo bảng thất bại: " + ex.Message;
            }
        }
    }
}