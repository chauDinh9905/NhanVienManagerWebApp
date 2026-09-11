using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
namespace NhanVienWebApp.App_Code
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public string DataType { get; set; }   // vd: int, varchar, datetime
        public string ColumnKey { get; set; }  // "PRI" nếu là khoá chính
        public bool IsPrimaryKey { get { return ColumnKey == "PRI"; } }
    }
    public static class SchemaHelper
    {
        public static bool DatabaseExists(string dbName)
        {
            using (var conn = DbSessionHelper.CreateServerConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name = @db", conn))
                {
                    cmd.Parameters.AddWithValue("@db", dbName);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
        public static void CreateDatabase(string dbName)
        {
            if (!ValidationHelper.IsValidIdentifier(dbName))
                throw new ArgumentException("Tên database không hợp lệ.");

            using (var conn = DbSessionHelper.CreateServerConnection())
            {
                conn.Open();
                // Tên DB không tham số hoá được trong DDL -> đã whitelist bằng Regex ở trên
                using (var cmd = new MySqlCommand(
                    "CREATE DATABASE `" + dbName + "` CHARACTER SET utf8mb4", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static bool TableExists(string tableName)
        {
            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "SELECT COUNT(*) FROM information_schema.tables " +
                    "WHERE table_schema = @db AND table_name = @tbl", conn))
                {
                    cmd.Parameters.AddWithValue("@db", DbSessionHelper.CurrentDatabase);
                    cmd.Parameters.AddWithValue("@tbl", tableName);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
        public static List<ColumnInfo> GetColumns(string tableName)
        {
            var list = new List<ColumnInfo>();
            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "SELECT column_name, data_type, column_key FROM information_schema.columns " +
                    "WHERE table_schema = @db AND table_name = @tbl ORDER BY ordinal_position", conn))
                {
                    cmd.Parameters.AddWithValue("@db", DbSessionHelper.CurrentDatabase);
                    cmd.Parameters.AddWithValue("@tbl", tableName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ColumnInfo
                            {
                                Name = reader.GetString(0),
                                DataType = reader.GetString(1),
                                ColumnKey = reader.IsDBNull(2) ? "" : reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return list;
        }
        public static void CreateTable(string tableName, List<Tuple<string, string, bool>> columns)
        {
            // columns: (tenCot, kieuDuLieu, laPrimaryKey)
            if (!ValidationHelper.IsValidIdentifier(tableName))
                throw new ArgumentException("Tên bảng không hợp lệ.");

            var sb = new StringBuilder();
            sb.Append("CREATE TABLE `" + tableName + "` (");
            sb.Append("`ID` INT AUTO_INCREMENT PRIMARY KEY");

            foreach (var col in columns)
            {
                string name = col.Item1, type = col.Item2;
                bool isPk = col.Item3;

                if (string.Equals(name, "ID", StringComparison.OrdinalIgnoreCase)) continue; // ID luôn tự sinh, không cho tạo trùng
                if (!ValidationHelper.IsValidIdentifier(name))
                    throw new ArgumentException("Tên cột không hợp lệ: " + name);
                if (!ValidationHelper.IsAllowedDataType(type))
                    throw new ArgumentException("Kiểu dữ liệu không được phép: " + type);

                sb.Append(", `" + name + "` " + type);
                if (isPk) sb.Append(" UNIQUE"); // ID đã là PRIMARY KEY thật sự; cột khác chỉ có thể là UNIQUE KEY bổ sung
            }
            sb.Append(") ENGINE=InnoDB CHARACTER SET utf8mb4");

            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sb.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void EnsureNhanVienColumns(string tableName)
        {
            var required = new Dictionary<string, string>
        {
            { "Account", "VARCHAR(255)" },
            { "HoTen", "VARCHAR(255)" },
            { "QueQuan", "VARCHAR(255)" },
            { "NgaySinh", "DATETIME" },
            { "GioiTinh", "INT" },
            { "TruongHoc", "VARCHAR(255)" }
        };
            var existing = GetColumns(tableName);
            var existingNames = existing.ConvertAll(c => c.Name.ToLowerInvariant());

            foreach (var kv in required)
            {
                if (!existingNames.Contains(kv.Key.ToLowerInvariant()))
                {
                    AddColumn(tableName, kv.Key, kv.Value);
                }
            }
        }
        public static void AddColumn(string tableName, string columnName, string dataType)
        {
            if (!ValidationHelper.IsValidIdentifier(tableName) || !ValidationHelper.IsValidIdentifier(columnName))
                throw new ArgumentException("Tên bảng/cột không hợp lệ.");
            if (!ValidationHelper.IsAllowedDataType(dataType))
                throw new ArgumentException("Kiểu dữ liệu không được phép.");

            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                string sql = "ALTER TABLE `" + tableName + "` ADD COLUMN `" + columnName + "` " + dataType;
                using (var cmd = new MySqlCommand(sql, conn)) { cmd.ExecuteNonQuery(); }
            }
        }
        public static void RenameOrChangeColumn(string tableName, string oldName, string newName, string newType)
        {
            if (!ValidationHelper.IsValidIdentifier(tableName) ||
                !ValidationHelper.IsValidIdentifier(oldName) ||
                !ValidationHelper.IsValidIdentifier(newName))
                throw new ArgumentException("Tên bảng/cột không hợp lệ.");
            if (!ValidationHelper.IsAllowedDataType(newType))
                throw new ArgumentException("Kiểu dữ liệu không được phép.");

            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                string sql = "ALTER TABLE `" + tableName + "` CHANGE `" + oldName + "` `" + newName + "` " + newType;
                using (var cmd = new MySqlCommand(sql, conn)) { cmd.ExecuteNonQuery(); }
            }
        }
        public static void DropColumn(string tableName, string columnName)
        {
            if (!ValidationHelper.IsValidIdentifier(tableName) || !ValidationHelper.IsValidIdentifier(columnName))
                throw new ArgumentException("Tên bảng/cột không hợp lệ.");
            if (string.Equals(columnName, "ID", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Không được xoá cột ID (khoá chính).");

            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                string sql = "ALTER TABLE `" + tableName + "` DROP COLUMN `" + columnName + "`";
                using (var cmd = new MySqlCommand(sql, conn)) { cmd.ExecuteNonQuery(); }
            }
        }
    }
}