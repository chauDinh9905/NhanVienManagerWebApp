using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace NhanVienWebApp.App_Code
{
    public static class LanguageHelper
    {
        private const string COOKIE_NAME = "site_lang";
        public static void ApplyCultureFromCookie(HttpContext context)
        {
            string lang = "vi-VN"; // mặc định
            HttpCookie cookie = context.Request.Cookies[COOKIE_NAME];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                lang = cookie.Value;
            }
            SetThreadCulture(lang);
        }
        private static void SetThreadCulture(string lang)
        {
            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        public static void SetLanguage(string lang)
        {
            HttpCookie cookie = new HttpCookie(COOKIE_NAME, lang);
            cookie.Expires = DateTime.Now.AddYears(1); // tồn tại lâu dài -> tắt/mở lại trình duyệt (và máy) vẫn còn
            HttpContext.Current.Response.Cookies.Add(cookie);
            SetThreadCulture(lang);
        }
    }
}