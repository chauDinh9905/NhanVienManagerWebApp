using NhanVienWebApp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NhanVienWebApp
{
    public partial class SelectDatabase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DbSessionHelper.HasCredentials())
            {
                Response.Redirect("~/Login.aspx");
            }
        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string dbName = txtDbName.Text.Trim();
            string tableName = txtTableName.Text.Trim();

            if (!ValidationHelper.IsValidIdentifier(dbName) || !ValidationHelper.IsValidIdentifier(tableName))
            {
                lblStatus.Text = "Tên database/bảng chỉ được gồm chữ, số, gạch dưới.";
                pnlResult.Visible = true;
                btnCreateDb.Visible = btnCreateTable.Visible = btnEditTable.Visible = btnViewData.Visible = false;
                return;
            }

            pnlResult.Visible = true;
            btnCreateDb.Visible = btnCreateTable.Visible = btnEditTable.Visible = btnViewData.Visible = false;

            if (!SchemaHelper.DatabaseExists(dbName))
            {
                lblStatus.Text = (string)GetGlobalResourceObject("Resource", "Msg_DbNotExist");
                ViewState["PendingDbName"] = dbName;
                ViewState["PendingTableName"] = tableName;
                btnCreateDb.Visible = true;
                return;
            }

            DbSessionHelper.SaveDatabaseAndTable(dbName, tableName);

            if (!SchemaHelper.TableExists(tableName))
            {
                lblStatus.Text = (string)GetGlobalResourceObject("Resource", "Msg_TableNotExist");
                btnCreateTable.Visible = true;
            }
            else
            {
                // Bảng đã có -> đảm bảo đủ cột chuẩn (yêu cầu gốc: thiếu cột nào bổ sung cột đó)
                SchemaHelper.EnsureNhanVienColumns(tableName);
                lblStatus.Text = "Đã kết nối tới " + dbName + "." + tableName;
                btnEditTable.Visible = true;
                btnViewData.Visible = true;
            }
        }
        protected void btnCreateDb_Click(object sender, EventArgs e)
        {
            string dbName = (string)ViewState["PendingDbName"];
            SchemaHelper.CreateDatabase(dbName);
            DbSessionHelper.SaveDatabaseAndTable(dbName, (string)ViewState["PendingTableName"]);
            Response.Redirect("~/CreateTable.aspx");
        }
        protected void btnCreateTable_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CreateTable.aspx");
        }
        protected void btnEditTable_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditTableStructure.aspx");
        }
        protected void btnViewData_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/NhanVienData.aspx");
        }
    }
}