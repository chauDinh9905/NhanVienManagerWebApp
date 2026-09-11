using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NhanVienWebApp.App_Code
{
    public static class ValidationHelper
    {
        private static readonly Regex IdentifierRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]{0,63}$", RegexOptions.Compiled);
        public static bool IsValidIdentifier(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && IdentifierRegex.IsMatch(name);
        }
        public static readonly string[] AllowedDataTypes = new[]
        {
            "INT", "BIGINT", "VARCHAR(255)", "TEXT", "DATETIME", "DATE",
            "DOUBLE", "DECIMAL(10,2)", "BOOLEAN"
        };
        public static bool IsAllowedDataType(string type)
        {
            foreach (var t in AllowedDataTypes)
                if (string.Equals(t, type, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
        public static string ValidateHoTen(string hoTen)
        {
            if (string.IsNullOrWhiteSpace(hoTen)) return "Họ tên không được để trống.";
            if (Regex.IsMatch(hoTen, @"^\d+$")) return "Họ tên không được chỉ gồm chữ số.";
            if (hoTen.Length > 100) return "Họ tên tối đa 100 ký tự.";
            return null; // null = hợp lệ
        }
        public static string ValidateAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account)) return "Account không được để trống.";
            if (!Regex.IsMatch(account, @"^[a-zA-Z0-9_\.]{3,50}$"))
                return "Account chỉ gồm chữ, số, dấu chấm/gạch dưới, 3-50 ký tự.";
            return null;
        }
        public static string ValidateQueQuan(string queQuan)
        {
            if (string.IsNullOrWhiteSpace(queQuan)) return "Quê quán không được để trống.";
            if (queQuan.Any(char.IsDigit)) return "Quê quán không được chứa chữ số.";
            return null;
        }
        public static string ValidateTruongHoc(string truongHoc)
        {
            if (string.IsNullOrWhiteSpace(truongHoc)) return "Trường học không được để trống.";
            return null;
        }
        public static string ValidateNgaySinh(string ngaySinhText, out DateTime ngaySinh)
        {
            ngaySinh = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(ngaySinhText)) return "Ngày sinh không được để trống.";
            if (!DateTime.TryParse(ngaySinhText, out ngaySinh)) return "Ngày sinh không đúng định dạng.";
            if (ngaySinh > DateTime.Now) return "Ngày sinh không được lớn hơn hiện tại.";
            if (ngaySinh < new DateTime(1900, 1, 1)) return "Ngày sinh không hợp lệ.";
            return null;
        }
        public static string ValidateGioiTinh(string gioiTinhText, out int gioiTinh)
        {
            gioiTinh = -1;
            if (!int.TryParse(gioiTinhText, out gioiTinh) || (gioiTinh != 0 && gioiTinh != 1))
                return "Giới tính không hợp lệ.";
            return null;
        }
    }
}