using NhanVienWebApp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NhanVienWebApp
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlLanguage.SelectedValue = System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "en-US" : "vi-VN";
            }
        }
        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            LanguageHelper.SetLanguage(ddlLanguage.SelectedValue);
            Response.Redirect(Request.RawUrl); // tải lại trang hiện tại để mọi nhãn/thông báo cập nhật theo ngôn ngữ mới
        }
    }
}