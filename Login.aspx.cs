using MySql.Data.MySqlClient;
using NhanVienWebApp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NhanVienWebApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnConnect_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int port;
            if (!int.TryParse(txtPort.Text.Trim(), out port))
            {
                lblMessage.Text = "Port không hợp lệ.";
                return;
            }

            try
            {
                // Test kết nối THẬT trước khi lưu vào Session — tránh lưu credential sai
                var csb = new MySqlConnectionStringBuilder
                {
                    Server = txtServer.Text.Trim(),
                    Port = (uint)port,
                    UserID = txtAccount.Text.Trim(),
                    Password = txtPassword.Text,
                    ConnectionTimeout = 5
                };
                using (var conn = new MySqlConnection(csb.ConnectionString))
                {
                    conn.Open(); // nếu sai tài khoản/mật khẩu sẽ ném MySqlException ở đây
                }

                DbSessionHelper.SaveCredentials(txtServer.Text.Trim(), port, txtAccount.Text.Trim(), txtPassword.Text);
                Response.Redirect("~/SelectDatabase.aspx");
            }
            catch (MySqlException ex)
            {
                lblMessage.Text = "Kết nối thất bại: " + ex.Message;
            }
        }
    }
}